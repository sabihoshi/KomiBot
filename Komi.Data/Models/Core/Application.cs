namespace Komi.Data.Models.Core
{
    public class Application
    {
        public Application(
            ulong owner,
            string token,
            string connectionString)
        {
            Owner = owner;
            Token = token;
            ConnectionString = connectionString;
        }

        public ulong Owner { get; }

        public string Token { get; }

        public string ConnectionString { get; }
    }
}