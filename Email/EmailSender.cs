using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace PanOpticon.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings emailSettings;

        public EmailSender(IOptions<EmailSettings> _email)
        {
            emailSettings = _email.Value;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(emailSettings.ApiBaseUri) })
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",Convert.ToBase64String(Encoding.ASCII.GetBytes(emailSettings.ApiKey)));

                var formUrlEncodedContent = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string,string>("from", emailSettings.From),
                    new KeyValuePair<string,string>("to", email),
                    new KeyValuePair<string,string>("subject", subject),
                    new KeyValuePair<string,string>("html", htmlMessage)
                    });

         
                await client.PostAsync(emailSettings.RequestUri, formUrlEncodedContent).ConfigureAwait(false);

            }
        }

      

    }
}
