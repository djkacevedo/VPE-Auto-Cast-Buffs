using HarmonyLib;
using System.Reflection;
using System;
using Verse;
using RimWorld;

namespace VPEAutoCastBuffs
{
    [StaticConstructorOnStartup]
    public static class VPEAutoCastBuffs
    {
        static VPEAutoCastBuffs()
        {
            Harmony harmony = new Harmony("NetzachSloth.VanillaExpandedFrameworkRealAutoAbilities");
            if (!UnregisterPatch(harmony, typeof(Pawn), "TryGetAttackVerb", HarmonyPatchType.Postfix, "OskarPotocki.VFECore"))
            {
                Log.Message("UnregisterPatch failed");
            }
            else
            {
                Log.Message("UnregisterPatch succeeded");
            }
            harmony.PatchAll();
        }

        public static bool UnregisterPatch(Harmony harmony, Type targetType, string methodName, HarmonyPatchType harmonyPatchType, String harmonyID)
        {
            try
            {
                // Get the MethodInfo for the private method using reflection
                MethodInfo targetMethod = targetType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

                if (targetMethod != null)
                {
                    // Attempt to remove the patch by specifying the MethodInfo
                    harmony.Unpatch(targetMethod, harmonyPatchType, harmonyID);
                    return true; // Patch removed successfully
                }
                else
                {
                    Log.Error($"Method '{methodName}' not found in type '{targetType.FullName}'.");
                    return false; // Method not found
                }
            }
            catch (Exception e)
            {
                // Handle any exceptions that may occur during unregistration
                Log.Error("Error unregistering Harmony patch: " + e);
                return false;
            }
        }
    }
}