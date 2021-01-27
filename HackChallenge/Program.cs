using Autofac;
using HackChallenge.BLL.Models;
using HackChallenge.BLL.Services;
using HackChallenge.Controllers;
using System;
using Telegram.Bot;

namespace HackChallenge
{
    class Program
    {
        static IContainer container = ConfigureContainers.Configure();

        static void Main(string[] args)
        {
            Bot bot = GetSomeService<Bot>();
            TelegramBotClient client = bot.GetClient();

            BotController botController = new BotController(client, bot);
            botController.StartBot();

            Console.ReadLine();
        }

        private static T GetSomeService<T>()
        {
            return container.Resolve<T>();
        }
    }
}
