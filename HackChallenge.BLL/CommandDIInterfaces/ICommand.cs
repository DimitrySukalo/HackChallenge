using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HackChallenge.BLL.CommandDIInterfaces
{
    public interface ICommand
    {
        string Name { get; }

        Task<bool> Execute(Message message, TelegramBotClient client);

        bool IsContains(Message message);
    }
}
