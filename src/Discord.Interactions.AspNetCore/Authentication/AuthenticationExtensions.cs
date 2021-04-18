using System;
using Microsoft.AspNetCore.Authentication;

namespace Discord.Interactions.AspNetCore.Authentication
{
    public static class SignatureAuthenticationExtensions
    {
        public static AuthenticationBuilder AddSlashSignature(this AuthenticationBuilder builder, Action<SignatureAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<SignatureAuthenticationOptions, SignatureAuthenticationHandler>(SignatureAuthenticationDefaults.AuthenticationScheme, configureOptions);
        }
    }
}
