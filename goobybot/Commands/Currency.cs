using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ChristieBot.Core.Util;
using Discord;
using Discord.Rest;
using Discord.WebSocket;

namespace goobybot.Core.Commands
{
    public class Currency : ModuleBase<SocketCommandContext>
    {
        [Command("cbalance")]
        public async Task DoGetBalance(IUser user = null)
        {
            await MiscUtil.EmbedResponse("This is a test", Context.Channel);
        }

    }
}