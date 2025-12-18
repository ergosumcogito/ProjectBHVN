using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int width = 10;
    [SerializeField] private int length = 10;
    [SerializeField] private float tileSize = 1f;
    
    [Header("Tile Prefab")]
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject borderPrefab;


    [Header("Generation Options")]
    [SerializeField] private bool generateOnStart = false;

    public int Width => width;
    public int Length => length;
    
    private void Start()
    {
        if (generateOnStart)
            GenerateLevel();
    }
    
    // Despawning a level
    public void ClearLevel()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
        Debug.Log("All Tiles destroyed");
    }

    public void GenerateLevel()
    {
        ClearLevel();
        
        //Check if TilePrefab or BorderPrefab equals null
        if (!tilePrefab || !borderPrefab)
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
        int borderX = width + 1;
        int borderY = length + 1;

        for (int x = -1; x < borderX; x++)
        {
            for (int y = -1; y < borderY; y++)
            {
                if (x == -1 || x == width || y == -1 || y == length)
                {
                    Vector3 pos = new Vector3(x * tileSize, y * tileSize, 0);
                    GameObject tile = Instantiate(borderPrefab, pos, Quaternion.identity, transform);
                    tile.name = $"Border x={x}, y={y}";
                }
                else
                {
                    Vector3 pos = new Vector3(x * tileSize, y * tileSize, 0);
                    GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                    tile.name = $"Tile x={x}, y={y}";
                }
            }
        }
    }
}
