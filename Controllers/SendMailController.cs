using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Projekt_DT102G.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Projekt_DT102G.Controllers
{
	public class SendMailController : Controller
	{
		private readonly IConfiguration _config;

		//Ctor
		public SendMailController(IConfiguration config)
		{
			_config = config;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Index(Email email)
		{
			string apiKey = _config.GetValue<string>("APIKey");
			var client = new SendGridClient(apiKey);
			var from = new EmailAddress(email.To, "Azure Username");
			var subject = email.Subject;
			var to = new EmailAddress(email.To, "Azure Username");
			var plainTextContent = email.Body;
			var htmlContent = "<strong>Testing to send Email from Azure with C#</strong>";
			var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
			await client.SendEmailAsync(msg);

			//try
			//{
			//	string to = email.To;
			//	string subject = email.Subject;
			//	string body = email.Body;
			//	MailMessage mailMessage = new MailMessage();
			//	mailMessage.To.Add(to);
			//	mailMessage.Subject = subject;
			//	mailMessage.Body = body;
			//	mailMessage.From = new MailAddress("johansson.andersson@telia.com");
			//	mailMessage.IsBodyHtml = false;
			//	SmtpClient smtp = new SmtpClient("mailout.telia.com");
			//	smtp.Port = 587;
			//	smtp.UseDefaultCredentials = true;
			//	smtp.EnableSsl = false;
			//	smtp.Credentials = new System.Net.NetworkCredential("johansson.andersson@telia.com", "Pissen30060");
			//	smtp.Send(mailMessage);
			//}
			//catch (Exception e)
			//{
			//	string foo = e.Message;
			//}

			ViewBag.message = "The mail has Been sent To " + email.To + " Successfully";
			return View();
		}
	}
}
