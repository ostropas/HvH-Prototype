using System;
using Scripts.Configs;
using Scripts.Enemies;
using Scripts.Player;
using Scripts.UI.EndScreen;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Scripts.Level {
    public class LevelManager : IInitializable, IDisposable, ITickable {
        private readonly LevelConfig _levelConfig;
        private readonly LevelTimeManager _levelTimeManager;
        private readonly EnemyManager _enemyManager;
        private PlayerPresenter _playerPresenter;
        private EndScreenFactory _endScreenFactory;
        private CompositeDisposable _disposable = new();
        private EndlessWaveModel _endlessWaveModel;
        private WaveConfig _currentWaveConfig;

        private int _currentWave;
        private LevelState _levelState;

        public LevelManager(LevelConfig levelConfig, LevelTimeManager levelTimeManager,
                            EnemyManager enemyManager, PlayerPresenter playerPresenter, EndScreenFactory endScreenFactory) {
            _levelConfig = levelConfig;
            _levelTimeManager = levelTimeManager;
            _enemyManager = enemyManager;
            _playerPresenter = playerPresenter;
            _endScreenFactory = endScreenFactory;
        }

        public void Initialize() {
            _playerPresenter.OnDeath += OnPlayerDead;
            _levelState = LevelState.WaitingToStart;
            _currentWave = 0;
            _enemyManager.EnemiesCount.Subscribe(OnEnemiesCountChanges).AddTo(_disposable);
            _levelTimeManager.TimeLeft.Subscribe(OnTimeUpdate).AddTo(_disposable);
            StartWave(_levelConfig.Waves[_currentWave]);
        }
        

        private void OnPlayerDead()
        {
            _playerPresenter.OnDeath -= OnPlayerDead;
            _levelState = LevelState.PlayerIsDead;
            _enemyManager.StopEnemies();
            _endScreenFactory.Create();
        }

        private void StartWave(WaveConfig waveConfig) {
            _currentWaveConfig = waveConfig;
            _levelState = LevelState.WaveInProgress;
            _enemyManager.StartInstantiating();
            
            if (!waveConfig.IsEndless) {
                _levelTimeManager.StartLevelCountdown(waveConfig.DurationInSeconds);
            } else {
                _endlessWaveModel = new EndlessWaveModel() {
                    DecreaseToSpawnDelay = 0,
                    CurrentStrengthIncrease = 1
                };
                _enemyManager.SetEndlessIncereaseVals(_endlessWaveModel);
                _levelTimeManager.StartLevelCountdown(-1);
            }
        }

        private void OnTimeUpdate(float time) {
            if (_levelState != LevelState.WaveInProgress) return;
            if (time <= 0) {
                _levelState = LevelState.TimeIsOver;
                _enemyManager.StopInstantiating();
                if (_enemyManager.EnemiesCount.Value == 0) {
                    OnWaveEnd();
                }
            }
        }

        private void OnEnemiesCountChanges(int count) {
            if (_levelState != LevelState.TimeIsOver) return;
            if (count == 0) {
                OnWaveEnd();
            }
        }

        private void OnWaveEnd() {
            _currentWave++;
            _currentWave = Mathf.Clamp(_currentWave, 0, _levelConfig.Waves.Count - 1); 
            StartWave(_levelConfig.Waves[_currentWave]);
        }

        public void ReloadLevel() {
            SceneManager.LoadScene(0);
        }
        

        public void Tick() {
            if (_endlessWaveModel != null) {
                _endlessWaveModel.CurrentStrengthIncrease += _currentWaveConfig.StrengthOverSecond * Time.deltaTime;
                if (_endlessWaveModel.DecreaseToSpawnDelay < _currentWaveConfig.MaxSpawnDecreasing) {
                    _endlessWaveModel.DecreaseToSpawnDelay += _currentWaveConfig.SpawnRateOverSecond * Time.deltaTime;
                }
            }
        }

        public void Dispose() {
            _disposable?.Dispose();
        }
    }

    public enum LevelState {
        WaitingToStart,
        WaveInProgress,
        TimeIsOver,
        PlayerIsDead
    }
}
