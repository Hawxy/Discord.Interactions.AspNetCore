using System;
using Discord.Interactions.AspNetCore.Authentication;
using Discord.Interactions.AspNetCore.Commands;
using Discord.Interactions.AspNetCore.Domain;
using Discord.Interactions.AspNetCore.Http;
using Discord.Interactions.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Discord.Interactions.AspNetCore
{
    public static class ServiceCollectionExtensions
    {
        public static InteractionsBuilder AddDiscordInteractions(this IServiceCollection services, Action<InteractionsHostConfiguration> config)
        {
            var hostConfig = new InteractionsHostConfiguration();
            config.Invoke(hostConfig);

            services.AddOptions<DiscordHttpConfig>().Configure(x =>
            {
                x.ApplicationId = hostConfig.ApplicationId;
                x.Token = hostConfig.Token;
            });

            services.AddHttpClient<DiscordInteractionsClient>();

            services.AddAuthentication().AddSlashSignature(x=> x.PublicKey = hostConfig.PublicKey);
            services.AddAuthorization(options =>
            {
                options.AddPolicy(SignatureAuthorizationDefaults.PolicyName, p =>
                    p.AddAuthenticationSchemes(SignatureAuthenticationDefaults.AuthenticationScheme)
                        .RequireAuthenticatedUser());
            });

            services.AddTransient<InteractionRouteMiddleware>();

            services.AddHostedService<CommandRegistrationService>();

            return new InteractionsBuilder(services);
        }
    }
    
    public class InteractionsBuilder : IInteractionsBuilder
    {
        public IServiceCollection Services { get; }
        public InteractionsBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }

    public interface IInteractionsBuilder
    {
        IServiceCollection Services { get; }

    }

    public static class InteractionsBuilderExtensions
    {
        public static IInteractionsBuilder AddSlashCommand<T>(this IInteractionsBuilder builder) where T : DiscordCommand
        {
            builder.Services.TryAddScoped<DiscordCommand, T>();
            return builder;
        }
    }
}
