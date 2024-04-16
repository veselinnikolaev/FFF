using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SendGrid;
using System.Diagnostics;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using SendGrid.Helpers.Mail;

namespace FFF.Services
{
	public class EmailService : IIdentityMessageService
	{
		public Task SendAsync(IdentityMessage message)
		{
			return ConfigSendGridAsync(message);
		}

		private async Task ConfigSendGridAsync(IdentityMessage message)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress("FFF@gmail.com", "FFF Team");
            var to = new EmailAddress(message.Destination);
            var subject = "Email Confirmation";
            var plainTextContent = $"Hello, please confirm your email by clicking on this link: {message.Body}";
            var htmlContent = $"<p>Hello,</p><p>Please confirm your email by clicking on this link: <a href='{message.Body}'>Confirm Email</a></p>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Accepted)
            {
                throw new Exception($"Failed to send email. Status code: {response.StatusCode}");
            }
        }
	}
}
