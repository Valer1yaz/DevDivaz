using System.IO;
using UnityEngine;

public class JsonGameDataRepository : IGameDataRepository
{
    private readonly string saveFilePath;

    public JsonGameDataRepository()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
    }

    public void Save(GameSaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
    }

    public GameSaveData Load()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            return JsonUtility.FromJson<GameSaveData>(json);
        }
        return null;
    }
}