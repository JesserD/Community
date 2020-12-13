using Community.Data;
using Community.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Community.Models
{
    public class CommunityLogic
    {
        private readonly IdentityContext Context;

        public CommunityLogic(IdentityContext context)
        {
            this.Context = context;
        }
        public IEnumerable<LoginEvent> GetUserLoginEvents(string UserId)
        {
            var loginEvents = this.Context.LoginEvents.Where(l => l.UserId == UserId).
                OrderByDescending(l => l.LoginDate).ToList();

            return loginEvents;
        }
        public DateTime GetLastLogin(string UserId)
        {
            List<DateTime> loginEvents = this.Context.LoginEvents.Where(l => l.UserId == UserId).
                OrderByDescending(l => l.LoginDate).Select(l => l.LoginDate).Take(2).ToList();
            if (loginEvents.Count == 1)
                return loginEvents[0];
            else
                return loginEvents[1];
        }
        public int GetNumOfLoginsLastMonth(string UserId)
        {
            List<DateTime> loginEvents = this.Context.LoginEvents.Where(l => l.UserId == UserId).
                OrderByDescending(l => l.LoginDate).Select(l => l.LoginDate).Take(30).ToList();
            if (loginEvents == null)
                return 0;
            else
                return loginEvents.Count;
        }
        public int GetNumOfUnseenMessages(string Recipient)
        {
            List<Message> UnreadMessages = this.Context.Messages.
                Where(m => m.Recipient == Recipient && m.Seen == false).ToList();
            if (UnreadMessages == null)
                return 0;
            else
                return UnreadMessages.Count;
        }
        public int GetNumOfMessages(string Recipient)
        {
            List<Message> Messages = this.Context.Messages.
                Where(m => m.Recipient == Recipient).ToList();
            if (Messages == null)
                return 0;
            else
                return Messages.Count;
        }
        public int GetNumOfDeletedMessages(string Recipient)
        {
            List<Message> Messages = this.Context.Messages.
                Where(m => m.Recipient == Recipient && m.Deleted == true).ToList();
            if (Messages == null)
                return 0;
            else
                return Messages.Count;
        }
        public List<string> GetAllEmailsOfUsers()
        {
            List<string> Emails = this.Context.Users.Select(u => u.Email).ToList();

            return Emails;
        }
        public Message InsertNewMessage(HomeSendMessageViewModel NewMessage)
        {
            Message message = new Message(Guid.NewGuid().ToString(), NewMessage.Subject, NewMessage.Content,
                DateTime.Now, NewMessage.Sender, NewMessage.Recipient, false, false);
            this.Context.Messages.Add(message);
            this.Context.SaveChanges();
            return message;
        }
        public IEnumerable<Message> GetAllMessagesOfSender(string Recipient, string Sender)
        {
            return this.Context.Messages.
                Where(m => m.Recipient == Recipient && m.Sender == Sender && m.Deleted == false).ToList();
        }
        public IEnumerable<String> GetAllIdsOfsenders(string RecipientId)
        {
             var SendersIds = this.Context.Messages.Where(m => m.Recipient == RecipientId && m.Deleted == false).
                Select(m => m.Sender).Distinct().ToList();

            return SendersIds;
        }
        public Message GetMessage(string Id)
        {
            return this.Context.Messages.Where(m => m.Id == Id).Select(m => m).FirstOrDefault();
        }
        public void HasBeenSeen(string MessageId)
        {
            Message message = this.Context.Messages.Where(m => m.Id == MessageId).Select(m => m).FirstOrDefault();
            message.Seen = true;
            this.Context.SaveChanges();
        }
        public void HasBeenDeleted(string MessageId)
        {
            Message message = this.Context.Messages.Where(m => m.Id == MessageId).Select(m => m).FirstOrDefault();
            message.Deleted = true;
            this.Context.SaveChanges();
        }

    }
}
