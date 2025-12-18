using UnityEngine;

public class ProjectileWeapon : WeaponBase
{
    protected GameObject projectilePrefab;
    protected float projectileSpeed;

    public override void Init(WeaponData stats)
    {
        base.Init(stats);

        projectilePrefab = stats.projectilePrefab;
        projectileSpeed = stats.projectileSpeed;
    }

    protected override void Attack(Transform target)
    {
        Vector3 dir = (target.position - transform.position).normalized;

        float finalDamage = CalculateDamage();
        
        GameObject projGO = Instantiate(
            projectilePrefab,
            transform.position,
            Quaternion.identity
        );

        Projectile proj = projGO.GetComponent<Projectile>();
        proj.Init(dir, projectileSpeed, finalDamage);
    }
}