using System;
using Unity.Properties;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // public variables
    public float Health { get; private set; }
    public int Level { get; private set; }
    public float Experience { get; private set; }
    
    // serialized variables
    [Header("General")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject playerCore;
    [SerializeField] private Healthbar healthbar;
    [SerializeField] private Experiencebar experiencebar;
    [SerializeField] private LevelDisplay levelDisplay;
    
    [Header("Stats")]
    [SerializeField] private float invincibility;
    [SerializeField] private float maxHealth;
    [SerializeField] private float healSpeed;

    [Header("Leveling")]
    [SerializeField] private float baseXpRequirement;
    [SerializeField] private float xpRequirementGrowth;

    [Header("Abilities")]
    [SerializeField] private AbilityHandler primary;
    [SerializeField] private AbilityHandler ability1;
    [SerializeField] private AbilityHandler ability2;
    [SerializeField] private AbilityHandler ability3;

    // component variables
    private Rigidbody2D playerBody;
    private PlayerInput playerInput;

    // input variables
    private InputAction lookAction;
	private InputAction fireAction;
    private InputAction abil1Action;
    private InputAction abil2Action;
    private InputAction abil3Action;

    // other variables
    private Vector2 lookDirection;
    private float invincibilityTimer = 0;
    private float xpRequirement;

    // runs before scene loads
    private void Awake()
	{
        // grab components
		playerBody = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        // input actions
        lookAction = playerInput.actions["Look"];
        fireAction = playerInput.actions["Fire"];
        abil1Action = playerInput.actions["Ability1"];
        abil2Action = playerInput.actions["Ability2"];
        abil3Action = playerInput.actions["Ability3"];
    }

    // runs when script loads
    private void Start()
    {
        Health = maxHealth;
        Level = 1;
        Experience = 0;
        xpRequirement = baseXpRequirement;
        GainXP(0);
    }

    // runs every frame
    private void Update()
    {
        // rotate pointer
        lookDirection = mainCamera.ScreenToWorldPoint(lookAction.ReadValue<Vector2>()).normalized;
        playerCore.transform.up = lookDirection;

        // handle abilities
        primary.Handle(fireAction.IsPressed());
        ability1.Handle(abil1Action.IsPressed());
        ability2.Handle(abil2Action.IsPressed());
        ability3.Handle(abil3Action.IsPressed());
        // if (fireAction.WasPressedThisFrame()) {
        //     Damage(25);
        //     Debug.Log(Health);
        // }

        healthbar.SetHealthPercent(Health / maxHealth);

        // decrease timers
        invincibilityTimer -= Time.deltaTime;
    }

    // runs at 60 fps
    private void FixedUpdate()
    {
        
    }

    // health modification
    public void Damage(float amt)
    {
        if (invincibilityTimer > 0) return;

        Health = Mathf.Max(Health-amt, 0);
        healthbar.SetHealthPercent(Health / maxHealth);
        invincibilityTimer = invincibility;
    }

    public void Heal(float amt)
    {
        Health = Mathf.Min(Health+amt, maxHealth);
        healthbar.SetHealthPercent(Health / maxHealth);
    }

    // level modification
    public void GainXP(float amt)
    {
        Experience += amt;
        if (Experience > xpRequirement) {
            Experience -= xpRequirement;
            LevelUp(1);
        } experiencebar.SetExperiencePercent(Experience / xpRequirement);
    }

    public void LevelUp(int amt)
    {
        Level += amt;
        xpRequirement *= xpRequirementGrowth;
        levelDisplay.SetLevel(Level);
    }

    public void ResetLevels()
    {
        Level = 0;
        Experience = 0;
        xpRequirement = baseXpRequirement;
    }
}
