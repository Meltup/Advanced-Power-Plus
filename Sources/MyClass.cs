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
        private static readonly Vector2 sd_adv_powergen_BarSize = new Vector2(2.3f, 0.14f);

        private static readonly Material BarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.5f, 0.475f, 0.1f));

        private static readonly Material BarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.15f, 0.15f, 0.15f));

        private float MinPowerOutput => 0f;
        private float MaxPowerOutput => -this.Props.basePowerConsumption;

        protected override float DesiredPowerOutput
        {
            get
            {
                return Mathf.Lerp(this.MinPowerOutput, this.MaxPowerOutput, this.parent.Map.skyManager.CurSkyGlow) * this.RoofedPowerOutputFactor;
            }
        }

        private float RoofedPowerOutputFactor => this.parent.OccupiedRect().Average(cell => this.parent.Map.roofGrid.Roofed(cell) ? 0 : 1f);

        public override void PostDraw()
        {
            GenDraw.FillableBarRequest r = default;
            r.center = this.parent.DrawPos + Vector3.up * 0.1f;
            r.size = sd_adv_powergen_BarSize;
            r.fillPercent = this.PowerOutput / this.MaxPowerOutput;
            r.filledMat = BarFilledMat;
            r.unfilledMat = BarUnfilledMat;
            r.margin = 0.15f;
            Rot4 rotation = this.parent.Rotation;
            rotation.Rotate(RotationDirection.Clockwise);
            r.rotation = rotation;
            GenDraw.DrawFillableBar(r);
        }
    }

    [DefOf]
    public static class ThingDefOf
    {
        public static ThingDef sd_adv_powergen_WatermillGenerator;
    }
}