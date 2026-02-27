using Core;
using Core.Enemy_Logic;
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
    // Game systems
    [SerializeField] private PlayerSpawn playerSpawner;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private LevelManager levelManager;

    // TODO testing weapons
    [SerializeField] private WeaponFactory weaponFactory;

    [SerializeField] private PlayerProgress playerProgress;
    [SerializeField] private ItemDatabase itemDatabase;

    // UI
    [SerializeField] private CoinsHUD coinsHUD;
    [SerializeField] private ItemsHUD itemsHUD;

    private GameObject playerInstance;
    private LevelData _currentLevelData;
    
    private bool _isGameOver = false; // if player died and we should start from the beginning

    private bool _isPlayingBackgroundMusic = false;

    public Vector2 GetCurrentLevelBounds()
    {
        return new Vector2(_currentLevelData.width, _currentLevelData.height);
    }

    private void Awake()
    {
        // --- TODO These block disables saves for developement/testing (remove in production)
        PlayerPrefs.DeleteAll();
        playerProgress.ResetProgress();
        // --- remove above in production
        
        PlayerProgressSaver.Load(playerProgress, itemDatabase);
        levelManager.InitFromProgress(playerProgress);
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
        playerProgress.SetSavedStageAndLevel(levelManager.CurrentStageIndex, levelManager.CurrentLevelIndex);
        PlayerProgressSaver.Save(playerProgress);
        
        CleanupPlayer();

        _currentLevelData = levelManager.GetLevelData();

        if (_currentLevelData.backgroundMusic)
        {
            StartMusic();
        }

        playerInstance = playerSpawner.SpawnPlayer();

        // Put items in the inventory from previous rounds
        var runtimeInventory = playerInstance.GetComponent<PlayerRuntimeInventory>();
        runtimeInventory.Init(playerProgress);

        // Init coins
        var runtimeCurrency = playerInstance.GetComponent<PlayerRuntimeCurrency>();
        runtimeCurrency.Init(playerProgress);

        // Init HUD
        coinsHUD.Init(runtimeCurrency);
        itemsHUD.Init(runtimeInventory);


        var playerHealthLogic = playerInstance.GetComponent<PlayerHealth>();
        playerHealthLogic.OnPlayerDied += HandlePlayerDeath;
        playerHealthLogic.OnPlayerDied += () => RoundEvents.OnPlayerDied?.Invoke();

        // -----------------------------
        // TEST: give player a bow
        // -----------------------------
        weaponFactory.weaponSlot = playerInstance.transform.Find("WeaponSlot");
        weaponFactory.CreateWeapon("AuraWeapon");

        // Future logic: when weapons are part of the inventory
        // foreach (var weaponName in playerProgress.weapons)
        // {
        //     weaponFactory.CreateWeapon(weaponName);
        // }

        // -----------------------------
        enemySpawner.ClearEnemies();

        var prefabs = _currentLevelData.enemyPrefabs;
        var maxEnemies = _currentLevelData.maxEnemies;
        var spawnInterval = _currentLevelData.spawnInterval;
        var width = _currentLevelData.width;
        var height = _currentLevelData.height;
        var type = _currentLevelData.levelType;

        enemySpawner.StartSpawning(prefabs, maxEnemies, spawnInterval, width, height, type);

        // levelManager.MoveToNextLevel(); // after setting enemies, increase level counter
    }

    private void HandleRoundEnd()
    {
        if (!_isGameOver)
        {
            levelManager.MoveToNextLevel();
        }
        
        _isGameOver = true;
        CleanupRound();
    }

    private void HandlePlayerDeath()
    {
        _isGameOver = true;
        
        CleanupPlayer(); // remove player on game over screen
        levelManager.ResetToFirstLevel();
        CleanupRound();
        playerProgress.ResetProgress();
        
        PlayerProgressSaver.Save(playerProgress);
    }

    private void StartMusic()
    {
        if (!MusicManager.Instance) return;

        MusicManager.Instance.PlayLevelMusic(
            _currentLevelData.backgroundMusic,
            _currentLevelData.musicVolume,
            _currentLevelData.loopMusic,
            _currentLevelData.fadeIn,
            _currentLevelData.fadeOut
        );

        _isPlayingBackgroundMusic = true;
    }

    private void StopMusic(float fadeVal)
    {
        if (MusicManager.Instance)
        {
            MusicManager.Instance.StopMusic(_currentLevelData != null ? _currentLevelData.fadeOut : fadeVal);
        }
    }

    private void CleanupRound()
    {
        if (_isPlayingBackgroundMusic)
        {
            StopMusic(0.25f);
            _isPlayingBackgroundMusic = false;
        }

        enemySpawner.StopSpawning();
        enemySpawner.ClearEnemies();
        CleanupCoins();
    }

    private void CleanupCoins()
    {
        var coins = FindObjectsByType<Coin>(FindObjectsSortMode.None);

        foreach (var coin in coins)
        {
            Destroy(coin.gameObject);
        }
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