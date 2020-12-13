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
            int NumOfUnseenMessages = _communityLogic.GetNumOfUnseenMessages(_userManager.GetUserId(User));
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

        public async Task<IActionResult> Inbox()
        {
            List<string> SendersId = (List<string>)_communityLogic.GetAllIdsOfsenders(_userManager.GetUserId(User));
            List<string> SendersEmails = new List<string>();
            foreach (string id in SendersId)
            {
                IdentityUser SendertUser = await _userManager.FindByIdAsync(id);
                SendersEmails.Add(SendertUser.Email);
            }
            int NumOfMessages = _communityLogic.GetNumOfMessages(_userManager.GetUserId(User));
            int NumOfUnseenMessages = _communityLogic.GetNumOfUnseenMessages(_userManager.GetUserId(User));
            int NumOfDeletedMessages = _communityLogic.GetNumOfDeletedMessages(_userManager.GetUserId(User));
            HomeInboxViweModel vm = new HomeInboxViweModel(SendersEmails, SendersId, NumOfMessages, NumOfUnseenMessages, NumOfDeletedMessages);

            return View(vm);
        }
        public async Task<IActionResult> RecivedMessages(Dictionary<string, string> Parameters)
        {
            if (Parameters.ContainsKey("Deleted"))
                _communityLogic.HasBeenDeleted(Parameters["MessageId"]);
            var vm = new List<HomeRecivedMessagesViewModel>();
            IEnumerable<Message> Messages = _communityLogic.GetAllMessagesOfSender(_userManager.GetUserId(User), Parameters["SenderId"]);
            if (Messages.Count() == 0)
                return RedirectToAction("Index", "Home");
            foreach (Message m in Messages)
            {
                IdentityUser SendertUser = await _userManager.FindByIdAsync(m.Sender);
                vm.Add(new HomeRecivedMessagesViewModel(m.Id, m.Sender, SendertUser.Email, m.Subject, m.SendingDate.ToString(), m.Seen, m.Deleted));
            }
                
            return View(vm);
        }
        public async Task<IActionResult> ViewMessage(string MessageId)
        {
            Message message = _communityLogic.GetMessage(MessageId);
            if (!message.Recipient.Equals(_userManager.GetUserId(User)))
                return View(null);
            _communityLogic.HasBeenSeen(MessageId);
            IdentityUser SendertUser = await _userManager.FindByIdAsync(message.Sender);
            HomeViewMessageViewModel Model = new HomeViewMessageViewModel (
                message.Id, message.Content, message.Subject, message.Sender, SendertUser.Email, message.SendingDate);
            return View(Model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
