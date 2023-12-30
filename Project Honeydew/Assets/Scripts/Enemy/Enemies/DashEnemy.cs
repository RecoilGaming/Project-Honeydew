using UnityEngine;

[CreateAssetMenu(fileName = "Dash", menuName = "Enemy/Dash")]
public class DashEnemy : Enemy
{
    [Header("Dash")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    private float dashActiveTime = 0f;
    private float dashCooldownTime = 0f;
    private bool dashing = false;

    public override void Chase(GameObject player, GameObject enemy)
    {
        if (dashing) {
            enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, player.transform.position, dashSpeed * Time.deltaTime);
        } else {
            enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        } enemy.transform.up = (player.transform.position - enemy.transform.position).normalized;

        if (!dashing && dashCooldownTime <= 0) {
            dashActiveTime = dashDuration;
            dashing = true;
        } else if (dashing && dashActiveTime <= 0) {
            dashCooldownTime = dashCooldown;
            dashing = false;
        }

        dashActiveTime -= Time.deltaTime;
        dashCooldownTime -= Time.deltaTime;
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