using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // public variables
    public static GameObject Player { get; private set; }
    public float Health { get; private set; }
    public int Level { get; private set; }
    public float Experience { get; private set; }
    
    // serialized variables
    [Header("General")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject playerCore;
    [SerializeField] private Healthbar healthbar;
    [SerializeField] private Experiencebar experiencebar;
    [SerializeField] private LevelDisplay levelDisplay;
    [SerializeField] private Material flashMaterial;
    
    [Header("Stats")]
    public float maxHealth;
    public float healSpeed;
    public float invincibility;
    public float attackDamage;
    public float attackSpeed;
    public float attackKnockback;
    public float experienceMultiplier;
    public float cameraSize;
    public int bulletCount;
    public int bulletPierce;

    [Header("Leveling")]
    [SerializeField] private float baseXpRequirement;
    [SerializeField] private float xpRequirementGrowth;

    [Header("Abilities")]
    [SerializeField] private AbilityHandler primary;
    [SerializeField] private AbilityHandler ability1;
    [SerializeField] private AbilityHandler ability2;
    [SerializeField] private AbilityHandler ability3;

    [Header("Sound")]
    [SerializeField] private AudioClip damageClip;

    // component variables
    private Rigidbody2D playerBody;
    private PlayerInput playerInput;
    private SpriteRenderer sprite;
    private Material spriteMaterial;

    // input variables
    private InputAction lookAction;
	private InputAction fireAction;
    private InputAction abil1Action;
    private InputAction abil2Action;
    private InputAction abil3Action;
    private InputAction pauseAction;

    // other variables
    private Vector2 lookDirection;
    private float invincibilityTimer = 0;
    private float healingTimer = 0;
    private float xpRequirement;
    private bool flashing = false;

    private void OnEnable() { Player = gameObject; }
    private void OnDisable() { Player = null; }

    // runs before scene loads
    private void Awake()
	{
        // grab components
		playerBody = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        spriteMaterial = sprite.material;

        // input actions
        lookAction = playerInput.actions["Look"];
        fireAction = playerInput.actions["Fire"];
        abil1Action = playerInput.actions["Ability1"];
        abil2Action = playerInput.actions["Ability2"];
        abil3Action = playerInput.actions["Ability3"];
        pauseAction = playerInput.actions["Pause"];
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
        primary.Handle(fireAction.IsPressed(), this);
        ability1.Handle(abil1Action.IsPressed(), this);
        ability2.Handle(abil2Action.IsPressed(), this);
        ability3.Handle(abil3Action.IsPressed(), this);

        // pausing
        pauseAction.started += _ => { gameManager.Pause(); };

        // healing
        if (healSpeed > 0) Heal(healSpeed);

        // change camera size
        if (mainCamera.orthographicSize != cameraSize) mainCamera.orthographicSize = cameraSize;

        // decrease timers
        invincibilityTimer -= Time.deltaTime;
        healingTimer -= Time.deltaTime;
    }

    // health modification
    public void Damage(float amt)
    {
        if (invincibilityTimer > 0) return;

        Health = Mathf.Max(Health-amt, 0);
        healthbar.SetHealthPercent(Health / maxHealth);
        Flash(0.15f);
        AudioManager.instance.PlaySoundClip(damageClip, transform, 0.8f);
        invincibilityTimer = invincibility;
    }

    public void Heal(float amt)
    {
        if (healingTimer > 0) return;

        Health = Mathf.Min(Health+amt, maxHealth);
        healthbar.SetHealthPercent(Health / maxHealth);
        healingTimer = 1;
    }

    // level modification
    public void GainXP(float amt)
    {
        Experience += amt * experienceMultiplier;
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
        gameManager.Upgrades();
    }

    public void ResetLevels()
    {
        Level = 0;
        Experience = 0;
        xpRequirement = baseXpRequirement;
    }

    // addstats
    public void AddStats(PlayerUpgrade upgrade)
    {
        Health += upgrade.health + upgrade.maxHealth;
        maxHealth += upgrade.maxHealth;
        healSpeed += upgrade.healSpeed;
        invincibility += upgrade.invincibility;
        attackDamage += upgrade.attackDamage;
        attackSpeed += upgrade.attackSpeed;
        attackKnockback += upgrade.attackKnockback;
        experienceMultiplier += upgrade.experienceMultiplier;
        cameraSize += upgrade.cameraSize;
        bulletCount += upgrade.bulletCount;
        bulletPierce += upgrade.bulletPierce;
    }
    
    // damage flash
    public void Flash(float duration)
    {
        if (flashing) {
            StopCoroutine(nameof(FlashSprite));
        } StartCoroutine(nameof(FlashSprite), duration);
    }

    private IEnumerator FlashSprite(float dur)
    {
        flashing = true;
        sprite.material = flashMaterial;
        yield return new WaitForSeconds(dur);
        sprite.material = spriteMaterial;
        flashing = false;
    }
}
