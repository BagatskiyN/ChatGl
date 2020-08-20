using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ChatGl.Data;
using ChatGl.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ChatGl.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        // на странице увидим протокол чата и поле ввода
        public List<Message> Messages { set; get; }
        [BindProperty]
        [Required]
        public string Text { set; get; }

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            Messages = _db.Messages.ToList();
        }
        public IActionResult OnPostAjax()
        {
            if (ModelState.IsValid)
            {
                _db.Messages.Add(new Message
                {
                    Text = Text,
                    When = DateTime.Now,
                    Sign = User.Identity.Name
                });
                _db.SaveChanges();
                return new JsonResult(new { userName = User.Identity.Name });     // 200
            }
            var errMes = ModelState["text"]?.Errors[0]?.ErrorMessage ?? "Unknown error";
            return BadRequest(errMes);     // 400
        }
        public void OnPost()
        {
            if (ModelState.IsValid)
            {
                _db.Messages.Add(new Message
                {
                    Text = Text,
                    When = DateTime.Now,
                    Sign = User.Identity.Name
                });
                _db.SaveChanges();
            }
            Messages = _db.Messages.ToList();
        }
    }
}
