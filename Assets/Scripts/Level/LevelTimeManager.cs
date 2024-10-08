using UniRx;
using UnityEngine;
using Zenject;

namespace Scripts.Level {
    public class LevelTimeManager : ITickable {
        public ReactiveProperty<float> TimeLeft { get; private set; } = new();
        public bool IsEndless { get; private set; }
        
        public void StartLevelCountdown(int seconds) {
            IsEndless = seconds < 0;
            TimeLeft.Value = seconds;
        }
        
        public void Tick() {
            if (TimeLeft == null) return;
            if (TimeLeft.Value > 0) {
                TimeLeft.Value -= Time.deltaTime;
            }
        }
    }
}
