using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    public CinemachineCamera cinemachineCamera;
    
    [HideInInspector] 
    public LevelData levelData;

    public int Width => levelData.width;
    public int Height => levelData.height;
    
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

        if (!levelData.topWallBottom)
        {
            Debug.LogError("Basic Top Wall Prefab is missing");
            return;
        }
        
        if (levelData.topWallHeight == 2 && !levelData.topWallTop)
        {
            Debug.LogError("Top Wall Top is missing");
            return;
        }

        if (levelData.topWallHeight >= 3 && !levelData.topWallMiddle ||
            levelData.topWallHeight >= 3 && !levelData.topWallTop)
        {
            Debug.LogError("Top Wall Top or Top Wall Middle is missing");
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
        Debug.Log($"Level generated: {levelData.width}x{levelData.height}");
    }

    //Generates Tiles
    public void GenerateTiles()
    {
        
        int wallHeight = levelData.topWallHeight;
        
        GenerateFloor();
        
        GenerateSideWalls(wallHeight);
        GenerateTopWall(wallHeight);
        GenerateBottomWall();
        
        GenerateCorners(wallHeight);
        
        GenerateCameraBounds(wallHeight);
}

    private void GenerateFloor()
    {
        for (int x = 0; x < levelData.width; x++)
        {
            for (int y = 0; y < levelData.height; y++)
            {
                SpawnTile(GetRandomFloor(), x, y, "Floor");
            }
        }

        if (levelData.underBorderBottom)
        {
            for (int x = 0; x < levelData.width; x++)
            {
                SpawnTile(GetRandomFloor(), x, -1, "Floor_underBorder_bottom");
            }
        }
        if (levelData.underBorderTop)
        {
            for (int x = 0; x < levelData.width; x++)
            {
                SpawnTile(GetRandomFloor(), x, levelData.height, "Floor_underBorder_top");
            }
        }
        
        if (levelData.underBorderLeft)
        {
            for (int y = 0; y < levelData.height; y++)
            {
                SpawnTile(GetRandomFloor(), -1, y, "Floor_underBorder_left");
            }
        }
        
        if (levelData.underBorderRight)
        {
            for (int y = 0; y < levelData.height; y++)
            {
                SpawnTile(GetRandomFloor(), levelData.width, y, "Floor_underBorder_right");
            }
        }

        if (levelData.underCornerBottomLeft)
        {
            SpawnTile(GetRandomFloor(), -1, -1, "Floor_underBorder_corner_bl");
        }
        if (levelData.underCornerBottomRight)
        {
            SpawnTile(GetRandomFloor(), levelData.width, -1, "Floor_underBorder_corner_br");
        }
        
        if (levelData.underCornerTopLeft)
        {
            SpawnTile(GetRandomFloor(), -1, levelData.height, "Floor_underBorder_corner_tl");
        }
        
        if (levelData.underCornerTopRight)
        {
            SpawnTile(GetRandomFloor(), levelData.width, levelData.height, "Floor_underBorder_corner_tr");
        }
        
    }

    private void GenerateTopWall(int wallHeight)
    {
        int height = Mathf.Max(1, wallHeight);

        for (int x = 0; x < levelData.width; x++)
        {
            if (height == 1)
            {
                SpawnTile(levelData.topWallBottom, x, levelData.height, "TopWall_Flat");
            }
            else
            {
                SpawnTile(levelData.topWallBottom, x, levelData.height, "TopWall_Base");
                
                for (int h = 1; h < height - 1; h++)
                {
                    SpawnTile(levelData.topWallMiddle, x, levelData.height + h, "TopWall_Middle");
                }
                
                SpawnTile(levelData.topWallTop, x, levelData.height + (height - 1), "TopWall_Top");
            }
        }
    }

    private void GenerateCorners(int wallHeight)
    {
        int height = Mathf.Max(1, wallHeight);
        
        SpawnTile(levelData.cornerBottomLeft, -1, -1, "Corner_BL");
        SpawnTile(levelData.cornerBottomRight, levelData.width, -1, "Corner_BR");
        
        int topY = levelData.height + (height - 1); 
        SpawnTile(levelData.cornerTopLeft, -1, topY, "Corner_TL");
        SpawnTile(levelData.cornerTopRight, levelData.width, topY, "Corner_TR");
    }
    
    private void GenerateBottomWall()
    {
        for (int x = 0; x < levelData.width; x++)
        {
            SpawnTile(levelData.borderBottom, x, -1, "Wall_Bottom");
        }
    }

    private void GenerateSideWalls(int wallHeight)
    {
        int height = Mathf.Max(1, wallHeight);
        int maxY = levelData.height + (height - 1);

        for (int y = -1; y < maxY; y++)
        {
            if (y == -1) continue;
            
            SpawnTile(levelData.borderLeft, -1, y, "Wall_Left");
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
        
        int height = Mathf.Max(1, topWallHeight);
    
        float totalWidth = (levelData.width + 2) * levelData.tileSize;
        float totalHeight = (levelData.height + height + 1) * levelData.tileSize; 

        float centerX = (levelData.width - 1) * 0.5f * levelData.tileSize;
        
        float groundCenterY = (levelData.height - 1) * 0.5f;
        float wallOffset = (height - 1) * 0.5f;
        
        float centerY = (groundCenterY + wallOffset) * levelData.tileSize;

        boxCollider.size = new Vector2(totalWidth, totalHeight);
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
