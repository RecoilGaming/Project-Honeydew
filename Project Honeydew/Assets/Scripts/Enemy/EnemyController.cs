using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // public variable
    public float Health { get; private set; }

    // private variables
    [SerializeField] private EnemyHandler enemyHandler;
    [SerializeField] private GameObject player;
    
    // runs when script loads
    private void Start()
    {
        Health = enemyHandler.GetEnemy().maxHealth;
    }

    // runs every frame
    void Update()
    {
        enemyHandler.Handle(player, gameObject);
    }

    // health modification
    public void Damage(float amt)
    {
        Health -= amt;
        Debug.Log(Health);
    }

    public void Heal(float amt)
    {
        Health += amt;
    }
}
