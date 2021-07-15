using Discord.Commands;
using System.Threading.Tasks;
using Discord;
using goobybot.Core.Util;

namespace goobybot.Core.Commands
{
    public class Currency : ModuleBase<SocketCommandContext>
    {
        [Command("test")]
        public async Task DoGetBalance(IUser user = null)
        {
            await MiscUtil.EmbedResponse("This is a test", Context.Channel);
        }

    }
}