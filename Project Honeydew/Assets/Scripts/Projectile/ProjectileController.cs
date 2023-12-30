using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public Projectile projectile;
    private Rigidbody2D Body { get; set; }
	private Vector2 moveDirection;

    // runs before scene loadeds
    private void Awake()
	{
		// grab components
		Body = GetComponent<Rigidbody2D>();
    }

    // runs at 60 fps
	private void FixedUpdate()
	{
		moveDirection = Body.transform.up;
        Body.AddForce((projectile.speed * moveDirection - Body.velocity) * projectile.accel, ForceMode2D.Force);

        if (transform.position.magnitude > 10) {
            Destroy(gameObject);
        }
    }

    // collision destroy
    private void OnTriggerEnter2D(Collider2D collider)
    {
        EnemyController enemy = collider.gameObject.GetComponent<EnemyController>();
        if (projectile.enemy == (projectile.enemy | (1 << collider.gameObject.layer))) {
            enemy.Damage(projectile.damage + projectile.damageAddon);
            enemy.Flash(0.15f);
            Destroy(gameObject);
        }
    }
}
