using UnityEngine;
using UnityEngine.UI;

public class duckSlider : MonoBehaviour
{
    public Slider volumeSlider;
    public AudioSource backgroundMusic; // 👈 Add this line

    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            load();
        }
        else
        {
            load();
        }

        // Apply saved volume to background music
        backgroundMusic.volume = volumeSlider.value;
    }

    public void changeVolume(float volume)
    {
        backgroundMusic.volume = volume; // 👈 Update music volume
        save();
    }

    private void load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}
