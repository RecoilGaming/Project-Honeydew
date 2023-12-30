using UnityEngine;

[CreateAssetMenu(fileName = "Shoot", menuName = "Abilities/Shoot")]
public class ShootAbility : Ability
{
    [Header("Projectile")]
    public GameObject projectile;

    public override void Activate(GameObject player)
    {
        Transform firePoint = player.transform.GetChild(1).GetChild(0);
        GameObject newProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation);
        newProjectile.GetComponent<ProjectileController>().projectile.damageAddon = player.GetComponent<PlayerController>().attackDamage;
    }
}