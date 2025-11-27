using System;
using System.Threading;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.VisualBasic;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Parse;
using ParseLave2;

namespace ParseLave2;

//Описано взаимодействие с Telegramm

public class Telegramm
{
    private static TelegramBotClient botClient;

    public static async Task SendMessageAsyncWithBot(ForumTopic topic)
    {
        string botToken = "7891717896:AAEECgQrQsu-l1oedkyc7Wqab4Adjx2esMQ";
        string chatId = "-1002599117583";

        try
        {
            var botClient = new TelegramBotClient(botToken);

            await botClient.SendPhoto(
                chatId: chatId,
                InputFileUrl.FromString(topic.ImgUrl),
                caption: "Описание: " + topic.Title + Environment.NewLine +
                         "Старт: " + topic.StartCost + Environment.NewLine +
                         "Блиц: " + topic.BlitzCost + Environment.NewLine +
                         "Ссылка: " + topic.Url,
                parseMode: ParseMode.Html);

            Console.WriteLine("Сообщение успешно отправлено!");
            await Task.Delay(1000);
        }
        catch (ApiRequestException ex)
        {
            Console.WriteLine($"Ошибка Telegram API: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Общая ошибка: {ex.Message}");
        }
    }

    //-----------------------------------------------------------------
    //-----------------------------------------------------------------

    public static async Task SendMessageAsync(string messageText)
    {
        string botToken = "7891717896:AAEECgQrQsu-l1oedkyc7Wqab4Adjx2esMQ";
        string chatId = "-1002599117583";

        using var httpClient = new HttpClient();

        string url = $"https://api.telegram.org/bot{botToken}/sendMessage?" +
                        $"chat_id={chatId}&" +
                        $"text={Uri.EscapeDataString(messageText)}";

        try
        {
            var response = await httpClient.GetAsync(url);
            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                Console.WriteLine("Сообщение отправлено!");
            else
                Console.WriteLine($"Ошибка: {responseContent}");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при отправке: {ex.Message}");
        }
    }
}

