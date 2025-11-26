using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParseLave2;

public class IntervalService : IDisposable
{
    private Timer _timer;

    public IntervalService()
    {
        // Таймер запускается сразу с интервалом 5 минут
        _timer = new Timer(ExecuteScheduledMethod, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
    }

    private async void ExecuteScheduledMethod(object state)
    {
        Console.WriteLine($"[{DateTime.Now}] Выполняется запланированный метод...");

        var parser = new ForumParser();
        var topics = new List<ForumTopic>();

        var collTopics = DataBase.InitBase();
        //collTopics.DeleteMany(p => 1 == 1);
        //return;

        topics = await parser.ParseForumPage("https://lave.pro/viewforum.php?f=152");
        //https://lave.pro/viewforum.php?f=116 - погодовка ссср
        //https://lave.pro/viewforum.php?f=119 - юб до 98
        //https://lave.pro/viewforum.php?f=149 - боны
        await Task.Delay(1000);

        foreach (var topic in topics)
        {
            var results = collTopics.Query()
                           .Where(p => p.Id == topic.ID)
                           .ToList();

            if (results.Count == 0)
            {
                Console.WriteLine($"ID: {topic.ID}");
                Console.WriteLine($"Title: {topic.Title}");
                Console.WriteLine($"URL: {topic.Url}");
                Console.WriteLine($"ImgURL: {topic.ImgUrl}");
                Console.WriteLine($"Start cost: {topic.StartCost}");
                Console.WriteLine($"Step cost: {topic.StepCost}");
                Console.WriteLine($"Blitz cost: {topic.BlitzCost}");
                Console.WriteLine($"Description: {topic.Description}");
                Console.WriteLine($"Year: {topic.Year}");
                Console.WriteLine($"EndAuc: {topic.EndAuc}");
                Console.WriteLine("-----------------------");

                collTopics.Insert(new Topics { Id = topic.ID, Title = topic.Title });

                if (topic.ID != "2874269")
                    await Telegramm.SendMessageAsyncWithBot(topic);
            }
        }

    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
