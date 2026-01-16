using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmongUs.GameOptions;
using GameRecorder.Recorders.Map;
using HarmonyLib;
using Il2CppSystem.Runtime.InteropServices.ComTypes;
using OpenAI.Chat;
using UnityEngine.Tilemaps;

namespace GameRecorder.Recorders.Meeting
{
    public static class MeetingRecorder
    {
        public static NanoMessage Message { get; set; }
        public static NanoMessage Message2 { get; set; }
        public static NanoMessage ChatMessage { get; set; }
    }
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Awake))]
    public static class MeetingOpenUpdate
    {
        public static void Postfix(MeetingHud __instance)
        {
            var players = PlayerControl.AllPlayerControls.ToArray().Where(p => !p.Data.IsDead);
            MeetingRecorder.Message.Set(NanoMessageType.Common, $"会议已开启，当前存活玩家列表：【{players}】");
            ModMain.GameData.AppendLine(MeetingRecorder.Message.ToString());
        }
    }

    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Close))]
    public static class MeetingCloseUpdate
    {
        public static void Postfix(MeetingHud __instance)
        {
            MeetingRecorder.Message2.Set(NanoMessageType.Common, $"会议结束");
            ModMain.GameData.AppendLine(MeetingRecorder.Message2.ToString());
        }
    }
    [HarmonyPatch(typeof(ChatController), nameof(ChatController.AddChat))]
    public static class Chat_Pipe_Patch
    {
        public static void Prefix(ChatController __instance, PlayerControl sourcePlayer, System.String chatText)
        {
            if (!sourcePlayer.Data.IsDead)
            {
                MeetingRecorder.ChatMessage.Set(NanoMessageType.Common, $"");
                MeetingRecorder.ChatMessage.Set(NanoMessageType.Common, ModMain.TranslateColorName(sourcePlayer.Data.ColorName) + "说：" + chatText);
                ModMain.GameData.AppendLine(MeetingRecorder.ChatMessage.ToString());
            }
        }
    }
}
