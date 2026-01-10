using System;
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
        movement.setInputReader(inputReader);

        if (cinemachineCamera != null)
        {
            cinemachineCamera.LookAt= playerInstance.transform;
            cinemachineCamera.Follow = playerInstance.transform;
        } else
            Debug.LogError("No Cinemachine found");
     
        return playerInstance;
    }
    
    public void TeleportPlayerToCenter()
    {
        LevelData levelData = levelManager.GetLevelData();
        float posX = levelData.width / 2f;
        float posY = levelData.length / 2f;
        
        Vector3 centerPosition = new Vector3(posX, posY, 0f);
        
        spawnPoint.position = centerPosition;
    }
}
