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
        private readonly ISendPackagesCommand _sendPackagesCommand;
        private readonly ICDCommand _cDCommand;
        private readonly IBackCDCommand _backCDCommand;
        private readonly IPWDCommand _pWDCommand;
        private readonly IAirCrackNgCommand _airCrackNgCommand;
        private readonly INetDiscoverCommand _netDiscoverCommand;
        private readonly INmapCommand _nmapCommand;
        private readonly ISMTPUserCheckCommand _sMTPUserCheckCommand;
        private readonly IHelpCommand _helpCommand;
        private readonly ISSHBruteForceCommand _sSHBruteForceCommand;

        #endregion

        public IReadOnlyList<ICommand> Commands => _commands.AsReadOnly();

        public Bot(IStartCommand startCommand, IEnterSignInDataCommand enterSignInDataCommand,
                   IShowDirsCommand showDirsCommand, IPingCommand pingCommand,
                   IIfConfingCommand ifConfingCommand, IMonitorModeCommand monitorModeCommand,
                   IAllWifiInterception allWifiInterception, ISendPackagesCommand sendPackagesCommand,
                   ICDCommand cDCommand, IBackCDCommand backCDCommand, IPWDCommand pWDCommand,
                   IAirCrackNgCommand airCrackNgCommand, INetDiscoverCommand netDiscover,
                   INmapCommand nmapCommand, ISMTPUserCheckCommand sMTPUserCheckCommand,
                   IHelpCommand helpCommand, ISSHBruteForceCommand sSHBruteForceCommand)
        {
            _startCommand = startCommand ?? throw new ArgumentNullException(nameof(startCommand), " was null");
            _enterSignInDataCommand = enterSignInDataCommand ?? throw new ArgumentNullException(nameof(enterSignInDataCommand), " was null");
            _showDirsCommand = showDirsCommand ?? throw new ArgumentNullException(nameof(showDirsCommand), " was null");
            _pingCommand = pingCommand ?? throw new ArgumentNullException(nameof(pingCommand), " was null");
            _ifConfigCommand = ifConfingCommand ?? throw new ArgumentNullException(nameof(ifConfingCommand), " was null");
            _monitorModeCommand = monitorModeCommand ?? throw new ArgumentNullException(nameof(monitorModeCommand), " was null");
            _allWifiInterception = allWifiInterception ?? throw new ArgumentNullException(nameof(allWifiInterception), " was null");
            _sendPackagesCommand = sendPackagesCommand ?? throw new ArgumentNullException(nameof(sendPackagesCommand), " was null");
            _cDCommand = cDCommand ?? throw new ArgumentNullException(nameof(cDCommand), " was null");
            _backCDCommand = backCDCommand ?? throw new ArgumentNullException(nameof(backCDCommand), " was null");
            _pWDCommand = pWDCommand ?? throw new ArgumentNullException(nameof(pWDCommand), " was null");
            _airCrackNgCommand = airCrackNgCommand ?? throw new ArgumentNullException(nameof(airCrackNgCommand), " was null");
            _netDiscoverCommand = netDiscover ?? throw new ArgumentNullException(nameof(netDiscover), " was null.");
            _nmapCommand = nmapCommand ?? throw new ArgumentNullException(nameof(nmapCommand), " was null.");
            _sMTPUserCheckCommand = sMTPUserCheckCommand ?? throw new ArgumentNullException(nameof(sMTPUserCheckCommand), " was null.");
            _helpCommand = helpCommand ?? throw new ArgumentNullException(nameof(helpCommand), " was null.");
            _sSHBruteForceCommand = sSHBruteForceCommand ?? throw new ArgumentNullException(nameof(sSHBruteForceCommand), " was null.");
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
                _allWifiInterception,
                _sendPackagesCommand,
                _cDCommand,
                _backCDCommand,
                _pWDCommand,
                _airCrackNgCommand,
                _netDiscoverCommand,
                _nmapCommand,
                _sMTPUserCheckCommand,
                _helpCommand,
                _sSHBruteForceCommand
            };

            _client = new TelegramBotClient(AppConfig.Token);
            return _client;
        }
    }
}
