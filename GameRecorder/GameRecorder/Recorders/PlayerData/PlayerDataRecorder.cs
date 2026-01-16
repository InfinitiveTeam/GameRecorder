//using System.Linq;
//using System.Text;
//using AmongUs.GameOptions;
//using HarmonyLib;
//using UnityEngine;

//namespace GameRecorder.Recorders.PlayerData
//{
//    public static class PlayerDataRecorder
//    {
//        public static NanoMessage Message { get; set; }
//        public static StringBuilder SendData { get; } = new StringBuilder();
//        public static StringBuilder ImposterData { get; } = new StringBuilder();
//        public static StringBuilder WifiTaskData { get; } = new StringBuilder();
//        public static bool InMeeting { get; set; }
//        public static MapType Map { get; set; }
//    }

//    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
//    public static class PlayerDataRecorderPatch
//    {
//        public static void Postfix(PlayerControl __instance)
//        {
//            if (__instance == null || ShipStatus.Instance == null)
//                return;

//            if (__instance != null && __instance == PlayerControl.LocalPlayer)
//            {
//                RecordPlayerData(__instance);
//            }
//        }

//        private static void RecordPlayerData(PlayerControl player)
//        {
//            bool areLightsOff = false;
//            bool imposter = IsPlayerImposter(player.Data);
//            string output = "";

//            // 玩家位置
//            Vector2 position = player.GetTruePosition();
//            output += $"{position.x} {position.y}\n";

//            // 角色
//            output += imposter ? "impostor\n" : "crewmate\n";

//            // 任务列表
//            var currentTasks = player.myTasks.ToArray();
//            output += RecordTaskList(currentTasks, ref areLightsOff, player.Data.IsDead);

//            // 任务位置
//            output += RecordTaskLocations(currentTasks, player.Data.IsDead);

//            // 任务进度
//            output += RecordTaskProgress(currentTasks, imposter);

//            // 地图ID
//            output += PlayerDataRecorder.Map.ToString() + "\n";

//            // 是否死亡
//            output += player.Data.IsDead ? "1\n" : "0\n";

//            // 是否在会议中
//            output += PlayerDataRecorder.InMeeting ? "1\n" : "0\n";

//            // 玩家速度
//            output += GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.PlayerSpeedMod) + "\n";

//            // 玩家颜色ID
//            output += player.CurrentOutfit.ColorId + "\n";

//            // 房间信息
//            output += GetPlayerRoom(position) + "\n";

//            // 灯光状态
//            output += areLightsOff ? "1\n" : "0\n";

//            // 其他玩家信息
//            output += RecordOtherPlayersInfo(position, player);

//            // 存储数据
//            PlayerDataRecorder.SendData.Clear();
//            PlayerDataRecorder.SendData.Append(output);

//            if (imposter)
//            {
//                RecordImposterData(position, player);
//            }
//        }

//        private static string RecordTaskList(PlayerTask[] tasks, ref bool areLightsOff, bool isDead)
//        {
//            string result = "[";
//            bool skip = false;

//            if (isDead && tasks.Length > 0 &&
//                TranslateSystemTypes(tasks[0].StartAt).Equals("Hallway") &&
//                TranslateTaskTypes(tasks[0].TaskType).Equals("Submit Scan"))
//            {
//                skip = true;
//            }

//            var taskList = tasks.Where((task, index) => !(skip && index == 0))
//                               .Select(task =>
//                               {
//                                   if (TranslateTaskTypes(task.TaskType).Equals("Fix Lights"))
//                                       areLightsOff = true;
//                                   return TranslateTaskTypes(task.TaskType);
//                               });

//            result += string.Join(", ", taskList);
//            result += "]\n";
//            return result;
//        }

//        private static string RecordTaskLocations(PlayerTask[] tasks, bool isDead)
//        {
//            return "[任务位置数据]\n";
//        }

//        private static string RecordTaskProgress(PlayerTask[] tasks, bool isImposter)
//        {
//            if (isImposter) return "[]\n";

//            string result = "[";
//            result += "]\n";
//            return result;
//        }

//        private static string GetPlayerRoom(Vector2 position)
//        {
//            return "当前房间";
//        }

//        private static string RecordOtherPlayersInfo(Vector2 currentPosition, PlayerControl currentPlayer)
//        {
//            return "[其他玩家信息]\n";
//        }

//        private static void RecordImposterData(Vector2 position, PlayerControl player)
//        {
//            string imposterOutput = "[内鬼数据]\n";
//            PlayerDataRecorder.ImposterData.Clear();
//            PlayerDataRecorder.ImposterData.Append(imposterOutput);
//        }

//        private static string TranslateTaskTypes(TaskTypes type)
//        {
//            return type.ToString();
//        }

//        private static string TranslateSystemTypes(SystemTypes type)
//        {
//            return type.ToString();
//        }

//        private static bool IsPlayerImposter(GameData.PlayerInfo playerInfo)
//        {
//            if (playerInfo.Role == null) return false;
//            return playerInfo.Role.TeamType == RoleTeamTypes.Impostor;
//        }
//    }
//}