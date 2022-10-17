using System.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Petzey.Appointments.Business
{
    public class SMSNotification : INotification
    {
        public void Send(string mob, string msg)
        {
            string accountSid = ConfigurationManager.AppSettings["AccountSid"];
            string authToken = ConfigurationManager.AppSettings["AuthToken"];
            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                    body: msg,
                    from: new Twilio.Types.PhoneNumber(ConfigurationManager.AppSettings["SenderNo"]),
                    to: new Twilio.Types.PhoneNumber(ConfigurationManager.AppSettings["ReceiverNo"])
                );
        }
    }

}
