namespace KomiBot.Services.Core
{
    public class ApplicationService
    {
        private readonly Application _application;

        public ApplicationService() => _application = ConfigService.GetJson<Application>();

        public string Token => _application.Token;

        public ulong Owner => _application.Owner;

        public string ConnectionString => _application.ConnectionString;
    }
}