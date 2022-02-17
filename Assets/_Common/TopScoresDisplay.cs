using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Arcadeum.Common
{
    public class TopScoresDisplay : MonoBehaviour
    {
        [SerializeField] private SharedString gameName;
        [SerializeField] private List<TextMeshProUGUI> _texts;

        private void Start()
        {
            PlayerPrefs.DeleteAll();
            Refresh();
        }

        void Refresh()
        {
            LoadScoresFromPlayerPrefs();
        }

        void LoadScoresFromPlayerPrefs()
        {
            for (int i = 0; i < _texts.Count; i++)
            {
                var name = PlayerPrefs.HasKey($"{gameName.Value}_p_score_{i}_name") ? PlayerPrefs.GetString($"{gameName.Value}_p_score_{i}_name") : "NOBDY";
                var score = PlayerPrefs.HasKey($"{gameName.Value}_p_score_{i}_score") ? PlayerPrefs.GetInt($"{gameName.Value}_p_score_{i}_score") : 0;

                string scoreString = score.ToString();
                string fill = "00000000";

                fill = fill.Remove(fill.Length - scoreString.Length);
                string fullScoreString = fill + scoreString;

                _texts[i].text = $"{i+1}. {name} {fullScoreString}";
            }
        }
    }
}
