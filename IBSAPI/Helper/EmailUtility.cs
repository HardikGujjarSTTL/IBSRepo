using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using IBSAPI.Models;

namespace IBSAPI.Helper
{
    public class EmailUtility
    {
        private readonly SMTPEmailConfigModel _emailConfiguration = new();

        public EmailUtility(IConfiguration configuration)
        {
            configuration.Bind("Email", _emailConfiguration);
        }

        public string SendEmail(EmailDetails emailDetails)
        {
            MailMessage mm;
            SmtpClient smtp;
            string strBCCAdr = string.Empty;
            string strCCAdr = string.Empty;
            try
            {
                mm = new MailMessage();

                ServicePointManager.ServerCertificateValidationCallback += (s, ce, ca, p) => true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                MailAddress from = new(_emailConfiguration.FromEmailID);
                mm.From = from;
                mm.To.Add(emailDetails.ToEmailID);
                mm.Body = emailDetails.Body;
                mm.Subject = emailDetails.Subject;
                mm.IsBodyHtml = _emailConfiguration.IsBodyHtml;

                if (!string.IsNullOrEmpty(emailDetails.CCEmailID))
                {
                    string[] emailIDs = emailDetails.CCEmailID.Split(',');
                    for (int i = 0; i < emailIDs.Length; i++)
                    {
                        MailAddress To = new(emailIDs[i]);
                        mm.CC.Add(To);

                        strCCAdr = strCCAdr + emailIDs[i] + ",";
                    }
                }

                if (!string.IsNullOrEmpty(emailDetails.BCCEmailID))
                {
                    string[] emailIDs = emailDetails.BCCEmailID.Split(',');
                    for (int i = 0; i < emailIDs.Length; i++)
                    {
                        MailAddress To = new(emailIDs[i]);
                        mm.Bcc.Add(To);

                        strBCCAdr = strBCCAdr + emailIDs[i] + ",";
                    }
                }

                if (emailDetails.AttachmentNames != null)
                {
                    foreach (string AttachmentName in emailDetails.AttachmentNames)
                    {
                        if (!string.IsNullOrEmpty(AttachmentName))
                        {
                            if (AttachmentName.Trim().Length > 0 && File.Exists(AttachmentName))
                                mm.Attachments.Add(new Attachment(AttachmentName));
                        }
                    }
                }

                System.Net.NetworkCredential myCredential = new(_emailConfiguration.UserName, _emailConfiguration.Password);
                smtp = new SmtpClient(_emailConfiguration.Host, _emailConfiguration.Port)
                {
                    Credentials = myCredential,
                    EnableSsl = _emailConfiguration.EnableSsl
                };

                smtp.Send(mm);
                mm.Dispose();

                return "";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public bool SendEmail(ArrayList attachments, string Subject, string mailBody, string ToEmailId)
        {
            MailMessage message = new MailMessage();

            ServicePointManager.ServerCertificateValidationCallback += (s, ce, ca, p) => true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            message.IsBodyHtml = true;
            message.From = new MailAddress(_emailConfiguration.FromEmailID);
            //message.Bcc.Add("dhruvdarji4@gmail.com");
            message.To.Add(ToEmailId);
            //message.CC.Add("jdpmssksb-mod@gov.in");
            message.Subject = Subject;
            message.Body = mailBody;

            if (attachments != null)
            {
                for (int i = 0; i < attachments.Count; i++)
                {
                    Attachment attached = (Attachment)attachments[i];
                    message.Attachments.Add(attached);
                }
            }

            SmtpClient smtpClient = new SmtpClient(_emailConfiguration.Host);
            smtpClient.Port = _emailConfiguration.Port;

            smtpClient.Host = _emailConfiguration.Host;
            NetworkCredential nc = new NetworkCredential(_emailConfiguration.UserName, _emailConfiguration.Password);
            //smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = nc;
            smtpClient.EnableSsl = _emailConfiguration.EnableSsl;

            try
            {
                smtpClient.Send(message);
                message.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message.ToString());
                return false;
            }

        }

    }
}