using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int maxEnemies = 15;
    [SerializeField] private float spawnInterval = 0.5f;
    public event System.Action<int> OnEnemyCountChanged;

    [SerializeField] private int minSpawnDistance = 3;
    [SerializeField] private int maxSpawnDistance = 7;

    private List<WeightedEnemy> _enemyPrefabs;
    private int _currentWidth;
    private int _currentHeight;

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
    private float LevelWidth => _currentWidth;
    private float LevelHeight => _currentHeight;

    private float _spawnTimer;
    private bool _isSpawning;

    private readonly List<GameObject> _activeEnemies = new();


    public int CurrentEnemyCount => _activeEnemies.Count(e => e != null);

    private void Update()
    {
        if (!_isSpawning) return;

        if (!_player)
        {
            GetInstances();
            return;
        }

        _activeEnemies.RemoveAll(e => !e);
        OnEnemyCountChanged?.Invoke(CurrentEnemyCount);

        _spawnTimer += Time.deltaTime;

        if (!(_spawnTimer >= spawnInterval) || _activeEnemies.Count >= maxEnemies) return;
        SpawnEnemy();
        _spawnTimer = 0f;
    }

    private void GetInstances()
    {
        _player = GameObject.FindWithTag("Player")?.transform;
    }

    private void SpawnEnemy()
    {
        if (_enemyPrefabs == null || _enemyPrefabs.Count == 0) return;

        //var prefab = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Count)];
        var prefab = PickWeightedEnemy(_enemyPrefabs);
        if (!prefab) return;

        var spawnPos = GetSpawnPoint(_player.position);

        var enemy = Instantiate(prefab, spawnPos, Quaternion.identity);
        _activeEnemies.Add(enemy);

        OnEnemyCountChanged?.Invoke(CurrentEnemyCount);
    }

    private static GameObject PickWeightedEnemy(List<WeightedEnemy> list)
    {
        var total = list.Where(e => e.prefab && e.weight > 0f).Sum(e => e.weight);

        if (total <= 0f) return null;

        var r = Random.value * total;

        foreach (var e in list.Where(e => e.prefab && !(e.weight <= 0f)))
        {
            r -= e.weight;
            if (r <= 0f)
                return e.prefab;
        }

        return list.FirstOrDefault(e => e.prefab && e.weight > 0f)?.prefab;
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
    public void StartSpawning(List<WeightedEnemy> enemies, int width, int height)
    {
        _enemyPrefabs = enemies;
        _currentWidth = width;
        _currentHeight = height;

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
        foreach (var e in _activeEnemies.Where(e => e))
        {
            Destroy(e);
        }

        _activeEnemies.Clear();

        OnEnemyCountChanged?.Invoke(CurrentEnemyCount);
    }
}