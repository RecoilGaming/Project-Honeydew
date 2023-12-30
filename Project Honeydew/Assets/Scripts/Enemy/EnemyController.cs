using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // public variable
    public float Health { get; private set; }

    // private variables
    [SerializeField] private EnemyHandler enemyHandler;
    [SerializeField] private Material flashMaterial;
    private SpriteRenderer sprite;
    private Material spriteMaterial;
    private bool flashing = false;

    // runs before scene loads
    private void Awake()
	{
		sprite = GetComponentInChildren<SpriteRenderer>();
        spriteMaterial = sprite.material;
    }

    // runs when script loads
    private void Start()
    {
        Health = enemyHandler.GetEnemy().maxHealth;
    }

    // runs every frame
    void Update()
    {
        enemyHandler.Handle(PlayerController.Player, gameObject);
    }

    // health modification
    public void Damage(float amt)
    {
        Health -= amt;
    }

    public void Heal(float amt)
    {
        Health += amt;
    }

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
