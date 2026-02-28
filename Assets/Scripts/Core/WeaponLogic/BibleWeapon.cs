using UnityEngine;

public class BibleWeapon : WeaponBase
{
    protected GameObject projectilePrefab;
    protected float projectileSpeed;
    Transform player;
    [SerializeField]private float projectileDistance;

    public override void Init(WeaponData stats)
    {
        base.Init(stats);
        player = GetComponentInParent<Transform>();
        projectilePrefab = stats.projectilePrefab;
        projectileSpeed = stats.projectileSpeed;
    }

    protected override void Attack(Transform target)
    {

        float finalDamage = CalculateDamage();
        CreateProjectile(finalDamage, new Vector3(player.position.x + projectileDistance, player.position.y, player.position.z));
        CreateProjectile(finalDamage, new Vector3(player.position.x , player.position.y + projectileDistance, player.position.z));
        CreateProjectile(finalDamage, new Vector3(player.position.x - projectileDistance, player.position.y, player.position.z));
        CreateProjectile(finalDamage, new Vector3(player.position.x, player.position.y - projectileDistance, player.position.z));
        
    }

    private void CreateProjectile(float finalDamage, Vector3 position)
    {
        GameObject projGO = Instantiate(
            projectilePrefab,
            transform.position,
            Quaternion.identity
        );
        
        BibleProjectile proj = projGO.GetComponent<BibleProjectile>();
        proj.Init(projectileSpeed, finalDamage, attackSpeed, position, player);
    }

    protected override void TryAttack()
    {
        Attack(null);
        attackCooldown = 1f / attackSpeed;
    }
}
