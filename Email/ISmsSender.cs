using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace PanOpticon.Email
{
    public interface ISmsSender
    {
         void SendSMS(string phone, string msg);
    }

    public  class SMSSender : ISmsSender
    {
        public  void SendSMS(string phone, string msg)
        {
    
                var accountSid = "AC4b60764b9cb705f796f462d28925194e";
                var authToken = "f5488675640805128bddcd01d1b8d4d5";
                TwilioClient.Init(accountSid, authToken);

                var messageOptions = new CreateMessageOptions(
                    new PhoneNumber("+1" + phone));
                messageOptions.MessagingServiceSid = "MGf63e72d7352e5f57cba4de8190e4ed6e";
                messageOptions.Body = msg;

                MessageResource.Create(messageOptions);

            
        }
    }

}
