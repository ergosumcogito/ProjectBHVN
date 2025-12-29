using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
   	[Header("LevelSystemConfig")]
    [SerializeField] LevelSystemConfig masterConfig;
    
    [Header("LevelEditor")]
    [SerializeField] public LevelEditor levelEditor;

    [Header("Current Stage and Level")]
    [SerializeField] private int currentStageDisplay = 1;
    [SerializeField] private int currentLevelDisplay = 1;

    private int CurrentStageIndex => currentStageDisplay - 1;
    private int CurrentLevelIndex => currentLevelDisplay - 1;
    
    [HideInInspector]
    public LevelData nextLevelData;
    
    void Start()
    {
        if (masterConfig == null || levelEditor == null)
        {
            Debug.LogError("MasterConfig or LevelEditor missing in LevelManager");
            return;
        }
    }

    public LevelData GetLevelData()
    {
        return masterConfig.stages[CurrentStageIndex].levels[CurrentLevelIndex];
    }
    public void LoadCurrentLevel()
    {
        try
        {
            StageConfig currentStage = masterConfig.stages[CurrentStageIndex];
            LevelData levelData = currentStage.levels[CurrentLevelIndex];
            
            if (levelData != null)
            {
                levelEditor.LoadAndStart(levelData);
            }
            else
            {
                Debug.LogError($"LevelData missing in Stage {CurrentStageIndex}, Level {CurrentLevelIndex}");
            }
        }
        catch (ArgumentOutOfRangeException)
        {
            Debug.LogError("Stage Index or Level Index out of range");
        }
    }
    
    public void MoveToNextLevel()
    {
        int nextLevel = CurrentLevelIndex + 1;
        int nextStage = CurrentStageIndex; 
        
        if(nextStage < 0 || nextLevel < 0)
        {
            Debug.LogError("Stage Index or Level Index out of range");
            return;
        }
        if(nextLevel >= masterConfig.stages[nextStage].levels.Count)
        {
            nextLevel = 0;
            nextStage++;
        }
        
        if (nextStage >= masterConfig.stages.Count)
        {
            Debug.Log("No more stages available");
        }
        else
        {
           nextLevelData = masterConfig.stages[nextStage].levels[nextLevel];

           currentLevelDisplay = nextLevel + 1;
           currentStageDisplay = nextStage + 1;
           
           // Debug.Log("Next Level loaded");
        }
    }
    
    public void ResetToFirstLevel()
    {
        currentStageDisplay = 1;
        currentLevelDisplay = 1;
        nextLevelData = null;
    }
}