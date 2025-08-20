namespace Core.CustomerRegistration.Domain.Entities
{
    public class CustomerEntity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Occupation { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public CustomerEntity() { }

        public CustomerEntity(string name, string email, string occupation)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name is required");
            if (string.IsNullOrEmpty(email)) throw new ArgumentException("Email is required");
            if (string.IsNullOrEmpty(occupation)) throw new ArgumentException("Occupation is required");

            Name = name;
            Email = email;
            Occupation = occupation;
        }

        public void Update(string name, string email, string occupation)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name is required");
            if (string.IsNullOrEmpty(email)) throw new ArgumentException("Email is required");
            if (string.IsNullOrEmpty(occupation)) throw new ArgumentException("Occupation is required");
            Name = name;
            Email = email;
            Occupation = occupation;
        }
    }
}
