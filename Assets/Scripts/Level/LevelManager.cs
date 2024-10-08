using System;
using Scripts.Configs.Level;
using Scripts.Enemies;
using UniRx;
using UnityEngine;
using Zenject;

namespace Scripts.Level {
    public class LevelManager : IInitializable, IDisposable {
        private readonly LevelConfig _levelConfig;
        private readonly LevelTimeManager _levelTimeManager;
        private readonly EnemyManager _enemyManager;
        private CompositeDisposable _disposable = new();

        private int _currentWave;

        public LevelManager(LevelConfig levelConfig, LevelTimeManager levelTimeManager, EnemyManager enemyManager) {
            _levelConfig = levelConfig;
            _levelTimeManager = levelTimeManager;
            _enemyManager = enemyManager;
        }

        public void Initialize() {
            _currentWave = 0;
            StartWave(_levelConfig.Waves[_currentWave]);
        }

        private void StartWave(WaveConfig waveConfig) {
            _enemyManager.StartInstantiating();
            if (!waveConfig.IsEndless) {
                _levelTimeManager.StartLevelCountdown(waveConfig.DurationInSeconds);
                _levelTimeManager.TimeLeft.Subscribe(OnTimeUpdate).AddTo(_disposable);
            }
        }

        private void OnTimeUpdate(float time) {
            if (time <= 0) {
                _disposable.Dispose();
                _disposable = new CompositeDisposable();
                OnWaveEnd();
            }
        }

        private void OnWaveEnd() {
            _currentWave++;
            _currentWave = Mathf.Clamp(_currentWave, 0, _levelConfig.Waves.Count - 1); 
            StartWave(_levelConfig.Waves[_currentWave]);
        }

        public void Dispose() {
            _disposable?.Dispose();
        }
    }
}
