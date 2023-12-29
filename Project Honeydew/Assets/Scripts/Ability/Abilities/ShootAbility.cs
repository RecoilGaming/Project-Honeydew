using UnityEngine;

[CreateAssetMenu(fileName = "Shoot", menuName = "Abilities/Shoot")]
public class ShootAbility : Ability
{
    [Header("Projectile")]
    public GameObject projectile;

    public override void Activate(GameObject player)
    {
        Transform firePoint = player.transform.GetChild(1).GetChild(0);
        Instantiate(projectile, firePoint.position, firePoint.rotation);
    }
}