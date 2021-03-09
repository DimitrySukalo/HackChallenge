using HackChallenge.BLL.CommandDIInterfaces;
using HackChallenge.BLL.Commands;
using HackChallenge.BLL.Models;
using HackChallenge.Controllers;
using HackChallenge.DAL.DB;
using HackChallenge.DAL.Interfaces;
using HackChallenge.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Telegram.Bot;

namespace HackChallenge
{
    class Program
    {
        static ServiceProvider ServiceProvider = null;

        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationContext>(options => options
                    .UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=hackChallenge;Trusted_Connection=True;"));
            services.AddTransient<IStartCommand, StartCommand>();
            services.AddTransient<IEnterSignInDataCommand, EnterSignInDataCommand>();
            services.AddTransient<IShowDirsCommand, ShowDirsCommand>();
            services.AddTransient<IPingCommand, PingCommand>();
            services.AddTransient<IIfConfingCommand, IfConfigCommand>();
            services.AddTransient<IMonitorModeCommand, MonitorModeCommand>();
            services.AddTransient<IAllWifiInterception, AllWifiInterception>();
            services.AddTransient<ISendPackagesCommand, SendPackagesCommand>();
            services.AddTransient<ICDCommand, CDCommand>();
            services.AddTransient<IBackCDCommand, BackCDCommand>();
            services.AddTransient<IPWDCommand, PWDCommand>();
            services.AddTransient<IAirCrackNgCommand, AirCrackNgCommand>();
            services.AddTransient<INetDiscoverCommand, NetDiscoverCommand>();
            services.AddTransient<INmapCommand, NmapCommand>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ISMTPUserCheckCommand, SMTPUserCheckCommand>();
            services.AddTransient<IHelpCommand, HelpCommand>();
            services.AddTransient<ISSHBruteForceCommand, SSHBruteForceCommand>();
            services.AddTransient<ISSHConnectCommand, SSHConnectCommand>();
            services.AddTransient<Bot>();

            ServiceProvider = services.BuildServiceProvider();

            Bot bot = GetSomeService<Bot>();
            TelegramBotClient client = bot.GetClient();

            BotController botController = new BotController(client, bot);
            botController.StartBot();


            Console.ReadLine();
        }

        private static T GetSomeService<T>()
        {
            return ServiceProvider.GetService<T>();
        }
    }
}
