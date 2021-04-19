using Discord.Interactions.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Discord.Interactions.AspNetCore.Routing
{
    public static class EndpointBuilderExtensions
    {
        public static IEndpointConventionBuilder MapDiscordInteractions(this IEndpointRouteBuilder endpoints, string path = "/discordinteractions")
        {
            var pipeline = endpoints.CreateApplicationBuilder()
                .UseMiddleware<InteractionRouteMiddleware>()
                .Build();

            return endpoints.MapPost(path, pipeline)
                .RequireAuthorization(SignatureAuthorizationDefaults.PolicyName);
        }
    }
}
