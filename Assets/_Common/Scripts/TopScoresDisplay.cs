using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

namespace Arcadeum.Common
{
    public class TopScoresDisplay : MonoBehaviour
    {
        private static List<TopScoresDisplay> _instances = new List<TopScoresDisplay>();

        [SerializeField] private SharedString _gameName;
        [SerializeField] private List<TextMeshProUGUI> _texts;

        /// <summary>Refresh current top scores display by loading new ones from player prefs</summary>
        public static void Refresh()
        {
            foreach (var instance in _instances)
                instance.LoadScoresFromPlayerPrefs();
        }
        void LoadScoresFromPlayerPrefs()
        {
            for (int i = 0; i < _texts.Count; i++)
            {
                var name = PlayerPrefs.HasKey($"{_gameName.Value}_p_score_{i}_name") ? PlayerPrefs.GetString($"{_gameName.Value}_p_score_{i}_name") : "NOBDY";
                var score = PlayerPrefs.HasKey($"{_gameName.Value}_p_score_{i}_score") ? PlayerPrefs.GetInt($"{_gameName.Value}_p_score_{i}_score") : 0;

                string scoreString = score.ToString();
                string fill = "00000000";

                fill = fill.Remove(fill.Length - scoreString.Length);
                string fullScoreString = fill + scoreString;

                _texts[i].text = $"{i + 1}. {(name == "" ? "NOBDY" : name)} {fullScoreString}";
            }
        }

        private void OnEnable()
        {
            if (_gameName == null)
                throw new System.Exception("Game Name is not provided! Please provide Game Name!");

            LoadScoresFromPlayerPrefs();
            _instances.Add(this);
        }
        private void OnDisable()
        {
            _instances.Remove(this);
        }
    }
}
