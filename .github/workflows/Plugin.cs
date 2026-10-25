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
            if (__instance.currentHP <= 0f)
            {
                __instance.currentHP = -1f; 
            }
        }
    }
}
