using UnityEngine;

public class Enemy : ScriptableObject
{
    [Header("General")]
    public new string name;
    [TextArea(3,3)] public string description;

    [Header("Stats")]
    public float maxHealth;
    public float moveSpeed;
    public float attackDamage;
    public float attackCooldown;

    [Header("Timers")]
    public float chaseRange;
    public float attackRange;

    public virtual void Idle(GameObject player, GameObject enemy) {}
    public virtual void Chase(GameObject player, GameObject enemy) {}
    public virtual void Attack(GameObject player, GameObject enemy) {}
    public virtual void Die(GameObject player, GameObject enemy) {}
}