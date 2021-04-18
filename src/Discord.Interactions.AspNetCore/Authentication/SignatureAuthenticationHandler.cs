using System;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using NSec.Cryptography;

namespace Discord.Interactions.AspNetCore.Authentication
{
    public class SignatureAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string PublicKey { get; set; } = null!;
    }
    public class SignatureAuthenticationHandler : AuthenticationHandler<SignatureAuthenticationOptions>
    {
        private const string SignatureHeader = "X-Signature-Ed25519";
        private const string TimestampHeader = "X-Signature-Timestamp";
        public SignatureAuthenticationHandler(IOptionsMonitor<SignatureAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) 
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(SignatureHeader, out var signature) ||
                !Request.Headers.TryGetValue(TimestampHeader, out var timestamp))
            {
                return AuthenticateResult.NoResult();
            }

            var body = await ReadRequestBodyAsync();

            if (ValidateSignature(signature, timestamp, body))
            {
                var identity = new ClaimsIdentity(Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }

            return AuthenticateResult.Fail("Invalid Signature");

        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 401;
            return Task.CompletedTask;
        }
        
        private async Task<string> ReadRequestBodyAsync()
        {
            Request.EnableBuffering();
            using StreamReader reader = new(Request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            Request.Body.Position = 0;
            return body;
        }

        private bool ValidateSignature(StringValues signature, StringValues timestamp, string body)
        {
            // Signature
            var sigBytes = Convert.FromHexString(signature);
            // Body
            var data = Encoding.UTF8.GetBytes($"{timestamp}{body}");

            // Public key 
            var byteKey = Convert.FromHexString(Options.PublicKey);
            var algorithm = SignatureAlgorithm.Ed25519;

            var key = PublicKey.Import(algorithm, byteKey, KeyBlobFormat.RawPublicKey);

            return algorithm.Verify(key, data, sigBytes);
        }
    }
}
