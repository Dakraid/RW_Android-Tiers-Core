using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal class MemoryThoughtHandler_Patch
    {
        private static bool shouldSkipCurrentMemory(ThoughtDef memDef, MemoryThoughtHandler __instance)
        {
            var cas = __instance.pawn.TryGetComp<CompAndroidState>();
            return Utils.IgnoredThoughtsByAllAndroids.Contains(memDef.defName) && Utils.ExceptionAndroidList.Contains(__instance.pawn.def.defName)
                   || Utils.lastButcheredPawnIsAndroid
                   || cas != null && cas.isSurrogate && cas.surrogateController == null
                   || Utils.pawnCurrentlyControlRemoteSurrogate(__instance.pawn)
                   || Utils.IgnoredThoughtsByBasicAndroids.Contains(memDef.defName) &&
                   (Utils.ExceptionAndroidListBasic.Contains(__instance.pawn.def.defName) || __instance.pawn.story.traits.HasTrait(Utils.traitSimpleMinded));
        }

        [HarmonyPatch(typeof(MemoryThoughtHandler), "TryGainMemoryFast")]
        public class TryGainMemoryFast
        {
            [HarmonyPrefix]
            public static bool Listener(ThoughtDef mem, MemoryThoughtHandler __instance)
            {
                try
                {
                    if (shouldSkipCurrentMemory(mem, __instance)) return false;
                    return true;
                }
                catch (Exception e)
                {
                    Log.Message("[ATPP] MemoryThoughtHandler.TryGainMemoryFast : " + e.Message + " - " + e.StackTrace);
                    return true;
                }
            }
        }

        /*
        * Postfix evitant que les droids est le debuff "Eat without table"
        */
        [HarmonyPatch(typeof(MemoryThoughtHandler), "TryGainMemory")]
        [HarmonyPatch(new[] {typeof(Thought_Memory), typeof(Pawn)}, new[] {ArgumentType.Normal, ArgumentType.Normal})]
        public class TryGainMemory_Patch
        {
            [HarmonyPrefix]
            public static bool Listener(Thought_Memory newThought, Pawn otherPawn, MemoryThoughtHandler __instance)
            {
                try
                {
                    //Log.Message(newThought.def.defName);

                    //Si android (en général) alors squeeze de certains moods OU alors si android surrogate suppression de TOUT les moods (si pas controllé) DE MEME si controlleur avec connection en cours désaction des MOODS
                    if (shouldSkipCurrentMemory(newThought.def, __instance)) return false;
                    return true;
                }
                catch (Exception e)
                {
                    Log.Message("[ATPP] MemoryThoughtHandler.TryGainMemory : " + e.Message + " - " + e.StackTrace);
                    return true;
                }
            }
        }
    }
}