using HarmonyLib;
using Verse;
using VFECore.Abilities;

namespace VPEAutoCastBuffs
{
    [HarmonyAfter("OskarPotocki.VFECore")]
    [HarmonyPatch(typeof(Pawn), nameof(Pawn.TryGetAttackVerb))]
    public static class TryGetAttackVerb_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(Pawn __instance, ref Verb __result, Thing target)
        {
            Verb_CastAbility verb_CastAbility = __result as Verb_CastAbility;

            if (verb_CastAbility != null)
            {
                if (verb_CastAbility.ability.def.defName == "VPE_StealVitality"
                    || verb_CastAbility.ability.def.defName == "VPEP_BrainLeech")
                {
                    __result = null;
                }
            }
        }
    }
}