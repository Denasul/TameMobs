using System.Collections;
using UnityEngine;
using HarmonyLib;

// This script makes the Ideas tab for the mod red

namespace Denny_TameMobsNS
{
    public class Denny_BlueprintGroupSolver : MonoBehaviour
    {
        private static bool subscribedToEvent = false;

        public static ExpandableLabel DennyTameMobsIdeasTab;

        public void InitializeBlueprintCategory()
        {

            GameObject? ideaParent = Denny_GameObjectUtil.FindByPathIncludeInactive("Canvas/GameScreen/Sidebar/IdeasTab/Scroll View/Viewport/IdeaParent");

            if (ideaParent == null)
            {
                Debug.Log("IdeaParent not found!");
                Destroy(gameObject);
                return;
            }

            // includeInactive = true
            ExpandableLabel[] labels = ideaParent.GetComponentsInChildren<ExpandableLabel>(true);

            foreach (var label in labels)
            {
                // Tag can be null on some labels
                if (label == null || label.Tag == null)
                    continue;

                if (label.Tag is not BlueprintGroup group)
                    continue;

                if (group != Denny_TameMobs.blueprintCatValue)
                    continue;

                if (!subscribedToEvent)
                {
                    label.OnExpand += Label_OnExpand;
                    subscribedToEvent = true;
                }



                //label.SetText(Denny_TameMobs.BlueprintTabName);

                // IMPORTANT: get Colorizer from the label (or its GameObject), not from your solver object
                var colorizer = label.GetComponent<Colorizer>();
                if (colorizer == null)
                {
                    Debug.LogWarning("[TameMobs] Colorizer missing on the matched ExpandableLabel, skipping color.");
                    continue;
                }

                colorizer.Color = Denny_TameMobs.BlueprintTabColor;

                // SetColors might be non-public; also might not exist depending on game version
                var setColors = AccessTools.Method(colorizer.GetType(), "SetColors");
                if (setColors != null)
                {
                    setColors.Invoke(colorizer, null);
                    Debug.Log("[TameMobs] SetColors() ran.");
                }
                else
                {
                    Debug.LogWarning("[TameMobs] Colorizer.SetColors not found.");
                }
            }

            Destroy(gameObject);
        }

        private void Label_OnExpand()
        {
            // runs when the tab is expanded
        }


    }
}