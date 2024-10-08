using Scripts.Configs;
using UniRx;
using UnityEngine;
using Zenject;

namespace Scripts.Player
{
    public class PlayerModel
    {
        private const string MaxScoreKey = "MaxScore";

        private int _maxScore;
        
        public ReactiveProperty<float> Health;
        public ReactiveProperty<int> CurrentScore;

        public int MaxScore
        {
            get => _maxScore;
            set
            {
                _maxScore = value;
                PlayerPrefs.SetInt(MaxScoreKey, value);
            }
        }

        public class Factory : IFactory<PlayerModel>
        {
            private CharacterSettings _characterSettings;

            public Factory(CharacterSettings characterSettings)
            {
                _characterSettings = characterSettings;
            }

            public PlayerModel Create()
            {
                return new PlayerModel()
                {
                    Health = new(_characterSettings.Health),
                    CurrentScore = new (),
                    MaxScore = PlayerPrefs.GetInt(MaxScoreKey, 0)
                };
            }
        }
    }
}