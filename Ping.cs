using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShikimoriNET;
using ShikimoriNET.Categories;
using ShikimoriNET.Categories.Interfaces;
using ShikimoriNET.Params.Anime;
using ShikimoriNET.Enums.Anime;
using ShikimoriNET.Enums;


namespace newb0t
{
   public class Ping : ModuleBase<SocketCommandContext>
    {

        private  ShikimoriApi _api;




        [Command("anime")]
        public async Task PingAsync()
        {


            _api = new ShikimoriApi();
            var animes = await _api.Anime.SearchAsync(new SearchParams
            {
               
                Order = Order.Random,
                Censored = false,
                Score = 7,
                Limit = 100000
              
            });

            Random r = new Random();

            var a = animes.ElementAt(r.Next(0, animes.Count()));
         

            await ReplyAsync(a.Name+"|"+ a.Russian + "\n https://shikimori.org" + a.Url);
          

        }

      

        [Command("ban"), RequireUserPermission(Discord.GuildPermission.ManageRoles)]
        public async Task Ban(IGuildUser user)
        {
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == "banned");

            
            await (user as IGuildUser).AddRoleAsync(role);
            await ReplyAsync($"{user.Mention} был забанен, видимо есть за что..." );

        }

        [Command("unban"), RequireUserPermission(Discord.GuildPermission.ManageRoles)]
        public async Task unBan(IGuildUser user)
        {
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == "banned");


            await (user as IGuildUser).RemoveRoleAsync(role);
            await ReplyAsync($"{user.Mention} Разбанен!");

        }

    }
}
