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

		//Call View to display the Form
		public IActionResult Index()
		{
			return View();
		}

		//When the form is filled in we post it to this method wher se send it by using SendGrid
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
			ViewBag.message = "The mail has Been sent To " + email.To + " Successfully";
			return View();
		}
	}
}
