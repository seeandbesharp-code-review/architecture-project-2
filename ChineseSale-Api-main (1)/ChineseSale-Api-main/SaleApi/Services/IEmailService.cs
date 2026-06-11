
public interface IEmailService
{
    Task SendWinnerEmailAsync(string targetEmail, string userName, string giftName);
}