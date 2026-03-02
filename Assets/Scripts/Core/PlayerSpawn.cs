using System;
using Core.PlayerLogic.Abilities;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private LevelManager levelManager;
    

    
    public GameObject SpawnPlayer()
    {
        GameObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        PlayerMovement movement = playerInstance.GetComponent<PlayerMovement>();
        
        var abilitySlots = playerInstance.GetComponentsInChildren<PlayerAbilitySlot>();
        foreach (var slot in abilitySlots)
            slot.SetInputReader(inputReader);
        
        movement.setInputReader(inputReader);

        if (cinemachineCamera != null)
        {
            cinemachineCamera.LookAt= playerInstance.transform;
            cinemachineCamera.Follow = playerInstance.transform;
        } else
            Debug.LogError("No Cinemachine found");
     
        return playerInstance;
    }

    public void SpawnPlayerToPosition()
    {
        LevelData levelData = levelManager.GetLevelData();
        
        float posX = levelData.width / 2f;
        float posY = levelData.height / 2f;
        
        
        switch (levelData.playerSpawnPosition)
        {
            case PlayerSpawnPosition.TopLeft:
                posX = 1;
                posY = levelData.height - 2;
                break;
            case PlayerSpawnPosition.TopRight:
                posX = levelData.width - 2;
                posY = levelData.height - 2;
                break;
            case PlayerSpawnPosition.BottomLeft:
                posX = 1;
                posY = 1;
                break;
            case PlayerSpawnPosition.BottomRight:
                posX = levelData.width - 2;
                posY = 1;
                break;
            case PlayerSpawnPosition.Left:
                posX = 1;
                break;
            case PlayerSpawnPosition.Right:
                posX = levelData.width - 2;
                break;
            case PlayerSpawnPosition.Top:
                posY = levelData.height - 2;
                break;
            case PlayerSpawnPosition.Bottom:
                posY = 1;
                break;
            case PlayerSpawnPosition.Center:
                break;
        }
        spawnPoint.position = new Vector3(posX, posY, 0);
    }
    
    
}
