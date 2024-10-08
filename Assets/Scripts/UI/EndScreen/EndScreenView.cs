using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

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

	public class EndScreenViewFactory : IFactory<EndScreenView> {
		private DiContainer _container;
		private Canvas _uiCanvas;

		public EndScreenViewFactory(DiContainer container, Canvas uiCanvas) {
			_container = container;
			_uiCanvas = uiCanvas;
		}

		public EndScreenView Create() {
			EndScreenView obj = _container.InstantiatePrefabForComponent<EndScreenView>(Resources.Load<EndScreenView>("EndScreen"), _uiCanvas.transform);
			return obj;
		}
	}
}