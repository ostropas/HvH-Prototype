using System;
using System.Collections.Generic;
using Scripts.Weapon;
using UnityEngine;

namespace Scripts.Configs
{
    [Serializable]
    public class CharacterSettings
    {
        [Range(float.Epsilon, 5f)]
        public float Speed;
        public float Health;
        public List<Weapon> Weapons;
    }

    [Serializable]
    public class Weapon
    {
        public WeaponView WeaponView;
        public float CheckRange;
        public float AttackInterval;
        public float Speed;
        public float LifeTime;
        public float Attack;
    }
}