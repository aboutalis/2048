using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;

    public Sound[] sounds;

    public static AudioManager instance;

    private Scene currentScene;
    private string sceneName;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            DontDestroyOnLoad(gameObject);
            return;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        currentScene = SceneManager.GetActiveScene();

        sceneName = currentScene.name;

        if (sceneName == "MainMenu")
        {
            Play("MainMenuSong");
        }
        else if (sceneName == "3x3" || sceneName == "4x4" || sceneName == "8x8")
        {
            Play("GameSong");
        }
    }

    public void ClickButton()
    {
        Play("Click");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }

    public void MuteButton(bool muted)
    {
        if (muted)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = volumeSlider.value;
        }

        if (sceneName == "3x3" || sceneName == "4x4" || sceneName == "8x8")
            volumeSlider.value = 0;
    }

    public void MaxButton(bool muted)
    {
        if (muted)
        {
            AudioListener.volume = 1;
        }
        else
        {
            AudioListener.volume = volumeSlider.value;
        }

        if (sceneName == "3x3" || sceneName == "4x4" || sceneName == "8x8")
            volumeSlider.value = 1;
    }

    public void Pause()
    {
        AudioListener.pause = true;
    }

    public void Resume()
    {
        AudioListener.pause = false;
    }
}
