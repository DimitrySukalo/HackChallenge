using HackChallenge.BLL.CommandDIInterfaces;
using HackChallenge.DAL;
using System;
using System.Collections.Generic;
using Telegram.Bot;

namespace HackChallenge.BLL.Models
{
    public class Bot
    {
        private TelegramBotClient _client;
        private List<ICommand> _commands;

        #region Commands

        private readonly IStartCommand _startCommand;

        #endregion

        public IReadOnlyList<ICommand> Commands => _commands.AsReadOnly();

        public Bot(IStartCommand startCommand)
        {
           _startCommand = startCommand ?? throw new ArgumentNullException(nameof(startCommand), " was null");
        }

        public TelegramBotClient GetClient()
        {
            if (_client != null)
                return _client;

            _commands = new List<ICommand>()
            {
                _startCommand
            };

            _client = new TelegramBotClient(AppConfig.Token);
            return _client;
        }
    }
}
