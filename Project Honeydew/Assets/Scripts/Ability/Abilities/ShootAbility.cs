using UnityEngine;

[CreateAssetMenu(fileName = "Shoot", menuName = "Abilities/Shoot")]
public class ShootAbility : Ability
{
    [Header("Projectile")]
    public GameObject projectile;
    [SerializeField] private AudioClip fireClip;

    public override void Activate(GameObject player)
    {
        int bullets = player.GetComponent<PlayerController>().bulletCount;
        Transform firePoint = player.transform.GetChild(1).GetChild(0);
        for (int i = 0; i < bullets; i++) {
             GameObject newProjectile = Instantiate(projectile, Quaternion.Euler(0, 0, 15*(i-bullets/2)) * firePoint.position, firePoint.rotation);
            newProjectile.GetComponent<ProjectileController>().projectile.damageAddon = player.GetComponent<PlayerController>().attackDamage;
            newProjectile.GetComponent<ProjectileController>().projectile.knockback = player.GetComponent<PlayerController>().attackKnockback;
            newProjectile.GetComponent<ProjectileController>().projectile.pierce = player.GetComponent<PlayerController>().bulletPierce;
            AudioManager.instance.PlaySoundClip(fireClip, player.transform, 1f);
        }
    }
}