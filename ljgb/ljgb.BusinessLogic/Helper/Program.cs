using System.Collections.Generic;
using System.IO;
using ljgb.BusinessLogic.Helper;
using Microsoft.Extensions.Configuration;

namespace ljgb.BusinessLogic
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            EmailConfiguration emailConfiguration = new EmailConfiguration()
            {
                SmtpServer = "smtp.gmail.com",
                SmtpPort = 587,
                SmtpUsername = "admin@lojualguebeli.com",
                SmtpPassword = "Lojualguebeli.com"
            }; //configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();

            ExampleEmail exampleEmail = new ExampleEmail(emailConfiguration);

            EmailAddress emailAddress = new EmailAddress();
            emailAddress.Address = "kampus.victor@gmail.com";
            emailAddress.Name = "Victor Boy";
            List<EmailAddress> listEmailAddress = new List<EmailAddress>();
            listEmailAddress.Add(emailAddress);

            EmailAddress emailAddressFrom = new EmailAddress();
            emailAddressFrom.Address = "admin@lojualguebeli.com";
            emailAddressFrom.Name = "Admin";
            List<EmailAddress> listEmailAddressFrom = new List<EmailAddress>();
            listEmailAddressFrom.Add(emailAddressFrom);

            EmailMessage emailMessage = new EmailMessage();
            emailMessage.ToAddresses = listEmailAddress;
            emailMessage.Subject = "Test";
            emailMessage.FromAddresses = listEmailAddressFrom;
            emailMessage.Content = "Test kirim email nehhhhh";
            exampleEmail.Send(emailMessage);
            
        }
    }
}
