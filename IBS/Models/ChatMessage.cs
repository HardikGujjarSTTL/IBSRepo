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
        public string RelativePath { get; set; }
        public string Field_ID { get; set; }
        public string Extension { get; set; }
        public string FileDisplayName { get; set; }
        public int CurrDateCount { get; set; }
        public List<ChatMessage> lstMsg { get; set; }
    }

    public class FormData
    {        
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Message { get; set; }
        public int MsgType { get; set; }
        public IFormFile MyFiles { get; set; }
    }

    public class ImageData
    {
        public string Description { get; set; }
        public string FileName { get; set; }
        public string Image { get; set; }
    }
}

