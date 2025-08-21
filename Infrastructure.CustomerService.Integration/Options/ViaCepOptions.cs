namespace Infrastructure.CustomerRegistration.Integration.Options
{
    public sealed class ViaCepOptions
    {
        public const string SectionName = "ViaCep";
        public string BaseUrl { get; set; } = "https://viacep.com.br";
    }
}