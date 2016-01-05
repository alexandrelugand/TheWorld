using System.Diagnostics;
using TheWorld.Services;

namespace TheWorld.Tests.Fakes
{
    public class FakeMailService : IMailService
    {
        public bool SendMail(string to, string from, string subject, string body)
        {
            Debug.WriteLine($"Sending Mail: To: {to}, Subject: {subject}, Body: {body}");
            return true;
        }
    }
}
