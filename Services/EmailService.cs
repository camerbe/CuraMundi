using CuraMundi.Dto;
using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;

namespace CuraMundi.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public async Task<bool> SendEmailAsync(EmailSendDto emailSendDto)
        {
            MailjetClient client = new MailjetClient(_config["MailJet:ApiKey"], _config["MailJet:SecretKey"]);
            var email= new TransactionalEmailBuilder()
                .WithFrom(new SendContact(_config["Email:From"], _config["Email:applicationName"]))
                .WithSubject(emailSendDto.Subject)
                .WithHtmlPart(emailSendDto.Body)
                .WithTo(new SendContact(emailSendDto.To))
                .Build();
            var response = await client.SendTransactionalEmailAsync(email);
            if (response.Messages is not null)
            {
                if(response.Messages[0].Status == "success")
                {
                    return true;
                }
               
            }
            return false;
        }
    }
}
