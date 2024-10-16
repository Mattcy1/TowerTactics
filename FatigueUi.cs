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

namespace Ui
{
    [RegisterTypeInIl2Cpp(false)]
    public class FatigueUi : MonoBehaviour
    {
        public static FatigueUi? instance = null;
        public ModHelperImage? Image;
        public static Tower? selectedTower1 = null;
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
                var panel = rect.gameObject.AddModHelperPanel(new("Panel_", 0, -1100, 750, 450), ModContent.GetSpriteReference<TowerTactics.TowerTactics>("BarBackground").GetGUID());
                instance = panel.AddComponent<FatigueUi>();
                string towerModelBaseId = selectedTower1.towerModel.baseId;
                int Fatigue = TowerTactics.TowerTactics.TowerFatigue[towerModelBaseId];
                float barSize = 25 * Fatigue;
                var bar = panel.AddImage(new("Image_", 0, 0, barSize, 450), ModContent.GetSpriteReference<TowerTactics.TowerTactics>("bar").GetGUID());
                instance.Image = bar;
            }
        }
    }
}