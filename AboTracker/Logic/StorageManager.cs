using System.Text.Json;
using AboTracker.Model;

namespace AboTracker.Logic;

public static class StorageManager
{
    private static List<Subscription> _aboList = [];
    
    private static readonly string JsonFilePath = Path.Combine(
        AppContext.BaseDirectory, 
        "aboList.json"
    );
    
    public static List<Subscription> Subscriptions => _aboList;
    
    public static void InitializeArray()
    {
        _aboList = ParseJson();
    }

    private static List<Subscription> ParseJson() 
    {
        _aboList = [];
        
        try
        {
            string jsonString = File.ReadAllText(JsonFilePath);
            
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

    public static void SaveListToJson()
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            
            string jsonString = JsonSerializer.Serialize(_aboList, options);
            
            File.WriteAllText(JsonFilePath, jsonString);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving JSON to {JsonFilePath}: {ex.Message}");
        }
    }
    
    public static bool RemoveSubscriptionByName(string name)
    {
        var subscriptionToRemove = _aboList.FirstOrDefault(sub => sub.Name == name);
        
        if (subscriptionToRemove != null)
        {
            _aboList.Remove(subscriptionToRemove);
            SaveListToJson();
            return true;
        }

        return false;
    }
}