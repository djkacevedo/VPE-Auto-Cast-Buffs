using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System;
using Verse;
using VFECore.Abilities;

namespace VPEAutoCastBuffs
{
    [HarmonyBefore(new string[] { "legodude17.mvcf" })]
    [HarmonyPatch(typeof(Pawn), "TryGetAttackVerb")]
    public static class Pawn_TryGetAttackVerb_Postfix
    {
        [HarmonyPostfix]
        public static void Postfix(Pawn __instance, ref Verb __result, Thing target)
        {
            CompAbilities compAbilities = __instance.TryGetComp<CompAbilities>();
            if (compAbilities == null)
            {
                return;
            }

            string reason;
            List<Verb_CastAbility> list = (from ab in compAbilities.LearnedAbilities
                                           where ab.AutoCast && !AbilityIsBlacklisted(ab) && ab.IsEnabledForPawn(out reason) && (target == null || ab.CanHitTarget(target))
                                           select ab.verb).ToList();
            if (list.NullOrEmpty())
            {
                return;
            }

            if (target != null)
            {
                if ((from x in list
                     where x.ability.AICanUseOn(target)
                     select x into ve
                     select new Tuple<Verb, float>(ve, ve.ability.Chance)).AddItem(new Tuple<Verb, float>(__result, 1f)).TryRandomElementByWeight((Tuple<Verb, float> t) => t.Item2, out var result))
                {
                    __result = result.Item1;
                }
            }
            else
            {
                Verb verb = list.AddItem(__result).MaxBy((Verb ve) => ve.verbProps.range);
                __result = verb;
            }
        }

        private static bool AbilityIsBlacklisted(Ability ab)
        {
            switch (ab.def.defName)
            {
                case "VPE_StealVitality": return true;
                case "VPEP_BrainLeech": return true;
                default: return false;
            }
        }
    }
}

