// SaveManager.cs - handles JSON persistence for the game
using System.IO;
using UnityEngine;
using BookSellGame.Data;
using BookSellGame.Core;

namespace BookSellGame.Core
{
    public static class SaveManager
    {
        private static readonly string SavePath = Path.Combine(Application.persistentDataPath, "save.json");

        public static SaveData CurrentSave { get; private set; } = new SaveData();

        public static void Save()
        {
            try
            {
                string json = JsonUtility.ToJson(CurrentSave, true);
                File.WriteAllText(SavePath, json);
                Debug.Log($"Game saved to {SavePath}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to save game: {ex.Message}");
            }
        }

        public static void Load()
        {
            if (File.Exists(SavePath))
            {
                try
                {
                    string json = File.ReadAllText(SavePath);
                    CurrentSave = JsonUtility.FromJson<SaveData>(json);
                    Debug.Log($"Game loaded from {SavePath}");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Failed to load save file: {ex.Message}");
                    CurrentSave = new SaveData();
                }
            }
            else
            {
                Debug.Log("No save file found, creating default save data.");
                CurrentSave = new SaveData();
                Save();
            }
        }

        public static void ResetSave()
        {
            CurrentSave = new SaveData();
            Save();
        }
    }
}
