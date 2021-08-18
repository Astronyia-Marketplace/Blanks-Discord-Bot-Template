using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanks_Discord_Bot_Template
{
    class Commands : ModuleBase
    {
        [Command("ping")]
        public async Task PingCommand()
        {
            await Context.Channel.SendMessageAsync("pong");
        }
    }
}
