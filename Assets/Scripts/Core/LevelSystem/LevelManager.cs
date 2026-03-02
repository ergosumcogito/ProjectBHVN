using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("LevelSystemConfig")]
    [SerializeField] private LevelSystemConfig masterConfig;

    [Header("LevelEditor")]
    [SerializeField] private LevelEditor levelEditor;

    [SerializeField] private PlayerSpawn playerSpawner;

    [Header("Current Stage and Level (0-based)")]
    [SerializeField] private int currentStageIndex = 0;
    [SerializeField] private int currentLevelIndex = 0;

    public int CurrentStageIndex => currentStageIndex;
    public int CurrentLevelIndex => currentLevelIndex;

    [HideInInspector]
    public LevelData nextLevelData;

    private void Start()
    {
        if (masterConfig == null || levelEditor == null)
        {
            Debug.LogError("MasterConfig or LevelEditor missing in LevelManager");
        }
    }

    public LevelData GetLevelData()
    {
        if (!IsValidIndex(currentStageIndex, currentLevelIndex))
        {
            Debug.LogError("Invalid stage/level index in GetLevelData");
            return null;
        }

        return masterConfig.stages[currentStageIndex].levels[currentLevelIndex];
    }

    public void LoadCurrentLevel()
    {
        if (!IsValidIndex(currentStageIndex, currentLevelIndex))
        {
            Debug.LogError("Stage Index or Level Index out of range");
            return;
        }

        LevelData levelData = masterConfig.stages[currentStageIndex].levels[currentLevelIndex];

        if (levelData == null)
        {
            Debug.LogError($"LevelData missing in Stage {currentStageIndex}, Level {currentLevelIndex}");
            return;
        }

        levelEditor.LoadAndStart(levelData);
        playerSpawner.SpawnPlayerToPosition();
    }

    public void MoveToNextLevel()
    {
        if (masterConfig == null || masterConfig.stages.Count == 0)
        {
            Debug.LogError("MasterConfig is empty");
            return;
        }

        int nextStage = currentStageIndex;
        int nextLevel = currentLevelIndex + 1;

        // If next level exceeds level count → go to next stage
        if (nextLevel >= masterConfig.stages[nextStage].levels.Count)
        {
            nextLevel = 0;
            nextStage++;
        }

        // If no more stages → stop
        if (nextStage >= masterConfig.stages.Count)
        {
            Debug.Log("No more stages available");
            return;
        }

        currentStageIndex = nextStage;
        currentLevelIndex = nextLevel;

        nextLevelData = masterConfig.stages[currentStageIndex].levels[currentLevelIndex];
    }

    public void ResetToFirstLevel()
    {
        currentStageIndex = 0;
        currentLevelIndex = 0;
        nextLevelData = null;
    }

    public void InitFromProgress(PlayerProgress progress)
    {
        if (masterConfig == null || masterConfig.stages.Count == 0)
        {
            currentStageIndex = 0;
            currentLevelIndex = 0;
            return;
        }

        int stage = Mathf.Clamp(progress.savedStageIndex, 0, masterConfig.stages.Count - 1);

        int levelCount = masterConfig.stages[stage].levels.Count;
        int level = Mathf.Clamp(progress.savedLevelIndex, 0, levelCount - 1);

        currentStageIndex = stage;
        currentLevelIndex = level;
    }

    private bool IsValidIndex(int stage, int level)
    {
        if (stage < 0 || stage >= masterConfig.stages.Count)
            return false;

        if (level < 0 || level >= masterConfig.stages[stage].levels.Count)
            return false;

        return true;
    }
}