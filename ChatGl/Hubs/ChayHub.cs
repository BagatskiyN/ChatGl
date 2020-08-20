using ChatGl.Data;
using ChatGl.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatGl.Hubs
{
    public class ChatHub : Hub
    {
        readonly ApplicationDbContext _db;

        public ChatHub(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task SendToAll(string text)
        {
            var message = new Message { Text = text, Sign = Context.User.Identity.Name, When = DateTime.Now };

            // save the message in DB
            _db.Messages.Add(message);
            _db.SaveChanges();

            // send message to all
            var dataToSend = new { message.Text, message.Sign, When = $"{message.When:HH%:mm}" };
            await Clients.All.SendAsync("ReceiveMessage", dataToSend);
        }
    }
}
