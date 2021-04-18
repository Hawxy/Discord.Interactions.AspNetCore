using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Discord.Interactions.AspNetCore.External.ApplicationCommand;
using Discord.Interactions.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Discord.Interactions.AspNetCore.Commands
{
    public class CommandRegistrationService : IHostedService
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordInteractionsClient _client;
        private readonly ILogger<CommandRegistrationService> _logger;
        private readonly IOptions<DiscordHttpConfig> _config;

        public CommandRegistrationService(IServiceProvider provider, DiscordInteractionsClient client,  ILogger<CommandRegistrationService> logger, IOptions<DiscordHttpConfig> config)
        {
            _provider = provider;
            _client = client;
            _logger = logger;
            _config = config;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _provider.CreateScope();
            var commands = scope.ServiceProvider.GetServices<DiscordCommand>();

            foreach (DiscordCommand discordCommand in commands)
            {
                
                var create = new CreateGuildCommand(discordCommand.Name!, discordCommand.Description!,
                    new List<ApplicationCommandOption>()
                    {
                        new()
                        {
                            Name = discordCommand.Name,
                            Choices = discordCommand.Choices,
                            Type = discordCommand.Type,
                            Description = discordCommand.Description
                        }
                    }, true);

                await _client.CreateGuildCommand(
                    _config.Value.ApplicationId,
                    379588857286492170,
                    create
                );

                _logger.LogInformation("Registers command {name}", discordCommand.Name);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
            // Maybe optional deletion?
        }
    }
}
