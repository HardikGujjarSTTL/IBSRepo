namespace IBS.Models
{
    public class SMTPEmailConfigModel
    {
        public string AdminEmails { get; set; }
        public string TemplatePath { get; set; }
        public string FromEmailID { get; set; }
        public string SenderDisplayName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public bool IsBodyHtml { get; set; }
    }

    public class EmailDetails
    {
        public string ToEmailID { get; set; }
        public string CCEmailID { get; set; }
        public string BCCEmailID { get; set; }
        public string ToName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string[] AttachmentNames { get; set; }
        public bool IsAgendaMail { get; set; } = false;
    }

    public class MailTracker
    {
        public string FromEmailID { get; set; }
        public string ToEmailID { get; set; }
        public string ToName { get; set; }
        public string Subject { get; set; }
        public System.DateTime? SentOnDt { get; set; }
    }

}
