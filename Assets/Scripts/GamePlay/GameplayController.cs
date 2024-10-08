using Scripts.Player;
using UnityEngine.SceneManagement;
using Zenject;

namespace Scripts.GamePlay
{
    public class GameplayController : IInitializable
    {
        private PlayerPresenter _playerPresenter;

        public GameplayController(PlayerPresenter playerPresenter)
        {
            _playerPresenter = playerPresenter;
        }

        public void Initialize()
        {
            _playerPresenter.OnDeath += OnPlayerDead;
        }

        private void OnPlayerDead()
        {
            _playerPresenter.OnDeath -= OnPlayerDead;
            SceneManager.LoadScene(0);
        }
    }
}