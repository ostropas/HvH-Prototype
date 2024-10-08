using Scripts.Level;
using Scripts.Player;
using Zenject;

namespace Scripts.UI.EndScreen {
	public class EndScreenPresenter {
		private	readonly EndScreenView _view;
        private readonly PlayerModel _playerModel;
		private readonly LevelManager _levelManager;

		public EndScreenPresenter(EndScreenView view, PlayerModel playerModel, LevelManager levelManager) {
			_view = view;
			_playerModel = playerModel;
			_levelManager = levelManager;
			
			_view.SetTotalScore(_playerModel.CurrentScore.Value, _playerModel.CurrentScore.Value > _playerModel.PreviousMaxScore);
			_view.SetOnCloseAction(OnClickNext);	
		}

		public void OnClickNext() {
			_levelManager.ReloadLevel();	
		}
	}

	public class EndScreenFactory : PlaceholderFactory<EndScreenPresenter> {
			
	}
}