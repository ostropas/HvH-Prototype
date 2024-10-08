using System;
using Scripts.Configs;
using Scripts.Player;
using UniRx;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Scripts.Enemies
{
    public class EnemyPresenter : ITickable, IDisposable
    {
        public event Action<EnemyPresenter> OnKill;
        
        private readonly EnemyView _view;
        private readonly PlayerPresenter _playerPresenter;
        private readonly EnemySettings _enemySettings;
        private readonly EnemyModel _model;
        private readonly CompositeDisposable _disposable = new();
        private readonly CreateEnemyMulSettings _mulSettings;
            
        public EnemyPresenter(EnemyView enemyView, PlayerPresenter playerPresenter,
            EnemyModel model, EnemySettings enemySettings, CreateEnemyMulSettings mulSettings)
        {
            _view = enemyView;
            _playerPresenter = playerPresenter;
            _enemySettings = enemySettings;
            _mulSettings = mulSettings;
            _model = model;

            _view.OnDamage += ApplyDamage;
            _model.Health.Select(x => x / (_enemySettings.Health * _mulSettings.HealthMul)).Subscribe(_view.UpdateHealth).AddTo(_disposable);
        }

        public void Tick()
        {
            switch (_model.State)
            {
                case EnemyModel.EnemyState.Seek:
                   SeekState();
                    break;
                case EnemyModel.EnemyState.Attack:
                    AttackState();
                    break;
            }
        }

        private void SeekState()
        {
            Vector2 distance = _playerPresenter.Position - _view.Position;
            Vector2 direction = distance.normalized;
            _view.LookDirection(direction);
            _view.Move(direction * _enemySettings.Speed * _mulSettings.SpeedIncrease);
            if (distance.magnitude <= _enemySettings.AttackRange)
            {
                _model.State = EnemyModel.EnemyState.Attack;
            }
        }

        private void AttackState()
        {
            Vector2 distance = _playerPresenter.Position - _view.Position;
            if (distance.magnitude >= _enemySettings.AttackRange)
            {
                _model.State = EnemyModel.EnemyState.Seek;
                return;
            }

            if (_model.LastTimeAttack + _enemySettings.AttackInterval < Time.time)
            {
                _playerPresenter.ApplyDamage(_enemySettings.Attack * _mulSettings.StrengthMul);
                _model.LastTimeAttack = Time.time;
            }
        }
        

        public Vector2 Position { get => _view.Position; set => _view.Position = value; }

        private void ApplyDamage(float val)
        {
            float res = _model.Health.Value - val;
            // Due float accuracy it's better to check not for zero, but for a value a little bit larger than zero
            if (res <= 1E-06f)
            {
                _model.Health.Value = 0;
                OnKill?.Invoke(this);
            }
            else
            {
                _model.Health.Value = res;
            }
        }
        
        public void Dispose()
        {
            _view.OnDamage -= ApplyDamage;
            _disposable.Dispose();
            Object.Destroy(_view.gameObject);
        }
            
        public class Factory : PlaceholderFactory<CreateEnemySettings, CreateEnemyMulSettings, EnemyPresenter>
        {
        }
    }
}