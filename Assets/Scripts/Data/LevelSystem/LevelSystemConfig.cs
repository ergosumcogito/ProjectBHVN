using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class StageConfig
{
    public string stageName = "";
    
    public List<LevelData> levels = new List<LevelData>();
}

[CreateAssetMenu(fileName = "LevelSystem", menuName = "LevelSystem/Master Config")]
public class LevelSystemConfig : ScriptableObject
{
    [Header("All Stages")]
    public List<StageConfig> stages = new List<StageConfig>();
}