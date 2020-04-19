using System;
using System.Net;
using System.Net.Mail;

namespace RemoteSystemAuditor
{
    public static class SendMail
    {
        public static void NewMessage(string subject, string message)
        {
            Settings settings = new Settings();

            try
            {
                SmtpClient SmtpClient = new SmtpClient();

                // set smtp-client with basicAuthentication
                SmtpClient.Host = settings.host;
                SmtpClient.Port = settings.port;

                // must be set before NetworkCredentials
                SmtpClient.UseDefaultCredentials = false;
                SmtpClient.Credentials = new NetworkCredential(settings.userName, settings.password);
                SmtpClient.EnableSsl = true;

                MailMessage Mail = new MailMessage();

                // add from,to mailaddresses
                Mail.From = new MailAddress(settings.emailAddress);
                Mail.To.Add(settings.destinationEmail);

                // set subject and encoding
                Mail.Subject = subject;
                Mail.SubjectEncoding = System.Text.Encoding.UTF8;

                // set body-message and encoding
                Mail.Body = message;
                Mail.BodyEncoding = System.Text.Encoding.UTF8;

                // send Mail
                SmtpClient.Send(Mail);

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
