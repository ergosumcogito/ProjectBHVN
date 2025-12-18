using UnityEngine;

[System.Serializable]
public class WeaponPrefabEntry
{
    public string weaponName;
    public GameObject prefab;
}

[CreateAssetMenu(fileName = "WeaponPrefabsConfig", menuName = "Scriptable Objects/WeaponPrefabs")]
public class WeaponPrefabsConfig : ScriptableObject
{
    public WeaponPrefabEntry[] weapons;

    public GameObject GetPrefab(string weaponName)
    {
        foreach (var entry in weapons)
        {
            if (entry.weaponName == weaponName)
                return entry.prefab;
        }
        return null;
    }
}