using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using System.Text;

namespace GameRecorder;

[BepInPlugin("ai.infinitive.gamerecorder", "Game Recorder", "1.0.0")]
[BepInProcess("Among Us.exe")]
public partial class ModMain : BasePlugin
{
    public Harmony Harmony { get; } = new("ai.infinitive.gamerecorder");
    public ConfigEntry<string> ApiKey { get; private set; }

    public static StringBuilder GameData = new StringBuilder();
    public static StringBuilder OptionData = new StringBuilder();

    public override void Load()
    {
        ApiKey = Config.Bind("ApiKey", "Name", ":>");
        Harmony.PatchAll();
        InitializeRecorders();
    }

    private void InitializeRecorders()
    {
    }

    public static int TranslateColorName(string colorName)
    {
        colorName = colorName.ToUpper().Replace("(", "").Replace(")", "");
        string[] COLOR_NAMES = {
                "红色", "蓝色", "绿色", "粉色",
                "橙色", "黄色", "黑色", "白色",
                "紫色", "棕色", "青色", "浅绿色",
                "玫红色", "浅粉色", "焦黄色", "灰色",
                "茶色", "珊瑚色"};

        return System.Array.IndexOf(COLOR_NAMES, colorName.ToUpper());
    }
}