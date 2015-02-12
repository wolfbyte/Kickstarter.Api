using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Kickstarter.Api.Model;

namespace Kickstarter.Api
{
    internal class KickStarterSession : IKickstarterSession
    {
        private const string Root = "https://api.kickstarter.com/";
        private string _accessToken;

        public async Task<TResult> Get<TResult>(string path)
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetStringAsync(GetUrl(path)).ConfigureAwait(false);
                return await result.ParsedAsJson<TResult>().ConfigureAwait(false);
            }
        }

        public async Task<TResult> Post<TResult>(string path, object parameters)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(parameters.ToJson());
                var result = await client.PostAsync(GetUrl(path), content).ConfigureAwait(false);
                var resultText = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                return await resultText.ParsedAsJson<TResult>().ConfigureAwait(false);
            }
        }

        private string GetUrl(string path)
        {
            var builder = new StringBuilder();
            if (!path.StartsWith("http"))
                builder.Append(Root);
            builder.Append(path);
            if (!String.IsNullOrWhiteSpace(_accessToken))
            {
                builder.Append(path.Contains("?") ? "&" : "?");
                builder.AppendFormat("oauth_token={0}", _accessToken);
            }

            return builder.ToString();
        }

        public User User { get; private set; }

        internal async Task<bool> Logon(string email, string password, string clientId)
        {
            var logonResult = await Post<LogonResult>(
                String.Format("xauth/access_token?client_id={0}", clientId),
                new {email, password}).ConfigureAwait(false);

            _accessToken = logonResult.AccessToken;
            User = logonResult.User;

            return true;
        }
    }
}