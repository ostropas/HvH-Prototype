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
    public class EnemyManager : IInitializable, ITickable, IDisposable
    {
        private EnemyPresenter.Factory _factory;
        private EnemyManagerSettings _settings;
        private PlayerView _player;
        
        private Dictionary<int, CreateEnemySettings> _enemiesCreateDictionary = new();
        private int _maxRandomCreateNumber;
        private List<EnemyPresenter> _enemyPresenters = new();

        private CompositeDisposable _disposable = new();

        public List<EnemyPresenter> Enemies => _enemyPresenters;
        public event Action OnKillEnemy;

        public readonly ReactiveProperty<int> EnemiesCount = new();
        
        public EnemyManager(EnemyPresenter.Factory factory, EnemyManagerSettings settings, PlayerView player) {
            _factory = factory;
            _settings = settings;
            _player = player;
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
            Observable.Interval(TimeSpan.FromSeconds(_settings.DelayBetweenEnemies))
                .Subscribe(t => CreateEnemy())
                .AddTo(_disposable);
        }

        public void StopInstantiating() {
           _disposable.Dispose();
           _disposable = new();
        }
        
        public void Tick()
        {
            foreach (EnemyPresenter enemyPresenter in _enemyPresenters)
            {
                enemyPresenter.Tick();
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

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}