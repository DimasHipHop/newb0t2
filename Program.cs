using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace newb0t
{
    class Program
    {
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public async Task RunBotAsync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            string botToken = "NTU3MTI5OTM4Mzg2NTUwNzk0.D3JqLg.bPxltMahCM0iINl9CubKYLSiAVo";


            _client.Log += Log;
            _client.UserJoined += Joined;
          await  _client.SetGameAsync("Заходи!", "https://twitch.tv/newb0dy", ActivityType.Streaming);

            await RegisterCommandsAsync();

            await _client.LoginAsync(Discord.TokenType.Bot, botToken);
            await _client.StartAsync();

         

            await Task.Delay(-1);
        }

        private async Task Joined(SocketGuildUser user)
        {
            var guild = user.Guild;
          //  var channel = guild.DefaultChannel;
            var channel = guild.GetTextChannel(547080441971212330);
      
            Random r = new Random();
            switch (r.Next(1, 5))
            {
                case 1 :  await channel.SendMessageAsync($"Добро пожаловать, {user.Mention}");break;
                case 2: await channel.SendMessageAsync($"Все приветствуем {user.Mention}!"); break;
                case 3: await channel.SendMessageAsync($"{user.Mention} теперь один из нас!"); break;
                case 4: await channel.SendMessageAsync($"лол, кек, {user.Mention}"); break;
                case 5: await channel.SendMessageAsync($"А вот и ты! {user.Mention}!"); break;
            }
        }

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(),_services);

        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            if (message is null || message.Author.IsBot) return;

            int argPos = 0;
            if (message.HasStringPrefix("0!", ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(_client, message);
               var result =  await _commands.ExecuteAsync(context, argPos, _services);

                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
            }
        }
    }
}
