using System.Net.Mail;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;

namespace Petzey.Appointments.Business
{
    public class EmailNotification : INotification
    {
        public void Send(string addr, string msg)
        {
            MailMessage message = new MailMessage();

            //sender
            string mailId = ConfigurationManager.AppSettings["MailId"];
            string pw = ConfigurationManager.AppSettings["Password"];

            message.From = new MailAddress(mailId);
            message.Subject = "Appointment Booked";
            message.Body = $"{msg}";

            //receiver
            message.To.Add(new MailAddress(addr));
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mailId, pw);

            smtpClient.Send(message);
        }
    }
}
