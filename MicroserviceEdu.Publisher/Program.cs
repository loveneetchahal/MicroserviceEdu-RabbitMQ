// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.Json;
using MicroserviceEdu.Publisher;
using RabbitMQ.Client;

Console.WriteLine("Publisher");

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

channel.QueueDeclare("demo-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
channel.ConfirmSelect();


Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    var userCreatedEvent = new UserCreatedEvent
    {
        Id = x,
        Name = "Loveneet",
        Email = "loveneet@outlook.com"
    };
    var userCreatedEventAsJson = JsonSerializer.Serialize(userCreatedEvent);


    var userCreatedEventAsBinary = Encoding.UTF8.GetBytes(userCreatedEventAsJson);

    try
    {
        channel.BasicPublish("", "demo-queue", null, userCreatedEventAsBinary);

        channel.WaitForConfirms(TimeSpan.FromSeconds(10));
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    } 
    Console.WriteLine("Message sent: ");
});