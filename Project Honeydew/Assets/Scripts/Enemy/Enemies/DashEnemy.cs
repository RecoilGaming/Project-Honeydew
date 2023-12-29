using UnityEngine;

[CreateAssetMenu(fileName = "Dash", menuName = "Enemy/Dash")]
public class DashEnemy : Enemy
{
    private float timeSinceLastDash = 0f;

    public override void Chase(GameObject player, GameObject enemy)
    {
        timeSinceLastDash += Time.deltaTime;
        if(timeSinceLastDash > 1f) {
            timeSinceLastDash -= 1f;
            Jump(player, enemy);
        }
    }

    public void Jump(GameObject player, GameObject enemy) 
    {
        enemy.GetComponent<Rigidbody2D>().velocity = Vector2.MoveTowards(enemy.transform.position, player.transform.position, moveSpeed * Time.deltaTime * 0.2f);
        enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(-enemy.GetComponent<Rigidbody2D>().velocity.x, -enemy.GetComponent<Rigidbody2D>().velocity.y);
        enemy.transform.up = player.transform.position - enemy.transform.position;
    }

    public override void Attack(GameObject player, GameObject enemy)
    {
        player.GetComponent<PlayerController>().Damage(attackDamage);
    }

    public override void Die(GameObject player, GameObject enemy)
    {
        Destroy(enemy);
    }
}