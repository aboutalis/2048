using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerName : MonoBehaviour
{
    public static PlayerName playerName;

    public TMP_InputField input;

    public Button subButton;

    public string name;

    bool isMainMenuSceneActive;

    private void Awake()
    {
        isMainMenuSceneActive = SceneManager.GetActiveScene().name == "MainMenu";

        //input = GameObject.Find("InputField (TMP)").GetComponent<TMP_InputField>();
        //subButton = GameObject.Find("SubmitButton").GetComponent<Button>();
        playerName = this;
        //if (playerName == null)
        //{
        //    playerName = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
    }

    public void Update()
    {
        if (isMainMenuSceneActive)
        {
            if (input.text.Length > 0)
            {
                subButton.interactable = true;
            }
            else
            {
                subButton.interactable = false;
            }
        }
    }

    public void SetPlayerName()
    {
        name = input.text;
    }

}
