using System;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Level_01", menuName = "LevelSystem/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Level Type")]
    public LevelType levelType = LevelType.Normal;
    
    [Header("Level background music")]
    public AudioClip backgroundMusic;
    [Range(0f, 1f)] public float musicVolume = 1f;
    public bool loopMusic = true;
    public float fadeIn = 0.5f;
    public float fadeOut = 0.5f;

    
    [Header("Player Spawnp Position")]
    public PlayerSpawnPosition playerSpawnPosition = PlayerSpawnPosition.Center;
    
    [Header("Level Size Settings")]
    
    [Range(20, 100)]
    public int width = 10;
    [Range(20, 100)]
    public int height = 10;

    [Header("Floor Tiles")]
    public List<GameObject> tilePrefabs = new();

    [Header("Top Wall Layers")]
    public GameObject topWallBottom;
    public GameObject topWallMiddle;
    public GameObject topWallTop;
    [Range(1, 10)]
    public int topWallHeight = 2;

    [Header("Border Tiles")]
    public GameObject borderLeft;
    public GameObject borderRight;
    public GameObject borderBottom;

    [Header("Corner Tiles")]
    public GameObject cornerTopLeft;
    public GameObject cornerTopRight;
    public GameObject cornerBottomLeft;
    public GameObject cornerBottomRight;

    [Header("Spawn Tiles under Border?")]
    public Boolean underBorderLeft = false;
    public Boolean underBorderRight = false;
    public Boolean underBorderBottom = false;
    public Boolean underBorderTop = false;

    [Header("Spawn Tiles under Border?")]
    public Boolean underCornerTopLeft = false;
    public Boolean underCornerTopRight = false;
    public Boolean underCornerBottomLeft = false;
    public Boolean underCornerBottomRight = false;

    //EnemyList
    [Header("Enemy List")]
    [SerializeField] public List<WeightedEnemy> enemyPrefabs = new();
}