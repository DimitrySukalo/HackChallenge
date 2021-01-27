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
        private readonly IEnterSignInDataCommand _enterSignInDataCommand;

        #endregion

        public IReadOnlyList<ICommand> Commands => _commands.AsReadOnly();

        public Bot(IStartCommand startCommand, IEnterSignInDataCommand enterSignInDataCommand)
        {
           _startCommand = startCommand ?? throw new ArgumentNullException(nameof(startCommand), " was null");
            _enterSignInDataCommand = enterSignInDataCommand ?? throw new ArgumentNullException(nameof(enterSignInDataCommand), " was null");
        }

        public TelegramBotClient GetClient()
        {
            if (_client != null)
                return _client;

            _commands = new List<ICommand>()
            {
                _startCommand,
                _enterSignInDataCommand
            };

            _client = new TelegramBotClient(AppConfig.Token);
            return _client;
        }
    }
}
