using UnityEngine;

public enum UpgradeType
{
    STAT_UPGRADE,
    PRIMARY_UPGRADE,
    WEAPON_UNLOCK,
    WEAPON_UPGRADE
}

[CreateAssetMenu(fileName = "Player Upgrade")]
public class PlayerUpgrade : ScriptableObject
{
    [Header("General")]
    public new string name;
    public UpgradeType upgradeType;
    [TextArea(3,3)] public string description;

    [Header("Stats")]
    public float maxHealth;
    public float health;
    public float healSpeed;
    public float invincibility;
    public float attackDamage;
    public float attackSpeed;
    public float attackKnockback;
    public float experienceMultiplier;
    public float cameraSize;
    public int bulletCount;
}