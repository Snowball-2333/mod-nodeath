using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace HPThresholdMod
{
    [BepInPlugin("com.example.hpthreshold", "HP Threshold Mod", "1.0.0")]
    public class Plugin : BasePlugin
    {
        public override void Load()
        {
            Harmony.CreateAndPatchAll(typeof(Patch_HPChange));
            Log.LogInfo("HPThresholdMod 已加载");
        }
    }

    [HarmonyPatch(typeof(PlayerControl), "HPChange")]
    public static class Patch_HPChange
    {
        [HarmonyPostfix]
public static void Postfix(PlayerControl __instance)
{
    // 加一句日志，如果游戏扣血，日志里就会疯狂刷这句话
    BepInEx.Logging.Logger.CreateLogSource("HPMod").LogInfo("触发扣血方法！当前HP: " + __instance.currentHP);

    if (__instance.currentHP <= 0f)
    {
        __instance.currentHP = -1f; 
    }
}
    }
}
