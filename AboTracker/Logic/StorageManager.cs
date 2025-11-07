using System.Text.Json;
using AboTracker.Model;

namespace AboTracker.Logic;

public static class StorageManager
{

    private static JsonSerializerOptions? _options;
        
    private static readonly string JsonFilePath = Path.Combine(
        AppContext.BaseDirectory, 
        "AppData/aboList.json"
    );
    
    public static List<Subscription> Subscriptions { get; private set; } = [];

    public static void InitializeArray()
    {
        Subscriptions = ParseJson();
    }

    private static List<Subscription> ParseJson() 
    {
        Subscriptions = [];
        
        try
        {
            var jsonString = File.ReadAllText(JsonFilePath);
            
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
            _options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            
            var jsonString = JsonSerializer.Serialize(Subscriptions, _options);
            
            File.WriteAllText(JsonFilePath, jsonString);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving JSON to {JsonFilePath}: {ex.Message}");
        }
    }
    
    public static bool RemoveSubscriptionByName(string name)
    {
        var subscriptionToRemove = Subscriptions.FirstOrDefault(sub => sub.Name == name);

        if (subscriptionToRemove == null) return false;
        
        Subscriptions.Remove(subscriptionToRemove);
        SaveListToJson();
        return true;

    }
}