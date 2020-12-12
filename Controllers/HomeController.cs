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
            DateTime lastLogin = _communityLogic.GetLastLogin(_userManager.GetUserId(User));
            int NumOfLoginsLastMonth = _communityLogic.GetNumOfLoginsLastMonth(_userManager.GetUserId(User));
            int NumOfUnseenMessages = _communityLogic.GetNumOfUnseenMessages(User.Identity.Name);
            HomeIndexViewModel vm = new HomeIndexViewModel(lastLogin, NumOfLoginsLastMonth, NumOfUnseenMessages);
            return View(vm);
        }

        public IActionResult Compose(HomeComposeViewModel Model)
        {
            ViewBag.AllEmailsOfUsers = _communityLogic.GetAllEmailsOfUsers();
            if (Model.Id == null)
                ViewBag.HomeComposeViewModel = null;
            else
                ViewBag.HomeComposeViewModel = Model;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage(HomeSendMessageViewModel NewMessage)
        {
            NewMessage.Sender = _userManager.GetUserId(User);
            IdentityUser RecipientUser = await _userManager.FindByNameAsync(NewMessage.Recipient);
            string Recipient = NewMessage.Recipient;
            NewMessage.Recipient = RecipientUser.Id;
            Message InsertedMessage = _communityLogic.InsertNewMessage(NewMessage);
            HomeComposeViewModel vm = new HomeComposeViewModel(
                InsertedMessage.Id, Recipient, InsertedMessage.SendingDate);
            return RedirectToAction("Compose", "Home", vm);
        }

        public IActionResult Inbox()
        {
            List<string> Senders = (List<string>)_communityLogic.GetAllEmailsOfsenders(User.Identity.Name);
            int NumOfMessages = _communityLogic.GetNumOfMessages(User.Identity.Name);
            int NumOfUnseenMessages = _communityLogic.GetNumOfUnseenMessages(User.Identity.Name);
            int NumOfDeletedMessages = _communityLogic.GetNumOfDeletedMessages(User.Identity.Name);
            HomeInboxViweModel vm = new HomeInboxViweModel(Senders, NumOfMessages, NumOfUnseenMessages, NumOfDeletedMessages);

            return View(vm);
        }
        public IActionResult RecivedMessages(Dictionary<string, string> Parameters)
        {
            if (Parameters.ContainsKey("Deleted"))
                _communityLogic.HasBeenDeleted(Parameters["MessageId"]);
            var vm = new List<HomeRecivedMessagesViewModel>();
            IEnumerable<Message> Messages = _communityLogic.GetAllMessagesOfSender(User.Identity.Name, Parameters["Sender"]);
            if (Messages.Count() == 0)
                return RedirectToAction("Index", "Home");
            foreach (Message m in Messages)
                vm.Add(new HomeRecivedMessagesViewModel(m.Id, m.Sender, m.Subject, m.SendingDate.ToString(), m.Seen, m.Deleted));
            return View(vm);
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
