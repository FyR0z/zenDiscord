using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zenDiscord.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("say")]
        [Alias("say", "Say")]
        public async Task SayAsync([Remainder] string msg)
        {
            await ReplyAsync(msg.ToString());
        }

        [Command("help")]
        [Alias("help", "Help")]
        public async Task HelpAsync()
        {
            await ReplyAsync("__**zenDiscord Bot**__\n!info - Information about the bot\n!say - Make the bot talk!");
        }

        [Command("info")]
        [Alias("info", "Info")]
        public async Task InfoAsync()
        {
            await ReplyAsync("__**zenDiscord Bot**__\n! Created by Motley");
        }
    }
    
}
