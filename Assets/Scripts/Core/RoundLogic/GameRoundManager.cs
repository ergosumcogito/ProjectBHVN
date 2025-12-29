using Core;
using UnityEngine;

// --------------------------
// --- Game Round Manager ---
// --------------------------
// Handles gameplay: spawning, etc.
// Clears objets (player, enemies)
//
// Basically it does things on the command of round system
// (RoundSystem - state, GameRoundManager - execute actions)
//
// ? (possibly moved to other class) Assigns weapon to player via Weapon Factory


public class GameRoundManager : MonoBehaviour
{
    [SerializeField] private PlayerSpawn playerSpawner;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private LevelManager levelManager;
    
    // TODO testing weapons
    [SerializeField] private WeaponFactory weaponFactory;

    [SerializeField] private PlayerProgress playerProgress;
    
    private GameObject playerInstance;
    private LevelData _currentLevelData;

    
    private void Awake()
    {
        // TODO for debug and demo: reset progress on game start
        playerProgress.ResetProgress();
    }

    private void OnEnable()
    {
        RoundEvents.OnRoundStart += HandleRoundStart;
        RoundEvents.OnRoundEnd += HandleRoundEnd;
    }

    private void OnDisable()
    {
        RoundEvents.OnRoundStart -= HandleRoundStart;
        RoundEvents.OnRoundEnd -= HandleRoundEnd;
    }

    private void HandleRoundStart(float duration)
    {
        levelManager.LoadCurrentLevel();
        CleanupPlayer();

        _currentLevelData = levelManager.GetLevelData();
        
        playerInstance = playerSpawner.SpawnPlayer();
        
        // Put items in the inventory from previous rounds
        var inventory = playerInstance.GetComponent<PlayerRuntimeInventory>();
        inventory.Init(playerProgress);
        
        var playerHealthLogic = playerInstance.GetComponent<PlayerHealth>();
        playerHealthLogic.OnPlayerDied += HandlePlayerDeath;
        playerHealthLogic.OnPlayerDied += () => RoundEvents.OnPlayerDied?.Invoke();
        
        // -----------------------------
        // TEST: give player a bow
        // -----------------------------
        weaponFactory.weaponSlot = playerInstance.transform.Find("WeaponSlot");
        weaponFactory.CreateWeapon("Bow");
        
        // Future logic: when weapons are part of the inventory
        // foreach (var weaponName in playerProgress.weapons)
        // {
        //     weaponFactory.CreateWeapon(weaponName);
        // }
        
        // -----------------------------
        enemySpawner.ClearEnemies();
        enemySpawner.StartSpawning(_currentLevelData.enemyPrefabs, _currentLevelData.length, _currentLevelData.width);
    }

    private void HandleRoundEnd()
    {
        CleanupRound();
    }
    
    private void HandlePlayerDeath()
    {
        levelManager.ResetToFirstLevel();
        CleanupRound();
        playerProgress.ResetProgress();
    }
    
    private void CleanupRound()
    {
        enemySpawner.StopSpawning();
        enemySpawner.ClearEnemies();
        
    }

    private void CleanupPlayer()
    {
        
        if (playerInstance != null)
        {
            var health = playerInstance.GetComponent<PlayerHealth>();
            health.OnPlayerDied -= HandlePlayerDeath;
            Destroy(playerInstance);
        }
    }

}