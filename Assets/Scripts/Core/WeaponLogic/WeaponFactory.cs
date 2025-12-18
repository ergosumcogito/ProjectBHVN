using UnityEngine;

public class WeaponFactory : MonoBehaviour
{
    public WeaponsData weaponsData;
    public WeaponPrefabsConfig prefabsConfig;
    [HideInInspector] public Transform weaponSlot;

    public WeaponBase CreateWeapon(string weaponName)
    {
        WeaponData stats = weaponsData.GetWeaponByName(weaponName);
        if (stats == null)
        {
            Debug.LogError("Weapon stats not found: " + weaponName);
            return null;
        }

        GameObject prefab = prefabsConfig.GetPrefab(weaponName);
        if (prefab == null)
        {
            Debug.LogError("Weapon prefab not found: " + weaponName);
            return null;
        }

        GameObject weaponObj = Instantiate(prefab, weaponSlot);
        WeaponBase weapon = weaponObj.GetComponent<WeaponBase>();

        if (weapon == null)
        {
            Debug.LogError("Prefab has no WeaponBase: " + weaponName);
            return null;
        }

        weapon.Init(stats);
        return weapon;
    }
}