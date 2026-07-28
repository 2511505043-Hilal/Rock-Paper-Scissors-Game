using System.IO;
using System.Text.Json;
namespace RockPaperScissors;
public static class SaveLoad
{
    public static void SaveData(List<Players> players)
    {
        JsonSerializerOptions options = new JsonSerializerOptions();
        options.WriteIndented = true;

        string json = JsonSerializer.Serialize(players, options);
        
        File.WriteAllText("Players.json", json);
    }
    public static List<Players> LoadData()
    {
        if (!File.Exists("Players.json"))
        return new List<Players>();

        string json = File.ReadAllText("Players.json");

        return JsonSerializer.Deserialize<List<Players>>(json) ?? new List<Players>();
    }
}   