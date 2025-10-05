using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] private string nextLevelSceneName;
    private HashSet<GameObject> playersInZone = new HashSet<GameObject>();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playersInZone.Add(other.gameObject);
            CheckAllPlayersInZone();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playersInZone.Remove(other.gameObject);
        }
    }

    void CheckAllPlayersInZone()
    {
        GameObject[] livingPlayers = GameObject.FindGameObjectsWithTag("Player");
        int livingCount = 0;
        foreach (var player in livingPlayers)
        {
            if (player.activeInHierarchy)
                livingCount++;
        }
        if (playersInZone.Count == livingCount && livingCount > 0)
        {
            StartNextLevel();
        }
    }

    void StartNextLevel()
    {
        Debug.Log("All players in zone! Starting next level...");
        if (!string.IsNullOrEmpty(nextLevelSceneName))
        {
            SceneManager.LoadSceneAsync(nextLevelSceneName);
        }
        else
        {
            Debug.LogWarning("Next level scene name not set!");
        }
    }
}
