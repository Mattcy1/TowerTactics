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
using System;

namespace Ui
{
    [RegisterTypeInIl2Cpp(false)]
    public class ShopUi : MonoBehaviour
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

            if (InGame.instance != null)
            {
                RectTransform rect = InGame.instance.uiRect;
                var panel = rect.gameObject.AddModHelperPanel(new("Panel_", 0, 0, 1750, 1250), ModContent.GetSpriteReference<TowerTactics.TowerTactics>("ShopBackgroud").GetGUID());
                instance = panel.AddComponent<ShopButton>();
                var image = panel.AddImage(new("Image_", 0, 425, 350), ModContent.GetSpriteReference<TowerTactics.TowerTactics>("MediumAmmoIcon").GetGUID());
                var image1 = panel.AddImage(new("Image_", 650, 425, 350), ModContent.GetSpriteReference<TowerTactics.TowerTactics>("LargeAmmoIcon").GetGUID());
                var image2 = panel.AddImage(new("Image_", -650, 425, 350), ModContent.GetSpriteReference<TowerTactics.TowerTactics>("AmmoIcon").GetGUID());
                var Buy1000 = panel.AddButton(new("Button_", -0, 150, 300, 150), VanillaSprites.GreenBtnLong, new System.Action(() =>
                {
                    if (InGame.instance.GetCashManager().cash.Value >= 1000)
                    {
                        InGame.instance.AddCash(-1000);
                        TowerTactics.Values.Values.ammo += 1000;
                        if (TowerTactics.TowerTactics.Popup == true)
                        {
                            PopupScreen.instance?.ShowOkPopup("You have bought 1000 ammo for 1k!");
                        }
                    }
                }));
                Buy1000.AddText(new("Title_", 0, 0, 300, 150), "Buy 1k Ammo for 1k!", 45);
                var Buy100 = panel.AddButton(new("Button_", -650, 150, 300, 150), VanillaSprites.GreenBtnLong, new System.Action(() =>
                {
                    if (InGame.instance.GetCashManager().cash.Value >= 100)
                    {
                        InGame.instance.AddCash(-100);
                        TowerTactics.Values.Values.ammo += 100;
                        if (TowerTactics.TowerTactics.Popup == true)
                        {
                            PopupScreen.instance?.ShowOkPopup("You have bought 100 ammo for 100!");
                        }
                    }
                }));
                Buy100.AddText(new("Title_", 0, 0, 300, 150), "Buy 100 Ammo for 100!", 45);
                var Buy10000 = panel.AddButton(new("Button_", 650, 150, 300, 150), VanillaSprites.GreenBtnLong, new System.Action(() =>
                {
                    if (InGame.instance.GetCashManager().cash.Value >= 10000)
                    {
                        InGame.instance.AddCash(-10000);
                        TowerTactics.Values.Values.ammo += 10000;
                        if (TowerTactics.TowerTactics.Popup == true)
                        {
                            PopupScreen.instance?.ShowOkPopup("You have bought 10000 ammo for 10k!");
                        }
                    }
                }));
                Buy10000.AddText(new("Title_", 0, 0, 300, 150), "Buy 10k Ammo for 10k!", 45);
                var image3 = panel.AddImage(new("Image_", -350, -200, 350), ModContent.GetSpriteReference<TowerTactics.TowerTactics>("BuyBanana").GetGUID());
                var image4 = panel.AddImage(new("Image_", 350, -200, 350), ModContent.GetSpriteReference<TowerTactics.TowerTactics>("BuyBanana").GetGUID());
                var BuyBanana = panel.AddButton(new("Button_", -350, -450, 300, 150), VanillaSprites.GreenBtnLong, new System.Action(() =>
                {
                    if (InGame.instance.GetCashManager().cash.Value >= 500)
                    {
                        InGame.instance.AddCash(-500);
                        TowerTactics.Values.Values.banana += 1;
                        if (TowerTactics.TowerTactics.Popup == true)
                        {
                            PopupScreen.instance?.ShowOkPopup("You have bought 1 banana for 500");
                        }
                    }
                }));
                BuyBanana.AddText(new("Title_", 0, 0, 300, 150), "Buy 1 Banana for 500!", 45);

                var BuyBulkBanana = panel.AddButton(new("Button_", 350, -450, 300, 150), VanillaSprites.GreenBtnLong, new Action(() =>
                {
                    if (PopupScreen.instance != null)
                    {
                        PopupScreen.instance.ShowSetValuePopup("Buy Bananas!", "500 Cash Per!", new Action<int>(input =>
                        {
                            if (input * 500 > InGame.instance.GetCashManager().cash.Value)
                            {
                                PopupScreen.instance.ShowOkPopup("Not Enough Cash!");
                            }
                            else
                            {
                                InGame.instance.AddCash(-input * 500);
                                TowerTactics.Values.Values.banana += input;
                                if (TowerTactics.TowerTactics.Popup == true)
                                {
                                    if (input == 1)
                                    {
                                        PopupScreen.instance?.ShowOkPopup($"You have bought 1 banana for 500");
                                    }
                                    else
                                    {
                                        PopupScreen.instance?.ShowOkPopup($"You have bought {input} bananas for {input * 500}");
                                    }
                                }
                            }
                        }), 0);
                    }
                }));
                BuyBulkBanana.AddText(new("Title_", 0, 0, 300, 150), "Buy Any Amount Of Banana", 45);
            }
        }
    }
}