using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] private string sceneName; // Set your scene name here
    [SerializeField, Range(2, 4)] private int playerCount = 2; // Set number of players (2-4)

    public void LoadLevelWithPlayers(string sceneName, int playerCount)
    {
        int clampedCount = Mathf.Clamp(playerCount, 2, 4);
        PlayerPrefs.SetInt("PlayerCount", clampedCount);
        SceneManager.LoadScene(sceneName);
    }

    public void LoadLevel()
    {
        LoadLevelWithPlayers(sceneName, playerCount);
    }
}
