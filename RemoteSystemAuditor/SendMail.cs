using System;
using System.Net;
using System.Net.Mail;

namespace RemoteSystemAuditor
{
    public class SendMail
    {
        private readonly SmtpClient smtp;
        private readonly MailMessage mail;
        private readonly Settings settings;

        public SendMail()
        {
            this.smtp = new SmtpClient();
            this.mail = new MailMessage();
            this.settings = new Settings();
        }

        public void NewMessage(string subject, string message)
        {
            try
            {
                // Set smtp-client with basicAuthentication.
                this.smtp.Host = this.settings.host;
                this.smtp.Port = this.settings.port;

                // Must be set before NetworkCredentials.
                this.smtp.UseDefaultCredentials = false;
                this.smtp.Credentials = new NetworkCredential(this.settings.userName, this.settings.password);
                this.smtp.EnableSsl = true;

                // Add from & to mailaddresses.
                this.mail.From = new MailAddress(this.settings.emailAddress);
                this.mail.To.Add(this.settings.destinationEmail);

                // Set subject & encoding.
                this.mail.Subject = subject;
                this.mail.SubjectEncoding = System.Text.Encoding.UTF8;

                // Set body-message & encoding.
                this.mail.Body = message;
                this.mail.BodyEncoding = System.Text.Encoding.UTF8;

                // Send Mail.
                this.smtp.Send(this.mail);

                Console.WriteLine("Mail Sent!");
            }
            catch (SmtpException ex)
            {
                Console.WriteLine("SmtpException has occured: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
