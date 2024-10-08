using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.EndScreen {
	public class EndScreenView : MonoBehaviour {
		[SerializeField] private TMP_Text _totalScore;
		[SerializeField] private Button _nextButton;
		[SerializeField] private GameObject _newRecordObj;

		public void SetOnCloseAction(Action action) {
			_nextButton.onClick.AddListener(() => action?.Invoke());
		}

		public void SetTotalScore(int score, bool isRecord) {
			string currentText = _totalScore.text;
			_totalScore.text = string.Format(currentText, score);
			_newRecordObj.SetActive(isRecord);
		}
	}
}