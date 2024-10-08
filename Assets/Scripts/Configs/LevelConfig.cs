using System;
using System.Collections.Generic;

namespace Scripts.Configs {
	[Serializable]
	public class LevelConfig {
		public List<WaveConfig> Waves;
	}

	[Serializable]
	public class WaveConfig {
		public int DurationInSeconds;
		public int OptimalEnemiesCount;
		public bool IsEndless;
		public float StrengthOverSecond;
		public float HealthOverSecond;
		public float SpeedOverSecond;
	}

	public class EndlessWaveModel {
		public float CurrentStrengthIncrease = 1;
		public float CurrentHealthIncrease = 1;
		public float CurrentSpeedIncrease = 1;
	}
}
