using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public Projectile projectile;
    private Rigidbody2D Body { get; set; }
	private Vector2 moveDirection;
    private float pierce;

    // runs before scene loadeds
    private void Awake()
	{
		// grab components
		Body = GetComponent<Rigidbody2D>();
        pierce = projectile.pierce;
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
            pierce--;

            Physics2D.IgnoreCollision(collider, transform.GetComponent<CircleCollider2D>());
            enemy.Damage(projectile.damage + projectile.damageAddon);
            StartCoroutine(AudioManager.instance.ApplyKnockback(enemy.GetComponent<Rigidbody2D>(), projectile.knockback));
            if (pierce < 0) Destroy(gameObject);
        }
    }
}
