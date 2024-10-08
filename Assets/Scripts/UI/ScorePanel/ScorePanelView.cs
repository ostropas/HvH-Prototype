using System;
using TMPro;
using UnityEngine;

namespace Scripts.UI.ScorePanel
{
    public class ScorePanelView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _totalScore;
        [SerializeField] private TMP_Text _maxScore;
        private string _totalFormat;
        private string _maxFormat;

        private void Awake()
        {
            _totalFormat = _totalScore.text;
            _maxFormat = _maxScore.text;
        }

        public void UpdateScore(int score) => _totalScore.text = string.Format(_totalFormat, score.ToString());
        public void UpdateMaxScore(int score) => _maxScore.text = string.Format(_maxFormat, score.ToString());
    }
}