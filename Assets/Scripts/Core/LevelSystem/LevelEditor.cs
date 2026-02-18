using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    public CinemachineCamera cinemachineCamera;
    
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
        if (levelData.tilePrefabs.Count == 0|| !levelData.borderPrefab)
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
                    int randomIndex = Random.Range(0, levelData.tilePrefabs.Count);
                    Vector3 pos = new Vector3(x * levelData.tileSize, y * levelData.tileSize, 0);
                    GameObject tile = Instantiate(levelData.tilePrefabs[randomIndex], pos, Quaternion.identity, transform);
                    tile.name = $"Tile x={x}, y={y}";
                }
            }
        }
        GenerateCameraBounds();
    }
    
    private void GenerateCameraBounds()
    {
        GameObject boundsObj = GameObject.Find("RuntimeCameraBounds");
        if (boundsObj == null) {
            boundsObj = new GameObject("RuntimeCameraBounds");
        }

        BoxCollider2D boxCollider = boundsObj.GetComponent<BoxCollider2D>();
        if (boxCollider == null) boxCollider = boundsObj.AddComponent<BoxCollider2D>();
    
        boxCollider.isTrigger = true;
        
        float totalWidth = (levelData.width + 1) * levelData.tileSize;
        float totalLength = (levelData.length + 1) * levelData.tileSize;
        
        float centerX = (levelData.width - 1) * 0.5f * levelData.tileSize;
        float centerY = (levelData.length - 1) * 0.5f * levelData.tileSize;

        boxCollider.size = new Vector2(totalWidth, totalLength);
        boundsObj.transform.position = new Vector3(centerX, centerY, 0);

        var confiner = cinemachineCamera.GetComponent<CinemachineConfiner2D>();
        if (confiner != null)
        {
            confiner.BoundingShape2D = boxCollider;
            confiner.InvalidateBoundingShapeCache();
        }
    }
    
}
