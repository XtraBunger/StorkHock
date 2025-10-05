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
        // Invert the slider value: 1 is mute, 0 is full volume
        float mappedVolume = 1f - volume;
        backgroundMusic.volume = mappedVolume;
        save();
    }

    public void ChangeVolumeFromSlider()
    {
        changeVolume(volumeSlider.value);
    }

    private void load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        backgroundMusic.volume = 1f - volumeSlider.value;
    }

    private void save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}
