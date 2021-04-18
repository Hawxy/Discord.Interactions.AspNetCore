using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Discord.Interactions.AspNetCore.External.ApplicationCommand;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Discord.Interactions.AspNetCore.Http
{

    public class DiscordHttpConfig
    {
        public string? Token { get; set; }
        public ulong ApplicationId { get; set; }
    }

    public class DiscordInteractionsClient : IDiscordInteractionsClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<DiscordInteractionsClient> _interactionClient;

        public DiscordInteractionsClient(HttpClient client, IOptions<DiscordHttpConfig> config, ILogger<DiscordInteractionsClient> interactionClient)
        {
            _client = client;
            _interactionClient = interactionClient;
            _client.BaseAddress = new Uri("https://discord.com/api/v8/");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bot", config.Value.Token);
        }

        public async Task CreateGuildCommand(ulong applicationId, ulong guildId, CreateGuildCommand createGuildCommand)
        {
            var payload = new ApplicationCommand
            {
                ApplicationId = applicationId,
                Name = createGuildCommand.Name,
                Description = createGuildCommand.Description,
                Options = createGuildCommand.Options,
            };

            var result = await _client.PostAsJsonAsync($"applications/{applicationId}/guilds/{guildId}/commands", payload, SerializerOptions.InteractionsSerializer);
            if (!result.IsSuccessStatusCode)
            {
                _interactionClient.LogInformation("{payload}", await result.Content.ReadAsStringAsync());
            }
        }
    }

  

    public record CreateGuildCommand(string Name, string Description, List<ApplicationCommandOption> Options,
        bool? DefaultPermission);
}
