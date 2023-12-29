using UnityEngine;
using UnityEngine.Timeline;

public class ScytheController : MonoBehaviour
{
    // action variables
    public bool Flying { get; private set; }
    public bool Holding { get; private set; }

    // component variables
    private Rigidbody2D Body { get; set; }
    private BoxCollider2D Collider { get; set; }

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
        transform.position = player.transform.position;
        ChangeState("HOLD");

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
                transform.position = player.transform.position;
                ChangeState("HOLD");
            }
        }
    }
    
    public void ScytheDoubleJump() {
        if (Flying) {
            Body.velocity = new Vector2(Body.velocity.x, 0);
            Body.AddForce(scytData.scytheSpeed * Vector2.up, ForceMode2D.Force);
        }
    }
    // collision detection
    private void OnCollisionEnter2D()
    {
        // scythe ground
        if (Flying)
        {
            player.transform.position = transform.position;
            ChangeState("HOLD");
            player.GetComponent<PlayerController>().SlowTime(scytData.recoveryTime);
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

            Vector2 scale = transform.localScale;
            scale.x = Mathf.Sign(player.GetComponent<PlayerController>().Facing);
            transform.localScale = scale;

            Body.bodyType = RigidbodyType2D.Kinematic;
            Body.velocity = Vector2.zero;
            transform.SetParent(player.transform);
        }
        else if (state == "FLY")
        {
            Holding = false;
            Flying = true;

            Body.bodyType = RigidbodyType2D.Dynamic;
            // Collider.enabled = true;
            transform.SetParent(null);
        }
    }

    // throw scythe
    public void ThrowScythe(Vector2 direction)
    {
        // scythe fly
        if (Holding)
        {
            ChangeState("FLY");
            Body.AddForce(scytData.scytheSpeed * direction, ForceMode2D.Force);
        }
    }
}
