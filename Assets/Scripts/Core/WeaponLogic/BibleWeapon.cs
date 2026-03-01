using UnityEngine;

public class BibleWeapon : WeaponBase
{
    protected GameObject projectilePrefab;
    protected float projectileSpeed;
    private Transform player;
    [SerializeField]private float projectileDistance;

    public override void Init(WeaponData stats)
    {
        base.Init(stats);
        player = playerStats.GetComponentInParent<Transform>();
        projectilePrefab = stats.projectilePrefab;
        projectileSpeed = stats.projectileSpeed;
    }

    protected override void Attack(Transform target)
    {
        float finalDamage = CalculateDamage();
        CreateProjectile(finalDamage, new Vector3(player.position.x + projectileDistance, player.position.y, 0));
        CreateProjectile(finalDamage, new Vector3(player.position.x , player.position.y + projectileDistance, 0));
        CreateProjectile(finalDamage, new Vector3(player.position.x - projectileDistance, player.position.y, 0));
        CreateProjectile(finalDamage, new Vector3(player.position.x, player.position.y - projectileDistance, 0));
        
    }

    private void CreateProjectile(float finalDamage, Vector3 position)
    {
        GameObject projGO = Instantiate(
            projectilePrefab,
            position,
            Quaternion.identity,
            player
        );
        
        BibleProjectile proj = projGO.GetComponent<BibleProjectile>();
        proj.Init(projectileSpeed, finalDamage, attackSpeed, player);
    }

    protected override void TryAttack()
    {
        Attack(null);
        attackCooldown = 5f / attackSpeed;
    }
}
