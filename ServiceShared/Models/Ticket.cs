namespace ServiceShared.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Quantity { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public DateTime Time { get; set; }
    }
}
