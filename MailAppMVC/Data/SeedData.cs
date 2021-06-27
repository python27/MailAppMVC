using MailAppMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MailAppMVC.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MailDbContext(serviceProvider.GetRequiredService<DbContextOptions<MailDbContext>>()))
            {
                if (context.Mail.Any()) { return; }

                List<Mail> mails = new List<Mail>()
                {
                    //new Mail(){ FirstName = "Janusz", LastName = "Kowalski", Email = "123@gmail.com", Subject = "Temat 1", Message = "Hej" },
                };

                context.Mail.AddRange(mails);
                context.SaveChanges();
            }
        }
    }
}
