using Scripts.Configs;
using UniRx;
using Zenject;

namespace Scripts.Enemies
{
    public class EnemyModel
    {
        public ReactiveProperty<float> Health;
        public float LastTimeAttack = float.MinValue;
        public EnemyState State = EnemyState.Seek;
        
        public enum EnemyState
        {
           Seek,
           Attack
        }
        
        public class Factory : IFactory<EnemyModel>
        {
            private EnemySettings _enemySettings;

            public Factory(EnemySettings enemySettings)
            {
                _enemySettings = enemySettings;
            }
            
            public EnemyModel Create()
            {
                return new EnemyModel()
                {
                    Health = new(_enemySettings.Health)
                };
            }
        }
    }
}