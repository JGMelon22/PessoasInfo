using Microsoft.Extensions.Options;
using PessoasInfo.Helpers;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PessoasInfo.Services;

public class SendGridEmailService : ISendGridEmailService
{
    private readonly ILogger<SendGridEmailService> _logger;

    public SendGridEmailService(IOptions<AuthMessageSenderOptions> options, ILogger<SendGridEmailService> logger)
    {
        Options = options.Value; // gets api key value
        _logger = logger;
    }

    public AuthMessageSenderOptions Options { get; set; } // Type Check on service


    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        if (string.IsNullOrEmpty(Options.ApiKey))
            throw new Exception("Null SendGridKey");

        await Execute(Options.ApiKey, subject, message, toEmail);
    }

    // Boiler plate da documentação do SendGrid
    private async Task Execute(string apiKey, string subject, string message, string toEmail)
    {
        var client = new SendGridClient(apiKey);
        var msg = new SendGridMessage
        {
            From = new EmailAddress("sendgrid@email.com"), // Nome/e-mail da sua conta SendGrid
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };
        msg.AddTo(new EmailAddress(toEmail));

        // Disable click tracking.
        // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
        msg.SetClickTracking(false, false);
        var response = await client.SendEmailAsync(msg);
        var dummy = response.StatusCode;
        var dummy2 = response.Headers;
        _logger.LogInformation(response.IsSuccessStatusCode
            ? $"Email to {toEmail} queued successfully!"
            : $"Failure Email to {toEmail}");
    }
}