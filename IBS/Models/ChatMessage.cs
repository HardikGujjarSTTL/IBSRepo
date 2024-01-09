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
        public int CurrDateCount { get; set; }
        public string FileName { get; set; }
        public List<ChatMessage> lstMsg { get; set; }
    }

    public class FormData
    {
        //public int lastModified { get; set; }
        //public string name { get; set; }
        //public int size { get; set; }
        //public string type { get; set; }
        //public string webkitRelativePath { get; set; }

        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Message { get; set; }
        public string MsgType { get; set; }
        public IFormFile Photos { get; set; }

    }
}

