using HarmonyLib;
using Verse;
using UnityEngine;
using RimWorld;
using HugsLib;
using HugsLib.Settings;

namespace SearchableMenus {
    [StaticConstructorOnStartup]
    public class Main : ModBase {
        public static SettingHandle<int>  Threshold;
        public static SettingHandle<bool> Recursive;

        public static Main Instance { get; private set; }

        public override string ModIdentifier => Strings.ID;

        public Main() {
            Instance = this;
        }

        public override void DefsLoaded() {
            Threshold = Settings.GetHandle("threshold", Strings.ThresholdTitle, Strings.ThresholdDesc, 15);
            Recursive = Settings.GetHandle("recursive", Strings.RecursiveTitle, Strings.RecursiveDesc, true);

            Threshold.CustomDrawer = ThresholdSlider;
        }

        private static bool ThresholdSlider(Rect rect) {
            int old = Threshold;
            Text.Anchor = TextAnchor.MiddleRight;
            Widgets.Label(rect.LeftPartPixels(24f), old.ToString());
            rect.xMin += 28f;
            int value = Mathf.RoundToInt(Widgets.HorizontalSlider(rect, old, 1, 60, roundTo: 1));
            GenUI.ResetLabelAlign();
            if (value != old) {
                Threshold.Value = value;
                return true;
            }
            return false;
        }
    }
}
