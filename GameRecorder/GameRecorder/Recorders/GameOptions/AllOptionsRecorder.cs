using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmongUs.GameOptions;

namespace GameRecorder.Recorders.GameOptions
{
    public static class AllOptionsRecorder
    {
        public static NanoMessage Message { get; set; }
    }
    [HarmonyPatch(typeof(GameManager),nameof(GameManager.StartGame))]
    public static class GameOptionsReader
    {
        public static void Postfix(AmongUsClient __instance, PlayerControl player)
        {
            AllOptionsRecorder.Message.Set(NanoMessageType.Common, $"当前游戏选项：{GameOptionsManager.Instance.CurrentGameOptions.ToString()}");
            ModMain.OptionData.AppendLine(AllOptionsRecorder.Message.ToString());
        }
    }
}
