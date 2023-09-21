namespace IBS.Models
{
    public class EmailFeedback
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string ToRegion { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
    }
}
