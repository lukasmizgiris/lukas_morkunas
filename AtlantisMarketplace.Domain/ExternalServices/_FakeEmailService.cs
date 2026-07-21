using System;
using System.Threading.Tasks;

namespace AtlantisMarketplace.Domain.ExternalServices;

/// <summary>
/// This class does not need to be changed for the purposes of the task - though you may use it.
/// </summary>
public class FakeEmailService
{
    public async Task SendEmail(string to, string subject, string message)
    {
        var email = "To: " + to + "Subject: " + subject + "Message: " + message;

        await Task.Delay(1000);
        Console.WriteLine(email);
    }
}