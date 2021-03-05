using HackChallenge.BLL.CommandDIInterfaces;
using HackChallenge.DAL.Entities;
using HackChallenge.DAL.Interfaces;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = HackChallenge.DAL.Entities.User;
using File = HackChallenge.DAL.Entities.File;
using System.Collections.Generic;

namespace HackChallenge.BLL.Commands
{
    public class SSHBruteForceCommand : ISSHBruteForceCommand
    {
        private readonly IUnitOfWork _unitOfWork;

        public string Name => "hydra";

        public SSHBruteForceCommand(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork), " was null.");

            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Execute(Message message, TelegramBotClient client)
        {
            long chatId = message.Chat.Id;
            User user = await _unitOfWork.UserAccessRepository.GetUserByChatId(chatId);

            if(user != null && user.isAuthorized)
            {
                LinuxSystem linuxSystem = await _unitOfWork.LinuxRepository.GetByIdAsync(user.Id);
                if(linuxSystem != null && linuxSystem.IsConnectedTheInternet)
                {
                    string[] commandParams = message.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if(commandParams[0] == Name && !string.IsNullOrWhiteSpace(commandParams[1])
                        && commandParams[2] == "-P")
                    {
                        File passwordFile = await _unitOfWork.FileRepository.GetByPath(commandParams[3]);

                        if (commandParams.Length == 5)
                        {
                            string[] sshParams = commandParams[4].Split(new string[] { "//" }, StringSplitOptions.RemoveEmptyEntries);
                            if (sshParams.Length == 2)
                            {
                                if (passwordFile != null && sshParams[0] == "ssh:" && !string.IsNullOrWhiteSpace(sshParams[1]))
                                {
                                    LinuxSystem victimSystem = await _unitOfWork.LinuxRepository.GetByIP(sshParams[1]);
                                    if (victimSystem != null)
                                    {
                                        List<Vulnerability> vulnerabilities = _unitOfWork.VulnerabilityRepository.GetVulnerabilitiesByLinuxSystemId(victimSystem.Id);
                                        if (vulnerabilities.Count > 0)
                                        {
                                            foreach (var vuln in vulnerabilities)
                                            {
                                                foreach (var port in vuln.Ports)
                                                {
                                                    await client.SendTextMessageAsync(chatId, "<code>Подбор пароля...</code>", ParseMode.Html);
                                                    if (passwordFile.Text.Contains(port.Password) && commandParams[1] == port.Login)
                                                    {
                                                        await client.SendTextMessageAsync(chatId, $"<code>[22][ssh]  host: {sshParams[1]}   login: {port.Login}   password: {port.Password}</code>", ParseMode.Html);
                                                        return true;
                                                    }
                                                }
                                            }
                                        }

                                        await client.SendTextMessageAsync(chatId, "<code>В системе нету уязвимостей</code>", ParseMode.Html);
                                        return false;
                                    }

                                    await client.SendTextMessageAsync(chatId, "<code>Неверный IP адресс.</code>", ParseMode.Html);
                                    return false;
                                }

                                await client.SendTextMessageAsync(chatId, "<code>Неверный IP адресс.</code>", ParseMode.Html);
                                return false;
                            }
                        }

                        await client.SendTextMessageAsync(chatId, "<code>Проверьте корректность параметров команды.</code>", ParseMode.Html);
                        return false;
                    }

                    await client.SendTextMessageAsync(chatId, "<code>Неверная команда.</code>", ParseMode.Html);
                    return false;
                }

                await client.SendTextMessageAsync(chatId, "<code>Ошибка. Проверьте подключение к интернету.</code>", ParseMode.Html);
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
