using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;


namespace ParseLave2;

public class Topics
{
    public string Id { get; set; }
    public string Title { get; set; }
}

public  class DataBase
{    
    public static ILiteCollection<Topics> InitBase()
    {
        using var db = new LiteDatabase("Filename=Topics.db; Connection=Shared");
        return db.GetCollection<Topics>("proba");
    }
}

