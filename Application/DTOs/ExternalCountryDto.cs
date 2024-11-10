namespace Application.DTOs
{
    public class ExternalCountryDto
    {
        public Name Name { get; set; }
        public List<string> Capital { get; set; }
        public List<string> Borders { get; set; }
    }

    public class Name
    {
        public string Common { get; set; }
        public string Official { get; set; }
    }
}
