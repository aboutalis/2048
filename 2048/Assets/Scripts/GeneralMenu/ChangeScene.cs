using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string sceneName3x3;
    public string sceneName4x4;
    public string sceneName8x8;
    public string sceneNameTimer;

    public void ChangeToScene3x3()
    {
        SceneManager.LoadScene(sceneName3x3);
    }

    public void ChangeToScene4x4()
    {
        SceneManager.LoadScene(sceneName4x4);
    }

    public void ChangeToScene8x8()
    {
        SceneManager.LoadScene(sceneName8x8);
    }

    public void ChangeToSceneTimer()
    {
        SceneManager.LoadScene(sceneNameTimer);
    }
}
