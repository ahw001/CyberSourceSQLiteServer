using System;

namespace CybsClass.EntityModels;

public class CybsClassContextLogger
{
    public static void WriteLine(string message)
    {

        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "../CybsClass.Cybersource/Logs", $"CybsClass.txt");

        try 
        {
            StreamWriter textFile = File.CreateText(path);
            textFile.WriteLine(message);
            textFile.Close();
        }
        catch (Exception ex)
        { 
            Console.WriteLine(ex.Message);
        } 

    }
}

