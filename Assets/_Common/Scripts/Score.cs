using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Arcadeum.Common
{
    [DisallowMultipleComponent]
    public class Score : MonoBehaviour
    {
        private static Score _instance;

        [SerializeField] private SharedString _gameName;
        [SerializeField] private int _playersCount;

        [Header("Interface")]
        [SerializeField] private List<TextMeshProUGUI> _scoreTexts;
        [SerializeField] private List<TextMeshProUGUI> _highscoreTexts;


        private int _currentScore;
        private string _username = "test";
        private List<PlayerScore> _players = new List<PlayerScore>();

        /// <summary>Inputs current player score to system and saves the sorted scores to player data </summary>
        public static void Save()
        {
            _instance.SaveScoresToPlayerPrefs();
        }

        /// <summary>Helper to set player name based on TextMeshPro text.</summary>
        public static void SetName(TextMeshProUGUI name) => SetName(name.text);

        /// <summary>Set current player name to the inputed string</summary>
        public static void SetName(string name)
        {
            _instance._username = name;
        }

        /// <summary>Adds provided amount of score to current one and displays it on the UI</summary>
        public static void AddScore(int points)
        {
            _instance._currentScore += points;

            string scoreString = ScoreToText(_instance._currentScore);
            foreach(TextMeshProUGUI text in _instance._scoreTexts)
                text.text = scoreString;

            string highscoreString = ScoreToText(Mathf.Max(_instance._currentScore, _instance._players[0].score));
            foreach (TextMeshProUGUI text in _instance._highscoreTexts)
                text.text = highscoreString;
        }
       
        private void Start()
        {
            if (_instance != null)
                Destroy(this);

            _instance = this;

            LoadScoresFromPlayerPrefs();

            _currentScore = 0;
            AddScore(0); // Refresh Score GUI
        }

        private static string ScoreToText(int points)
        {
            string m = points.ToString();
            string fill = "00000000";

            fill = fill.Remove(fill.Length - m.Length);
            string res = "<#363636>" + fill + "</color>" + m;
            return res;
        }

        private void LoadScoresFromPlayerPrefs()
        {
            for (int i = 0; i < _playersCount; i++)
            {
                PlayerScore score = new PlayerScore
                {
                    name = PlayerPrefs.HasKey($"{_gameName.Value}_p_score_{i}_name") ? PlayerPrefs.GetString($"{_gameName.Value}_p_score_{i}_name") : "",
                    score = PlayerPrefs.HasKey($"{_gameName.Value}_p_score_{i}_score") ? PlayerPrefs.GetInt($"{_gameName.Value}_p_score_{i}_score") : 0
                };
                _players.Add(score);
            }
        }
        private void SaveScoresToPlayerPrefs()
        {
            _players.Add(new PlayerScore() { name = _username, score = _currentScore });
            _players.Sort((a, b) => b.score - a.score);

            for (int i = 0; i < _playersCount; i++)
            {

                PlayerPrefs.SetString($"{_gameName.Value}_p_score_{i}_name", _players[i].name);
                PlayerPrefs.SetInt($"{_gameName.Value}_p_score_{i}_score", _players[i].score);
            }

            PlayerPrefs.Save();
        }
    }

    public struct PlayerScore
    {
        public string name;
        public int score;
    }
}