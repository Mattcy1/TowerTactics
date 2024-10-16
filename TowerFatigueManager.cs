using BTD_Mod_Helper;
using BTD_Mod_Helper.Extensions;
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
        public static void IncreaseFatigueForTowerType(string towerModelBaseId)
        {
            if (!TowerTactics.TowerFatigue.ContainsKey(towerModelBaseId))
            {
                TowerTactics.TowerFatigue[towerModelBaseId] = 0;
            }
            if (TowerTactics.TowerFatigue[towerModelBaseId] <= 30)
            {
                TowerTactics.TowerFatigue[towerModelBaseId] += 1;
            }
            if (TowerTactics.TowerFatigue[towerModelBaseId] >= 31)
            {
                TowerTactics.TowerFatigue[towerModelBaseId] -= 1;
            }
        }
        public static void ApplyFatigueDebuff(string towerModelBaseId)
        {
            var Towers = InGame.instance.GetAllTowerToSim().FindAll(sim => sim.tower.towerModel.baseId == towerModelBaseId);
            foreach (var Tower in Towers)
            {
                {
                    var towerModel = Tower.tower.rootModel.Duplicate().Cast<TowerModel>();
                    foreach (var Weapon in towerModel.GetWeapons())
                    {
                        Weapon.rate += 0.05f;
                        ModHelper.Msg<TowerTactics>(towerModel.GetWeapon().rate);
                    }
                    if (FatigueUi.instance != null)
                    {
                        FatigueUi.instance.Close();
                        FatigueUi.CreatePanel();
                    }
                    Tower.tower.UpdateRootModel(towerModel);
                }
            }
        }
        public static async Task ResetRateAsync(string towerModelBaseId)
        {
            Values.Values.IsResting = true;
            var Towers = InGame.instance.GetAllTowerToSim().FindAll(sim => sim.tower.towerModel.baseId == towerModelBaseId);
            ModHelper.Msg<TowerTactics>($"Found {Towers.Count} towers with baseId {towerModelBaseId}");

            foreach (var Tower in Towers)
            {
                var towerModel = Tower.tower.rootModel.Duplicate().Cast<TowerModel>();
                int fatigue = TowerTactics.TowerFatigue[towerModelBaseId];

                foreach (var Weapon in towerModel.GetWeapons())
                {
                    if (fatigue > 0)
                    {

                        for (int i = 0; i < fatigue; i++)
                        {
                            Weapon.rate -= 0.05f;
                            ModHelper.Msg<TowerTactics>(towerModel.GetWeapon().rate);
                        }
                    }
                    if (FatigueUi.instance != null)
                        {
                        FatigueUi.instance.Close();
                        FatigueUi.CreatePanel();
                    }
                }

                Tower.tower.UpdateRootModel(towerModel);

                TowerTactics.TowerFatigue[towerModelBaseId] = 0;
            }
            await Task.Delay(5000); 
            Values.Values.IsResting = false;
        }
    }
}

