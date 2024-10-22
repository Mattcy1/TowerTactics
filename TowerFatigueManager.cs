using BTD_Mod_Helper;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Unity.UI_New.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ui;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using static MelonLoader.MelonLogger;

namespace TowerTactics
{
    internal static class TowerFatigueManager
    {
        public static void IncreaseFatigueForTowerType(string towerId)
        {
            if (!TowerTactics.TowerFatigue.ContainsKey(towerId))
            {
                TowerTactics.TowerFatigue[towerId] = 0;
            }
            if (TowerTactics.TowerFatigue[towerId] < TowerTactics.MaxFatigue)
            {
                TowerTactics.TowerFatigue[towerId] += 1;
            }
            if (TowerTactics.TowerFatigue[towerId] > TowerTactics.MaxFatigue)
            {
                TowerTactics.TowerFatigue[towerId] = TowerTactics.MaxFatigue;
            }
        }
        public static void ApplyFatigueDebuff(string towerId)
        {
            var tower = InGame.instance.GetAllTowerToSim().First(tts => tts.tower.towerModel.baseId == towerId).tower;

            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            foreach (var Weapon in towerModel.GetWeapons())
            {
                Weapon.rate += 0.05f;
                //ModHelper.Msg<TowerTactics>(towerModel.GetWeapon().rate);
            }
            if (FatigueUi.instance != null)
            {
                FatigueUi.instance.Close();
                FatigueUi.CreatePanel();
            }
            tower.UpdateRootModel(towerModel);
        }

        public static async Task ResetRateAsync(string towerId)
        {
            Values.Values.IsResting = true;
            var tower = InGame.instance.GetAllTowerToSim().First(tts => tts.tower.towerModel.baseId == towerId).tower;

            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            int fatigue = TowerTactics.TowerFatigue[towerId];

            foreach (var Weapon in towerModel.GetWeapons())
            {
                if (fatigue > 0)
                {

                    for (int i = 0; i < fatigue; i++)
                    {
                        Weapon.rate -= 0.05f;
                        //ModHelper.Msg<TowerTactics>(towerModel.GetWeapon().rate);
                    }
                }
                if (FatigueUi.instance != null)
                {
                    FatigueUi.instance.Close();
                    FatigueUi.CreatePanel();
                }
            }

            tower.UpdateRootModel(towerModel);

            TowerTactics.TowerFatigue[towerId] = 0;
            await Task.Delay(5000);
            Values.Values.IsResting = false;
        }
    }
}

