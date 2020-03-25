using HarmonyLib;
using Verse;

namespace sd_adv_powergen
{
    [StaticConstructorOnStartup]
    public class Patch
    {
        static Patch()
        {
            var harmony = new Harmony("yamabuki.sd.advpowergen");
            harmony.PatchAll();
        }
    }
}