using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace VPEAutoCastBuffs
{
    public static class PawnHelper
    {
        public static List<Pawn> GetPawnsWithHediff(List<Pawn> pawns, String hediffDefName)
        {
            return pawns.Where(pawn => PawnHasHediff(pawn, hediffDefName)).ToList();
        }

        public static List<Pawn> GetPawnsWithoutHediff(List<Pawn> pawns, String hediffDefName)
        {
            return pawns.Where(pawn => !PawnHasHediff(pawn, hediffDefName)).ToList();
        }

        public static List<Pawn> GetColonists(List<Pawn> pawns)
        {
            return pawns.Where(pawn => pawn.IsColonist).ToList();
        }

        public static List<Pawn> GetSlaves(List<Pawn> pawns)
        {
            return pawns.Where(pawn => pawn.IsSlaveOfColony).ToList();
        }

        public static List<Pawn> GetPrisoners(List<Pawn> pawns)
        {
            return pawns.Where(pawn => pawn.IsPrisoner).ToList();
        }

        public static List<Pawn> GetPawnsNotDown(List<Pawn> pawns)
        {
            return pawns.Where(pawn => !PawnIsDown(pawn)).ToList();
        }

        public static List<Pawn> GetPawnsDown(List<Pawn> pawns)
        {
            return pawns.Where(pawn => PawnIsDown(pawn)).ToList();
        }

        public static List<Pawn> GetPawnsWithMentalBreak(List<Pawn> pawns)
        {
            return pawns.Where(pawn => pawn.MentalState != null).ToList();
        }

        public static Pawn GetHighestSensitivity(List<Pawn> pawns)
        {
            return pawns.OrderByDescending(pawn => pawn.psychicEntropy.PsychicSensitivity).FirstOrDefault();
        }

        public static Pawn GetLowestSensitivity(List<Pawn> pawns)
        {
            return pawns.OrderByDescending(pawn => pawn.psychicEntropy.PsychicSensitivity).LastOrDefault();
        }

        public static bool PawnHasHediff(Pawn pawn, string hediffDefName)
        {
            return pawn?.health?.hediffSet?.hediffs.Any(hediff => hediff.def.defName == hediffDefName) ?? false;
        }

        public static bool PawnIsDown(Pawn pawn)
        {
            return pawn.CurJobDef == JobDefOf.LayDown || pawn.jobs.curDriver.asleep || pawn.Downed;
        }

        public static List<Pawn> GetPawnsInRange(Pawn referencePawn, float range)
        {
            return referencePawn.Map?.mapPawns.AllPawns
                .Where(pawn => pawn != referencePawn && !pawn.Dead && pawn.Spawned &&
                               (pawn.Position - referencePawn.Position).LengthHorizontal <= range)
                .ToList();
        }

        public static bool PawnCanCast(Pawn pawn)
        {
            if (pawn.CurJobDef == JobDefOf.LayDown) return false;
            if (pawn.Downed) return false;
            if (!pawn.HasPsylink) return false;
            if (pawn.psychicEntropy.CurrentPsyfocus < 0.5f) return false;
            if (pawn.jobs == null) return true;
            if (pawn.jobs.curDriver.asleep) return false;
            if (pawn.CurJob == null) return false;
            if (pawn.CurJob.def.defName == "VFEA_GotoTargetAndUseAbility") return false;
            return true;
        }

        public static List<Pawn> GetPawnsWithDamagedEquipment(List<Pawn> pawns)
        {
            if (pawns == null) return new List<Pawn>(); // Return empty list if colonists list is null

            return pawns
                .Where(colonist =>
                    (colonist.equipment?.Primary != null && ThingHelper.ThingIsDamaged(colonist.equipment.Primary)) ||
                    (colonist.apparel != null && colonist.apparel.WornApparel
                        .Any(apparel => ThingHelper.ThingIsDamaged(apparel) && !apparel.def.label.ToLower().Contains("warcasket")))
                )
                .ToList();
        }

        public static List<Pawn> GetLowJoyPawns(List<Pawn> pawns)
        {
            return pawns
                .Where(pawn => pawn.needs.TryGetNeed<Need_Joy>()?.CurLevel < 0.20f)
                .ToList();
        }

    }
}
