
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [Header("Upgrades")]
    [SerializeField] private List<Upgradable> upgradables = new();

    private List<PlayerUpgrade> upgrades = new();

    // runs when script loads
    private void Start()
    {
        for (int i = 0; i < upgradables.Count; i++)
        for (int j = 0; j < upgradables[i].weight; j++)
            upgrades.Add(upgradables[i].upgrade);
    }

    public PlayerUpgrade GetUpgrade() { return upgrades[Random.Range(0, upgrades.Count)]; }
    public void RemoveUpgrade(PlayerUpgrade upgrade) { upgrades.Remove(upgrade); }
}

[System.Serializable]
public class Upgradable
{
    public PlayerUpgrade upgrade;
    public int weight;
}