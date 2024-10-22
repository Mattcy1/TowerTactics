using MelonLoader;
using BTD_Mod_Helper;
using TowerTactics;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Simulation.Bloons;
using HarmonyLib;
using UnityEngine;
using Il2CppAssets.Scripts.Simulation.Towers.Weapons;
using Il2CppAssets.Scripts.Models;
using Ui;
using Il2CppAssets.Scripts.Unity.UI_New.Popups;
using BTD_Mod_Helper.Api.ModOptions;
using System.Collections.Generic;
using System.Linq;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using System;
using Il2CppAssets.Scripts.Models.Towers;
using static MelonLoader.MelonLogger;
using Il2CppAssets.Scripts.Data;
using Il2CppAssets.Scripts.Models.TowerSets;
using Il2CppAssets.Scripts.Simulation.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Rounds;
using Il2CppAssets.Scripts.Models.ServerEvents;
using Il2CppAssets.Scripts.Models.Gameplay.Mods;
using Il2CppAssets.Scripts.Simulation.Track;
using Il2CppAssets.Scripts.Simulation;
using Il2CppAssets.Scripts;
using Il2CppAssets.Scripts.Unity.Bridge;
using Il2CppAssets.Scripts.Models.Profile;

[assembly: MelonInfo(typeof(TowerTactics.TowerTactics), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace TowerTactics;

public class TowerTactics : BloonsTD6Mod
{
    public bool PopupShown = false;
    public static Dictionary<string, int> TowerFatigue = new Dictionary<string, int>();
    public static Dictionary<string, float> OriginalAttackRates = new Dictionary<string, float>();

    public static readonly ModSettingInt MaxFatigue = new(30)
    {
        description = "What's the max fatigue a tower can get? (Higher values mean towers can get slower than usual)"
    };

    public static readonly ModSettingEnum<TaxUI.TaxMode> TaxType = new(TaxUI.TaxMode.IncomeAndSell)
    {
        description = "Current Cash: Tax amounts are based off of how much money you have when round 10 starts.\n" +
        "Income: Tax amounts are based off of the amount of cash you've made in the last 10 rounds.\n" +
        "Income And Sell: Just like Income, but if you don't have enough your towers will be sold to pay off your debt."
    };

    public override void OnApplicationStart()
    {
        ModHelper.Msg<TowerTactics>("Tower Tactics loaded!");
        Values.Values.ammo = 100;
        Values.Values.rounds = 9;
        TaxUI.taxedIncome = 0;
    }
    public override void OnTowerSelected(Il2CppAssets.Scripts.Simulation.Towers.Tower tower)
    {
        if (CounterUi.instance != null)
        {
            CounterUi.instance.Close();
        }

        if (ShopButton.instance != null)
        {
            ShopButton.instance.Close();
        }
        Ui.FedUI.selectedTower = tower;
        Ui.FatigueUi.tower = tower;
        Ui.FedUI.CreatePanel();
        Ui.FatigueUi.CreatePanel();
    }

    public override void OnCashAdded(double amount, Simulation.CashType from, int cashIndex, Simulation.CashSource source, Tower tower)
    {
        if (from != Simulation.CashType.CoopCash && source != Simulation.CashSource.TowerSold && amount > 0) 
        {
            TaxUI.taxedIncome += amount;
        }
    }

    public override void OnNewGameModel(GameModel result)
    {
        CounterUi.CreatePanel();
        ShopButton.CreatePanel();
        Values.Values.ammo = 100;
        Values.Values.rounds = 9;
        Values.Values.banana = 3;
        TaxUI.taxedIncome = 0;
        TowerFatigue.Clear();
    }
    public override void OnRestart()
    {
        if (CounterUi.instance != null)
        {
            CounterUi.instance.Close();
        }
        if (ShopButton.instance != null)
        {
            ShopButton.instance.Close();
        }
        if (ShopUi.instance != null)
        {
            ShopUi.instance.Close();
        }
        if (Ui.TaxUI.instance != null)
        {
            Ui.TaxUI.instance.Close();
        }
        CounterUi.CreatePanel();
        ShopButton.CreatePanel();
        TowerFatigue.Clear();
        Values.Values.ammo = 100;
        Values.Values.rounds = 9;
        Values.Values.banana = 3;
        TaxUI.taxedIncome = 0;
        //PopupScreen.instance?.ShowOkPopup($"Would you like to pay the tower's fee of {InGame.instance.GetCashManager().cash.Value / 2}? If you refuse, action will be taken.");
    }
    public override void OnTowerDeselected(Il2CppAssets.Scripts.Simulation.Towers.Tower tower)
    {
        CounterUi.CreatePanel();
        ShopButton.CreatePanel();
        Ui.FedUI.selectedTower = null;
        Ui.FatigueUi.tower = null;
        if (Ui.FedUI.instance != null)
        {
            Ui.FedUI.instance.Close();
        }
        if (Ui.FatigueUi.instance != null)
        {
            Ui.FatigueUi.instance.Close();
        }
    }
    public override void OnUpdate()
    {
        if (InGame.instance != null && CounterUi.instance != null)
        {
            var AmmoValue = NumberFormatter.FormatNumber(Values.Values.ammo);
            CounterUi.instance.Text.SetText($"{AmmoValue}");
        }

        if (InGame.instance != null && CounterUi.instance != null)
        {
            var BananaValue = NumberFormatter.FormatNumber(Values.Values.banana);
            CounterUi.instance.BananaText.SetText($"{BananaValue}");
        }

        if (InGame.instance != null && InGame.instance.bridge != null)
        {
            int currentRound = InGame.instance.bridge.GetCurrentRound();

            if (currentRound == Values.Values.rounds)
            {
                TaxUI.CreatePanel();
                Values.Values.rounds += 10;
                PopupShown = false;
            }
            if (currentRound == Values.Values.rounds - 2 && PopupShown == false && Popup == true)
            {
                PopupScreen.instance?.ShowOkPopup($"Warning! u will need to pay taxes in 2 rounds");
                PopupShown = true;
            }
        }
    }
    public static readonly ModSettingBool Popup = new(true)
    {
        displayName = "Enable Popup?",
        description = "This setting allow you to choose if u want popup or not",
    };
}

[HarmonyPatch(typeof(Weapon), nameof(Weapon.SpawnDart))]
public static class RemoveAmmo
{
    [HarmonyPostfix]
    public static void Postfix(Weapon __instance)
    {
        List<string> BlacklistedTowers = new List<string> { "BananaFarm", "IceMonkey", "Sauda", "Benjamin", "BeastHandler", "PatFusty", "Adora"};

        if (InGame.instance != null)
        {
            var tower = __instance.attack.tower;
            if (tower != null)
            {
                if (Values.Values.ammo > 0 && !BlacklistedTowers.Contains(tower.towerModel.baseId))
                {
                    Values.Values.ammo -= 1;
                }
            }
        }
    }

    [HarmonyPrefix]
    public static bool Prefix()
    {
        if (Values.Values.ammo <= 0)
        {
            return false;
        }
        return true;
    }
}

[HarmonyPatch(typeof(Bloon), nameof(Bloon.Damage))]
public static class Bloon_Damage
{
    private static readonly System.Random random = new System.Random();

    [HarmonyPrefix]
    public static void Prefix(Bloon __instance, double totalAmount, Il2CppAssets.Scripts.Simulation.Towers.Tower tower)
    {
        if (tower != null && !__instance.bloonModel.IsRegrowBloon())
        {
            if (random.Next(40) == 0 && Values.Values.IsResting == false && TowerTactics.TowerFatigue[tower.towerModel.baseId] < TowerTactics.MaxFatigue)
            {
                TowerFatigueManager.IncreaseFatigueForTowerType(tower.towerModel.baseId);
                TowerFatigueManager.ApplyFatigueDebuff(tower.towerModel.baseId);

                //ModHelper.Msg<TowerTactics>(TowerTactics.TowerFatigue[tower.towerModel.baseId]);

                if (InGame.instance != null && FatigueUi.instance != null)
                {
                    FatigueUi.instance.Close();
                    FatigueUi.CreatePanel();
                }
            }
            else if (random.Next(40) == 0 && Values.Values.IsResting == true)
            {
                //ModHelper.Msg<TowerTactics>("Cannot add fatigue, fatigue is in reset state");
            }
        }
    }


[HarmonyPatch(typeof(Il2CppAssets.Scripts.Simulation.Towers.Tower), nameof(Il2CppAssets.Scripts.Simulation.Towers.Tower.OnPlace))]
    public class Tower_OnPlace_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(Il2CppAssets.Scripts.Simulation.Towers.Tower __instance)
        {
            string towerModelBaseId = __instance.towerModel.baseId;
            if (!TowerTactics.TowerFatigue.ContainsKey(__instance.towerModel.baseId))
            {
                TowerTactics.TowerFatigue[__instance.towerModel.baseId] = 0;
            }
            else if (TowerTactics.TowerFatigue.ContainsKey(__instance.towerModel.baseId))
            {
                var towerModel = __instance.rootModel.Duplicate().Cast<TowerModel>();
                int fatigue = TowerTactics.TowerFatigue[__instance.towerModel.baseId];
                for (int i = 0; i < fatigue; i++)
                {
                    foreach (var Weapon in towerModel.GetWeapons())
                    {
                        Weapon.rate += 0.05f;
                        //ModHelper.Msg<TowerTactics>(towerModel.GetWeapon().rate);
                    }
                }
                __instance.UpdateRootModel(towerModel);
            }
        }
    }
}

[HarmonyPatch(typeof(Il2CppAssets.Scripts.Simulation.Towers.Tower), nameof(Il2CppAssets.Scripts.Simulation.Towers.Tower.OnUpgraded))]
public class Tower_OnUpgrade_Patch
{
    [HarmonyPostfix]
    public static void Postfix(Il2CppAssets.Scripts.Simulation.Towers.Tower __instance)
    {
        string towerModelBaseId = __instance.towerModel.baseId;
        var towerModel = __instance.rootModel.Duplicate().Cast<TowerModel>();
        int fatigue = TowerTactics.TowerFatigue[__instance.towerModel.baseId];
        for (int i = 0; i < fatigue; i++)
        {
            foreach (var Weapon in towerModel.GetWeapons())
            {
                Weapon.rate += 0.05f;
                //ModHelper.Msg<TowerTactics>(towerModel.GetWeapon().rate);
            }
        }
        __instance.UpdateRootModel(towerModel);
    }
}

[HarmonyPatch(typeof(InGame), nameof(InGame.SellTower))]
static class InGame_SellTower_Patch
{
    [HarmonyPostfix]
    public static void Postfix(InGame __instance, TowerToSimulation tower)
    {
        if(TowerTactics.TowerFatigue.ContainsKey(tower.tower.towerModel.baseId))
        {
            TowerTactics.TowerFatigue.Remove(tower.tower.towerModel.baseId);
        }
    }
}

