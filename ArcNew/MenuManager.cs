﻿using System.Collections.Generic;
using System.Linq;
using ArcAnnihilation.Panels;
using Ensage.Common.Enums;
using Ensage.Common.Menu;
using SharpDX;
using AbilityId = Ensage.Common.Enums.AbilityId;

namespace ArcAnnihilation
{
    internal class MenuManager
    {
        public static readonly Menu Menu = new Menu("Arc Annihilation", "Arc Annihilationv2.0", true,
            "npc_dota_hero_arc_warden", true).SetFontColor(Color.DarkOrange);

        public static MenuItem DefaultCombo;
        public static MenuItem TempestCombo;
        public static MenuItem SparkSpamCombo;
        public static MenuItem SparkSpamTempestOnlyCombo;
        public static MenuItem AutoPushingCombo;

        private static readonly List<string> AbilityList = new List<string>
        {
            "arc_warden_tempest_double",
            "arc_warden_spark_wraith",
            "arc_warden_magnetic_field",
            "arc_warden_flux"
        };

        private static readonly Dictionary<string, byte> Items = new Dictionary<string, byte>
        {
            {"item_hurricane_pike", 1},
            {"item_mask_of_madness", 7},
            {"item_ancient_janggo", 1},
            {"item_dagon", 2},
            /*{"item_dagon_2", 2},
            {"item_dagon_3", 2},
            {"item_dagon_4", 2},
            {"item_dagon_5", 2},*/
            {"item_blink", 5},
            {"item_orchid", 4},
            {"item_manta", 1},
            {ItemId.item_abyssal_blade.ToString(), 5},
            {ItemId.item_diffusal_blade.ToString(), 5},
            {ItemId.item_black_king_bar.ToString(), 5},
            {"item_arcane_boots", 1},
            {"item_guardian_greaves", 1},
            {"item_shivas_guard", 1},
            {"item_ethereal_blade", 3},
            {"item_bloodthorn", 4},
            {"item_soul_ring", 4},
            {"item_blade_mail", 4},
            {"item_veil_of_discord", 4},
            {"item_heavens_halberd", 1},
            {"item_necronomicon", 2},
            /*{"item_necronomicon_2", 2},
            {"item_necronomicon_3", 2},*/
            {"item_mjollnir", 1},
            //{ "item_hurricane_pike",1},

            {"item_sheepstick", 5},
            {"item_urn_of_shadows", 5}

            /*{"item_dust", 4}*/
        };

        public static bool DebugInGame => Menu.Item("Dev.Text.enable").GetValue<bool>();
        public static bool DebugInConsole => Menu.Item("Dev.Text2.enable").GetValue<bool>();

        public static bool IsEnable => GetBool("Enable");
        public static bool DoCombo => GetKey("Combo.Key");
        public static bool CloneCombo => GetKey("Combo.Tempest.Key");
        public static bool SparkSpam => GetKey("Combo.Sparks.Key");
        public static bool OrbWalkType => GetBool("OrbWalking.Type");

        public static bool MagneticField => GetBool("MagneticField.InFront");
        public static bool CustomComboPriorityHero => GetBool("customOrderHero");
        public static bool IsInfoPanelEnabled => GetBool("InfoPanel.Enable");
        public static bool CustomComboPriorityTempest => GetBool("customOrderTempest");
        public static float GetItemPanelSize => GetSlider("ItemPanelSize");
        public static bool AutoPushingTargetting => GetBool("AutoPushing.AutoTargetting");
        public static float GetInfoPanelSize => GetSlider("InfoPanel.Size");
        public static float GetPushLaneSelectorSize => GetSlider("PushLaneSelector.Size");
        public static bool UseTravels => GetBool("AutoPushing.Travels");
        public static bool IsAbilityEnabledTempest(AbilityId id) => GetToggle("spellTempest", id.ToString());
        public static bool IsAbilityEnabled(AbilityId id) => GetToggle("spellHero", id.ToString());

        public static bool IsItemEnabledTempest(ItemId id) =>
            GetToggle("itemTempestEnable",
                id == ItemId.item_necronomicon_2 || id == ItemId.item_necronomicon_3
                    ? ItemId.item_necronomicon.ToString()
                    : id == ItemId.item_dagon_2 || id == ItemId.item_dagon_3 || id == ItemId.item_dagon_4 ||
                      id == ItemId.item_dagon_5
                        ? ItemId.item_dagon.ToString()
                        : id == ItemId.item_diffusal_blade_2 ? ItemId.item_diffusal_blade.ToString() : id.ToString());

        public static bool IsItemEnabled(ItemId id) =>
            GetToggle("itemHeroEnable",
                id == ItemId.item_necronomicon_2 || id == ItemId.item_necronomicon_3
                    ? ItemId.item_necronomicon.ToString()
                    : id == ItemId.item_dagon_2 || id == ItemId.item_dagon_3 || id == ItemId.item_dagon_4 ||
                      id == ItemId.item_dagon_5
                        ? ItemId.item_dagon.ToString()
                        : id == ItemId.item_diffusal_blade_2 ? ItemId.item_diffusal_blade.ToString() : id.ToString());

        public static uint GetItemOrderHero(ItemId id) => GetPriority("itemHero",
            id == ItemId.item_necronomicon_2 || id == ItemId.item_necronomicon_3
                ? ItemId.item_necronomicon
                : id == ItemId.item_dagon_2 || id == ItemId.item_dagon_3 || id == ItemId.item_dagon_4 ||
                  id == ItemId.item_dagon_5
                    ? ItemId.item_dagon
                    : id == ItemId.item_diffusal_blade_2 ? ItemId.item_diffusal_blade : id);

        public static uint GetItemOrderTempest(ItemId id) => GetPriority("itemTempest",
            id == ItemId.item_necronomicon_2 || id == ItemId.item_necronomicon_3
                ? ItemId.item_necronomicon
                : id == ItemId.item_dagon_2 || id == ItemId.item_dagon_3 || id == ItemId.item_dagon_4 ||
                  id == ItemId.item_dagon_5
                    ? ItemId.item_dagon
                    : id == ItemId.item_diffusal_blade_2 ? ItemId.item_diffusal_blade : id);

        public static void Init()
        {
            Menu.AddItem(new MenuItem("Enable", "Enable").SetValue(true));
            var settings = new Menu("Settings", "Settings");

            DefaultCombo = new MenuItem("Combo.Key", "Main+Tempest Combo").SetValue(new KeyBind('0', KeyBindType.Press));
            TempestCombo =
                new MenuItem("Combo.Tempest.Key", "Tempest Combo").SetValue(new KeyBind('0', KeyBindType.Toggle));
            SparkSpamCombo = new MenuItem("Combo.Sparks.Key", "Spark Spam").SetValue(new KeyBind('0', KeyBindType.Press));
            SparkSpamTempestOnlyCombo =
                new MenuItem("Combo.Sparks.Tempest.Key", "[TempestOnly] Spark Spam").SetValue(new KeyBind('0',
                    KeyBindType.Toggle));
            AutoPushingCombo =
                new MenuItem("Combo.AutoPushing.Key", "Auto Pushing").SetValue(new KeyBind('0', KeyBindType.Toggle));
            var keys = new Menu("Hotkeys", "Hotkeys");
            keys.AddItem(DefaultCombo).ValueChanged += Core.ComboStatusChanger;
            keys.AddItem(TempestCombo).ValueChanged += Core.ComboStatusChanger;
            keys.AddItem(SparkSpamCombo).ValueChanged += Core.ComboStatusChanger;
            keys.AddItem(SparkSpamTempestOnlyCombo).ValueChanged += Core.ComboStatusChanger;
            keys.AddItem(AutoPushingCombo).ValueChanged += Core.ComboStatusChanger;
            var panels = new Menu("Panels", "Panels");
            settings.AddItem(new MenuItem("OrbWalking.Type", "[Orbwalking] move to target").SetValue(false))
                .SetTooltip("or to mouse");
            settings.AddItem(
                new MenuItem("MagneticField.InFront", "Use Magnetic Field in front of ur hero").SetValue(true));

            var usages = new Menu("Using in combo", "usages");
            var itemPanel = new Menu("Item Panel", "ItemPanel");
            itemPanel.AddItem(new MenuItem("itemPanel.Enable", "Enable").SetValue(true)).ValueChanged +=
                ItemPanel.OnChange;
            itemPanel.AddItem(new MenuItem("ItemPanelSize", "Size").SetValue(new Slider(40, 20, 70)));
            var pushLaneSelectorPanel = new Menu("AutoPushLaneSelector Panel", "PushLaneSelector");
            pushLaneSelectorPanel.AddItem(new MenuItem("AutoPushLaneSelector.Enable", "Enable").SetValue(true))
                .ValueChanged += PushLaneSelector.OnChange;
            pushLaneSelectorPanel.AddItem(
                new MenuItem("PushLaneSelector.Size", "Text Size").SetValue(new Slider(30, 1, 70)));
            var autoPushingSettings = new Menu("AutoPushing Settings", "AutoPushingSettings");
            autoPushingSettings.AddItem(new MenuItem("AutoPushing.Travels", "Enable travel boots").SetValue(true));
            autoPushingSettings.AddItem(new MenuItem("AutoPushing.AutoTargetting", "Do tempest combo").SetValue(true))
                .SetTooltip("if you find any target in attack range");

            var infoPanel = new Menu("Info Panel", "InfoPanel");
            infoPanel.AddItem(new MenuItem("InfoPanel.Enable", "Enable").SetValue(true)).ValueChanged +=
                InfoPanel.OnChange;
            infoPanel.AddItem(new MenuItem("InfoPanel.Size", "Text Size").SetValue(new Slider(30, 1, 70)));

            var mainHero = new Menu("For Main Hero", "mainHero");
            var spellHero = new Menu("Spells:", "HeroSpells");
            var itemHero = new Menu("Items:", "HeroItems");

            var tempest = new Menu("Tempest", "tempest");
            var spellTempest = new Menu("Spells:", "TempestSpells");
            var itemTempest = new Menu("Items:", "TempestItems");
            var dict = AbilityList.ToDictionary(item => item, item => true);
            var dict2 = AbilityList.ToDictionary(item => item, item => true);
            var itemListHero = Items.Keys.ToList().ToDictionary(item => item, item => true);
            var itemListTempest = Items.Keys.ToList().ToDictionary(item => item, item => true);
            itemHero.AddItem(
                new MenuItem("itemHeroEnable", "Toggle Items:").SetValue(new AbilityToggler(itemListHero)));
            itemHero.AddItem(new MenuItem("customOrderHero", "Use Custom Order").SetValue(false));
            itemHero.AddItem(new MenuItem("itemHero", "Items:").SetValue(new PriorityChanger(Items.Keys.ToList())));

            itemTempest.AddItem(
                new MenuItem("itemTempestEnable", "Toggle Items:").SetValue(new AbilityToggler(itemListTempest)));
            itemTempest.AddItem(new MenuItem("customOrderTempest", "Use Custom Order").SetValue(false));
            itemTempest.AddItem(
                new MenuItem("itemTempest", "Items:").SetValue(new PriorityChanger(Items.Keys.ToList())));

            spellHero.AddItem(new MenuItem("spellHero", "Ability:").SetValue(new AbilityToggler(dict)));
            spellTempest.AddItem(new MenuItem("spellTempest", "Ability:").SetValue(new AbilityToggler(dict2)));

            var devolper = new Menu("Developer", "Developer");
            devolper.AddItem(new MenuItem("Dev.Text.enable", "Debug messages ingame").SetValue(false));
            devolper.AddItem(new MenuItem("Dev.Text2.enable", "Debug messages in console").SetValue(false));


            settings.AddSubMenu(usages);
            usages.AddSubMenu(mainHero);
            usages.AddSubMenu(tempest);
            mainHero.AddSubMenu(spellHero);
            mainHero.AddSubMenu(itemHero);
            tempest.AddSubMenu(spellTempest);
            tempest.AddSubMenu(itemTempest);
            settings.AddSubMenu(panels);

            Menu.AddSubMenu(settings);
            settings.AddSubMenu(keys);
            panels.AddSubMenu(itemPanel);
            panels.AddSubMenu(infoPanel);
            panels.AddSubMenu(pushLaneSelectorPanel);
            settings.AddSubMenu(autoPushingSettings);

            Menu.AddSubMenu(devolper);
            Menu.AddToMainMenu();
        }

        private static float GetSlider(string item)
        {
            return Menu.Item(item).GetValue<Slider>().Value;
        }

        private static bool GetKey(string item)
        {
            return Menu.Item(item).GetValue<KeyBind>().Active;
        }

        private static bool GetBool(string item)
        {
            return Menu.Item(item).GetValue<bool>();
        }

        private static bool GetToggle(string name, string item)
        {
            return Menu.Item(name).GetValue<AbilityToggler>().IsEnabled(item);
        }

        private static bool GetToggle(string name, AbilityId item)
        {
            return Menu.Item(name).GetValue<AbilityToggler>().IsEnabled(item.ToString());
        }

        private static uint GetPriority(string name, ItemId item)
        {
            return Menu.Item(name).GetValue<PriorityChanger>().GetPriority(item.ToString());
        }
    }
}