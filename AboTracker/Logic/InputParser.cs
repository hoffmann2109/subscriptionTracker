using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using AboTracker.Model;

namespace AboTracker.Logic;

public static class InputParser
{
    private static List<Subscription> _aboList = [];
    
    public static List<Subscription> Subscriptions => _aboList;

    public static void InitializeArray()
    {
        // Load the data from the JSON file.
        _aboList = ParseJson();
    }

    private static List<Subscription> ParseJson() 
    {
        _aboList = [];
        string jsonFilePath = "/home/thomas/RiderProjects/AboTracker/AboTracker/aboList.json";

        try
        {
            string jsonString = File.ReadAllText(jsonFilePath);
            
            // Deserialize the JSON into the Array
            var subscriptions = JsonSerializer.Deserialize<List<Subscription>>(jsonString);
            return subscriptions ?? [];
        }
        catch (FileNotFoundException)
        {
            return []; 
        }
        catch (JsonException)
        {
            return [];
        }
    }

    public static string PrintSubscription(int i)
    {
        return Subscriptions[i].ToString();
    }
}