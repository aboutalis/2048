using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class LeaderboardTable : MonoBehaviour
{
    private Transform entry;
    private Transform hsTemplate;
    private List<HighscoreEntry> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;
    private Highscores highscores;

    private Transform entryTransform;

    private string jsonString;
    public GameObject originalPrefab;


    private void Awake()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        // Set up high score entry list
        entry = transform.Find("Entry");
        hsTemplate = entry.Find("HighscoreTemplate");
        hsTemplate.gameObject.SetActive(false);
        highscoreEntryList = new List<HighscoreEntry>();

        // Retrieve high score data from PlayerPrefs and create a new Highscores object if necessary
        jsonString = PlayerPrefs.GetString($"list{sceneName}");
        highscores = string.IsNullOrEmpty(jsonString) ? new Highscores { highscoreEntryList = highscoreEntryList } : JsonUtility.FromJson<Highscores>(jsonString);

        // Print high scores to console
        PrintHighscore();
    }

    public void AddHighScoreEntry(int score, string name)
    {
        string sceneName = SceneManager.GetActiveScene().name;

        // Create new HighscoreEntry
        HighscoreEntry newEntry = new HighscoreEntry { score = score, userName = name };

        // Add new entry to high score list and update PlayerPrefs
        highscores.highscoreEntryList.Add(newEntry);
        string json = JsonUtility.ToJson(highscores);

        PlayerPrefs.SetString($"list{sceneName}", json);
        SortHighscoretable();
        PlayerPrefs.Save();
    }

    private void HighScoreGenerator(HighscoreEntry highscoreEntry, Transform entries, List<Transform> transformList)
    {
        float tmpHeight = 35f;

        entryTransform = Instantiate(hsTemplate, entries);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -tmpHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        //Create rank string
        string rankString = GetRankString(transformList.Count + 1);
        //int rank = int.Parse(rankString[0].ToString());
        int rank = int.Parse(new string(rankString.Where(c => char.IsDigit(c)).ToArray()));

        entryTransform.Find("RankEntryText").GetComponent<TextMeshProUGUI>().text = rankString;

        entryTransform.Find("UserEntryText").GetComponent<TextMeshProUGUI>().text = highscoreEntry.userName;

        entryTransform.Find("ScoreEntryText").GetComponent<TextMeshProUGUI>().text = highscoreEntry.score.ToString();

        entryTransform.Find("Background").gameObject.SetActive(rank % 2 == 1);

        //Highlight the first
        if (rank == 1)
        {
            entryTransform.Find("RankEntryText").GetComponent<TextMeshProUGUI>().color = Color.green;
            entryTransform.Find("UserEntryText").GetComponent<TextMeshProUGUI>().color = Color.green;
            entryTransform.Find("ScoreEntryText").GetComponent<TextMeshProUGUI>().color = Color.green;
        }

        switch (rank)
        {
            case 1:
                entryTransform.Find("Icon").GetComponent<Image>().color = new Color(1f, 0.843f, 0f);
                break;
            case 2:
                entryTransform.Find("Icon").GetComponent<Image>().color = new Color(0.804f, 0.498f, 0.196f);

                break;
            case 3:
                entryTransform.Find("Icon").GetComponent<Image>().color = new Color(0.753f, 0.753f, 0.753f);
                break;
            default:
                entryTransform.Find("Icon").gameObject.SetActive(false);
                break;
        }

        transformList.Add(entryTransform);
    }

    public static string GetRankString(int position)
    {
        string rankString = position.ToString();

        switch (position % 10)
        {
            case 1:
                rankString += "st";
                break;
            case 2:
                rankString += "nd";
                break;
            case 3:
                rankString += "rd";
                break;
            default:
                rankString += "th";
                break;
        }

        return rankString;
    }

    public void SortHighscoretable()
    {
        highscores.highscoreEntryList = highscores.highscoreEntryList.OrderByDescending(x => x.score).ToList();
    }

    public void DeleteClones()
    {
        // Find all instances of MyPrefab in the scene
        GameObject[] clones = FindObjectsOfType<GameObject>()
            .Where(go => go.name.StartsWith("HighscoreTemplate") && go != originalPrefab)
            .ToArray();

        // Delete all clones
        foreach (GameObject clone in clones)
        {
            Destroy(clone);
        }
    }

    public void PrintHighscore()
    {
        //Sort Highscore Entry List by score
        SortHighscoretable();

        highscoreEntryTransformList = new List<Transform>();

        int counter = 0;
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
        {
            //SortHighscoretable();

            if (counter < 10)
            {
                HighScoreGenerator(highscoreEntry, entry, highscoreEntryTransformList);
                counter++;
            }
        }
    }

    //Represent a single Highscore entry
    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string userName;
    }

    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

}
