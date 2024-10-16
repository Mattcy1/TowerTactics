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

namespace Ui
{
    [RegisterTypeInIl2Cpp(false)]
    public class TaxUI : MonoBehaviour
    {
        public static ShopButton? instance = null;
        public ModHelperInputField? input;
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
            if (ShopUi.instance != null)
            {
                ShopUi.instance.Close();
            }
            if (InGame.instance != null)
            {
                RectTransform rect = InGame.instance.uiRect;
                var panel = rect.gameObject.AddModHelperPanel(new("Panel_", 0, 0, 1000, 500), ModContent.GetSpriteReference<TowerTactics.TowerTactics>("ShopBackgroud").GetGUID());
                instance = panel.AddComponent<ShopButton>();
                var text = panel.AddText(new("Title_", 0, 50, 600, 450), $"Would you like to pay the towers's fee of {InGame.instance.GetCashManager().cash.Value * 0.3f}? If you refuse, action will be taken.", 60);
                var Yes = panel.AddButton(new("Button_", -225, -225, 300, 150), VanillaSprites.GreenBtnLong, new System.Action(() =>
                {
                    {
                        InGame.instance.AddCash(-InGame.instance.GetCashManager().cash.Value * 0.3f);
                        PopupScreen.instance?.ShowOkPopup("You have paid the towers's fee.");
                        instance.Close();
                    }
                }));
                Yes.AddText(new("Title_", 0, 0, 300, 150), "Yes", 45);
                var No = panel.AddButton(new("Button_", 225, -225, 300, 150), VanillaSprites.RedBtnLong, new System.Action(() =>
                {
                    PopupScreen.instance?.ShowOkPopup("You have refused to pay the towers's fee.");
                    foreach (var tower in InGame.instance.bridge.GetAllTowers().ToList())
                    {
                        tower.tower.worth = 0;
                        tower.tower.SellTower();
                        instance.Close();
                    }
                }));
                No.AddText(new("Title_", 0, 0, 300, 150), "No", 45);
            }
        }
    }
}