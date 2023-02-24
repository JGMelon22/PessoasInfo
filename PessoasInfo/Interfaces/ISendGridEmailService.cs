namespace PessoasInfo.Interfaces;

public interface ISendGridEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string message);
}