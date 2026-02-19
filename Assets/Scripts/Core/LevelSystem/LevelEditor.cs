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
        
        if (levelData.tilePrefabs == null || levelData.tilePrefabs.Count == 0)
        {
            Debug.LogError("Floor Tiles missing");
            return;
        }

        if (!levelData.topWallBottom || !levelData.topWallMiddle || !levelData.topWallTop)
        {
            Debug.LogError("Top Wall Prefabs missing");
            return;
        }
        
        if (!levelData.borderBottom || 
            !levelData.borderLeft || !levelData.borderRight ||
            !levelData.cornerTopLeft || !levelData.cornerTopRight || 
            !levelData.cornerBottomLeft || !levelData.cornerBottomRight)
        {
            Debug.LogError("One of the corner Prefabs missing");
            return;
        }
        
        GenerateTiles();
        Debug.Log($"Level generated: {levelData.width}x{levelData.length}");
    }

    //Generates Tiles
    public void GenerateTiles()
    {
        
        int wallHeight = 4;
        
        GenerateFloor();
        GenerateBottomWall();
        GenerateLeftWall();
        GenerateRightWall();
        GenerateTopWall(wallHeight);
        GenerateCorners(wallHeight);
        GenerateCameraBounds(wallHeight);
}

    private void GenerateFloor()
    {
        for (int x = 0; x < levelData.width; x++)
        {
            for (int y = 0; y < levelData.length; y++)
            {
                SpawnTile(GetRandomFloor(), x, y, "Floor");
            }
        }
    }

    private void GenerateTopWall(int wallHeight)
    {
        
        if (wallHeight < 2) wallHeight = 2;

        for (int x = 0; x < levelData.width; x++)
        {
            SpawnTile(levelData.topWallBottom, x, levelData.length, "TopWall_Base");
            
            for (int h = 1; h < wallHeight - 1; h++)
            {
                SpawnTile(levelData.topWallMiddle, x, levelData.length + h, "TopWall_Middle");
            }
            
            SpawnTile(levelData.topWallTop, x, levelData.length + (wallHeight - 1), "TopWall_Top");
        }
    }

    private void GenerateCorners(int wallHeight)
    {
        SpawnTile(levelData.cornerBottomLeft, -1, -1, "Corner_BL");
        SpawnTile(levelData.cornerBottomRight, levelData.width, -1, "Corner_BR");
        
        int topY = levelData.length + wallHeight - 1; 
        SpawnTile(levelData.cornerTopLeft, -1, topY, "Corner_TL");
        SpawnTile(levelData.cornerTopRight, levelData.width, topY, "Corner_TR");


        for (int i = 0; i < wallHeight - 1; i++)
        {
            SpawnTile(levelData.borderLeft, -1, levelData.length + i, "Wall_Left_Fill");
            SpawnTile(levelData.borderRight, levelData.width, levelData.length + i, "Wall_Right_Fill");
        }
    }
    
    private void GenerateBottomWall()
    {
        for (int x = 0; x < levelData.width; x++)
        {
            SpawnTile(levelData.borderBottom, x, -1, "Wall_Bottom");
        }
    }

    private void GenerateLeftWall()
    {
        for (int y = 0; y < levelData.length; y++)
        {
            SpawnTile(levelData.borderLeft, -1, y, "Wall_Left");
        }
    }

    private void GenerateRightWall()
    {
        for (int y = 0; y < levelData.length; y++)
        {
            SpawnTile(levelData.borderRight, levelData.width, y, "Wall_Right");
        }
    }
    
    private void GenerateCameraBounds(int topWallHeight)
    {
        GameObject boundsObj = GameObject.Find("RuntimeCameraBounds");
        if (boundsObj == null) {
            boundsObj = new GameObject("RuntimeCameraBounds");
        }

        BoxCollider2D boxCollider = boundsObj.GetComponent<BoxCollider2D>();
        if (boxCollider == null) boxCollider = boundsObj.AddComponent<BoxCollider2D>();
    
        boxCollider.isTrigger = true;
        
        float totalWidth = (levelData.width + 1) * levelData.tileSize;
        float totalLength = (levelData.length + topWallHeight + 1) * levelData.tileSize; 
    
        float centerX = (levelData.width - 1) * 0.5f * levelData.tileSize;
        float centerY = (levelData.length + topWallHeight - 1) * 0.5f * levelData.tileSize;

        boxCollider.size = new Vector2(totalWidth, totalLength);
        boundsObj.transform.position = new Vector3(centerX, centerY, 0);

        var confiner = cinemachineCamera.GetComponent<CinemachineConfiner2D>();
        if (confiner != null)
        {
            confiner.BoundingShape2D = boxCollider;
            confiner.InvalidateBoundingShapeCache();
        }
    }
    
    private void SpawnTile(GameObject prefab, int x, int y, string name)
    {
        if (prefab == null) return;
        Vector3 pos = new Vector3(x * levelData.tileSize, y * levelData.tileSize, 0);
        GameObject tile = Instantiate(prefab, pos, Quaternion.identity, transform);
        tile.name = $"{name} ({x},{y})";
    }

    private GameObject GetRandomFloor()
    {
        return levelData.tilePrefabs[Random.Range(0, levelData.tilePrefabs.Count)];
    }
    
}
