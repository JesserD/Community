using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Community.Models;
using Microsoft.AspNetCore.Identity;
using Community.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Community.ViewModels;
using System.Collections;

namespace Community.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IdentityContext _identityContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly CommunityLogic _communityLogic;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager,
            IdentityContext identityContext)
        {
            _userManager = userManager;
            _logger = logger;
            _identityContext = identityContext;
            _communityLogic = new CommunityLogic(identityContext);
        }

        public IActionResult Index()
        {
            ViewBag.LastLogin = _communityLogic.GetLastLogin(_userManager.GetUserId(User));
            ViewBag.NumOfLoginsLastMonth = _communityLogic.GetNumOfLoginsLastMonth(_userManager.GetUserId(User));
            return View();
        }

        public IActionResult Compose(HomeComposeViewModel Model)
        {
            ViewBag.AllEmailsOfUsers = _communityLogic.GetAllEmailsOfUsers();
            if (Model.ToString().Equals("Id "))
                ViewBag.HomeComposeViewModel = null;
            else
                ViewBag.HomeComposeViewModel = Model;
            //Console.WriteLine("HomeComposeViewModel test " + Model.ToString());
            return View();
        }
        [HttpPost]
        public IActionResult SendMessage(HomeSendMessageViewModel NewMessage)
        {
            NewMessage.Sender = User.Identity.Name;
            //Console.WriteLine("ModelState.IsValid: " + ModelState.IsValid);
            Message InsertedMessage = _communityLogic.InsertNewMessage(NewMessage);
            HomeComposeViewModel _homeComposeViewModel = new HomeComposeViewModel(
                InsertedMessage.Id, InsertedMessage.Recipient, InsertedMessage.SendingDate);
            return RedirectToAction("Compose", "Home", _homeComposeViewModel);
        }

        public IActionResult Inbox()
        {
            ViewBag.Senders = _communityLogic.GetAllEmailsOfsenders(User.Identity.Name);
            ViewBag.NumOfMessages = _communityLogic.GetNumOfMessages(User.Identity.Name);
            ViewBag.NumOfUnseenMessages = _communityLogic.GetNumOfUnseenMessages(User.Identity.Name);
            ViewBag.NumOfDeletedMessages = _communityLogic.GetNumOfDeletedMessages(User.Identity.Name);
            return View();
        }
        public IActionResult RecivedMessages(Dictionary<string, string> Parameters)
        {
            if (Parameters.ContainsKey("Deleted"))
                _communityLogic.HasBeenDeleted(Parameters["MessageId"]);
            var Model = new List<HomeRecivedMessagesViewModel>();
            IEnumerable<Message> Messages = _communityLogic.GetAllMessagesOfSender(User.Identity.Name, Parameters["Sender"]);
            if (Messages.Count() == 0)
                return RedirectToAction("Index", "Home");
            foreach (Message m in Messages)
                Model.Add(new HomeRecivedMessagesViewModel(m.Id, m.Sender, m.Subject, m.SendingDate.ToString(), m.Seen, m.Deleted));
            return View(Model);
        }
        public IActionResult ViewMessage(string MessageId)
        {
            _communityLogic.HasBeenSeen(MessageId);
            Message message = _communityLogic.GetMessage(MessageId);
            HomeViewMessageViewModel Model = new HomeViewMessageViewModel (
                message.Id, message.Content, message.Subject, message.Sender, message.SendingDate);
            return View(Model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
