using System;
using System.Collections.Generic;
using Scripts.Enemies;

namespace Scripts.Configs
{
    [Serializable]
    public class EnemySettings
    {
        public float Speed;
        public float AttackRange;
        public float AttackInterval;
        public float Attack;
        public float Health;
    }

    [Serializable]
    public class CreateEnemySettings
    {
        public EnemyView EnemyViewPrefab;
        public int InstantiateWeight;
        public EnemySettings EnemySettings;
    }
    

    [Serializable]
    public class EnemyManagerSettings
    {
        public float MinDistanceFromPlayer;
        public float MaxDistanceFromPlayer;
        public float DelayBetweenEnemies;
        public List<CreateEnemySettings> EnemiesSettings;
    }
}