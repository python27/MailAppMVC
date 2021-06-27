using MailAppMVC.Data;
using MailAppMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace MailAppMVC.Controllers
{
    public class MailController : Controller
    {
        private readonly MailDbContext _mailDbContext;
        public MailController(MailDbContext context)
        {
            _mailDbContext = context;
        }
        public IActionResult Index()
        {
            List<Mail> mails = new List<Mail>();
            try
            {
                mails = _mailDbContext.Mail.ToList();
            }
            catch (Exception)
            {

            }

            return View(mails);
        }

        public IActionResult Create()
        {
            var model = new Mail();

            return View(model);
        }

        [HttpPost]
        public IActionResult Create([Bind("FirstName,LastName,Email,Subject,Message")] Mail mail)
        {
            try
            {
                var mailFrom = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["MailFrom"];
                var mailTo = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["MailTo"];
                var smtpServer = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["SmtpServer"];
                var password = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["Password"];
                var port = Int32.Parse(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["Port"]);

                if (mailFrom == "" || mailTo == "" || smtpServer == "" || password == "")
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    MailMessage email = new MailMessage();
                    email.From = new MailAddress(mailFrom);
                    email.To.Add(mailTo);
                    email.Subject = mail.Subject;
                    email.CC.Add(mail.Email);
                    email.IsBodyHtml = false;
                    email.Body = mail.Message;

                    SmtpClient smtpClient = new SmtpClient(smtpServer);

                    NetworkCredential networkCredential = new NetworkCredential(mailFrom, password);
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = networkCredential;
                    smtpClient.Port = port;
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(email);

                    ViewBag.Message = "E-mail został wysłany";

                    ModelState.Clear();

                    // save data to database
                    _mailDbContext.Add(mail);
                    _mailDbContext.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Wystąpił problem. Spróbuj ponownie za jakiś czas.");
            }

            return View(mail);
        }

        public IActionResult Edit(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return NotFound();
                }
                Mail mail = _mailDbContext.Mail.FirstOrDefault(x => x.Id == id);
                if (mail == null)
                {
                    return NotFound();
                }
                return View(mail);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,FirstName,LastName,Email,Subject,Message")] Mail mail)
        {
            try
            {
                if (id != mail.Id)
                    return NotFound();

                if (ModelState.IsValid)
                {
                    _mailDbContext.Update(mail);
                    _mailDbContext.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Wystąpił problem. Spróbuj ponownie za jakiś czas.");
            }

            return View(mail);
        }


        public IActionResult Details(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return NotFound();
                }
                Mail mail = _mailDbContext.Mail.FirstOrDefault(x => x.Id == id);
                if (mail == null)
                {
                    return NotFound();
                }
                return View(mail);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return NotFound();
                }
                Mail mail = _mailDbContext.Mail.FirstOrDefault(x => x.Id == id);
                if (mail == null)
                {
                    return NotFound();
                }
                return View(mail);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return NotFound();
                }
                Mail mail = new Mail { Id = id };
                _mailDbContext.Entry(mail).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                _mailDbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
