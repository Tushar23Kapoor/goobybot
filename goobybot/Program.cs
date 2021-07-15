using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using goobybot.Core.Util;
using goobybot.Core.Config;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace goobybot
{
    public class Program
    {
        public static DiscordSocketClient Client;
        public static CommandService Commands;

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            LogManager.Instance.Init("Logs");

            Console.WriteLine($"Launching ChristieBot - {DateTime.Now}");

            Client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Error,
                MessageCacheSize = 30
            });

            // register commands
            Commands = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = true,
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Error
            });

            // event callbacks
            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);
            Client.MessageReceived += Client_MessageReceived;
            Client.UserJoined += UserJoined;
            Client.Ready += Client_Ready;
            //Client.ReactionAdded += Client_ReactionAdded;

            // actually initiate the client
            await Client.LoginAsync(TokenType.Bot, Config.BOT_TOKEN);
            await Client.StartAsync();

            // force to stall forever
            await UpdateLoop();
            await Task.Delay(-1);

        }

        private Task Client_Ready()
        {
            // loop through all users on the server and check if any arent in the database, then add them
            /*try
            {
                foreach (var guild in Client.Guilds)
                {
                    foreach (var user in guild.Users)
                    {
                        Data.DiscData.CreateDiscordUser(user.Id);
                    }
                }
            }
            catch (Exception) { }*/
            return null;
        }

        private Task UserJoined(SocketGuildUser arg)
        {
            // add the new user
            /* try
            {
                Data.DiscData.CreateDiscordUser(arg.Id);
            }
            catch (Exception) { }*/
            return null;
        }

        private async Task Client_MessageReceived(SocketMessage arg)
        {
            // commands
            try
            {
                var Message = arg as SocketUserMessage;
                var Context = new SocketCommandContext(Client, Message);

                if (Context.Message == null || Context.Message.Content == "") return;
                if (Context.User.Id != Client.CurrentUser.Id && Context.User.IsBot) return;
                if (Context.IsPrivate) return;

                int ArgPos = 0;

                if (!(Message.HasStringPrefix(Config.COMMAND_PREFIX, ref ArgPos) || Message.HasMentionPrefix(Client.CurrentUser, ref ArgPos))) return;

                // execute the command
                try
                {
                    // are commands locked?
                    /*if (CommandLock && !(Context.User as SocketGuildUser).GuildPermissions.Administrator)
                    {
                        // display this?
                        await MiscUtil.EmbedResponse($"Error: Command usage is currently locked.\nReason: ```{UtilCommands.LOCK_REASON}```", Context.Channel);
                        return;
                    }
                    // is user on command cooldown?
                    if (!CanExecuteUserCommand(Context.User.Id)) return;
                    */
                    // execute!
                    var Result = await Commands.ExecuteAsync(Context, ArgPos, null);
                    if (!Result.IsSuccess)
                    {
                        try
                        {
                            if (!Result.ErrorReason.Contains("Unknown command"))
                                await Context.Channel.SendMessageAsync($"{Result.ErrorReason}");
                        }
                        catch (Exception) { }
                    }
                }
                catch (Exception) { }
            }
            catch (Exception) { }
        }
        private async Task UpdateLoop()
        {
            await Task.Delay(5000);

            while (true)
            {
                //Console.WriteLine("Test");
            }
        }

    }
}