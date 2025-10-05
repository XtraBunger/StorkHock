using UnityEngine;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private GameObject button1;
    [SerializeField] private GameObject button2;
    [SerializeField] private GameObject button3;

    public void OnPlayClicked()
    {
        if (button1 != null) button1.SetActive(true);
        if (button2 != null) button2.SetActive(true);
        if (button3 != null) button3.SetActive(true);
        gameObject.SetActive(false); // Hide this PlayButton
    }

    
}
