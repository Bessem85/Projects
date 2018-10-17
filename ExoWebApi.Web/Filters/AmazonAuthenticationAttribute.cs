using ExoWebApi.Web.Helpers;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Results;


namespace VPwebapitest.Filters
{
    public class AmazonAuthenticationAttribute : Attribute, IAuthenticationFilter
    {

        public bool AllowMultiple
        {
            get
            {
                return false;
            }
        }

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var isValid = IsValidRequest(context.Request).Result;
            if (!isValid)
            {
                context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
            }

            return Task.FromResult(0);
        }


        private async Task<bool> IsValidRequest(HttpRequestMessage request)
        {
            if (request.Headers.Authorization == null ||
                !request.Headers.Authorization.Scheme.Equals("aws", StringComparison.OrdinalIgnoreCase) ||
                string.IsNullOrEmpty(request.Headers.Authorization.Parameter))
                return false;

            string tokenString = request.Headers.Authorization.Parameter;
            string[] authenticationParameters = tokenString.Split(new char[] { ':' });

            if (authenticationParameters.Length < 2)
                return false;


            //build string to sign
            var content = await GetContentBase64String(request.Content);
            var url = HttpUtility.UrlEncode(request.RequestUri.ToString().ToLowerInvariant());
            string stringToSign = $"{ConfigHelper.AccessKey}{request.Method.Method}{url}{content}";

            //Each AccessKey should have be configured with a unique shared key on
            var secretKeyBytes = Convert.FromBase64String(ConfigHelper.SecretKey);
            var authenticationTokenBytes = Encoding.UTF8.GetBytes(stringToSign);

            //Check fo data integrity and tampering
            using (HMACSHA512 hmac = new HMACSHA512(secretKeyBytes))
            {
                var hashedBytes = hmac.ComputeHash(authenticationTokenBytes);
                string tokenBase64 = Convert.ToBase64String(hashedBytes);

                if (!authenticationParameters[1].Equals(tokenBase64, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return true;
        }

        private async Task<string> GetContentBase64String(HttpContent content)
        {
            using (MD5 md5 = MD5.Create())
            {
                var bytes = await content.ReadAsByteArrayAsync();
                var md5Hash = md5.ComputeHash(bytes);
                return Convert.ToBase64String(md5Hash);
            }
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            context.Result = new ResultWithChallenge(context.Result);
            return Task.FromResult(0);
        }
    }

    public class ResultWithChallenge : IHttpActionResult
    {
        private readonly IHttpActionResult next;

        public ResultWithChallenge(IHttpActionResult next)
        {
            this.next = next;
        }

        public async Task<HttpResponseMessage>
           ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = await next.ExecuteAsync(cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("aws"));
            }

            return response;

        }
    }

}

