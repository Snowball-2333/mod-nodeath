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
            Log.LogInfo("God Mode Mod Loaded!"); // 纯英文日志
        }
    }

    // 注意：这里我们加了参数 float __0 或 float amount，来精确拦截带参数的版本
    [HarmonyPatch(typeof(PlayerControl), "HPChange")]
    public static class Patch_HPChange
    {
        [HarmonyPrefix]
        public static bool Prefix(PlayerControl __instance, float amount)
        {
            // 如果扣血值小于0（说明是掉血）
            if (amount < 0f)
            {
                // 直接返回 false 跳过原版扣血方法，不扣血了！
                BepInEx.Logging.Logger.CreateLogSource("HPMod").LogInfo("Blocked Damage: " + amount);
                return false; 
            }
            
            return true;
        }
    }
}
