using System.Threading.Tasks;

namespace Kickstarter.Api
{
    public class KickstarterClient
    {
        private const string DEFAULT_CLIENT_ID = "2II5GGBZLOOZAA5XBU1U0Y44BU57Q58L8KOGM7H0E0YFHP3KTG";
        private readonly string _clientId;

        public KickstarterClient(string clientId = null)
        {
            _clientId = clientId ?? DEFAULT_CLIENT_ID;
        }

        public async Task<IKickstarterSession> StartSession(string email, string password)
        {
            var session = new KickStarterSession();

            var loggedOn = await session.Logon(email, password, _clientId).ConfigureAwait(false);

            // TODO: Report successful logon

            return session;
        }
    }
}