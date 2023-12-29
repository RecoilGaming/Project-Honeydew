using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    // private variables
    [SerializeField] private Projectile projectile;
    private Rigidbody2D Body { get; set; }
	private Vector2 moveDirection;

    // runs before scene loadeds
    private void Awake()
	{
		// grab components
		Body = GetComponent<Rigidbody2D>();
    }

<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == projectile.enemy) {
            Destroy(gameObject);
        }
    }
}
