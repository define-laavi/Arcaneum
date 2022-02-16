using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;

[DisallowMultipleComponent]
public class Score : MonoBehaviour
{
    private int _currentScore;
    private string _username = "test";
    private static Score _instance;

    private List<PlayerScore> _players = new List<PlayerScore>();

    public TextMeshProUGUI scoreText;

    public static void AddScore(int points)
    {
        _instance._currentScore += points;
        string m = _instance._currentScore.ToString();
        string fill = "00000000";

        fill = fill.Remove(fill.Length - m.Length);
        string res = "<#363636>" + fill + "</color>" + m;
        _instance.scoreText.text = res;
    }

    void Start()
    {
        if (_instance != null)
            Destroy(this);

        _instance = this;
        _currentScore = 0;
        AddScore(0);

        LoadScoresFromPlayerPrefs();
    }

    private void OnDestroy()
    {
        SaveScoresToPlayerPrefs();
    }

    void LoadScoresFromPlayerPrefs()
    {
        for(int i = 0; i < 10; i++)
        {
            PlayerScore score = new PlayerScore
            {
                name = PlayerPrefs.HasKey($"p_score_{i}_name") ? PlayerPrefs.GetString($"p_score_{i}_name") : "",
                score = PlayerPrefs.HasKey($"p_score_{i}_score") ? PlayerPrefs.GetInt($"p_score_{i}_score") : 0
            };
            _players.Add(score);
        }
    }

    void SaveScoresToPlayerPrefs()
    {
        _players.Add(new PlayerScore() { name = _username, score = _currentScore });
        _players.Sort((a, b) => b.score - a.score);
        
        for (int i = 0; i < 10; i++)
        {
            
            PlayerPrefs.SetString($"p_score_{i}_name", _players[i].name);
            PlayerPrefs.SetInt($"p_score_{i}_score", _players[i].score);
        }

        PlayerPrefs.Save();
    }
}

public struct PlayerScore
{
    public string name;
    public int score;
}
