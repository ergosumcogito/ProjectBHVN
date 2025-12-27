using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    [HideInInspector] 
    public LevelData levelData;

    public int Width => levelData.width;
    public int Length => levelData.length;
    
    public void ClearLevel()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
        Debug.Log("All Tiles destroyed");
    }
    
    public void LoadAndStart(LevelData data)
    {
        levelData = data;
        GenerateLevel();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void GenerateLevel()
    {
        ClearLevel();
        //Check if TilePrefab or BorderPrefab equals null
        if (!levelData.tilePrefab || !levelData.borderPrefab)
        {
            Debug.Log("TilePrefab or BorderPrefab is null");
        }
        else
        {
            GenerateTiles();
            Debug.Log("Tiles generated");
        } 
    }

    //Generates Tiles
    public void GenerateTiles()
    {
        //Generates Tiles incl Border
        int borderX = levelData.width + 1;
        int borderY = levelData.length + 1;

        for (int x = -1; x < borderX; x++)
        {
            for (int y = -1; y < borderY; y++)
            {
                if (x == -1 || x == levelData.width || y == -1 || y == levelData.length)
                {
                    Vector3 pos = new Vector3(x * levelData.tileSize, y * levelData.tileSize, 0);
                    GameObject tile = Instantiate(levelData.borderPrefab, pos, Quaternion.identity, transform);
                    tile.name = $"Border x={x}, y={y}";
                }
                else
                {
                    Vector3 pos = new Vector3(x * levelData.tileSize, y * levelData.tileSize, 0);
                    GameObject tile = Instantiate(levelData.tilePrefab, pos, Quaternion.identity, transform);
                    tile.name = $"Tile x={x}, y={y}";
                }
            }
        }
    }
}
