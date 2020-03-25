using HarmonyLib;
using RimWorld;
using Verse;

namespace sd_adv_powergen
{
    [HarmonyPatch(typeof(CompPowerPlantWater), "RebuildCache")]
    public static class PatchCompPowerPlantWater_RebuildCache
    {
        public static void Postfix(CompPowerPlantWater __instance, ref bool ___waterDoubleUsed)
        {
            if (___waterDoubleUsed)
            {
                return;
            }

            // additional check for advanced version
            var advWatermills = __instance.parent.Map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.sd_adv_powergen_WatermillGenerator);
            foreach (IntVec3 current2 in __instance.WaterUseCells())
            {
                if (current2.InBounds(__instance.parent.Map))
                {
                    foreach (Building advWatermilll in advWatermills)
                    {
                        if (advWatermilll != __instance.parent && advWatermilll.GetComp<CompPowerPlantWater>().WaterUseRect().Contains(current2))
                        {
                            ___waterDoubleUsed = true;
                            break;
                        }
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(CompPowerPlantWater), "ForceOthersToRebuildCache")]
    public static class PatchCompPowerPlantWater_ForceOthersToRebuildCache
    {
        public static void Postfix(Map map)
        {
            // additional check for advanced version
            var advWatermillls = map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.sd_adv_powergen_WatermillGenerator);
            foreach (Building b in advWatermillls)
            {
                var comp = b.GetComp<CompPowerPlantWater>();
                ref var cacheDirty = ref AccessTools.FieldRefAccess<CompPowerPlantWater, bool>(comp, "cacheDirty");
                cacheDirty = true;
            }
        }
    }
}