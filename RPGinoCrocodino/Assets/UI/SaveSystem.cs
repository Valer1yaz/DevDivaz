using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/save.json";

    [System.Serializable]
    public class SaveData
    {
        public Vector3 playerPosition;
        public float playerHP;
        public int magicCharges;
        // Добавьте данные мобов и инвентаря
    }

    public static void SaveGame(PlayerController player)
    {
        SaveData data = new SaveData
        {
            playerPosition = player.transform.position,
            playerHP = player.GetComponent<Health>().currentHP,
            magicCharges = player.GetComponent<MagicSystem>().currentCharges
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, json);
    }

    public static void LoadGame(PlayerController player)
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            player.transform.position = data.playerPosition;
            player.GetComponent<Health>().currentHP = data.playerHP;
            player.GetComponent<MagicSystem>().currentCharges = data.magicCharges;
        }
    }
}