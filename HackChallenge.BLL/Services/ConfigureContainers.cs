using Autofac;
using HackChallenge.BLL.CommandDIInterfaces;
using HackChallenge.BLL.Commands;
using HackChallenge.BLL.Models;
using HackChallenge.DAL.DB;
using HackChallenge.DAL.Interfaces;
using HackChallenge.DAL.Repositories;

namespace HackChallenge.BLL.Services
{
    public static class ConfigureContainers
    {
        public static IContainer Configure()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<ApplicationContext>();
            builder.RegisterType<UserRepository>().As<IUserAccessRepository>();
            builder.RegisterType<StartCommand>().As<IStartCommand>();
            builder.RegisterType<EnterSignInDataCommand>().As<IEnterSignInDataCommand>();
            builder.RegisterType<ShowDirsCommand>().As<IShowDirsCommand>();
            builder.RegisterType<Bot>();

            IContainer container = builder.Build();
            return container;
        }
    }
}
