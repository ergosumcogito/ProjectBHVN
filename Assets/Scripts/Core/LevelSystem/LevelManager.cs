using UnityEngine;

public class LevelManager : MonoBehaviour
{
   	[Header("LevelSystemConfig")]
   	public LevelSystemConfig masterConfig;
    
    [Header("LevelEditor")]
    public LevelEditor levelEditor;

    [Header("Current Stage and Level")]
    public int currentStageIndex = 1;
    public int currentLevelIndex = 1;

    [HideInInspector]
    public LevelData nextLevelData;
    
    void Start()
    {
        if (masterConfig == null || levelEditor == null)
        {
            Debug.LogError("MasterConfig or LevelEditor missing in LevelManager");
            return;
        }
        LoadCurrentLevel();
    }

    public void LoadCurrentLevel()
    {
        StageConfig currentStage = masterConfig.stages[currentStageIndex - 1];
        LevelData levelData = currentStage.levels[currentLevelIndex - 1];
        
        if (levelData != null)
        {
            levelEditor.LoadAndStart(levelData);
            LoadNextLevel(); // TODO in the current state of the game we don't need pre-loading (i will call this method manually if we need). Please just remove this line. But in general i like the idea.
        }
        else
        {
            Debug.LogError($"LevelData missing in Stage {currentStageIndex - 1}, Level {currentLevelIndex - 1}");
        }
    }
    
    public void LoadNextLevel()
    {
        int nextLevel = currentLevelIndex;
        int nextStage = currentStageIndex - 1; // TODO Please check what happens if stage = 0. Do we get stages[-1] - the exception? 
        
        if (nextLevel >= masterConfig.stages[nextStage].levels.Count)
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
           Debug.Log("Next Level loaded");
        }
    }

    public void IncrementLevel()
    {
        if (masterConfig == null || masterConfig.stages.Count == 0)
            return;

        currentLevelIndex++;

        StageConfig currentStage = masterConfig.stages[currentStageIndex - 1];

        if (currentLevelIndex > currentStage.levels.Count)
        {
            currentLevelIndex = 1;
            currentStageIndex++;

            if (currentStageIndex > masterConfig.stages.Count)
            {
                currentStageIndex = 1;
            }
        }

        StageConfig nextStage = masterConfig.stages[currentStageIndex - 1];
        nextLevelData = nextStage.levels[currentLevelIndex - 1];

       // Debug.Log($"Incremented to Stage {currentStageIndex}, Level {currentLevelIndex}");
    }

    public void GoToNextLevel()
    {
        if (nextLevelData !=null)
        {
            levelEditor.LoadAndStart(nextLevelData);
            nextLevelData = null;
        }
        else
        {
            Debug.Log("No more stages and levels available");
        }
    }
}