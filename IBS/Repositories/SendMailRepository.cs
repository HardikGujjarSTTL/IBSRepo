using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Humanizer;
using MessagePack;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Core;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace IBS.Repositories
{
    public class SendMailRepository : ISendMailRepository
    {
        private readonly ModelContext context;

        private string SMTPFromEmail = "";
        private string SMTPToEmail = "";
        private string SMTPServer = "";
        private int SMTPPort = 0;
        private string Displayname = "";
        private string SMTPUser = "";
        private string SMTPPass = "";
        private bool EnableSsl = false;
        public SendMailRepository(ModelContext context)
        {
            this.context = context;
            var GenSettings = context.Emailconfigurations.Find(1);
            if (GenSettings != null)
            {
                SMTPFromEmail = GenSettings.Email ?? "";
                SMTPToEmail = GenSettings.Email ?? "";
                SMTPServer = GenSettings.Host ?? "";
                SMTPPort = GenSettings.Port ?? 0;
                Displayname = GenSettings.Displayname ?? "";
                SMTPUser = GenSettings.Username ?? "";
                SMTPPass = GenSettings.Password ?? "";
                EnableSsl = GenSettings.Enablessl ?? false;
            }
        }
        public bool SendMail(SendMailModel model, IFormFileCollection? Files)
        {
            using (var message = new MailMessage())
            {
                message.IsBodyHtml = true;
                if(model.From != null)
                {
                    message.From = new MailAddress(model.From);
                }
                else
                {
                    message.From = new MailAddress(SMTPFromEmail);
                }
                if (!string.IsNullOrEmpty(model.To))
                {
                    foreach (var item in model.To.Split(','))
                    {
                        message.To.Add(item);
                    }
                }

                if (!string.IsNullOrEmpty(model.CC))
                {
                    foreach (var item in model.CC.Split(','))
                    {
                        message.Bcc.Add(item);
                    }
                }

                if (!string.IsNullOrEmpty(model.To))
                {
                    foreach (var item in model.To.Split(','))
                    {
                        message.CC.Add(item);
                    }
                }

                message.Subject = model.Subject;
                message.Body = model.Message;
                if (Files != null)
                {
                    for (int i = 0; i < Files.Count; i++)
                    {
                        var _FileName = Files[i].FileName;
                        var memoryStream = new MemoryStream();
                        Files[i].CopyToAsync(memoryStream);
                        message.Attachments.Add(new Attachment(memoryStream, _FileName));
                    }
                }

                SmtpClient smtpClient = new SmtpClient(SMTPServer, SMTPPort);
                NetworkCredential nc = new NetworkCredential(SMTPUser, SMTPPass);
                smtpClient.Credentials = nc;
                smtpClient.EnableSsl = EnableSsl;
                try
                {
                    smtpClient.Send(message);
                    message.Dispose();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

        }
    }
}
