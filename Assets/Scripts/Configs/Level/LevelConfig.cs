using System;
using System.Collections.Generic;

namespace Scripts.Configs.Level {
	[Serializable]
	public class LevelConfig {
		public List<WaveConfig> Waves;
	}

	[Serializable]
	public class WaveConfig {
		public int DurationInSeconds;
		public bool IsEndless;
	}
}
