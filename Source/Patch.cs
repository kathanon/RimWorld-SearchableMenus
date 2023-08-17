using FloatSubMenus;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace SearchableMenus;
[HarmonyPatch]
public static class Patch {
    private static readonly Type subMenu = AccessTools.Inner(typeof(FloatSubMenu), "FloatSubMenuInner");

    [HarmonyPrefix]
    [HarmonyPatch(typeof(FloatMenu), MethodType.Constructor, typeof(List<FloatMenuOption>))]
    public static void FloatMenu_Ctor(List<FloatMenuOption> options, FloatMenu __instance) {
        bool recursive = Main.Recursive;
        if (recursive && subMenu.IsInstanceOfType(__instance)) return;
        if (options.OfType<FloatMenuSearch>().Any()) return;
        if ((recursive ? CountOptions(options) : options.Count) < Main.Threshold) return;

        options.Insert(0, new FloatMenuSearch(recursive));
    }

    private static int CountOptions(List<FloatMenuOption> options) 
        => options.Sum(x => (x is FloatSubMenu sub) ? CountOptions(sub.Options) : 1);
}
