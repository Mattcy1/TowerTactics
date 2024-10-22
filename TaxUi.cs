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
using System.Linq;
using Il2CppAssets.Scripts.Unity.Bridge;
using System;

namespace Ui
{
    [RegisterTypeInIl2Cpp(false)]
    public class TaxUI : MonoBehaviour
    {
        public static ShopButton? instance = null;
        public ModHelperInputField? input;

        public static double taxedIncome = 0;

        public enum TaxMode 
        {
            CurrentCash,
            Income,
            IncomeAndSell
        }

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

                
                double taxFee;
                if(TowerTactics.TowerTactics.TaxType ==  TaxMode.CurrentCash)
                {
                    taxFee = InGame.instance.GetCash() * 0.3;
                }
                else
                {
                    taxFee = taxedIncome * 0.3;
                }

                taxFee = Math.Round(taxFee);

                var text = panel.AddText(new("Title_", 0, 50, 600, 450), $"Would you like to pay the towers's fee of {taxFee}? If you refuse, action will be taken.", 60);
                var Yes = panel.AddButton(new("Button_", -225, -225, 300, 150), VanillaSprites.GreenBtnLong, new System.Action(() =>
                {
                    InGame.instance.AddCash(-taxFee);
                    taxedIncome = 0;

                    if (InGame.instance.GetCash() >= 0)
                    {
                        PopupScreen.instance?.ShowOkPopup("You have paid the towers's fee.");
                    }
                    else if (TowerTactics.TowerTactics.TaxType ==  TaxMode.IncomeAndSell)
                    {
                        foreach (var tower in InGame.instance.bridge.GetAllTowers().ToList().Select(tts => tts.tower).OrderBy(tower => tower.worth))
                        {
                            tower.SellTower();

                            if (InGame.instance.GetCash() >= 0)
                            {
                                break; ;
                            }
                        }

                        PopupScreen.instance?.ShowOkPopup("You do not have enough money, some or all of your towers have been sold to pay off your debt.");
                    }
                    instance.Close();
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