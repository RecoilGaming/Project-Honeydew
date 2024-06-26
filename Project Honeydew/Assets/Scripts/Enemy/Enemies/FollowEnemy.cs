using UnityEngine;

[CreateAssetMenu(fileName = "Follow", menuName = "Enemy/Follow")]
public class FollowEnemy : Enemy
{
    public override void Chase(GameObject player, GameObject enemy)
    {
        enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, player.transform.position, moveSpeed * Time.deltaTime);
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