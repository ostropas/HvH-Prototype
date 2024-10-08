using System;
using System.Collections.Generic;
using Scripts.Enemies;
using UnityEngine.Serialization;

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

    public class CreateEnemyMulSettings {
        public float StrengthMul = 1;
        public float HealthMul = 1;
        public float SpeedIncrease = 1;
    }
    

    [Serializable]
    public class EnemyManagerSettings
    {
        public float MinDistanceFromPlayer;
        public float MaxDistanceFromPlayer;
        public float RefreshEnemiesCountRate;
        public List<CreateEnemySettings> EnemiesSettings;
    }
}