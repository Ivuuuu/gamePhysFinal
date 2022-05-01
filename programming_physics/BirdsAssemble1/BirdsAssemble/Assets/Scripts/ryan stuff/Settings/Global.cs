using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global {
	public static class Performance {
		public static bool EnableFPSLimit = true;
		public static int FPSLimit = 200;
	}

	public static class Environment {
		public static float Gravity = -9.81F;
		public static float TimeScale = 1F;
		public static float FixedTimeStep = 1 / 50F;
	}

	public static class  RunservicePriority {
		public static class RenderStep {
			public static int First = 0;
			public static int Camera = 100;
			public static int Character = 200;
			public static int Last = 1000;
		}

		public static class Heartbeat {
			public static int First = 0;
			public static int Input = 100;
			public static int Physics = 200;
		}
	}
}
