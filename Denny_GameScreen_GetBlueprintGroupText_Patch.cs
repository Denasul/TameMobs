using HarmonyLib;

namespace Denny_TameMobsNS
{
    [HarmonyPatch(typeof(GameScreen), "GetBlueprintGroupText")]
    public static class Denny_GameScreen_GetBlueprintGroupText_Patch
    {
        static bool Prefix(BlueprintGroup group, ref string __result)
        {
            // The Extended Enum value
            if (group == Denny_TameMobs.blueprintCatValue)
            {
                __result = SokLoc.Translate(Denny_TameMobs.BlueprintTabNameTerm); // uses the term from Mod.cs in the loca sheet
                return false;
            }

            return true;
        }
    }
}