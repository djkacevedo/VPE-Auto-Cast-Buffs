using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace VPEAutoCastBuffs
{
    public static class ThingHelper
    {
        public static bool ThingIsDamaged(Thing thing)
        {
            if (thing is ThingWithComps thingWithComps)
            {
                return thingWithComps.HitPoints < (thingWithComps.MaxHitPoints * 0.5);
            }

            return false;
        }

        public static bool ThingIsBiocoded(Thing thing)
        {
            if (thing == null)
            {
                return false; // Return false if the thing is null to avoid null reference exception
            }

            // Get the CompBiocodable component
            CompBiocodable biocodeComp = thing.TryGetComp<CompBiocodable>();

            // Check if the component exists and the item is biocoded
            return biocodeComp != null && biocodeComp.Biocoded;
        }
        
        public static List<Thing> GetThingsInNamedStockpile(Map map, string stockpileName)
        {
            List<Thing> items = new List<Thing>();

            if (map == null || string.IsNullOrEmpty(stockpileName))
            {
                return items; // Return empty list if pawn is not on a map or stockpileName is null/empty
            }

            foreach (var thing in map.listerThings.AllThings)
            {
                if ((thing.def.IsWeapon || thing.def.IsApparel) && ThingInNamedStockpile(thing, stockpileName))
                {
                    items.Add(thing);
                }
            }

            return items;
        }

        public static bool ThingInNamedStockpile(Thing thing, string stockpileName)
        {
            // Check if the thing is in a stockpile zone with the specified name
            Zone zone = thing.Position.GetZone(thing.Map);
            return zone is Zone_Stockpile && zone.label.ToLower().Contains(stockpileName.ToLower());
        }

    }
}
