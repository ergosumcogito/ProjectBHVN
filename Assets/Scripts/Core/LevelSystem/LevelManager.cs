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
            LoadNextLevel();
        }
        else
        {
            Debug.LogError($"LevelData missing in Stage {currentStageIndex - 1}, Level {currentLevelIndex - 1}");
        }
    }
    
    public void LoadNextLevel()
    {
        int nextLevel = currentLevelIndex;
        int nextStage = currentStageIndex - 1;
        
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