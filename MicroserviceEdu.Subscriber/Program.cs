// See https://aka.ms/new-console-template for more information

using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json;
using MicroserviceEdu.Subscriber;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("Subscriber");

var connectionFactory = new ConnectionFactory
{
    HostName = "goose-01.rmq2.cloudamqp.com",
    UserName = "hapfnyrs",
    Password = "E7qLehawhCZ2VP8qN-ybhuWO8se90RGX",
    VirtualHost = "hapfnyrs",
    Uri = new Uri("amqps://hapfnyrs:E7qLehawhCZ2VP8qN-ybhuWO8se90RGX@goose.rmq2.cloudamqp.com/hapfnyrs")
};
var connection = connectionFactory.CreateConnection();

using var channel = connection.CreateModel();

//prefetch count

channel.BasicQos(0, 10, true);

var consumer = new EventingBasicConsumer(channel);

// event => delegate => method

consumer.Received += (sender, eventArgs) =>
{
    Thread.Sleep(1000);
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    var userCreatedEvent = JsonSerializer.Deserialize<UserCreatedEvent>(message);

    if (userCreatedEvent is not null)
    {
        Console.WriteLine(
            $"Message received:{userCreatedEvent.Id} - {userCreatedEvent.Name} - {userCreatedEvent.Email} ");

        channel.BasicAck(eventArgs.DeliveryTag, false);
    }
};


channel.BasicConsume("demo-queue", false, consumer);

Console.ReadLine();