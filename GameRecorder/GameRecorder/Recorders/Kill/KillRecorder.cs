using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmongUs.GameOptions;
using HarmonyLib;
using UnityEngine;

namespace GameRecorder.Recorders.Kill
{
    public static class KillRecorder
    {
        public static NanoMessage Message { get; set; }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.MurderPlayer))]
    public static class KillRecorderPatch
    {
        public static void Postfix(PlayerControl __instance, PlayerControl target)
        {
            if (__instance == null || target == null) return;

            var killerColor = ModMain.TranslateColorName(__instance.Data.ColorName);
            var victimColor = ModMain.TranslateColorName(target.Data.ColorName);

            KillRecorder.Message.Set(NanoMessageType.Kill, $"{killerColor} 击杀了 {victimColor}");
            ModMain.GameData.AppendLine(KillRecorder.Message.ToString());
        }
    }
}