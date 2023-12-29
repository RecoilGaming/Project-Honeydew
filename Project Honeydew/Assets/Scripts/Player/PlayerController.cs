using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // action variables
	public int Facing { get; private set; }
	public bool Jumping { get; private set; }
	public bool WallJumping { get; private set; }
	public bool Dashing { get; private set; }
	public bool Sliding { get; private set; }

	// timer variables
	public float LastOnGround { get; private set; }
	public float LastOnWall { get; private set; }
	public float LastOnWallRight { get; private set; }
	public float LastOnWallLeft { get; private set; }
	public float PrevJump { get; private set; }
	public float PrevDash { get; private set; }

    // component variables
    private Rigidbody2D Body { get; set; }
	private PlayerInput playerInput;
	private SpriteRenderer playerSprite;

    // input variables
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction dashAction;
    private InputAction throwAction;

	// direction variables
	private Vector2 moveDirection;
	private Vector2 throwDirection;

	// jump variables
	private bool weakerJump;
	private bool jumpFalling;
	private float wallJumpStart;
	private int wallJumpDir;
	private float gravityModifier;

	// dash variables
	private int dashCount;
	private bool dashRecharging;
	private Vector2 dashDir;
	private bool dashFloat;

	// general variables
	[Header("General")]
	[SerializeField] private MovementData moveData;

	// camera variables
	[Header("Camera")]
	[SerializeField] private Camera mainCamera;

	// scythe variables
	[Header("Scythe")]
	[SerializeField] private ScytheController scytheController;

    // collision variables
	[Header("Collision")]
    [SerializeField] private LayerMask groundLayer;
	[SerializeField] private Transform groundCheck;
	[SerializeField] private Vector2 groundCheckSize = new(0.45f, 0.03f);
	[SerializeField] private Transform frontCheck;
	[SerializeField] private Transform backCheck;
	[SerializeField] private Vector2 wallCheckSize = new(0.3f, 1.2f);

    // runs before scene is loaded
    private void Awake()
	{
		// grab components
		Body = GetComponent<Rigidbody2D>();
		playerSprite = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();

        // input actions
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        dashAction = playerInput.actions["Dash"];
		throwAction = playerInput.actions["Throw"];
	}

    // runs when script starts
    private void Start()
    {
        Body.gravityScale = moveData.gravityScale;
		Facing = 1;
		gravityModifier = 1;
    }

	// updates every frame
    private void Update()
    {
		TimerEvent();
		
		InputEvent();
		CollisionEvent();

		JumpChecks();
		JumpEvent();

		DashEvent();

		SlideCheck();
		GravityEvent();
    }

	// updates at 60 fps
	private void FixedUpdate()
	{
		TurnEvent();

		MoveEvent();
    }

	// update timers
	private void TimerEvent()
	{
		LastOnGround -= Time.deltaTime;
		LastOnWall -= Time.deltaTime;
		LastOnWallRight -= Time.deltaTime;
		LastOnWallLeft -= Time.deltaTime;
		PrevJump -= Time.deltaTime;
		PrevDash -= Time.deltaTime;
	}

	// take input
	private void InputEvent()
	{
		// movement input
		moveDirection = moveAction.ReadValue<Vector2>();
		if (moveDirection == Vector2.left || moveDirection == Vector2.right || moveDirection == Vector2.up || moveDirection == Vector2.down)
		{
			throwDirection = moveDirection;
		}

		// jump input
		jumpAction.started += _ => 
		{
			PrevJump = moveData.jumpBuffer;
		};

		jumpAction.canceled += _ =>
		{
			if ((Jumping || WallJumping) && Body.velocity.y > 0)
			{
				weakerJump = true;
			}
		};

		// dash input
		dashAction.started += _ => 
		{
			PrevDash = moveData.dashBuffer;
			Debug.Log("DASH");
		};

		// throw input
		throwAction.started += _ => 
		{
			scytheController.ThrowScythe((Vector2.left * -Facing + Vector2.up * 2).normalized);
			Debug.Log("THROW");
		};
	}

	// turn player
	private void TurnEvent()
	{
		// flip player sprite
		if ((moveDirection.x > 0 && Facing < 0) || (moveDirection.x < 0 && Facing > 0))
		{
			FlipSprite();
		}
	}

	// flips sprite
    private void FlipSprite()
    {
        Facing = -Facing;
		Vector2 scale = transform.localScale;
		scale.x = -scale.x;
		transform.localScale = scale;
    }

	// collision checks
	private void CollisionEvent()
	{
		if (!Jumping && !Dashing)
		{
			// ground check
			if (Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer) && !Jumping)
			{
				LastOnGround = moveData.coyoteTime;
            }		

			// wall checks
			if (((Physics2D.OverlapBox(frontCheck.position, wallCheckSize, 0, groundLayer) && Facing > 0) || (Physics2D.OverlapBox(backCheck.position, wallCheckSize, 0, groundLayer) && Facing < 0)) && !WallJumping)
			{
				LastOnWallRight = moveData.coyoteTime;
			}

			if (((Physics2D.OverlapBox(frontCheck.position, wallCheckSize, 0, groundLayer) && Facing < 0)
				|| (Physics2D.OverlapBox(backCheck.position, wallCheckSize, 0, groundLayer) && Facing > 0)) && !WallJumping)
			{
				LastOnWallLeft = moveData.coyoteTime;
			}

			LastOnWall = Mathf.Max(LastOnWallLeft, LastOnWallRight);
		}
	}

	// jumping checks
	private void JumpChecks()
	{
		// falling check 
		if (Jumping && Body.velocity.y < 0)
		{
			Jumping = false;

			if (!WallJumping)
			{
				jumpFalling = true;
			}
		}

		// wall jump check
		if (WallJumping && Time.time - wallJumpStart > moveData.wallJumpRecover)
		{
			WallJumping = false;
		}

		// stop falling check
		if (LastOnGround > 0 && !Jumping && !WallJumping)
        {
			weakerJump = false;

			if (!Jumping)
			{
				jumpFalling = false;
			}
		}
	}

	// jumping action
	private void JumpEvent()
	{
		// no jump while dashing
		if (Dashing)
		{
			return;
		}

		if (LastOnGround > 0 && !Jumping && PrevJump > 0)
		{
			
			// update variables
			Jumping = true;
			WallJumping = false;
			weakerJump = false;
			jumpFalling = false;
			
			PrevJump = 0;
			LastOnGround = 0;

			// perform jump
			float force = moveData.jumpAmount;
			if (Body.velocity.y < 0)
			{
				force -= Body.velocity.y;
			}

			Body.AddForce(Vector2.up * force, ForceMode2D.Impulse);
		}
		else if (LastOnWall > 0 && LastOnGround <= 0 && (!WallJumping ||
			 (LastOnWallRight > 0 && wallJumpDir > 0) || (LastOnWallLeft > 0 && wallJumpDir < 0)) && PrevJump > 0)
		{
			// update variables
			Jumping = false;
			WallJumping = true;
			weakerJump = false;
			jumpFalling = false;
			wallJumpStart = Time.time;
			wallJumpDir = (LastOnWallRight > 0) ? -1 : 1;
			
			PrevJump = 0;
			LastOnGround = 0;
			LastOnWallRight = 0;
			LastOnWallLeft = 0;

			// perform wall jump
			Vector2 force = new(moveData.wallJumpAmount.x, moveData.wallJumpAmount.y);
			force.x *= wallJumpDir;

			if (Mathf.Sign(Body.velocity.x) != Mathf.Sign(force.x))
			{
				force.x -= Body.velocity.x;
			}
			
			// remove gravity
			if (Body.velocity.y < 0)
			{
				force.y -= Body.velocity.y;
			}

			Body.AddForce(force, ForceMode2D.Impulse);
		}
	}

	// dashing action
	private void DashEvent()
	{
		// dash recharging
		if (!Dashing && dashCount < moveData.dashAmount && !dashRecharging && (LastOnGround > 0 || LastOnWall > 0))
		{
			StartCoroutine(nameof(RechargeDash), 1);
		}

		if (dashCount > 0 && PrevDash > 0)
		{
			// dash direction
			if (moveDirection != Vector2.zero)
			{
				dashDir = moveDirection;
			}
			else
			{
				dashDir = Facing == 1 ? Vector2.right : Vector2.left;
			}

			// update variables
			Dashing = true;
			Jumping = false;
			WallJumping = false;
			weakerJump = false;

			StartCoroutine(nameof(Dash), dashDir);
		}
	}

	// recharge dashes
	private IEnumerator RechargeDash(int amt)
	{
		dashRecharging = true;
		yield return new WaitForSeconds(moveData.dashCooldown);
		dashRecharging = false;
		dashCount = Mathf.Min(moveData.dashAmount, dashCount + amt);
	}

	// execute dash
	private IEnumerator Dash(Vector2 dir)
	{
		// update variables
		PrevDash = 0;
		LastOnGround = 0;
		dashCount--;
		dashFloat = true;

		float dashStart = Time.time;

		// disable gravity
		Body.gravityScale = 0;

		// dash attack
		while (Time.time - dashStart <= moveData.dashAttackTime)
		{
			Body.velocity = dir.normalized * moveData.dashSpeed;
			yield return null;
		}

		// update variables
		dashStart = Time.time;
		dashFloat = false;

		// revert gravity
		Body.gravityScale = moveData.gravityScale;
		Body.velocity = moveData.dashSlow * dir.normalized;

		// dash end
		while (Time.time - dashStart <= moveData.dashEnd)
		{
			yield return null;
		}

		// end dash
		Dashing = false;
	}

	// wall slide check
	private void SlideCheck()
	{
		Sliding = LastOnWall > 0 && !Jumping && !WallJumping && !Dashing && LastOnGround <= 0 && ((LastOnWallLeft > 0 && moveDirection.x < 0) || (LastOnWallRight > 0 && moveDirection.x > 0));
	}

	// gravity modifiers
	private void GravityEvent()
	{
		// dash antigravity
		if (dashFloat)
		{
			return;
		}
		
		if (Sliding)
		{
			// sliding anti-gravity
			Body.gravityScale = 0;
		}
		else if (Body.velocity.y < 0 && moveDirection.y < 0)
		{
			// faster falling
			Body.gravityScale = moveData.gravityScale * moveData.fastFallAccel;
			Body.velocity = new Vector2(Body.velocity.x, Mathf.Max(Body.velocity.y, -moveData.fastFallClamp));
		}
		else if (weakerJump)
		{
			// weaker jump
			Body.gravityScale = moveData.gravityScale * moveData.jumpWeakness;
			Body.velocity = new Vector2(Body.velocity.x, Mathf.Max(Body.velocity.y, -moveData.fallClamp));
		}
		else if ((Jumping || WallJumping || jumpFalling) && Mathf.Abs(Body.velocity.y) < moveData.apexThreshold)
		{
			// apex boost
			Body.gravityScale = moveData.gravityScale * moveData.apexFall;
		}
		else if (Body.velocity.y < 0)
		{
			// fall acceleration
			Body.gravityScale = moveData.gravityScale * moveData.fallAccel;
			Body.velocity = new Vector2(Body.velocity.x, Mathf.Max(Body.velocity.y, -moveData.fallClamp));
		}
		else
		{
			// default gravity
			Body.gravityScale = moveData.gravityScale;
		}
		Body.gravityScale *= gravityModifier;
	}

	// walking movement
	private void WalkEvent(float lerp)
	{
		// target speed
		float target = moveDirection.x * moveData.walkClamp, accel;

		// lerping
		target = Mathf.Lerp(Body.velocity.x, target, lerp);

		// acceleration
		if (LastOnGround > 0)
		{
			accel = (Mathf.Abs(target) > 0.01f) ? moveData.accelAmount : moveData.decelAmount;
		}
		else
		{
			accel = (Mathf.Abs(target) > 0.01f) ? moveData.accelAmount * moveData.airAccel : moveData.decelAmount * moveData.airDecel;
		}

		// apex boost
		if ((Jumping || WallJumping || jumpFalling) && Mathf.Abs(Body.velocity.y) < moveData.apexThreshold)
		{
			accel *= moveData.apexAccelBoost;
			target *= moveData.apexBoost;
		}

		// momentum conservation
		if(moveData.conserveMomentum && Mathf.Abs(Body.velocity.x) > Mathf.Abs(target) && Mathf.Sign(Body.velocity.x) == Mathf.Sign(target) && Mathf.Abs(target) > 0.01f && LastOnGround < 0)
		{
			accel = 0; 
		}

		// apply force
		Body.AddForce((target - Body.velocity.x) * accel * Vector2.right, ForceMode2D.Force);
	}

	// sliding action
	private void SlideEvent() 
	{
		float force = (moveData.slideSpeed - Body.velocity.y) * moveData.slideAccel;
		force = Mathf.Clamp(force, -Mathf.Abs(moveData.slideSpeed - Body.velocity.y)  * (1 / Time.fixedDeltaTime), Mathf.Abs(moveData.slideSpeed - Body.velocity.y) * (1 / Time.fixedDeltaTime));

		Body.AddForce(force * Vector2.up);
	}

	// player movement
	private void MoveEvent()
	{
		// player movement
		if (!Dashing)
		{
			if (WallJumping)
			{
				WalkEvent(moveData.wallJumpWeakness);
			}
			else
			{
				WalkEvent(1);
			}
		}
		else if (dashFloat)
		{
			WalkEvent(moveData.dashLerp);
		}

		// wall sliding
		if (Sliding)
		{
			SlideEvent();
		}
	}

	// debug lines
	private void OnDrawGizmos()
    {
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
		Gizmos.DrawWireCube(frontCheck.position, wallCheckSize);
		Gizmos.DrawWireCube(backCheck.position, wallCheckSize);
	}

	// gravity slowing
	public void SlowTime(float time)
	{
		StartCoroutine(nameof(SlowGravity), time);
	}

	private IEnumerator SlowGravity(float time)
	{
		gravityModifier = 0.1f;
		yield return new WaitForSeconds(time);
		gravityModifier = 1f;
	}
}