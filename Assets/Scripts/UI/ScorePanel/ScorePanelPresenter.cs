using System;
using Scripts.Player;
using UniRx;
using Zenject;

namespace Scripts.UI.ScorePanel
{
    public class ScorePanelPresenter : IInitializable, IDisposable
    {
        private readonly PlayerModel _playerModel;
        private readonly ScorePanelView _panelView;
        private readonly CompositeDisposable _disposable = new();

        public ScorePanelPresenter(PlayerModel playerModel, ScorePanelView panelView)
        {
            _playerModel = playerModel;
            _panelView = panelView;
        }

        public void Initialize()
        {
            _panelView.UpdateMaxScore(_playerModel.MaxScore);
            _panelView.UpdateScore(0);
            _playerModel.CurrentScore.Subscribe(_panelView.UpdateScore).AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}