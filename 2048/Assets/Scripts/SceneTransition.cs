using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public float delay; // Delay in seconds before loading the next scene
    public string nextSceneName; // Name of the next scene to load

    void Start()
    {
        // Call the LoadNextScene() function after the specified delay
        Invoke("LoadNextScene", delay);
    }

    void LoadNextScene()
    {
        // Load the next scene in the build settings by name
        SceneManager.LoadScene(nextSceneName);
    }
}

