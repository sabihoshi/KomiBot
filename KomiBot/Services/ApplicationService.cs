using KomiBot.Models;

namespace KomiBot.Services
{
    public class ApplicationService
    {
        private readonly Application _application;

        public ApplicationService()
        {
            _application = ConfigService.GetJson<Application>();
        }

        public string Token => _application.Token;

        public ulong Owner => _application.Owner;
    }
}