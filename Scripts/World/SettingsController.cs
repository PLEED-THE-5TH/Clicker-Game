using Godot;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
[Serializable]
public class Settings
{
    public bool DebugScreen { get; set; }
    public bool CRTEffect { get; set; }

    public Settings()
    {
        DebugScreen = true;
        CRTEffect = false;
    }
}
public partial class SettingsController : Node
{
    // Making, Loading, Saving, Updating the world
    static string settingsFilePath = "user://settings.bat";
    public static Settings settings;
    public override void _Ready()
    {
        settings = LoadFromFile();
    }
    public static Settings LoadFromFile()
    {
        Settings settings = null;
        try {
            if (File.Exists(settingsFilePath)) {
                using (FileStream fileStream = new FileStream(settingsFilePath, FileMode.Open)) {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    settings = (Settings)binaryFormatter.Deserialize(fileStream);
                }
            }
            else {
                settings = new Settings();
            }
        }
        catch (Exception ex) {
            Console.WriteLine("Error loading settings: " + ex.Message);
        }
        return settings;
    }
    public static void SaveToFile(Settings settings)
    {
        try {
            using (FileStream fileStream = new FileStream(settingsFilePath, FileMode.Create)) {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, settings);
            }
        }
        catch (Exception ex) {
            Console.WriteLine("Error saving settings: " + ex.Message);
        }
    }
    public static void UpdateWorld()
    {
        World.ToggleDebugScreen();
    }

    // Updating each setting

}
