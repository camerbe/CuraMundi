using CuraMundi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using CuraMundi.Application.BLL.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using CuraMundi.Dto;

namespace CuraMundi.Services
{
    public class EmailConfirmationService : IEmailConfirmationService
    {
        private readonly UserManager<User> _userManager;
        private readonly EmailService _emailService;
        private readonly IConfiguration _config;

        public EmailConfirmationService(UserManager<User> userManager, EmailService emailService, IConfiguration config)
        {
            _userManager = userManager;
            _emailService = emailService;
            _config = config;
        }

        public async Task<bool> SendConfirmedEmailAsync(User user, string baseUrl)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var issuer = baseUrl ?? _config["JWT:Issuer"];
            var url = $"{issuer}/api/Auth/email-confirm/?email={user.Email}&token={token}";

            var body = $"<p>Bonjour {user.Prenom} {user.Nom}</p>" +
                       "<p>Afin de pouvoir activer votre compte, nous devons valider votre adresse email. Cliquez simplement sur le lien :</p>" +
                       $"<p><a style='background-color: blue; color: white; padding: 10px 20px; border-radius: 8px; text-decoration: none; display: inline-block;' href='{url}'>Activer mon compte</a></p>" +
                       "<p>Bienvenue à bord !</p>";

            var emailSend = new EmailSendDto(user.Email, "Confirmation", body);
            return await _emailService.SendEmailAsync(emailSend);
        }
    }
}
