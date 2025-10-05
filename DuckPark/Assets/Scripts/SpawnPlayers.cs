using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    private int playerCount;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] playerPrefabs; // Assign 4 prefabs in Inspector
    [SerializeField] private CameraFollowMultipleTargets cameraFollow;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCount = PlayerPrefs.GetInt("PlayerCount", 1); // 1 is the default if not set
        Debug.Log("Spawning " + playerCount + " players.");
        for (int i = 0; i < playerCount; i++)
        {
            if (i < playerPrefabs.Length && i < spawnPoints.Length)
            {
                GameObject player = Instantiate(playerPrefabs[i], spawnPoints[i].position, spawnPoints[i].rotation);
                if (cameraFollow != null)
                {
                    cameraFollow.targets.Add(player.transform);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
