using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using System.Collections.Generic;
using UnityEngine;
using BTD_Mod_Helper.Extensions;
using MelonLoader;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api;
using UnityEngine.UI;
using Il2CppAssets.Scripts.Unity.UI_New.Popups;
using Il2CppAssets.Scripts.Unity.UI_New;
using Il2CppAssets.Scripts.Utils;
using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Unity.Bridge;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Unity.UI_New.InGame.TowerSelectionMenu;
using TowerTactics;
using Il2CppAssets.Scripts.Unity.Towers;
using System.Buffers.Text;

namespace Ui
{
    [RegisterTypeInIl2Cpp(false)]
    public class FedUI : MonoBehaviour
    {
        public static ShopButton? instance = null;
        public ModHelperInputField? input;
        public static Il2CppAssets.Scripts.Simulation.Towers.Tower? selectedTower = null;
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

            RectTransform rect = InGame.instance.uiRect;
            var panel = rect.gameObject.AddModHelperPanel(new("Panel_", 0f, -950f, 0f, 0f), ModContent.GetSpriteReference<TowerTactics.TowerTactics>("ShopBackgroud").GetGUID());
            instance = panel.AddComponent<ShopButton>();
            var image = panel.AddImage(new("Image_", 0f, 200f, 450f, 250f), ModContent.GetSpriteReference<TowerTactics.TowerTactics>("ShopBackgroud").GetGUID());
            var Feed = panel.AddButton(new("Button_", 0, 200, 300, 150), VanillaSprites.GreenBtnLong, new System.Action(() =>
            {
                if (TowerTactics.Values.Values.banana >= 1)
                {
                    if (TowerTactics.TowerTactics.Popup == true)
                    {
                        PopupScreen.instance?.ShowOkPopup("You have fed the monkey!");
                    }
                    TowerTactics.Values.Values.banana = TowerTactics.Values.Values.banana - 1;
                    var Tower = selectedTower;
                    var Baseid = selectedTower.towerModel.baseId;
                    TowerFatigueManager.ResetRateAsync(Baseid);
                }
                else if (TowerTactics.Values.Values.banana == 0 && TowerTactics.TowerTactics.Popup == true)
                {
                    PopupScreen.instance?.ShowOkPopup("You do not have any banana buy one from the shop!");
                }
            }));
            Feed.AddText(new("Title_", 0, 0, 300, 150), "Feed Monkey", 45);
        }
    }
}
