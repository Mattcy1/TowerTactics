using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using System.Collections.Generic;
using UnityEngine;
using BTD_Mod_Helper.Extensions;
using MelonLoader;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Unity;
using System.Collections;
using UnityEngine.UI;
using BTD_Mod_Helper;
using static TowerTactics.TowerTactics;
using TowerTactics;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Unity.UI_New.InGame.TowerSelectionMenu;
using TowerTactics.Values;
using System;

namespace Ui
{
    [RegisterTypeInIl2Cpp(false)]
    public class FatigueUi : MonoBehaviour
    {
        public static FatigueUi? instance = null;
        public ModHelperImage? Image;
        public static Tower? tower = null;
        public void Close()
        {
            if (gameObject)
            {
                gameObject.Destroy();
                instance = null;
            }
        }
        public static void CreatePanel()
        {
            if (instance != null)
            {
                instance.Close();
            }

            if (InGame.instance != null)
            {
                RectTransform rect = InGame.instance.uiRect;
                var panel = rect.gameObject.AddModHelperPanel(new("Panel_", 0, -1100, 750, 450), VanillaSprites.BrownInsertPanel);
                panel.AddImage(new("Outline", 750, 450), ModContent.GetTextureGUID<TowerTactics.TowerTactics>("BarBackground"));

                panel.AddText(new("Text", 0, 175, 750, 100), "Fatigue").Text.enableAutoSizing = true;

                instance = panel.AddComponent<FatigueUi>();
                int Fatigue = TowerFatigue[tower.towerModel.baseId];
                float barSize = 750f / MaxFatigue * Fatigue;
                var bar = panel.AddImage(new("Image_", 0, 0, barSize, 450), ModContent.GetSpriteReference<TowerTactics.TowerTactics>("bar").GetGUID());
                instance.Image = bar;

                bar.AddText(new("Text", 350, 150), $"{Fatigue}/{MaxFatigue.GetValue()}\n (+{Math.Round(Fatigue * 0.05f, 2)}s/shot)", 45);
                //instance.Image.RectTransform.localScale = new(barSize, instance.Image.RectTransform.localScale.y, instance.Image.RectTransform.localScale.z);
            }
        }
    }
}