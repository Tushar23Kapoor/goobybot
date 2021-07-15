using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace goobybot.Core.Util
{
    public static class MiscUtil
    {
        public static async Task EmbedResponse(string msg, IMessageChannel channel)
        {
            var embed = new EmbedBuilder();
            embed.WithDescription(msg);
            await channel.SendMessageAsync("", false, embed.Build());
        }

        /// <summary>
        /// Checks whether a name, without case, without spaces, are equal
        /// </summary>
        /// <returns></returns>
        public static bool NameBetterEquals(string input1, string input2)
        {
            return input1.Replace(" ", "").Equals(input2.Replace(" ", ""), StringComparison.CurrentCultureIgnoreCase);
        }

        public static IUser GetUser(ulong id)
        {
            foreach (var guild in Program.Client.Guilds)
            {
                foreach (var user in guild.Users)
                {
                    if (user.Id.Equals(id))
                        return user;
                }
            }
            return null;
        }
        public static string GetProfilePic(ulong id)
        {
            foreach (var guild in Program.Client.Guilds)
            {
                foreach (var user in guild.Users)
                {
                    if (user.Id.Equals(id))
                        return user.GetAvatarUrl();
                }
            }
            return "";
        }

        /// <summary>
        /// Give a role to a user by ID, only if role is found
        /// </summary>
        /// <param name="user"></param>
        /// <param name="guild"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static async Task GiveRoleById(SocketGuildUser user, SocketGuild guild, string roleId)
        {
            try
            {
                var role = guild.Roles.Where(x => x.Id.ToString().Equals(roleId)).First();
                if (role != null)
                {
                    await user.AddRoleAsync(role);
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Finds a text channel by id
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public static SocketTextChannel GetChannelById(string channelId)
        {
            foreach (var guild in Program.Client.Guilds)
            {
                var channel = guild.Channels.Where(x => x.Id.ToString().Equals(channelId)).FirstOrDefault();
                if (channel != null) return channel as SocketTextChannel;
            }
            return null;
        }

    }
}