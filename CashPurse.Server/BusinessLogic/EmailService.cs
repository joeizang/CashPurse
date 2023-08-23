using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;

namespace CashPurse.Server.BusinessLogic;

public interface IAppEmailSender : IEmailSender
{
    void SetUpEmailService(string apiKey, string to, string message);
}

public class EmailService : IAppEmailSender
{
    public EmailAddress _from = default!;
    public EmailAddress _to = default!;
    private string _apiKey = string.Empty;

    public void SetUpEmailService(string apiKey, string to, string message)
    {
        _apiKey = string.IsNullOrEmpty(apiKey)
            ? throw new ArgumentException("apiKey cannot be null!")
            : apiKey.Trim();
        _from = new EmailAddress("josephizang@prodigeenet.com", "ProdigeeNet Dev Team");
        _to = new EmailAddress(to, to);
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var mailMsg = MailHelper.CreateSingleEmail(_from, _to, subject,
            htmlMessage, htmlMessage);
        var client = new SendGridClient(_apiKey);
        var response = await client.SendEmailAsync(mailMsg).ConfigureAwait(false);
    }
}
