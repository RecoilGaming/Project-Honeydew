using System;
using UnityEngine;

public class ScytheController : MonoBehaviour
{
    // action variables
    public bool Holding { get; private set; }
    public bool Flying { get; private set; }
    public bool Returning { get; private set; }
    public bool Grounded { get; private set; }

    // component variables
    private Rigidbody2D Body { get; set; }
    private BoxCollider2D Collider { get; set; }

    // scythe variables
    private Vector2 throwDirection;

    // general variables
	[Header("General")]
	[SerializeField] private ScytheData scytData;
    [SerializeField] private GameObject player;

    // runs before scene is loaded
    private void Awake()
	{
		// grab components
		Body = GetComponent<Rigidbody2D>();
        Collider = GetComponent<BoxCollider2D>();
    }

    // runs when script starts
    private void Start()
    {
        // default configuration
        ChangeState("RETURN");

        // ignore player
        Physics2D.IgnoreCollision(Collider, player.GetComponent<BoxCollider2D>());
    }

    // updates every frame
    private void Update()
    {
        // scythe return
        if (Flying)
        {
            if (Vector3.Distance(player.transform.position,transform.position) >= scytData.scytheRange)
            {
                ChangeState("RETURN");
            }
        }

        // scythe hold
        if (Returning)
        {
            if (Vector3.Distance(player.transform.position,transform.position) < 1)
            {
                transform.position = player.transform.position;
                ChangeState("HOLD");
            }
        }
    }

    // updates at 60 fps
	private void FixedUpdate()
	{
        // scythe move
        if (Flying)
        {
            Vector2 target = (scytData.scytheSpeed * throwDirection - Body.velocity) * scytData.scytheAccel;
            Body.AddForce(target, ForceMode2D.Force);
        }
        else if (Returning)
        {
            throwDirection = (player.transform.position - transform.position).normalized;
            Body.AddForce((scytData.returnSpeed * throwDirection - Body.velocity) * scytData.scytheAccel, ForceMode2D.Force);
        }
    }

    // collision detection
    private void OnCollisionEnter2D()
    {
        // scythe ground
        if (Flying)
        {
            ChangeState("GROUND");
        }
    }

    // change state
    private void ChangeState(string state)
    {   
        // states
        if (state == "HOLD")
        {
            Holding = true;
            Flying = false;
            Returning = false;
            Grounded = false;

            Body.bodyType = RigidbodyType2D.Kinematic;
            Body.velocity = Vector2.zero;
            transform.SetParent(player.transform);

            if (player.GetComponent<PlayerController>().Facing != Mathf.Sign(transform.localScale.x))
            {
                Vector2 scale = transform.localScale;
                scale.x = -scale.x;
                transform.localScale = scale;
            }
        }
        else if (state == "FLY")
        {
            Holding = false;
            Flying = true;
            Returning = false;
            Grounded = false;

            Body.bodyType = RigidbodyType2D.Dynamic;
            Collider.enabled = true;
            transform.SetParent(null);
        }
        else if (state == "RETURN")
        {
            Holding = false;
            Flying = false;
            Returning = true;
            Grounded = false;

            Collider.enabled = false;
        }
        else if (state == "GROUND")
        {
            Holding = false;
            Flying = false;
            Returning = false;
            Grounded = true;

            Body.velocity = Vector2.zero;
            Body.bodyType = RigidbodyType2D.Static;
            Collider.enabled = false;
        }
    }

    // throw scythe
    public void ThrowScythe(Vector2 direction)
    {
        // scythe fly
        if (Holding)
        {
            throwDirection = direction;
            ChangeState("FLY");
        }

        // scythe teleport
        else if (Grounded)
        {
            player.transform.position = transform.position;
            ChangeState("HOLD");
            player.GetComponent<PlayerController>().SlowTime(scytData.recoveryTime);
        }
    }
}
