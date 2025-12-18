using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyPrefabs;

    [SerializeField] private int maxEnemies = 15;
    [SerializeField] private float spawnInterval = 0.5f;
    public event System.Action<int> OnEnemyCountChanged;

    [SerializeField] private int minSpawnDistance = 3;
    [SerializeField] private int maxSpawnDistance = 7;

    public int MaxEnemies
    {
        get => maxEnemies;
        set => maxEnemies = Mathf.Max(1, value);
    }

    public float SpawnInterval
    {
        get => spawnInterval;
        set => spawnInterval = Mathf.Max(0.001f, value);
    }

    private Transform _player;
    private LevelEditor _levelEditor;
    private float LevelWidth => _levelEditor.Width - 1;
    private float LevelHeight => _levelEditor.Length - 1;

    private float _spawnTimer;
    private bool _isSpawning;

    private readonly List<GameObject> _activeEnemies = new();


    public int CurrentEnemyCount => _activeEnemies.Count(e => e != null);

    private void Update()
    {
        if (!_isSpawning) return;

        if (!_player || !_levelEditor)
        {
            GetInstances();
            return;
        }

        _activeEnemies.RemoveAll(e => e == null);
        OnEnemyCountChanged?.Invoke(CurrentEnemyCount);

        _spawnTimer += Time.deltaTime;

        if (!(_spawnTimer >= spawnInterval) || _activeEnemies.Count >= maxEnemies) return;
        SpawnEnemy();
        _spawnTimer = 0f;
    }

    private void GetInstances()
    {
        _player = GameObject.FindWithTag("Player")?.transform;
        _levelEditor = FindFirstObjectByType<LevelEditor>();
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Count == 0) return;

        var spawnPos = GetSpawnPoint(_player.position);

        var prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

        var enemy = Instantiate(prefab, spawnPos, Quaternion.identity);
        _activeEnemies.Add(enemy);

        OnEnemyCountChanged?.Invoke(CurrentEnemyCount);
    }

    //creates a square around player that prevents enemies from spawning within, returns enemy spawn point
    private Vector2 GetSpawnPoint(Vector2 playerPos)
    {
        for (var i = 0; i < 100; i++)
        {
            var randomEnemySpawnPoint = GetRandomCoordinates();

            if (!IsInInvalidDistance(playerPos, randomEnemySpawnPoint)) return randomEnemySpawnPoint;
        }

        //fallback if no valid spawn is found
        return new Vector2(0, 0);
    }

    private bool IsInInvalidDistance(Vector2 playerPos, Vector2 enemySpawnPos)
    {
        var distance = Vector2.Distance(playerPos, enemySpawnPos);

        return distance < minSpawnDistance || distance > maxSpawnDistance;
    }

    //for enemy spawn points
    private Vector2 GetRandomCoordinates()
    {
        var x = Random.Range(0f, LevelWidth);
        var y = Random.Range(0f, LevelHeight);

        return new Vector2(x, y);
    }

    //these three are to be used by other systems to control spawning
    //starts enemy spawning
    public void StartSpawning()
    {
        _isSpawning = true;
        _spawnTimer = 0f;
    }

    //stops enemy spawning
    public void StopSpawning()
    {
        _isSpawning = false;
    }

    //clears all enemies, once time is up for example
    public void ClearEnemies()
    {
        foreach (var e in _activeEnemies.Where(e => e != null))
        {
            Destroy(e);
        }

        _activeEnemies.Clear();

        OnEnemyCountChanged?.Invoke(CurrentEnemyCount);
    }
}