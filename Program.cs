using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Linq;

namespace Blanks_Discord_Bot_Template
{
    class Program
    {
        static void Main(string[] args)
        {
            new DiscordBot().RunBotAsync();
        }
    }
    public class DiscordBot
    {
        public static DiscordSocketClient client;
        private CommandService _commands;
        private IServiceProvider _service;
        public async Task RunBotAsync()
        {
            client = new DiscordSocketClient();
            _commands = new CommandService();
            _service = new ServiceCollection()
        .AddSingleton(client)
        .AddSingleton(_commands)
        .BuildServiceProvider();
            string token = "Insert Token Here";
            await client.LoginAsync(TokenType.Bot, token);
            await RegisterCom();
            await client.StartAsync();
            client.Log += Logging;
            client.Ready += Ready;
            await Task.Delay(-1); //Holds the Task Open
        }
        private async Task Ready()
        {
        }
        private Task Logging(LogMessage arg)
        {
            Console.WriteLine(arg.Message);
            return Task.CompletedTask;
        }
        public async Task RegisterCom()
        {

            client.MessageReceived += HandleCommandAsync;
            _commands.CommandExecuted += OnCommandExecutedAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _service);
            Console.WriteLine($"CommandService: Added {_commands.Modules.Count()} modules using reflection, with a total of {_commands.Commands.Count()} commands");
        }
        
        private Task OnCommandExecutedAsync(Optional<CommandInfo> arg1, ICommandContext arg2, IResult arg3)
        {
            return Task.CompletedTask;
        }
        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(client, message);
            if (message.Author.IsBot) return;
            int argpso = 0;
            Console.WriteLine(message.Content); // Good for Debugging
            if (message.HasStringPrefix("!", ref argpso)) // Set Prewfix Here at <message.HasStringPrefix("Your Prefix", ref argpso)>
            {
                var result = await _commands.ExecuteAsync(context, argpso, _service);
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);//Outputs the Error Reason in the Console
            }
        }
    }
}
