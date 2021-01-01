using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Sunday.ContentDelivery.Framework.Extensions;
using Sunday.Core;
using Sunday.Core.Application;
using Sunday.Core.Extensions;

namespace Sunday.Portfolio.Project.Controllers
{
    public class ContactController : ControllerBase
    {
        private readonly IMailService _mailService;

        public ContactController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost]
        public IActionResult OnSubmit([FromForm] ContactInformation contactInfo)
        {
            if (!contactInfo.IsValid()) return BadRequest();
            var website = HttpContext.CurrentWebsite();
            if (website == null) return Ok();
            var template = "Subject: {0}. <br /> Message: {1}";
            var receiver = website.Properties.Get("EmailReceiver");
            if (receiver.IsNone) return Ok();
            _mailService.SendEmail($"[Sunday] {contactInfo.Name} contacted you", template, new List<string>()
            {
                receiver.Get()
            }, contactInfo.Subject, contactInfo.Message);
            return Ok();
        }
    }

    public class ContactInformation
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public bool IsValid() =>
            !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Subject);
    }
}
