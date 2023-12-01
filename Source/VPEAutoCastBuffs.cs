using HarmonyLib;
using Verse;

namespace VPEAutoCastBuffs
{
    [StaticConstructorOnStartup]
    public static class VPEAutoCastBuffs
    {
        static VPEAutoCastBuffs()
        {
            Harmony harmony = new Harmony("NetzachSloth.VanillaExpandedFrameworkRealAutoAbilities");
            harmony.PatchAll();
        }
    }
}