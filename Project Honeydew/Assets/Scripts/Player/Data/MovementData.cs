using UnityEngine;

[CreateAssetMenu(menuName = "Movement Data")]
public class MovementData : ScriptableObject
{
    // variables
    [HideInInspector] public float gravity;
	[HideInInspector] public float gravityScale;
    [HideInInspector] public float accelAmount;
	[HideInInspector] public float decelAmount;
    [HideInInspector] public float jumpAmount;
    
    [Header("Gravity")]
    public float fallClamp;
	public float fastFallClamp;
    public float fallAccel;
    public float fastFallAccel;
	public float fallDampThreshold;

	[Header("Walk")]
	public float walkClamp;
	public float walkAccel;
	public float walkDecel;
	[Range(0f, 1)] public float airAccel;
	[Range(0f, 1)] public float airDecel;
	public bool conserveMomentum = true;

	[Header("Jump")]
	public float jumpHeight;
	public float jumpTime;
	public float jumpWeakness;
	public float apexThreshold;
    public float apexBoost; 
	public float apexAccelBoost;
    [Range(0f, 1)] public float apexFall;

	[Header("Wall Jump")]
	public Vector2 wallJumpAmount;
	[Range(0f, 1f)] public float wallJumpWeakness;
	[Range(0f, 1.5f)] public float wallJumpRecover;
	public bool turnOnWallJump;

	[Header("Slide")]
	public float slideSpeed;
	public float slideAccel;

    [Header("Quality")]
	[Range(0.01f, 0.5f)] public float coyoteTime;
	[Range(0.01f, 0.5f)] public float jumpBuffer;

	[Header("Dash")]
	public int dashAmount;
	public float dashSpeed;
	public float dashAttackTime;
	public float dashEnd;
	public float dashCooldown;
	public Vector2 dashSlow;
	[Range(0f, 1f)] public float dashLerp;
	[Range(0.01f, 0.5f)] public float dashBuffer;
	
	// runs when script is loaded
    private void OnValidate()
    {
		// calculate gravity
		gravity = -(2 * jumpHeight) / (jumpTime * jumpTime);
		gravityScale = gravity / Physics2D.gravity.y;

		// calculate acceleration
		accelAmount = 50 * walkAccel / walkClamp;
		decelAmount = 50 * walkDecel / walkClamp;

		// calculate jump amount
		jumpAmount = Mathf.Abs(gravity) * jumpTime;

        // clamp acceleration
        walkAccel = Mathf.Clamp(walkAccel, 0.01f, walkClamp);
		walkDecel = Mathf.Clamp(walkDecel, 0.01f, walkClamp);
	}
}