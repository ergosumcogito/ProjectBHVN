using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Level_01", menuName = "LevelSystem/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Level Type")]
    public LevelType levelType = LevelType.Normal;
    
    [Header("Level Size Settings")]
    public int width = 10;
    public int length = 10;
    public float tileSize = 1f;
    
    [Header("Floor Tiles")]
    public List<GameObject> tilePrefabs = new();

    [Header("Top Wall Layers")]
    public GameObject topWallBottom;
    public GameObject topWallMiddle;
    public GameObject topWallTop; 
    
    [Header("Border Tiles")]
    public GameObject borderLeft;
    public GameObject borderRight;
    public GameObject borderBottom;
    
    [Header("Corner Tiles")]
    public GameObject cornerTopLeft;
    public GameObject cornerTopRight;
    public GameObject cornerBottomLeft;
    public GameObject cornerBottomRight;
    

    //EnemyList
    [Header("Enemy List")]
    [SerializeField] public List<WeightedEnemy> enemyPrefabs = new();
}