using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using System.Collections.Generic;
using UnityEngine;
using BTD_Mod_Helper.Extensions;
using MelonLoader;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api;
using UnityEngine.UI;

namespace Ui
{
    [RegisterTypeInIl2Cpp(false)]
    public class ShopButton : MonoBehaviour
    {
        public static ShopButton? instance = null;
        public ModHelperInputField? input;
        public bool IsShopOpen = false;
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
                var panel = rect.gameObject.AddModHelperPanel(new("Panel_", 0, 0, 0),VanillaSprites.BrownInsertPanel);
                instance = panel.AddComponent<ShopButton>();
                var image = panel.AddImage(new("Image_", 0f, 1150f, 256f), ModContent.GetSpriteReference<TowerTactics.TowerTactics>("ShopIcon").GetGUID());
                var button = image.gameObject.AddComponent<Button>();
                button.onClick.AddListener(instance.OpenShopUi);
            }
        }
        private void OpenShopUi()
        {
            if (IsShopOpen == true)
            {
                ShopUi.instance.Close();
                IsShopOpen = false;
            }
            else if (IsShopOpen == false)
            {
                ShopUi.CreatePanel();
                IsShopOpen = true;
            }
        }
    }
}
