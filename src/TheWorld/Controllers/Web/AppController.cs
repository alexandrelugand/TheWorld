using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Web
{
    public class AppController : Controller
    {
        private readonly IMailService _mailService;
        private readonly IWorldRepository _worldRepository;

        public AppController(IMailService mailService, IWorldRepository worldRepository)
        {
            _mailService = mailService;
            _worldRepository = worldRepository;
        }

        public IActionResult Index()
        {
            return View();
        }     

        [Authorize]
        public IActionResult Trips()
        {
            return View();
        }   

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel contactViewModel)
        {
            if(ModelState.IsValid)
            {
                var email = Startup.Configuration["AppSettings:SiteEmailAddress"];

                if (string.IsNullOrWhiteSpace(email))
                {
                    ModelState.AddModelError("", "Could not send email, configuration problem.");
                }

                if (_mailService.SendMail("", email,
                    $"Contact Page from {contactViewModel.Name} ({contactViewModel.Email})", contactViewModel.Message))
                {
                    ModelState.Clear();
                    ViewBag.Message = "Mail sent. Thanks!";
                }
            }
            return View();
        }


    }
}
