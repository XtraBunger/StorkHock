using UnityEngine;
using UnityEngine.SceneManagement;

public class AnyKeyToLevel0 : MonoBehaviour
{
    void Update()
    {
        // Check for *any* key press
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("level0");
        }
    }
}
