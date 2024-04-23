namespace MicroserviceEdu.Publisher
{
    internal class UserCreatedEvent
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}