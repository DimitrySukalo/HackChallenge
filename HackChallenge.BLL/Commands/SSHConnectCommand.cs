using HackChallenge.BLL.CommandDIInterfaces;
using HackChallenge.DAL.Entities;
using HackChallenge.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = HackChallenge.DAL.Entities.User;

namespace HackChallenge.BLL.Commands
{
    public class SSHConnectCommand : ISSHConnectCommand
    {
        private readonly IUnitOfWork _unitOfWork;
        public string Name => "ssh";

        public SSHConnectCommand(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork), " was null.");

            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Execute(Message message, TelegramBotClient client)
        {
            long chatId = message.Chat.Id;
            User user = await _unitOfWork.UserAccessRepository.GetUserByChatId(chatId);
            LinuxSystem linuxSystem = await _unitOfWork.LinuxRepository.GetByIdAsync(user.Id);

            if(user != null && linuxSystem != null && user.isAuthorized && linuxSystem.IsConnectedTheInternet)
            {
                string[] commandParams = message.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if(commandParams.Length == 3 && commandParams[0] == Name)
                {
                    string[] connectParams = commandParams[1].Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
                    if (connectParams.Length == 2)
                    {
                        LinuxSystem victimSystem = await _unitOfWork.LinuxRepository.GetByIP(connectParams[0]);
                        if (victimSystem != null)
                        {
                            List<Vulnerability> vulnerabilities = _unitOfWork.VulnerabilityRepository.GetVulnerabilitiesByLinuxSystemId(victimSystem.Id);
                            if (vulnerabilities.Count > 0)
                            {
                                foreach (var vuln in vulnerabilities)
                                {
                                    foreach (var port in vuln.Ports)
                                    {
                                        if (port.Password == commandParams[2] && port.Login == connectParams[1])
                                        {
                                            await client.SendTextMessageAsync(chatId, $"<code>Вы подлючены к {victimSystem.IP}</code>", ParseMode.Html);
                                            return true;
                                        }
                                    }
                                }

                                await client.SendTextMessageAsync(chatId, "<code>Порты либо закрыты, либо неверный пароль.</code>", ParseMode.Html);
                                return false;
                            }

                            await client.SendTextMessageAsync(chatId, "<code>Все порты закрыты.</code>", ParseMode.Html);
                            return false;
                        }

                        await client.SendTextMessageAsync(chatId, "<code>Системы по такому адресу не существует.</code>", ParseMode.Html);
                        return false;

                    }

                    await client.SendTextMessageAsync(chatId, "<code>Неверные параметры команды.</code>", ParseMode.Html);
                    return false;
                }

                await client.SendTextMessageAsync(chatId, "<code>Неверные параметры команды.</code>", ParseMode.Html);
                return false;
            }

            return false;
        }

        public bool IsContains(Message message)
        {
            if (!string.IsNullOrWhiteSpace(message.Text))
                return message.Text.Contains(Name);

            return false;
        }
    }
}
