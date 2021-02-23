using HackChallenge.BLL.CommandDIInterfaces;
using HackChallenge.DAL.Entities;
using HackChallenge.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = HackChallenge.DAL.Entities.User;

namespace HackChallenge.BLL.Commands
{
    public class NmapCommand : INmapCommand
    {
        private readonly IUnitOfWork _unitOfWork;

        public string Name => "nmap";

        public NmapCommand(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork), " was null.");

            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Execute(Message message, TelegramBotClient client)
        {
            long chatId = message.Chat.Id;
            User user = await _unitOfWork.UserAccessRepository.GetUserByChatId(chatId);
            if(user != null)
            {
                LinuxSystem linuxSystem = await _unitOfWork.LinuxRepository.GetByIdAsync(user.Id);
                if(linuxSystem != null)
                {
                    string[] commandParams = message.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if(commandParams.Length == 2 && commandParams[0] == Name)
                    {
                        LinuxSystem victimSystem = await _unitOfWork.LinuxRepository.GetByIP(commandParams[1]);
                        if(victimSystem != null)
                        {
                            List<Vulnerability> vulnerabilities = _unitOfWork.VulnerabilityRepository.GetVulnerabilitiesByLinuxSystemId(victimSystem.Id);
                            if(vulnerabilities.Count > 0)
                            {
                                StringBuilder nmapInfo = new StringBuilder();

                                nmapInfo.Append("PORT       STATE\n");
                                foreach(var vulnerabiliti in vulnerabilities)
                                {
                                    foreach(var port in vulnerabiliti.Ports)
                                    {
                                        nmapInfo.Append($"{port.TypeOfPort}     {port.PortState}\n");
                                    }
                                }

                                await client.SendTextMessageAsync(chatId, $"<code>{nmapInfo}</code>", ParseMode.Html);
                                return true;
                            }
                        }

                        await client.SendTextMessageAsync(chatId, "<code>Неверный IP адрес</code>", ParseMode.Html);
                        return false;
                    }

                    await client.SendTextMessageAsync(chatId, "<code>Неверные параметры команды</code>", ParseMode.Html);
                    return false;
                }
            }

            await client.SendTextMessageAsync(chatId, "<code>Пользователя не существует</code>", ParseMode.Html);
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
