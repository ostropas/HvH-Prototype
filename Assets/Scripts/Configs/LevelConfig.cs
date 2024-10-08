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
		public bool IsEndless;
		public float SpawnRateOverSecond;
		public float MinSpawnDelay;
		public float StrengthOverSecond;
	}
}
