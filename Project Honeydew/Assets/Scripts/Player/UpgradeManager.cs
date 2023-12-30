
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [Header("Upgrades")]
    [SerializeField] private List<Upgradable> upgradables = new();
    [SerializeField] private List<Upgradable> rareUpgradables = new();

    private List<PlayerUpgrade> upgrades = new();
    private List<PlayerUpgrade> rareUpgrades = new();

    // runs when script loads
    private void Start()
    {
        for (int i = 0; i < upgradables.Count; i++)
        for (int j = 0; j < upgradables[i].weight; j++)
            upgrades.Add(upgradables[i].upgrade);
        
        for (int i = 0; i < rareUpgradables.Count; i++)
        for (int j = 0; j < rareUpgradables[i].weight; j++)
            rareUpgrades.Add(rareUpgradables[i].upgrade);
    }

    public PlayerUpgrade GetUpgrade() { return upgrades[Random.Range(0, upgrades.Count)]; }
    public PlayerUpgrade GetRareUpgrade() { return rareUpgrades[Random.Range(0, rareUpgrades.Count)]; }
    public void RemoveUpgrade(PlayerUpgrade upgrade) { upgrades.Remove(upgrade); }
    public void RemoveRareUpgrade(PlayerUpgrade upgrade) { rareUpgrades.Remove(upgrade); }
}

[System.Serializable]
public class Upgradable
{
    public PlayerUpgrade upgrade;
    public int weight;
}