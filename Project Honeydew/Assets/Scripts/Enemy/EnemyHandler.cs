using UnityEngine;

enum EnemyState
{
    IDLE,
    CHASE,
    ATTACK
}

public class EnemyHandler : MonoBehaviour
{
    // private variables
    [SerializeField] private Enemy enemy;
    private EnemyState state;

    // handle ability
    public void Handle(GameObject player, GameObject obj)
    {
        float distance = Vector2.Distance(player.transform.position, obj.transform.position);
        switch (state)
        {
            case EnemyState.IDLE:
                enemy.Idle(player, obj);
                if (distance <= enemy.chaseRange) {
                    state = EnemyState.CHASE;
                }
            break;
            case EnemyState.CHASE:
                enemy.Chase(player, obj);
                if (distance <= enemy.attackRange) {
                    state = EnemyState.ATTACK;
                } else if (distance > enemy.chaseRange) {
                    state = EnemyState.IDLE;
                }
            break;
            case EnemyState.ATTACK:
                enemy.Attack(player, obj);
                if (distance > enemy.attackRange) {
                    state = EnemyState.CHASE;
                }
            break;
        }
        
        if (obj.GetComponent<EnemyController>().Health <= 0) {
            player.GetComponent<PlayerController>().GainXP(enemy.experienceYield);
            enemy.Die(player, obj);
        }
    }

    // get enemy
    public Enemy GetEnemy()
    {
        return enemy;
    }
}
