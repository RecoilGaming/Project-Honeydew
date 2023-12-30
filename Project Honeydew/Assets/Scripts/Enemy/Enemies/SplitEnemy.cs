using UnityEngine;

[CreateAssetMenu(fileName = "Split", menuName = "Enemy/Split")]
public class SplitEnemy : Enemy
{
    [Header("Split")]
    [SerializeField] private GameObject splitInto;

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
        Instantiate(splitInto, enemy.transform.position + new Vector3(0f,0.5f,0f), enemy.transform.rotation);
        Instantiate(splitInto, enemy.transform.position + new Vector3(0.3f,-0.2f,0f), enemy.transform.rotation);
        Instantiate(splitInto, enemy.transform.position + new Vector3(-0.3f,-0.2f,0f), enemy.transform.rotation);
        Destroy(enemy);
    }
}