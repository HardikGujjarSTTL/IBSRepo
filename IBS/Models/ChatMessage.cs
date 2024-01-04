namespace IBS.Models
{
    public class ChatMessage
    {
        public int ID { get; set; }
        public string msg_send_ID { get; set; }
        public string msg_recv_ID { get; set; }
        public string message { get; set; }
        public string Name { get; set; }
        public string HostUrl { get; set; }
        public int Master_ID { get; set; }
        public DateTime Msg_Date { get; set; }
        public List<ChatMessage> lstMsg { get; set; }
    }
}

