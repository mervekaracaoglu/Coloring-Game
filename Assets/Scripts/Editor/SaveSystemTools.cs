#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

namespace ColoringApp.Editor
{
    public static class SaveSystemTools
    {
        [MenuItem("ColoringApp/Reset Progress")]
        public static void ResetProgress()
        {
            // Clear PlayerPrefs
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();

            // Delete saved PNGs
            string[] files = Directory.GetFiles(Application.persistentDataPath, "level_*_save.png");
            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"Could not delete file {file}: {e.Message}");
                }
            }

            Debug.Log("Coloring App Progress Reset Successfully.");
        }
    }
}
#endif
