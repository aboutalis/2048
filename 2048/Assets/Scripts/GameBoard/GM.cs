using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GM : MonoBehaviour
{
    public TileBoard board;

    public CanvasGroup go;

    public GameObject timerUI;

    public LeaderboardTable leaderboardTable;

    public TextMeshProUGUI text;
    public TextMeshProUGUI highscore;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI nextGoalText;
    public TextMeshProUGUI animatedScore;

    public string playerName;
    
    private int score;

    private AudioManager audio;

    private void Awake()
    {
        audio = FindObjectOfType<AudioManager>();

        string sceneName = SceneManager.GetActiveScene().name;
        int sceneSize = int.Parse(sceneName.Split('x')[0]);

        highscore.text = PlayerPrefs.GetInt($"highscore{sceneName}", 0).ToString();

        NewGame();
    }

    public void Start()
    {
        //playerName = "pou";
        playerName = PlayerName.playerName.name;
    }

    public void NewGame()
    {
        timerUI.GetComponent<TileBoard>().enabled = true;
        board.highValue = 0;
        Score(0);
        
        highscore.text = LoadHighScore().ToString();
                       
        nextGoalText.text = "-";
        
        go.alpha = 0f;
        go.interactable = false;

        board.ClearingBoard();
        board.TileCreation();
        board.TileCreation();
        board.enabled = true;
    }

    public void GameOver()
    {
        audio.Play("GameOver");
        board.enabled = false;
        go.interactable = true;
        SaveHighScore();
        
        StartCoroutine(Fade(go, 1f, 1f));

    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay)
    {
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

    public void AddScore(int points)
    {
        Score(score + points);
        
        animatedScore.text = "+" + points.ToString();
        animatedScore.GetComponent<Animator>().Play("AnimatedScore", -1, 0f);
    }

    private void Score(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
        
    }

    public void SetNextGoal()
    {
        nextGoalText.text = board.nextGoalValue.ToString();
    }

    public void SaveHighScore()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        int high = PlayerPrefs.GetInt($"highscore{sceneName}", 0);
        leaderboardTable.AddHighScoreEntry(score, playerName);

        if (score > high)
        {
            PlayerPrefs.SetInt($"highscore{sceneName}", score);
            highscore.text = score.ToString();
        }
    }

    private int LoadHighScore()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        return PlayerPrefs.GetInt($"highscore{sceneName}", 0);
    }

}
