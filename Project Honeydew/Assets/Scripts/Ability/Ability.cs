using UnityEngine;

public enum Rarity
{
    COMMON,
    UNIQUE,
    MYTHICAL,
    CURSED
}

public class Ability : ScriptableObject
{
    [Header("General")]
    public new string name;
    public Rarity rarity;
    [TextArea(3,3)] public string description;

    [Header("Timers")]
    public float activation;
    public float cooldown;

    public virtual void Activate(GameObject player) {}
    public virtual void Deactivate(GameObject player) {}
}