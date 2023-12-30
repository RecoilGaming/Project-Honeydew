using UnityEngine;

[CreateAssetMenu(fileName = "Shoot", menuName = "Abilities/Shoot")]
public class ShootAbility : Ability
{
    [Header("Projectile")]
    public GameObject projectile;
    [SerializeField] private AudioClip fireClip;

    public override void Activate(GameObject player)
    {
        Transform firePoint = player.transform.GetChild(1).GetChild(0);
        GameObject newProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation);
        newProjectile.GetComponent<ProjectileController>().projectile.damageAddon = player.GetComponent<PlayerController>().attackDamage;
        newProjectile.GetComponent<ProjectileController>().projectile.knockback = player.GetComponent<PlayerController>().attackKnockback;
        AudioManager.instance.PlaySoundClip(fireClip, player.transform, 1f);
    }
}