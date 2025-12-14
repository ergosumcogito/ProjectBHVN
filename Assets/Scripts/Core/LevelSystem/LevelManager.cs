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
        StageConfig currentStage = masterConfig.stages[currentStageIndex-1];
        LevelData levelData = currentStage.levels[currentLevelIndex-1];
        
        if (levelData != null)
        {
            levelEditor.LoadAndStart(levelData);
        }
        else
        {
            Debug.LogError($"LevelData missing in Stage {currentStageIndex-1}, Level {currentLevelIndex-1}");
        }
    }
    
    public void LoadNextLevel()
    {
        
    }     
}