using System;
using Scripts.Level;
using UniRx;
using UnityEngine;
using Zenject;

namespace Scripts.UI.TimePanel {
	public class TimePanelPresenter : IInitializable, IDisposable {
        private readonly CompositeDisposable _disposable = new();
		private readonly LevelTimeManager _levelTimeManager;
		private readonly TimePanelView _timePanelView;

		public TimePanelPresenter(LevelTimeManager levelTimeManager, TimePanelView timePanelView) {
			_levelTimeManager = levelTimeManager;
			_timePanelView = timePanelView;
		}

		public void Initialize() {
			_levelTimeManager.TimeLeft.Subscribe(OnTimeUpdate).AddTo(_disposable);
		}

		public void OnTimeUpdate(float time) {
			if (_levelTimeManager.IsEndless) {
				_timePanelView.UpdateTimeText("Endless");	
			} else {
				string textTime = time > 0 ? Mathf.RoundToInt(time).ToString() : "Wave ended";
				_timePanelView.UpdateTimeText(textTime);
			}
		}
		
		public void Dispose() {
			_disposable?.Dispose();
		}

	}
}