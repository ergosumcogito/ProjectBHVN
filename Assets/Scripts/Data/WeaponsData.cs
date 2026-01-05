using UnityEngine;

[System.Serializable]
public class WeaponData
{
    public string weaponName;
    
    [Header("Prefab")]
    public GameObject weaponPrefab;

    [Header("Scaling Multipliers")]
    public float meleeDamageScale;
    public float rangedDamageScale;
    public float attackSpeedScale;
    public float attackRangeScale;
    public float critChanceScale;

    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public float projectileSpeed;
}

[CreateAssetMenu(fileName = "WeaponsData", menuName = "Scriptable Objects/WeaponsData")]
public class WeaponsData : ScriptableObject
{
    public WeaponData[] allWeapons;
    
    public WeaponData GetWeaponByName(string weaponName)
    {
        foreach (var w in allWeapons)
        {
            if (w.weaponName == weaponName)
                return w;
        }
        return null;
    }

    public GameObject GetWeaponPrefabByName(string weaponName)
    {
        foreach (var w in allWeapons)
        {
            if (w.weaponName == weaponName)
                return w.weaponPrefab;
        }
        return null;
        
    }
}