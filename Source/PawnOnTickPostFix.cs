using HarmonyLib;
using Verse;
using VFECore.Abilities;
using System.Collections.Generic;
using Ability = VFECore.Abilities.Ability;
using RimWorld.Planet;
using RimWorld;
using VanillaPsycastsExpanded;
using System.Security.Cryptography;
using static UnityEngine.GraphicsBuffer;
using System;
using System.Linq;

namespace VPEAutoCastBuffs
{
    [HarmonyPatch(typeof(Pawn), "Tick")]
    public static class PawnOnTickPostFix
    {
        [HarmonyPostfix]
        public static void Postfix(Pawn __instance)
        {
            if (!__instance.IsColonistPlayerControlled) return;

            int ticksGame = Find.TickManager.TicksGame;

            if (__instance.Drafted)
            {
                if (ticksGame % 30 == 0)
                {
                    // Add logic for drafted pawns here
                }
            }
            else if (ticksGame % 600 == 0)
            {
                if (!PawnHelper.PawnCanCast(__instance)) return;

                List<Ability> abilities = __instance.GetComp<CompAbilities>()?.LearnedAbilities;
                if (abilities == null) return;

                foreach (Ability ability in abilities)
                {
                    if (ability.autoCast && PsycastingHandler.HandleAbilityUndrafted(__instance, ability))
                    {
                        return;
                    }
                }
            }
        }
    }
}
