using System.Text;
using AmongUs.GameOptions;
using HarmonyLib;

namespace GameRecorder.Recorders.GameState
{
    public static class GameStateRecorder
    {
        public static NanoMessage Message { get; set; }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.OnGameStart))]
    public static class GameStartRecorderPatch
    {
        public static void Postfix()
        {
            GameStateRecorder.Message.Set(NanoMessageType.Common, "游戏开始");
            ModMain.GameData.AppendLine(GameStateRecorder.Message.ToString());
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.OnGameEnd))]
    public static class GameEndRecorderPatch
    {
        public static void Postfix()
        {
            GameStateRecorder.Message.Set(NanoMessageType.Common, "游戏结束");
            ModMain.GameData.AppendLine(GameStateRecorder.Message.ToString());
        }
    }
}