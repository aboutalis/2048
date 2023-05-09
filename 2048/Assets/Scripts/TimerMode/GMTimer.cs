using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GMTimer : MonoBehaviour
{
    public TileBoard board;

    public CanvasGroup go;
    public CanvasGroup goWin;
    public CanvasGroup justWin;

    public GameObject timerUI;

    public Button pausedButton;

    private int counter = 3;

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI highTime;
    public TextMeshProUGUI myTime;
    public TextMeshProUGUI nextGoalText;
    public TextMeshProUGUI displayTimerText;

    private AudioManager audio;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("ht"))
        {
            highTime.text = PlayerPrefs.GetFloat("ht").ToString("00:00:00");
        }
        else
        {
            PlayerPrefs.SetFloat("ht", float.MaxValue);
            highTime.text = "-";
        }

        audio = FindObjectOfType<AudioManager>();
        NewGame();       
    }

    private void Start()
    {
        timerUI.GetComponent<TileBoard>().enabled = false;
    }

    

    public void NewGame()
    {
        timeText.gameObject.SetActive(true);
        StartCoroutine(CountdownToStart());
        
        nextGoalText.text = "-";
        GameObject.Find("GM").GetComponent<Timer>().currentTime = 0;
        timerUI.GetComponent<TileBoard>().highValue = 0;

        go.alpha = 0f;
        go.interactable = false;
        go.blocksRaycasts = false;

        goWin.alpha = 0f;
        goWin.interactable = false;
        goWin.blocksRaycasts = false;

        justWin.alpha = 0f;
        justWin.interactable = false;
        justWin.blocksRaycasts = false;

    }

    public void GameOver()
    {
        board.enabled = false;
        StartCoroutine(Fade(go, 1f, 1f));
        
        go.interactable = true;
        go.blocksRaycasts = true;

        nextGoalText.text = "-";
    }

    public void YouWin()
    {
        StartCoroutine(Fade(justWin, 1f, 1f));

        justWin.interactable = true;
        justWin.blocksRaycasts = true;
        board.enabled = false;

        nextGoalText.text = "-";
    }

    public void BestWin(float time)
    {
        PlayerPrefs.SetFloat("ht", time);
        highTime.text = time.ToString("00:00:00");

        StartCoroutine(Fade(goWin, 1f, 1f));

        displayTimerText.text = time.ToString("00:00:00");

        goWin.interactable = true;
        goWin.blocksRaycasts = true;
        board.enabled = false;

        nextGoalText.text = "-";
    }

    public IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay)
    {
        pausedButton.gameObject.SetActive(false);

        yield return new WaitForSeconds(delay);

        float elapsed = 0f;
        float duration = 0.5f;
        float from = canvasGroup.alpha;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = to;
    }

    IEnumerator CountdownToStart()
    {
        while (counter > 0)
        {
            audio.Play("Beep");
            timeText.text = counter.ToString();
            timerUI.GetComponent<TileBoard>().enabled = false;
            yield return new WaitForSeconds(1f);

            counter--;
        }

        audio.Play("LongBeep");

        timeText.text = "GO!";

        yield return new WaitForSeconds(1f);

        timerUI.GetComponent<TileBoard>().enabled = true;

        timeText.gameObject.SetActive(false);
        counter = 3;

        GameObject.Find("GM").GetComponent<Timer>().enabled = true;
        board.ClearingBoard();
        board.TileCreation();
        board.TileCreation();
        board.enabled = true;

        pausedButton.gameObject.SetActive(true);
    }

    public void SetNextGoal()
    {
        nextGoalText.text = board.nextGoalValue.ToString();
    }    
}
