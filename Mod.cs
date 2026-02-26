using HarmonyLib;
using System;
using System.Collections;
using UnityEngine;

namespace Denny_TameMobsNS
{
    public class Denny_TameMobs : Mod
    {
        // CONSTANTS

        public static readonly string BlueprintTabNameTerm = "tamemobs_blueprint_group_title";
        public static UIColor BlueprintTabColor;
        public static  Color BlueprintGroup255Value = new Color32(255, 170, 135, 255);

        public static readonly string TamedMobPrefix = "Tamed ";
        public static readonly string TamedMobSuffix = "";
        //


        // CONFIG
        public static ConfigEntry<bool> dragUntamedMobs;
        //

        public static readonly string[] TamableMobsIDs =
        {
            "imp",
            "demon",
            "bear",
            "demon_lord",
            "mosquito",
            "momma_crab",
            "dark_elf",
            "elf",
            "elf_archer",
            "sadness_demon",
            "orc_wizard",
            "enchanted_shroom",
            "ent",
            "rat",
            "giant_rat", //
            "seagull",
            "shark",
            "skeleton",
            "snake",
            "ghost",
            "ghoul",
            "giant_snail",
            "goblin",
            "goblin_archer",
            "goblin_shaman",
            "tiger",

        };

        public static Denny_TameMobs instance;

        public static BlueprintGroup blueprintCatValue;

        private void Awake()
        {
            instance = this;
            ExtendEnums();
        }

        public override void Ready()
        {
            Logger.Log("Ready() reached!");

            AddConfigOptions();

            AddCardRecipesToPacks();
            LoadHarmony();
            

        }




        public void LoadBlueprintGroupSolver()
        {
            var rat = new GameObject("Denny's Blueprint Solver");
            UnityEngine.Object.DontDestroyOnLoad(rat);
            Denny_BlueprintGroupSolver blueprintSolver = rat.AddComponent<Denny_BlueprintGroupSolver>();
            blueprintSolver.InitializeBlueprintCategory();
        }

        private void AddCardRecipesToPacks()
        {
            BoosterInjector.InjectAllCardBags();
        }

        private void LoadHarmony()
        {

            var h = new HarmonyLib.Harmony("denny_tamemobs");
            h.PatchAll();

            foreach (var m in h.GetPatchedMethods())
            {
                Logger.Log("Patched: " + m.DeclaringType?.FullName + "." + m.Name);
            }

            Logger.Log("Harmony PatchAll called!");
        }

        private void ExtendEnums()
        {
            blueprintCatValue = EnumHelper.ExtendEnum<BlueprintGroup>("Denny_TameMobs");

            BlueprintTabColor = EnumHelper.ExtendEnum<UIColor>("Denny_LightRed");
            Logger.Log("Color set to" + BlueprintTabColor.ToString());
        }

        private void ForgetAllTameKnowledge()
        {
            var save = WorldManager.instance.CurrentSave;

            save.FoundCardIds.Remove("denny_rumor_blood_extraction");
            save.FoundCardIds.Remove("denny_rumor_taming");

            save.FoundCardIds.Remove("denny_blueprint_tesla_tower");
            save.FoundCardIds.Remove("denny_blueprint_tesla_coil");
            save.FoundCardIds.Remove("denny_blueprint_copper_coil");
            save.FoundCardIds.Remove("denny_blueprint_summoning_ring");
            save.FoundCardIds.Remove("denny_blueprint_occult_sigil");
            save.FoundCardIds.Remove("denny_blueprint_sigil_cage");
            save.FoundCardIds.Remove("denny_blueprint_blood_extractor");
            save.FoundCardIds.Remove("denny_blueprint_infernal_slaughterhouse");

            GameScreen.instance.UpdateIdeasLog();
        }

        private void AddConfigOptions()
        {

               ForgetKnowledgeButton();
               AllowUntamedDragToggle();

        }

        private void ForgetKnowledgeButton()
        {
            var forgetBtnEntry = Config.GetEntry<bool>("forget_all_tame_knowledge_button", true);

            // 2) Hide the default UI for the entry (so it doesn't show as a checkbox)
            forgetBtnEntry.UI.Name = "this should never appear";
            forgetBtnEntry.UI.Hidden = true;

            // 3) When Mod Options UI is generated, inject our own button
            forgetBtnEntry.UI.OnUI = (ConfigEntryBase entry) =>
            {
                var btn = UnityEngine.Object.Instantiate(
                    PrefabManager.instance.ButtonPrefab,
                    ModOptionsScreen.instance.ButtonsParent
                );

                btn.transform.localScale = Vector3.one;
                btn.transform.localPosition = Vector3.zero;
                btn.transform.localRotation = Quaternion.identity;

                btn.TextMeshPro.text = SokLoc.Translate("tamemobs_config_forget_button");
                btn.TooltipText = SokLoc.Translate("tamemobs_config_forget_button_tooltip");
                btn.Clicked += () =>
                {
                    ForgetAllTameKnowledge();
                };
            };
        }

        private void AllowUntamedDragToggle()
        {
            dragUntamedMobs = Config.GetEntry<bool>("enable_drag_untamed", false);

            dragUntamedMobs.UI.Name = SokLoc.Translate("tamemobs_config_dragging_name");
            dragUntamedMobs.UI.Tooltip = SokLoc.Translate("tamemobs_config_dragging_tooltip");

        }

    }
}