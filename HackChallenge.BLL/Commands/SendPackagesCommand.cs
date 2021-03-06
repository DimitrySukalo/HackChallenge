﻿using HackChallenge.BLL.CommandDIInterfaces;
using HackChallenge.DAL.Entities;
using HackChallenge.DAL.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = HackChallenge.DAL.Entities.User;
using File = HackChallenge.DAL.Entities.File;
using System.Text;
using System.Collections.Generic;

namespace HackChallenge.BLL.Commands
{
    public class SendPackagesCommand : ISendPackagesCommand
    {
        private readonly IUnitOfWork _unitOfWork;

        public string Name => "aireplay-ng";

        public SendPackagesCommand(IUnitOfWork unitOfWork)
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
            WifiModule wifiModule = await _unitOfWork.WifiModuleRepository.GetByIdAsync(linuxSystem.WifiModuleId);

            if(user != null && user.isAuthorized && wifiModule.ModuleMode == ModuleMode.Monitor)
            {
                string[] commandParams = message.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                IEnumerable<Wifi> wifis = _unitOfWork.WifiRepository.GetByWifisModuleId(wifiModule.Id);
                bool isExist = wifis.Any(w => w.BSSID == commandParams[4]);

                if(commandParams[1] == "--deauth" && commandParams[3] == "-a" &&
                   isExist && commandParams[5] == wifiModule.Name)
                {
                    for(int i = 0; i < 8; i++)
                    {
                        await client.SendTextMessageAsync(chatId, $"<code>{DateTime.UtcNow.ToString("HH:mm:ss")}  Sending DeAuth to broadcast -- BSSID: [{commandParams[4]}]</code>", ParseMode.Html);
                        Thread.Sleep(1000);
                    }

                    user.CountOfCrackWifi++;
                    await _unitOfWork.SaveAsync();

                    Wifi wifi = wifis.FirstOrDefault(w => w.BSSID == commandParams[4]);

                    if (wifi != null)
                    {
                        CurrentDirectory currentDirectory = await _unitOfWork.CurrentDirectoryRepository.GetByIdAsync(linuxSystem.CurrentDirectoryId);
                        if (currentDirectory != null)
                        {
                            Directory directory = _unitOfWork.DirectoryRepository.GetInDirectory
                                                                                  (await _unitOfWork.DirectoryRepository.GetByIdAsync(currentDirectory.DirectoryId));
                            List<File> curDirFiles = directory.Files.ToList();

                            File file = new File()
                            {
                                Name = $"Wifi-Crack{user.CountOfCrackWifi}.cap",
                                Size = new Random().Next(100, 500),
                                TimeOfCreating = DateTime.UtcNow,
                                Text = Convert.ToBase64String(Encoding.UTF8.GetBytes(wifi.Password))
                            };

                            directory.Files.Add(file);

                            await _unitOfWork.SaveAsync();

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    return false;
                }
                else
                {
                    await client.SendTextMessageAsync(chatId, "<code>Неверные данные</code>", ParseMode.Html);
                    return false;
                }
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
