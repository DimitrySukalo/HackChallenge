﻿using HackChallenge.BLL.CommandDIInterfaces;
using HackChallenge.DAL.Entities;
using HackChallenge.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = HackChallenge.DAL.Entities.User;
using File = HackChallenge.DAL.Entities.File;
using Telegram.Bot.Types.Enums;
using System.Text;
using System.Linq;

namespace HackChallenge.BLL.Commands
{
    public class StartCommand : IStartCommand
    {
        private readonly IUnitOfWork _unitOfWork;

        public StartCommand(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork), " was null.");

            _unitOfWork = unitOfWork;
        }

        public string Name => "/start";

        public async Task<bool> Execute(Message message, TelegramBotClient client)
        {
            long chatId = message.Chat.Id;

            //IP of linux system
            string IP = GetRandIP();

            //Directories of user who want to register
            List<Directory> realUserDirs = GetDirectories();

            foreach (var dir in realUserDirs)
            {
                string currentPath = IP + dir.Path;
                dir.Path = currentPath;
            }

            //Adding folder with password list to wifi
            realUserDirs.Add(
                new Directory()
                {
                    Name = "Files",
                    Path = $"{IP}@root/Files",
                    TimeOfCreating = DateTime.UtcNow,
                    Files = new List<File>()
                    {
                        new File()
                        {
                            Name = "wifiPasswords.txt",
                            Path = $"{IP}@root/Files/wifiPasswords.txt",
                            TimeOfCreating = DateTime.UtcNow,
                            Size = new Random().Next(60,700),
                            Text = GetPasswordValues().passwords
                        },
                        new File()
                        {
                            Name = "sshPass.txt",
                            Path = $"{IP}@root/Files/sshPass.txt",
                            TimeOfCreating = DateTime.UtcNow,
                            Size = new Random().Next(60,700),
                            Text = GetSshPasswordData().allPasswordsSsh 
                        }
                    }
                });

            //Wifis which will be dispayed to user
            ICollection<Wifi> wifis = GetWifis();

            //Creating user which pressed the start button
            User user = new User()
            {
                ChatId = chatId,
                FirstName = message.From.FirstName,
                UserName = message.From.Username,
                LastName = message.From.LastName,
                HaveLinuxPermission = false,
                LinuxSystem = new LinuxSystem()
                {
                    IsConnectedTheInternet = false,
                    WifiModule = new WifiModule()
                    {
                        ModuleMode = ModuleMode.Managed,
                        Name = "wlan0",
                        Wifis = wifis
                    },
                    AllDirectories = realUserDirs,
                    CurrentDirectory = new CurrentDirectory()
                    {
                        Directory = new Directory()
                        {
                            Directories = realUserDirs,
                            Files = new List<File>(),
                            TimeOfCreating = DateTime.UtcNow,
                            Path = $"{IP}@root",
                            Name = $"{IP}@root"
                        }
                    },
                    MACAddress = GetRandomBSSID(),
                    IP = IP,
                    Vulnerabilities = new List<Vulnerability>()
                    {
                        new Vulnerability()
                        {
                            Ports = new List<Port>()
                            {
                                new Port()
                                {
                                    Login = Guid.NewGuid().ToString(),
                                    Password = Guid.NewGuid().ToString(),
                                    PortState = PortState.Close,
                                    TypeOfPort = TypeOfPort.SSH
                                }
                            }
                        }
                    }
                },
                CountOfCrackWifi = 0,
                GlobalNetwork = new GlobalNetwork()
            };

            //Creating list of user's which will be hacked by real user
            List<User> victims = new List<User>();
            for(int i = 0; i < 4; i++)
            {
                victims.Add(GetVictimUser());
            }

            int counter = 0;
            foreach(var victim in victims)
            {
                victim.LinuxSystem.WifiModule.Wifis = wifis;
                victim.GlobalNetwork = wifis.ToList()[counter].GlobalNetwork;
                counter++;
            }

            //Checking if user is exist in the database
            User tempUser = await _unitOfWork.UserAccessRepository.GetUserByChatId(chatId);
            if(tempUser == null)
            {
                await _unitOfWork.UserAccessRepository.AddAsync(user);
                await _unitOfWork.UserAccessRepository.AddRange(victims);
                await client.SendTextMessageAsync(chatId, "<code>Спасибо за регистрацию! Теперь мы можем начать ✅</code>", ParseMode.Html);
                await client.SendTextMessageAsync(chatId, "<code>Процесс подготовки...</code>", ParseMode.Html);
                await client.SendTextMessageAsync(chatId, "<code>Ещё немного...</code>", ParseMode.Html);
                await client.SendTextMessageAsync(chatId, "<code>Введите логин и пароль в формате login:password 🌐</code>", ParseMode.Html);

                return true;
            }

            await client.SendTextMessageAsync(chatId, "<code>Вы уже зарегестрированы! ✅</code>", ParseMode.Html);
            return false;
        }

        /// <summary>
        /// Getting victim users which will be hacked by real user
        /// </summary>
        private User GetVictimUser()
        {
            List<Directory> artificialUserDirs = GetDirectories();
            string IP = GetRandIP();

            foreach(var dir in artificialUserDirs)
            {
                string currentPath = IP + dir.Path;
                dir.Path = currentPath;
            }

            string name = Guid.NewGuid().ToString();

            User victimSystem = new User()
            {
                ChatId = 1,
                FirstName = Guid.NewGuid().ToString(),
                UserName = name,
                LastName = Guid.NewGuid().ToString(),
                HaveLinuxPermission = true,
                LinuxSystem = new LinuxSystem()
                {
                    IsConnectedTheInternet = true,
                    WifiModule = new WifiModule()
                    {
                        ModuleMode = ModuleMode.Managed,
                        Name = "wlan0"
                    },
                    AllDirectories = artificialUserDirs,
                    CurrentDirectory = new CurrentDirectory()
                    {
                        Directory = new Directory()
                        {
                            Directories = artificialUserDirs,
                            Files = new List<File>(),
                            TimeOfCreating = DateTime.UtcNow,
                            Path = $"{IP}@root",
                            Name = $"{IP}@root"
                        }
                    },
                    MACAddress = GetRandomBSSID(),
                    IP = IP,
                    Vulnerabilities = new List<Vulnerability>()
                    {
                        new Vulnerability()
                        {
                            Ports = new List<Port>()
                            {
                                new Port()
                                {
                                    Login = name,
                                    Password = GetSshPasswordData().passwordSsh,
                                    PortState = PortState.Open,
                                    TypeOfPort = TypeOfPort.SSH
                                }
                            }
                        }
                    }
                }
            };

            return victimSystem;
        }

        private (string passwordSsh, string allPasswordsSsh) GetSshPasswordData()
        {
            Dictionary<int, string> passwords = new Dictionary<int, string>()
            {
                { 1, "root"},
                { 2, "toor"},
                { 3, "raspberry"},
                { 4, "dietpi"},
                { 5, "test"},
                { 6, "uploader"},
                { 7, "password"},
                { 8, "admin"},
                { 9, "administrator"},
                { 10, "marketing"},
                { 11, "12345678"},
                { 12, "1234"},
                { 13, "12345"},
                { 14, "qwerty"}
            };

            string randomSshPass = passwords[new Random().Next(1, 14)];
            StringBuilder pass = new StringBuilder();

            foreach(var password in passwords.Values)
            {
                pass.Append($"{password}\n");
            }

            return (randomSshPass, pass.ToString());
        }

        /// <summary>
        /// Getting ip address of future linux system
        /// </summary>
        /// <returns></returns>
        private string GetRandIP()
        {
            Random random = new Random();
            return $"{random.Next(1, 255)}.{random.Next(1, 255)}.{random.Next(1, 255)}.{random.Next(1, 255)}";
        }

        /// <summary>
        /// Getting directories of linux system
        /// </summary>
        /// <returns></returns>
        private List<Directory> GetDirectories()
        {
            List<Directory> directories = new List<Directory>()
            {
                new Directory()
                {
                    Name = "bin",
                    Path = "@root/bin",
                    TimeOfCreating = DateTime.UtcNow,
                    Files = new List<File>(),
                    Directories = new List<Directory>(),
                },
                new Directory()
                {
                    Name = "Downloads",
                    Path = "@root/Downloads",
                    TimeOfCreating = DateTime.UtcNow,
                    Files = new List<File>(),
                    Directories = new List<Directory>(),
                },
                new Directory()
                {
                    Name = "var",
                    Path = "@root/var",
                    TimeOfCreating = DateTime.UtcNow,
                    Files = new List<File>(),
                    Directories = new List<Directory>(),
                },
                new Directory()
                {
                    Name = "backups",
                    Path = "@root/backups",
                    TimeOfCreating = DateTime.UtcNow,
                    Files = new List<File>(),
                    Directories = new List<Directory>(),
                },
                new Directory()
                {
                    Name = "Music",
                    Path = "@root/Music",
                    TimeOfCreating = DateTime.UtcNow,
                    Files = new List<File>(),
                    Directories = new List<Directory>(),
                },
                new Directory()
                {
                    Name = "Videos",
                    Path = "@root/Videos",
                    TimeOfCreating = DateTime.UtcNow,
                    Files = new List<File>(),
                    Directories = new List<Directory>(),
                }
            };

            return directories;
        }

        private (string passwords, string password) GetPasswordValues()
        {
            Dictionary<int, string> passwords = new Dictionary<int, string>()
            {
                { 1, "VV8STSXHGl7q07lv"},
                { 2, "LltXsi4dVZcpwYBU"},
                { 3, "oVXdAiOjtjpvWDPO"},
                { 4, "9HLfyMVWXhTXbqGo"},
                { 5, "07otvIQ5k9w6S4kk"},
                { 6, "NTc1hglHN82Qo7Rg"},
                { 7, "5f2v7kMKMeSgOZw5"},
                { 8, "J20NJhsvAYOgczJb"},
                { 9, "0nHvX9dVZxjF9LiQ"},
                { 10, "dD6scbRgGRdM11FK"}
            };

            string randomPass = passwords[new Random().Next(1, 10)];
            StringBuilder passwordsStr = new StringBuilder();
            foreach(var pass in passwords)
            {
                passwordsStr.Append($"{pass.Value}\n");
            }

            return (passwordsStr.ToString(), randomPass);
        }

        private ICollection<Wifi> GetWifis()
        {
            return new List<Wifi>()
            {
                new Wifi()
                {
                    BSSID = GetRandomBSSID(),
                    Name = "Home",
                    Password = GetPasswordValues().password,
                    QualityOfSignal = QualityOfSignal.Good,
                    Speed = new Random().Next(10, 100),
                    Channel = new Random().Next(1, 10),
                    Cipher = Cipher.CCMP,
                    EncryptionType = EncryptionType.WPA2,
                    GlobalNetwork = new GlobalNetwork()
                },
                new Wifi()
                {
                    BSSID = GetRandomBSSID(),
                    Name = "Lan1979",
                    Password = GetPasswordValues().password,
                    QualityOfSignal = QualityOfSignal.Normal,
                    Speed = new Random().Next(10, 100),
                    Channel = new Random().Next(1, 10),
                    Cipher = Cipher.CCMP,
                    EncryptionType = EncryptionType.WEP,
                    GlobalNetwork = new GlobalNetwork()
                },
                new Wifi()
                {
                    BSSID = GetRandomBSSID(),
                    Name = "TP_Link-003245",
                    Password = GetPasswordValues().password,
                    QualityOfSignal = QualityOfSignal.Bad,
                    Speed = new Random().Next(10, 100),
                    Channel = new Random().Next(1, 10),
                    Cipher = Cipher.TKIP,
                    EncryptionType = EncryptionType.WPA2,
                    GlobalNetwork = new GlobalNetwork()
                },
                new Wifi()
                {
                    BSSID = GetRandomBSSID(),
                    Name = "Modem-13435",
                    Password = GetPasswordValues().password,
                    QualityOfSignal = QualityOfSignal.Good,
                    Speed = new Random().Next(10, 100),
                    Channel = new Random().Next(1, 10),
                    Cipher = Cipher.WEP,
                    EncryptionType = EncryptionType.WPA2,
                    GlobalNetwork = new GlobalNetwork()
                }
            };
        }

        private string GetRandomBSSID()
        {
            Random rnd = new Random();
            return $"{rnd.Next(10,99)}:{rnd.Next(10, 99)}:{rnd.Next(0,9)}D:{rnd.Next(0, 9)}F:{rnd.Next(10, 99)}:{rnd.Next(0, 9)}E";
        }

        public bool IsContains(Message message)
        {
            if (!string.IsNullOrWhiteSpace(message.Text))
                return message.Text.Contains(Name);

            return false;
        }
    }
}
