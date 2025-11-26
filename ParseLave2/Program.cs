using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using LiteDB;
using ParseLave2;
using static ParseLave2.LotParser;

namespace Parse;

class Programm
{
    static async Task Main(string[] args)
    {
        using (var service = new IntervalService())
        {
            Console.WriteLine("Сервис запущен. Метод будет вызываться каждые 5 минут.");
            Console.WriteLine("Нажмите любую клавишу для остановки...");
            Console.ReadKey();
        }

        //var parser = new ForumParser();
        //var topics = new List<ForumTopic>();

        //var collTopics = DataBase.InitBase();
        //collTopics.DeleteMany(p => 1==1);
        ////return;

        //topics = await parser.ParseForumPage("https://lave.pro/viewforum.php?f=152");

        //await Task.Delay(1000);

        //foreach (var topic in topics)
        //{
        //    var results = collTopics.Query()
        //                   .Where(p => p.Id == topic.ID)
        //                   .ToList();

        //    if (results.Count == 0)
        //    {
        //        Console.WriteLine($"ID: {topic.ID}");
        //        Console.WriteLine($"Title: {topic.Title}");
        //        Console.WriteLine($"URL: {topic.Url}");
        //        Console.WriteLine($"ImgURL: {topic.ImgUrl}");
        //        Console.WriteLine($"Start cost: {topic.StartCost}");
        //        Console.WriteLine($"Step cost: {topic.StepCost}");
        //        Console.WriteLine($"Blitz cost: {topic.BlitzCost}");
        //        Console.WriteLine($"Description: {topic.Description}");
        //        Console.WriteLine($"Year: {topic.Year}");
        //        Console.WriteLine($"EndAuc: {topic.EndAuc}");
        //        Console.WriteLine("-----------------------");

        //        collTopics.Insert(new Topics { Id = topic.ID, Title = topic.Title });

        //        if (topic.ID != "2874269")
        //            await Telegramm.SendMessageAsyncWithBot(topic);
        //    }                
        //}
    }
}

