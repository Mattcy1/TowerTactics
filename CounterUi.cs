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

namespace Ui
{
    [RegisterTypeInIl2Cpp(false)]
    public class CounterUi : MonoBehaviour
    {
        public static CounterUi? instance = null;
        public ModHelperInputField? input;
        public ModHelperText? Text;
        public ModHelperText? BananaText;
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
                var panel = rect.gameObject.AddModHelperPanel(new("Panel_", 1375, 950, 512, 256), ModContent.GetSpriteReference<TowerTactics.TowerTactics>("AmmoCounter").GetGUID());
                instance = panel.AddComponent<CounterUi>();
                var AmmoValue = NumberFormatter.FormatNumber(TowerTactics.Values.Values.ammo);
                var text = panel.AddText(new("Title_", 100, 0, 350, 200), $"{AmmoValue}", 90);
                var image = panel.AddImage(new("Image_", 0, -300, 512, 256), ModContent.GetSpriteReference<TowerTactics.TowerTactics>("BananaCounter").GetGUID());
                var BananaValue = NumberFormatter.FormatNumber(TowerTactics.Values.Values.banana);
                var Bananatext = panel.AddText(new("Title_", 100, -300, 350, 200), $"{BananaValue}", 90);
                text.Text.fontSizeMax = 50;
                text.Text.fontSizeMin = 10;
                Bananatext.Text.fontSizeMax = 50;
                Bananatext.Text.fontSizeMin = 10;
                instance.Text = text;
                instance.BananaText = Bananatext;
            }
        }
    }
}
