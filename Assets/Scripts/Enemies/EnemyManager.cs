using System;
using System.Collections.Generic;
using Scripts.Configs;
using Scripts.Player;
using Scripts.Utils;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Scripts.Enemies
{
    public class EnemyManager : IInitializable, ITickable
    {
        private EnemyPresenter.Factory _factory;
        private EnemyManagerSettings _settings;
        private PlayerView _player;
        
        private Dictionary<int, CreateEnemySettings> _enemiesCreateDictionary = new();
        private int _maxRandomCreateNumber;
        private List<EnemyPresenter> _enemyPresenters = new();

        private bool _isPlaying = true;

        public List<EnemyPresenter> Enemies => _enemyPresenters;
        public event Action OnKillEnemy;

        public readonly ReactiveProperty<int> EnemiesCount = new();

        private float _minSpawnDelay = 0;
        private float _spawnDelayIncreasing = 0;
        private float _currentSpawnDelay;
        private float _prevSpawnTime = float.MinValue;
        private bool _isSpawning;
        
        public EnemyManager(EnemyPresenter.Factory factory, EnemyManagerSettings settings, PlayerView player) {
            _factory = factory;
            _settings = settings;
            _player = player;
            _currentSpawnDelay = settings.DelayBetweenEnemies;
        }
        
        public void Initialize()
        {
            // Manipulations to have access to randomize selecting enemies depending on theirs weight
            _maxRandomCreateNumber = 0;
            foreach (CreateEnemySettings createEnemySettings in _settings.EnemiesSettings)
            {
                _maxRandomCreateNumber += createEnemySettings.InstantiateWeight;
                _enemiesCreateDictionary[_maxRandomCreateNumber] = createEnemySettings;
            }
        }

        public void StartInstantiating() {
            _isSpawning = true;
        }

        public void SetIncreaseRate(float rateIncreaseOverSecond, float minDelay) {
           _minSpawnDelay = minDelay; 
           _spawnDelayIncreasing = rateIncreaseOverSecond;
        }

        public void StopInstantiating() {
            _isSpawning = false;
        }
        
        public void Tick() {
            if (!_isPlaying) return;

            if (_isSpawning) {
                if (Time.time - _prevSpawnTime > _currentSpawnDelay) {
                    CreateEnemy();
                    _prevSpawnTime = Time.time;
                }
            }
            
            foreach (EnemyPresenter enemyPresenter in _enemyPresenters)
            {
                enemyPresenter.Tick();
            }

            if (_currentSpawnDelay > _minSpawnDelay) {
                _currentSpawnDelay -= _spawnDelayIncreasing * Time.deltaTime;
            }
        }

        private void CreateEnemy()
        {
            int randomNumber = Random.Range(0, _maxRandomCreateNumber);
            CreateEnemySettings selectedSettings = null;
            foreach (KeyValuePair<int, CreateEnemySettings> enemySettings in _enemiesCreateDictionary)
            {
                if (randomNumber < enemySettings.Key)
                {
                    selectedSettings = enemySettings.Value;
                    break;
                }
            }

            if (selectedSettings == null) return;
            EnemyPresenter res = _factory.Create(selectedSettings);
            AddEnemy(res);
            res.OnKill += OnEnemyKill;
            float angle = Random.Range(0, 360);
            float distance = Random.Range(_settings.MinDistanceFromPlayer, _settings.MaxDistanceFromPlayer);
            Vector2 vec = VectorUtils.rotate(Vector2.one * distance, angle * Mathf.Deg2Rad);
            Vector2 enemyPos = _player.Position + vec * distance;
            res.Position  = enemyPos;
        }

        private void OnEnemyKill(EnemyPresenter enemyPresenter)
        {
            enemyPresenter.OnKill -= OnEnemyKill;
            RemoveEnemy(enemyPresenter);
            enemyPresenter.Dispose();
            OnKillEnemy?.Invoke();
        }

        private void AddEnemy(EnemyPresenter enemyPresenter) {
            _enemyPresenters.Add(enemyPresenter);
            EnemiesCount.Value = _enemyPresenters.Count;
        }

        private void RemoveEnemy(EnemyPresenter enemyPresenter) {
            _enemyPresenters.Remove(enemyPresenter);
            EnemiesCount.Value = _enemyPresenters.Count;
        }

        public void StopEnemies() {
            _isPlaying = false;
        }
    }

    public class EnemyEndlessWaveIncreasingValue {
        public float StrengthMul;
    }
}