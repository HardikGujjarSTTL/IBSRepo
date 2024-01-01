﻿namespace IBS.Models
{
    public class ChatMessage
    {
        public int ID { get; set; }
        public string msg_send_ID { get; set; }
        public string msg_recv_ID { get; set; }
        public string send_message { get; set; }
        public string recv_message { get; set; }
        public string Name { get; set; }
        public List<ChatMessage> lstMsg { get; set; }
    }
}

