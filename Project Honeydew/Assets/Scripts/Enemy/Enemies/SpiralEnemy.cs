using UnityEngine;

[CreateAssetMenu(fileName = "Spiral", menuName = "Enemy/Spiral")]
public class SpiralEnemy : Enemy
{
    public override void Chase(GameObject player, GameObject enemy)
    {
        enemy.transform.position = Quaternion.Euler(0, 0, 0.1f) * Vector2.MoveTowards(enemy.transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        enemy.transform.up = (player.transform.position - enemy.transform.position).normalized;
    }

    public override void Attack(GameObject player, GameObject enemy)
    {
        player.GetComponent<PlayerController>().Damage(attackDamage*difficulty);
    }

    public override void Die(GameObject player, GameObject enemy)
    {
        Destroy(enemy);
    }
}