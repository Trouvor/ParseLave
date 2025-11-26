using System;
using System.Globalization;
using HtmlAgilityPack;
using static ParseLave2.LotParser;

namespace ParseLave2;

public class LotParser
{
    public class LotData
    {
        public string Title { get; set; }
        public string StartCost { get; set; }
        public string StepCost { get; set; }
        public string BlitzCost { get; set; }
        public string Description { get; set; }

        public string Year { get; set; }
        public string EndAuc { get; set; }

        public string Url { get; set; }
        public string ID { get; set; }
        public string ImgUrl { get; set; }
    }

    public static LotData ParseLotData(string input)
    {
        var result = new LotData();
        var lines = input.Split('\n');

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var parts = line.Split(new[] { ':' }, 2);
            if (parts.Length < 2) continue;

            var key = parts[0].Trim();
            var value = parts[1].Trim();

            switch (key)
            {
                case "Старт":
                    result.StartCost = value;
                    break;
                case "Шаг":
                    result.StepCost = value;
                    break;
                case "Блиц":
                    result.BlitzCost = value;
                    break;
                case "Описание лота":
                    result.Description = value;
                    break;
                case "Год":
                    result.Year = value;
                    break;
                case "Окончание":
                    result.EndAuc = value;
                    break;
            }
        }

        return result;
    }
}

public class ForumTopic
{
    public string ID { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
    public string ImgUrl { get; set; }
    public string StartCost { get; set; }
    public string StepCost { get; set; }
    public string BlitzCost { get; set; }
    public string Description { get; set; }
    public string Year { get; set; }
    public string EndAuc { get; set; }

    public static string GetIdFromUrl(string url)
    {
        if (url == null || url.Length == 0)
            return "";

        var parts = url.Split(new string[] { "t=" }, StringSplitOptions.None);
        if (parts.Length < 2)
            return "";

        return parts[1].Trim();
    }

    public ForumTopic(LotData dataLot)
    {
        Title = dataLot.Title;
        Url = dataLot.Url;
        ImgUrl = dataLot.ImgUrl;
        ID = dataLot.ID;
        StartCost = dataLot.StartCost;
        StepCost = dataLot.StepCost;
        BlitzCost = dataLot.BlitzCost;
        Description = dataLot.Description;
        Year = dataLot.Year;
        EndAuc = dataLot.EndAuc;
    }

    public static async void PrintResults(List<ForumTopic> topics)
    {
        // Вывод результатов
        foreach (var topic in topics)
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
            if (topic.ID != "2874269")
                await Telegramm.SendMessageAsync(topic.Title);
        }
    }

    public static string GetImgUrl(string url)
    {
        if (url == null || url.Length == 0)
            return "";

        return "";
    }
}

class ForumParser
{
    private readonly HttpClient _httpClient;

    public ForumParser()
    {
        _httpClient = new HttpClient();
        // Можно добавить заголовки, если нужно
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
    }

    public async Task<List<ForumTopic>> ParseForumPage(string url)
    {
        var topics = new List<ForumTopic>();

        try
        {
            // Загрузка HTML страницы
            var html = await _httpClient.GetStringAsync(url);

            // Загрузка HTML в HtmlDocument
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            // Парсинг тем форума            
            var topicNodes = htmlDocument.DocumentNode.SelectNodes("//td[contains(@class, 'row1')]");

            if (topicNodes != null)
            {
                foreach (var node in topicNodes)
                {
                    var titleNode = node.SelectSingleNode(".//a[contains(@class, 'topictitle')]");
                    if (titleNode != null)
                    {
                        var title = titleNode.GetAttributeValue("title", "");

                        var lotData = LotParser.ParseLotData(title);

                        lotData.Title = titleNode.InnerText.Trim();
                        lotData.Url = new Uri(new Uri(url), titleNode.GetAttributeValue("href", "")).AbsoluteUri;
                        lotData.ID = ForumTopic.GetIdFromUrl(lotData.Url);

                        var pars = new ForumParser();
                        await pars.GetImgUrl(lotData);

                        var topic = new ForumTopic(lotData);
                        topics.Add(topic);
                    }
                }
            }
            else
            {
                Console.WriteLine("Не удалось найти темы на странице.");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при парсинге: {ex.Message}");
        }

        return topics;
    }

    public async Task GetImgUrl(LotData dataLot)
    {
        if (!(dataLot.Url == null || dataLot.Url.Length == 0))
        {
            try
            {
                // Загрузка HTML страницы
                var html = await _httpClient.GetStringAsync(dataLot.Url.Replace("amp;", ""));

                // Загрузка HTML в HtmlDocument
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                var topicNodes = htmlDocument.DocumentNode.SelectNodes("//td[contains(@class, 'row2')]");

                if (topicNodes != null)
                {
                    foreach (var topicNode in topicNodes)
                    {
                        if (topicNode != null)
                        {
                            var imgNodes = topicNode.SelectNodes("//a[contains(@href, 'lave.pro/pic/')]");
                            if (imgNodes == null) continue;

                            foreach (var imgNode in imgNodes)
                            {
                                dataLot.ImgUrl = imgNode.GetAttributeValue("href", "");
                                if (dataLot.ImgUrl != null)
                                    return;
                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при парсинге: {ex.Message}");
            }
        }
    }
}