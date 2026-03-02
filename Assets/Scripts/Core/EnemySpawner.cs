using System.Collections.Generic;
using System.Linq;
using Core.Enemy_Logic;
using UnityEngine;

namespace Core
{
    public class EnemySpawner : MonoBehaviour
    {
        public event System.Action<int> OnEnemyCountChanged;

        [SerializeField] private int minSpawnDistance = 3;
        [SerializeField] private int maxSpawnDistance = 7;

        private List<WeightedEnemy> _enemyPrefabs;
        private int _currentWidth;
        private int _currentHeight;

        private Transform _player;
        private LevelEditor _levelEditor;

        private float _spawnTimer;
        private bool _isSpawning;

        public int MaxEnemies { get; private set; }
        public float SpawnInterval { get; private set; }

        private readonly List<GameObject> _activeEnemies = new();

        public int CurrentEnemyCount => _activeEnemies.Count(e => e != null);

        private void Update()
        {
            CleanupDeadEnemies();

            if (!_isSpawning) return;

            if (!_player)
            {
                GetInstances();
                return;
            }

            _spawnTimer += Time.deltaTime;

            if (!(_spawnTimer >= SpawnInterval) || _activeEnemies.Count >= MaxEnemies) return;
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

            var prefab = PickWeightedEnemy(_enemyPrefabs);
            if (!prefab) return;

            var spawnPos = GetSpawnPoint(_player.position);

            var enemy = Instantiate(prefab, spawnPos, Quaternion.identity);

            RegisterEnemy(enemy);
        }

        private void CleanupDeadEnemies()
        {
            if (_activeEnemies.RemoveAll(e => !e) > 0)
            {
                OnEnemyCountChanged?.Invoke(CurrentEnemyCount);
            }
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

        public void ForceSpawnEnemy(EnemyAbstract enemy, GameObject prefab, int amount, float spawnRadius)
        {
            for (var i = 0; i < amount; i++)
            {
                var offset = Random.insideUnitCircle * spawnRadius;
                var spawnPos = enemy.transform.position + (Vector3)offset;

                var confirmedSpawnPos = CheckEnemySpawn(spawnPos);

                var newEnemy = Instantiate(prefab, confirmedSpawnPos, Quaternion.identity);

                RegisterEnemy(newEnemy);
            }
        }

        private Vector3 CheckEnemySpawn(Vector3 spawnPos)
        {
            var confirmedPos = spawnPos;

            if (spawnPos.x <= 0) confirmedPos.x = 0.5f;
            if (spawnPos.y <= 0) confirmedPos.y = 0.5f;
            if (spawnPos.x >= _currentWidth) confirmedPos.x = _currentWidth - 0.5f;
            if (spawnPos.y >= _currentHeight) confirmedPos.y = _currentHeight - 0.5f;

            return confirmedPos;
        }

        //for enemy spawn points
        private Vector2 GetRandomCoordinates()
        {
            var x = Random.Range(0f, _currentWidth);
            var y = Random.Range(0f, _currentHeight);

            return new Vector2(x, y);
        }

        private void SpawnBoss(WeightedEnemy boss, int width, int height)
        {
            var x = width / 2f;
            var y = height - 3f;
            var spawnCoords = new Vector2(x, y);

            var bossObject = Instantiate(boss.prefab, spawnCoords, Quaternion.identity);

            RegisterEnemy(bossObject);
        }

        private void RegisterEnemy(GameObject enemy)
        {
            _activeEnemies.Add(enemy);
            OnEnemyCountChanged?.Invoke(CurrentEnemyCount);
        }

        //these three are to be used by other systems to control spawning
        //starts enemy spawning
        public void StartSpawning(List<WeightedEnemy> enemies, int amount, float interval, int width, int height,
            LevelType type)
        {
            _enemyPrefabs = enemies;
            MaxEnemies = amount;
            SpawnInterval = interval;
            _currentWidth = width;
            _currentHeight = height;

            if (type == LevelType.Boss)
            {
                SpawnBoss(_enemyPrefabs.First(), _currentWidth, _currentHeight);
                return;
            }

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
}