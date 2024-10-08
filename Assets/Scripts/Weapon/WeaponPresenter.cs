using System;
using Scripts.Enemies;
using UniRx;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Scripts.Weapon
{
    public class WeaponPresenter : IFixedTickable
    {
        private WeaponView.Factory _viewFactory;
        private WeaponView _view;
        private WeaponModel _model;
        private Configs.Weapon _weapon;
        private EnemyManager _enemyManager;

        private readonly Subject<float> _weaponAvailable = new();
        public IObservable<float> WeaponAvailable => _weaponAvailable;

        public WeaponPresenter(WeaponModel model, WeaponView view, Configs.Weapon weapon, EnemyManager enemyManager)
        {
            _model = model;
            _view = view;
            _weapon = weapon;
            _enemyManager = enemyManager;
        }

        public void FixedTick()
        {
            float val = GetWaitTime();
            if (val > 0)
            {
                _weaponAvailable.OnNext(val / _weapon.AttackInterval);
                return;
            }
            _weaponAvailable.OnNext(0);
            
            EnemyPresenter closestEnemy = FindClosetEnemyPresenter();
            if (closestEnemy == null) return;
            
            WeaponBullet bullet = _view.Attack((closestEnemy.Position - (Vector2)_view.transform.position).normalized, _weapon.Speed);
            _model.PrevShootTime = Time.time;
            bullet.OnEnemyEnter += OnEnemyEnter;
            Object.Destroy(bullet.gameObject, _weapon.LifeTime);
            _model.PrevShootTime = Time.time;
        }

        private void OnEnemyEnter(WeaponBullet bullet, EnemyView enemyView)
        {
           enemyView.TakeDamage(_weapon.Attack); 
           Object.Destroy(bullet.gameObject);
        }

        public float GetWaitTime()
        {
            float time = _model.PrevShootTime + _weapon.AttackInterval - Time.time;
            return Mathf.Max(0, time);
        }

        private EnemyPresenter FindClosetEnemyPresenter()
        {
            EnemyPresenter closestEnemy = null;
            float minDistance = float.MaxValue;

            foreach (EnemyPresenter enemy in _enemyManager.Enemies)
            {
                float distance = ((Vector2)_view.transform.position - enemy.Position).magnitude;
                if (distance < minDistance && distance < _weapon.CheckRange)
                {
                    minDistance = distance;
                    closestEnemy = enemy;
                }
            }

            return closestEnemy;
        }
        

        public class Factory : PlaceholderFactory<Configs.Weapon, Transform, WeaponPresenter>
        {
            private WeaponModel.Factory _modelFactory;
            private WeaponView.Factory _viewFactory;
            private EnemyManager _enemyManager;

            public Factory(WeaponModel.Factory modelFactory, WeaponView.Factory viewFactory, EnemyManager enemyManager)
            {
                _modelFactory = modelFactory;
                _viewFactory = viewFactory;
                _enemyManager = enemyManager;
            }

            public override WeaponPresenter Create(Configs.Weapon param1, Transform param2)
            {
                WeaponView view = _viewFactory.Create(param1.WeaponView, param2);
                return new WeaponPresenter(_modelFactory.Create(), view, param1, _enemyManager);
            }
        }
    }
}