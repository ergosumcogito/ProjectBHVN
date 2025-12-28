using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Level_01", menuName = "LevelSystem/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Generation Settings")]
    public int width = 10;
    public int length = 10;
    public float tileSize = 1f;
    
    [Header("Required Prefabs")]
    public GameObject tilePrefab;
    public GameObject borderPrefab;

    //EnemyList
    [Header("Enemy List")]
    [SerializeField] public List<WeightedEnemy> enemyPrefabs = new();
}