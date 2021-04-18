namespace Discord.Interactions.AspNetCore.Authentication
{
    public static class SignatureAuthenticationDefaults
    {
        public const string AuthenticationScheme = "Interaction-Signature";
    }

    public static class SignatureAuthorizationDefaults
    {
        public const string PolicyName = "discord-interactions-policy";
    }
}
