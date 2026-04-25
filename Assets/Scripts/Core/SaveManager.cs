using System.IO;
using UnityEngine;

namespace ColoringApp.Core
{
    public static class SaveManager
    {
        private const string COMPLETE_KEY_PREFIX = "LevelComplete_";
        private const string COLOR_INDEX_KEY = "LastSelectedColorIndex";

        public static void SaveLevelComplete(int levelId)
        {
            PlayerPrefs.SetInt(COMPLETE_KEY_PREFIX + levelId, 1);
            PlayerPrefs.Save();
        }

        public static bool IsLevelComplete(int levelId)
        {
            return PlayerPrefs.GetInt(COMPLETE_KEY_PREFIX + levelId, 0) == 1;
        }

        public static void SaveLastColorIndex(int index)
        {
            PlayerPrefs.SetInt(COLOR_INDEX_KEY, index);
            PlayerPrefs.Save();
        }

        public static int GetLastColorIndex()
        {
            return PlayerPrefs.GetInt(COLOR_INDEX_KEY, 0);
        }



        //saves the current texture state so progress is not lost
        public static void SaveTextureState(int levelId, Texture2D texture)
        {
            byte[] pngData = texture.EncodeToPNG();
            string path = GetSaveFilePath(levelId);
            File.WriteAllBytes(path, pngData);
        }

        //loads the saved texture state if it exists
        public static Texture2D LoadTextureState(int levelId)
        {
            string path = GetSaveFilePath(levelId);
            if (File.Exists(path))
            {
                byte[] pngData = File.ReadAllBytes(path);
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(pngData);
                return tex;
            }
            return null;
        }



        private static string GetSaveFilePath(int levelId)
        {
            return Path.Combine(Application.persistentDataPath, $"level_{levelId}_save.png");
        }
    }
}
