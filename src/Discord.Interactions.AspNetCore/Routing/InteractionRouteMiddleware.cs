using System.Linq;
using System.Threading.Tasks;
using Discord.Interactions.AspNetCore.Commands;
using Discord.Interactions.AspNetCore.External;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Optional.Unsafe;

namespace Discord.Interactions.AspNetCore.Routing
{
    internal class InteractionRouteMiddleware : IMiddleware
    {
        private readonly ILogger<InteractionRouteMiddleware> _logger;
        public InteractionRouteMiddleware(ILogger<InteractionRouteMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var interaction = await context.Request.ReadFromJsonAsync<Interaction>(SerializerOptions.InteractionsSerializer);

            if (interaction is null)
                return;

            if (interaction.Type == InteractionType.Ping)
            {
                await context.Response.WriteAsJsonAsync(new InteractionResponse(InteractionResponseType.Pong), SerializerOptions.InteractionsSerializer);
                return;
            }

            var interactionData = interaction.Data.ValueOrFailure("Interaction data is null");

            var handlers = context.RequestServices.GetServices<DiscordCommand>();
            //TODO replace with proper command routing - consider controller-pattern or a simpler design.
            var requiredHandler = handlers.SingleOrDefault(x => x.Name.Equals(interactionData.Name));

            if (requiredHandler is null)
            {
                _logger.LogWarning("No handler could be found for command {name}", interactionData.Name);
                return;
            }

            var res = await requiredHandler.HandleCommandAsync(interaction);

            await context.Response.WriteAsJsonAsync(res, SerializerOptions.InteractionsSerializer);
        }
    }
}
