using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;

//
//
//
//
//
//          zenDiscord - The open source Discord bot
//           
//          Coded by Motley
//          https://hackforums.net/member.php?action=profile&uid=2368933
//
//
//
//
//

namespace zenDiscord
{
    class Program
    {

        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;            


        public async Task RunBotAsync()
        {
            Console.WriteLine("zenDiscord - The open source Discord Bot/nCoded by Motley/nhttps://hackforums.net/member.php?action=profile&uid=2368933");

            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();


            // INSERT BOT TOKEN HERE

            string botToken = "INSERT BOT TOKEN HERE";

            //




            _client.Log += Log;

            await RegisterCommandAsync();

            await _client.LoginAsync(TokenType.Bot, botToken);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task RegisterCommandAsync()
        {
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {

            var message = arg as SocketUserMessage;
            if (message is null || message.Author.IsBot) return;

            int argPos = 0;

            if (message.HasStringPrefix("!", ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                    var context = new SocketCommandContext(_client, message);

                    var result = await _commands.ExecuteAsync(context, argPos, _services);

                    if (!result.IsSuccess)
                        Console.WriteLine("{" + DateTime.Now.ToString() + "} - " + result.ErrorReason);
            }
        }
        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            var logSev = Console.ForegroundColor;
            switch (arg.Severity)
            {
                case LogSeverity.Critical:
                    Console.ForegroundColor = ConsoleColor.Red;
                    return Task.CompletedTask;
                    break;
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    return Task.CompletedTask;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    return Task.CompletedTask;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    return Task.CompletedTask;
                    break;
                case LogSeverity.Verbose:
                    Console.ForegroundColor = ConsoleColor.White;
                    return Task.CompletedTask;
                    break;
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.Green;
                    return Task.CompletedTask;
                    break;
            }

            Console.WriteLine($"{DateTime.Now,-19} [{arg.Severity,8}] {arg.Source}: {arg.Message}");
            Console.ForegroundColor = ConsoleColor.Black;

            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });
            return Task.CompletedTask;
        }
    }
}
