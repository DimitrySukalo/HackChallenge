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
        private readonly IShowDirsCommand _showDirsCommand;
        private readonly IPingCommand _pingCommand;
        private readonly IIfConfingCommand _ifConfigCommand;
        private readonly IMonitorModeCommand _monitorModeCommand;
        private readonly IAllWifiInterception _allWifiInterception;

        #endregion

        public IReadOnlyList<ICommand> Commands => _commands.AsReadOnly();

        public Bot(IStartCommand startCommand, IEnterSignInDataCommand enterSignInDataCommand,
                   IShowDirsCommand showDirsCommand, IPingCommand pingCommand,
                   IIfConfingCommand ifConfingCommand, IMonitorModeCommand monitorModeCommand,
                   IAllWifiInterception allWifiInterception)
        {
            _startCommand = startCommand ?? throw new ArgumentNullException(nameof(startCommand), " was null");
            _enterSignInDataCommand = enterSignInDataCommand ?? throw new ArgumentNullException(nameof(enterSignInDataCommand), " was null");
            _showDirsCommand = showDirsCommand ?? throw new ArgumentNullException(nameof(showDirsCommand), " was null");
            _pingCommand = pingCommand ?? throw new ArgumentNullException(nameof(pingCommand), " was null");
            _ifConfigCommand = ifConfingCommand ?? throw new ArgumentNullException(nameof(ifConfingCommand), " was null");
            _monitorModeCommand = monitorModeCommand ?? throw new ArgumentNullException(nameof(monitorModeCommand), " was null");
            _allWifiInterception = allWifiInterception ?? throw new ArgumentNullException(nameof(allWifiInterception), " was null");
        }

        public TelegramBotClient GetClient()
        {
            if (_client != null)
                return _client;

            _commands = new List<ICommand>()
            {
                _startCommand,
                _enterSignInDataCommand,
                _showDirsCommand,
                _pingCommand,
                _ifConfigCommand,
                _monitorModeCommand,
                _allWifiInterception
            };

            _client = new TelegramBotClient(AppConfig.Token);
            return _client;
        }
    }
}
