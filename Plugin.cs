using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using System.Reflection;

namespace HPThresholdMod
{
    [BepInPlugin("com.example.hpthreshold", "HP Threshold Mod", "1.0.0")]
    public class Plugin : BasePlugin
    {
        public override void Load()
        {
            Harmony.CreateAndPatchAll(typeof(Patch_HPChange));
            // 只要你看到这句英文，说明插件彻底活了
            Log.LogInfo(">>> HPThresholdMod Loaded successfully! <<<");
        }
    }

    [HarmonyPatch]
    public static class Patch_HPChange
    {
        // 精确锁定必须带一个 float 参数的那个 HPChange，防止被其他同名方法干扰
        static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(PlayerControl), "HPChange", new System.Type[] { typeof(float) });
        }

        [HarmonyPrefix]
        public static bool Prefix(PlayerControl __instance, float amount)
        {
            // 只要本次扣血会导致血量小于等于 0，我们就出手干预！
            if (amount < 0f && (__instance.currentHP + amount <= 0f))
            {
                // 暴力锁血：把血量强行拉回 1 点
                __instance.currentHP = 1f;
                
                // 刷新一下血条比例，让 UI 显示满血
                __instance.hpRate = 1f;
                
                // 手动通知 UI 更新（这一步如果报错，可以删掉这一行）
                // __instance.OnHPChanged?.Invoke(1f);

                // 返回 false 代表：跳过原版扣血代码，直接结束！
                return false; 
            }

            // 平时正常扣血，不干预
            return true;
        }
    }
}
