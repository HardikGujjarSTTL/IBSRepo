namespace IBSAPI.Models
{
    public class ChatMessageModel
    {
        public int ID { get; set; }
        public string msg_send_ID { get; set; }
        public string msg_recv_ID { get; set; }
        public string message { get; set; }
        public string Name { get; set; }
        public string HostUrl { get; set; }
        public int Master_ID { get; set; }
        public DateTime Msg_Date { get; set; }
        public string RelativePath { get; set; }
        public string Field_ID { get; set; }
        public string Extension { get; set; }
        public string FileDisplayName { get; set; }
        public string FileSize { get; set; }
        public int CurrDateCount { get; set; }
    }
}
