using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float currentTime = 0f;
    public float pausedTime = 0f;

    public void CountTimer()
    {
        currentTime += Time.deltaTime;

        int sec = (int)(currentTime % 60);
        int min = (int)(currentTime / 60) % 60;
        int hours = (int)(currentTime / 3600) % 24;

        string timerToString = string.Format("{0:00}:{1:00}:{2:00}", hours, min, sec);
        timerText.text = timerToString;

        GameObject.Find("GM").GetComponent<Timer>().enabled = true;
    }

    // Update is called once per frame
    public void Update()
    {
        CountTimer();
    }

    public void PauseTimer()
    {
        pausedTime = currentTime;
        timerText.text = pausedTime.ToString("00:00:00");
        GameObject.Find("GM").GetComponent<Timer>().enabled = false;
    }
}
