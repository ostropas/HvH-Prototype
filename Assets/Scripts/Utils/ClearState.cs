using UnityEditor;
using UnityEngine;

namespace Scripts.Utils {
	public static class ClearState {
		[MenuItem("Tools/Clear Save")]
		public static void Clear() {
			PlayerPrefs.DeleteAll();
			PlayerPrefs.Save();

			Caching.ClearCache();
		}
	}
}