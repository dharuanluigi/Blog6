using Blog6.Configurations;
using System.Net;
using System.Net.Mail;

namespace Blog6.Services
{
  public class SendEmailService
  {
    public bool Send(
      string toName,
      string toEmail,
      string subject,
      string body,
      string fromName = "Some name",
      string fromEmail = "some-email"
      )
    {
      try
      {
        var smtpClient = new SmtpClient(Configuration.Smtp.Host, int.Parse(Configuration.Smtp.Port))
        {
          Credentials = new NetworkCredential(Configuration.Smtp.UserName, Configuration.Smtp.Password),
          DeliveryMethod = SmtpDeliveryMethod.Network,
          EnableSsl = true
        };

        var mail = new MailMessage
        {
          From = new MailAddress(fromEmail, fromName)
        };
        mail.To.Add(new MailAddress(toEmail, toName));
        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true;

        smtpClient.Send(mail);
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return default;
      }
    }
  }
}