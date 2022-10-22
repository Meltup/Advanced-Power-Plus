/*
 * Created by SharpDevelop.
 * User: sulusdacor
 * Date: 17.11.2016
 * Time: 10:32
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using RimWorld;            // RimWorld specific functions are found here (like 'Building_Battery')
using System.Linq;
using UnityEngine;         // Always needed
using Verse;               // RimWorld universal objects are here (like 'Building')

namespace sd_adv_powergen
{
    [StaticConstructorOnStartup]
    public class sd_adv_powergen_CompAdvPowerPlantSolar : CompPowerPlantSolar
    {
        protected override float DesiredPowerOutput
        {
            get
            {
                return Mathf.Lerp(0f, FullSunPower, this.parent.Map.skyManager.CurSkyGlow) * this.RoofedPowerOutputFactor;
            }
        }

        private float RoofedPowerOutputFactor
        {
            get
            {
                int num = 0;
                int num2 = 0;
                foreach (IntVec3 c in this.parent.OccupiedRect())
                {
                    num++;
                    if (this.parent.Map.roofGrid.Roofed(c))
                    {
                        num2++;
                    }
                }
                return (float)(num - num2) / (float)num;
            }
        }

        public override void PostDraw()
        {
            base.PostDraw();
            GenDraw.FillableBarRequest r = default(GenDraw.FillableBarRequest);
            r.center = this.parent.DrawPos + Vector3.up * 0.1f;
            r.size = BarSize;
            r.fillPercent = base.PowerOutput / FullSunPower;
            r.filledMat = PowerPlantSolarBarFilledMat;
            r.unfilledMat = PowerPlantSolarBarUnfilledMat;
            r.margin = 0.15f;
            Rot4 rotation = this.parent.Rotation;
            rotation.Rotate(RotationDirection.Clockwise);
            r.rotation = rotation;
            GenDraw.DrawFillableBar(r);
        }

        private const float FullSunPower = 3400f;

        private const float NightPower = 0f;

        private static readonly Vector2 BarSize = new Vector2(2.3f, 0.14f);

        private static readonly Material PowerPlantSolarBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.5f, 0.475f, 0.1f), false);

        private static readonly Material PowerPlantSolarBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.15f, 0.15f, 0.15f), false);
    }

    [DefOf]
    public static class ThingDefOf
    {
        public static ThingDef sd_adv_powergen_WatermillGenerator;
    }
}