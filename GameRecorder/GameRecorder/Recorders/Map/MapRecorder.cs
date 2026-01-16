using HarmonyLib;
using static Il2CppSystem.Xml.Schema.FacetsChecker.FacetsCompiler;

namespace GameRecorder.Recorders.Map
{
    public static class MapRecorder
    {
        public static MapType Map { get; set; }
        public static NanoMessage Message {  get; set; }
    }
    public enum MapType : int
    {
        Skeld = 0,
        MiraHQ = 1,
        Polus = 2,
        Airship = 3,
        Fungle = 4
    }
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.OnEnable))]
    public static class ShipStatusUpdate
    {
        public static void Prefix(ShipStatus __instance)
        {
            MapRecorder.Map = (MapType)__instance.Type;
            MapRecorder.Message.Set(NanoMessageType.Common, $"已切换至地图：{MapRecorder.Map.ToString()})");
            ModMain.OptionData.AppendLine(MapRecorder.Message.ToString());
        }
    }

    [HarmonyPatch(typeof(PolusShipStatus), nameof(PolusShipStatus.OnEnable))]
    public static class PolusShipStatusUpdate
    {
        public static void Prefix(PolusShipStatus __instance)
        {
            MapRecorder.Map = MapType.Polus;
            MapRecorder.Message.Set(NanoMessageType.Common, $"已切换至地图：{MapRecorder.Map.ToString()})");
            ModMain.OptionData.AppendLine(MapRecorder.Message.ToString());
        }
    }

    [HarmonyPatch(typeof(AirshipStatus), nameof(AirshipStatus.OnEnable))]
    public static class AirshipStatusUpdate
    {
        public static void Prefix(AirshipStatus __instance)
        {
            MapRecorder.Map = MapType.Airship;
            MapRecorder.Message.Set(NanoMessageType.Common, $"已切换至地图：{MapRecorder.Map.ToString()})");
            ModMain.OptionData.AppendLine(MapRecorder.Message.ToString());
        }
    }
    [HarmonyPatch(typeof(FungleShipStatus), nameof(FungleShipStatus.OnEnable))]
    public static class FungleShipStatusUpdate
    {
        public static void Prefix(FungleShipStatus __instance)
        {
            MapRecorder.Map = MapType.Fungle;
            MapRecorder.Message.Set(NanoMessageType.Common, $"已切换至地图：{MapRecorder.Map.ToString()})");
            ModMain.OptionData.AppendLine(MapRecorder.Message.ToString());
        }
    }
}
