using HackChallenge.BLL.CommandDIInterfaces;
using HackChallenge.DAL.Interfaces;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = HackChallenge.DAL.Entities.User;
using File = HackChallenge.DAL.Entities.File;
using HackChallenge.DAL.Entities;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace HackChallenge.BLL.Commands
{
    public class AirCrackNgCommand : IAirCrackNgCommand
    {
        private readonly IUnitOfWork _unitOfWork;

        public string Name => "aircrack-ng";

        public AirCrackNgCommand(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork), " was null.");

            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Execute(Message message, TelegramBotClient client)
        {
            long chatId = message.Chat.Id;
            User user = await _unitOfWork.UserAccessRepository.GetUserByChatId(chatId);

            if (user != null && user.isAuthorized)
            {
                string[] parameters = message.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if(parameters[0] == Name && parameters[2] == "-w")
                {
                    LinuxSystem linuxSystem = await _unitOfWork.LinuxRepository.GetByIdAsync(user.Id);
                    CurrentDirectory currentDirectory = await _unitOfWork.CurrentDirectoryRepository.GetByIdAsync(linuxSystem.CurrentDirectoryId);
                    Directory directory = _unitOfWork.DirectoryRepository.GetInDirectory(await _unitOfWork.DirectoryRepository.GetByIdAsync(currentDirectory.DirectoryId));
                    string fileExtension = "";
                    string fileName = "";

                    if (!parameters[1].Contains("/"))
                    {
                        fileExtension = parameters[1].Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[1];
                        fileName = parameters[1];
                    }
                    else
                    {
                        string filePath = parameters[1].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Last();
                        fileExtension = filePath.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[1];
                        fileName = filePath;
                    }
                        

                    File handShakeFile = directory.Files.FirstOrDefault(f => f.Name == parameters[1]);
                    File passwordFile = null;
                    if (fileExtension == "cap" && handShakeFile != null)
                    {
                        if (parameters[3].Contains("/"))
                        {
                            passwordFile = await _unitOfWork.FileRepository.GetByPath(parameters[3]);
                        }
                        else
                        {
                            passwordFile = directory.Files.FirstOrDefault(f => f.Name == parameters[3]);
                        }

                        if(passwordFile != null)
                        {
                            WifiModule wifiModule = await _unitOfWork.WifiModuleRepository.GetByIdAsync(linuxSystem.WifiModuleId);
                            IEnumerable<Wifi> wifis = _unitOfWork.WifiRepository.GetByWifisModuleId(wifiModule.Id);
                            if(wifis.Count() > 0)
                            {
                                string password = Encoding.UTF8.GetString(Convert.FromBase64String(handShakeFile.Text));
                                if (passwordFile.Text.Contains(password))
                                {
                                    foreach (var wifi in wifis)
                                    {
                                        await client.SendTextMessageAsync(chatId, $"<code>[{DateTime.UtcNow}] Подбор пароля...</code>", ParseMode.Html);
                                        if (wifi.Password == password)
                                        {
                                            linuxSystem.IsConnectedTheInternet = true;
                                            await client.SendTextMessageAsync(chatId, $"<code>Пароль успешно найден, вы подсойдены к {wifi.Name}\nСкорость: {wifi.Speed}мб/с</code>", ParseMode.Html);
                                            break;
                                        }
                                    }

                                    wifiModule.ModuleMode = ModuleMode.Managed;
                                    wifiModule.Name = "wlan0";

                                    await _unitOfWork.SaveAsync();

                                    return true;
                                }

                                await client.SendTextMessageAsync(chatId, "<code>Пароль не найдено</code>", ParseMode.Html);
                            }

                            return false;
                        }

                        await client.SendTextMessageAsync(chatId, "<code>Файл с паролями не найден. Перейдите в папку с этим файлом или укажите путь.</code>", ParseMode.Html);
                        return false;
                    }
                    else
                    {
                        await client.SendTextMessageAsync(chatId, "<code>Файл не найден. Перейдите в папку с этим файлом или укажите путь..</code>", ParseMode.Html);
                        return false;
                    }
                }
                else
                {
                    await client.SendTextMessageAsync(chatId, "<code>Неверная команда</code>", ParseMode.Html);
                    return false;
                }
            }

            return false;
        }

        private Directory FindDir(Directory directory, string dirName)
        {
            if (directory != null)
            {
                directory = _unitOfWork.DirectoryRepository.GetInDirectory(directory);

                if (dirName == directory.Name)
                {
                    return directory;
                }

                if (directory.Directories != null)
                {
                    foreach (var dir in directory.Directories)
                    {
                        if (dir.Name == dirName)
                        {
                            Directory fullDir = _unitOfWork.DirectoryRepository.GetInDirectory(dir);
                            return fullDir;
                        }

                        return FindDir(dir, dirName);
                    }
                }
            }

            return null;
        }

        public bool IsContains(Message message)
        {
            if(!string.IsNullOrWhiteSpace(message.Text))
                return message.Text.Contains(Name);

            return false;
        }
    }
}
