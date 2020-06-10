using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;

namespace KickBot
{
    public class Program
    {
        static DiscordSocketClient _client;
        static System.Timers.Timer _timer;
        static IRole tempRole;
        static async Task Main(string[] args)
        {
            _client = new DiscordSocketClient();
            // log the bot into discord test this
            await _client.LoginAsync(Discord.TokenType.Bot, "NzE5NTgzNzY2Mjk2OTIwMTc0.Xt_eDQ.roDYond - X - F3tajLORJSXLHw5VA");
            await _client.StartAsync();
            _client.JoinedGuild += _client_JoinedGuild;
            _client.UserJoined += _client_UserJoined;
            Console.ReadLine();
        }

        static async Task _client_UserJoined(SocketGuildUser user)
        {
            await user.AddRoleAsync(tempRole);
        }

        static async Task _client_JoinedGuild(SocketGuild arg)
        {
            tempRole = await arg.CreateRoleAsync("TempMember", null, Discord.Color.LightGrey, false, null);
            Scheduler();
        }

        static void Scheduler()
        {
            var oneWeek = DateTime.Now.AddDays(7);
            var secondTillThen = (double)(oneWeek - DateTime.Now).TotalMilliseconds;
            _timer = new System.Timers.Timer(secondTillThen);
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        async static void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            foreach(var guild in _client.Guilds)
            {
                foreach(var user in guild.Users.Where(u => u.Roles.Contains(tempRole)))
                {
                    if(user.JoinedAt >= new DateTimeOffset().AddDays(1))
                    {
                        await user.KickAsync();
                    }
                }
            }
            Scheduler();
        }
    }
}
