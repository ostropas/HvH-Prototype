using System;
using System.Collections.Generic;
using Scripts.Configs;
using Scripts.Enemies;
using Scripts.Utils;
using Scripts.Weapon;
using UniRx;
using UnityEngine;
using Zenject;

namespace Scripts.Player
{
    public class PlayerPresenter : IInitializable, IFixedTickable, IDisposable
    {
        public event Action OnDeath;
        
        private CharacterSettings _characterSettings;
        private PlayerView _playerView;
        private PlayerModel _playerModel;
        private Joystick _joystick;
        private WeaponPresenter.Factory _weaponFactory;
        private EnemyManager _enemyManager;
        
        private CompositeDisposable _disposer = new();
        private bool _isAlive = true;
        private List<WeaponPresenter> _currentWeapons = new();
        
        public PlayerPresenter(Joystick joystick, PlayerView playerView,
            CharacterSettings characterSettings, PlayerModel playerModel,
            WeaponPresenter.Factory weaponFactory, EnemyManager enemyManager)
        {
            _characterSettings = characterSettings;
            _playerModel = playerModel;
            _playerView = playerView;
            _joystick = joystick;
            _weaponFactory = weaponFactory;
            _enemyManager = enemyManager;
        }
        
        public void Initialize()
        {
            _joystick.OnInput.Select(x => x * _characterSettings.Speed).Subscribe(_playerView.Move).AddTo(_disposer);
            _playerModel.Health
                .Select(x => x / _characterSettings.Health)
                .Subscribe(_playerView.UpdateHealth).AddTo(_disposer);
            foreach (Configs.Weapon characterSettingsWeapon in _characterSettings.Weapons) {
                WeaponPresenter weapon = _weaponFactory.Create(characterSettingsWeapon, _playerView.WeaponParent);
                _currentWeapons.Add(weapon);
            }
            _enemyManager.OnKillEnemy += OnKillEnemy;
            _playerView.UpdateWeaponReload(0);
            _currentWeapons[0].WeaponAvailable.Subscribe(_playerView.UpdateWeaponReload).AddTo(_disposer);
        }

        public Vector2 Position { get => _playerView.Position; set => _playerView.Position = value; }
        public void ApplyDamage(float val)
        {
            float newVal = _playerModel.Health.Value - val;
            if (newVal <= 0)
            {
                _playerModel.Health.Value = 0;
                _disposer.Dispose();
                _playerModel.MaxScore = Mathf.Max(_playerModel.CurrentScore.Value, _playerModel.MaxScore);
                _isAlive = false;
                OnDeath?.Invoke();
            }
            else
            {
                _playerModel.Health.Value = newVal;
            }
        }

        private void OnKillEnemy()
        {
            _playerModel.CurrentScore.Value++;
        }

        public void FixedTick()
        {
            if (!_isAlive) return;
            foreach (WeaponPresenter weaponPresenter in _currentWeapons) {
                weaponPresenter.FixedTick();
            }
        }

        public void Dispose()
        {
            _enemyManager.OnKillEnemy -= OnKillEnemy;
        }
    }
}