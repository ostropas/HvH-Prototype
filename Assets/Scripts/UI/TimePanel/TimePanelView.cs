using TMPro;
using UnityEngine;

namespace Scripts.UI.TimePanel {
	public class TimePanelView : MonoBehaviour {
		[SerializeField] private TMP_Text _time;
		private string _initialText;

		private void Awake() {
			_initialText = _time.text;
		}

		public void UpdateTimeText(string time) {
			_time.text = string.Format(_initialText, time);
		}
	}
}