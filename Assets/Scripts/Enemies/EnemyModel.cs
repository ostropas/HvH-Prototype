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
            private CreateEnemyMulSettings _enemyMulSettings;

            public Factory(EnemySettings enemySettings, CreateEnemyMulSettings enemyMulSettings) {
                _enemySettings = enemySettings;
                _enemyMulSettings = enemyMulSettings;
            }
            
            public EnemyModel Create()
            {
                return new EnemyModel()
                {
                    Health = new(_enemySettings.Health * _enemyMulSettings.HealthMul)
                };
            }
        }
    }
}