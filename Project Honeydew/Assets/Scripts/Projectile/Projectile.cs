using UnityEngine;

[CreateAssetMenu(menuName = "Projectile")]
public class Projectile : ScriptableObject
{
    [Header("Movement")]
    public float speed;
    public float accel;

    [Header("Collision")]
    public LayerMask enemy;

    [Header("Damage")]
    public float damage;
    public float damageAddon;
    public float knockback;
}