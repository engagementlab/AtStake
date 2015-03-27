#define DEBUG
using UnityEngine;
using System.Collections;

public class TimerValues {

	#if DEBUG
		public static readonly float brainstorm = 1;
		public static readonly float pitch = 1;
		public static readonly float deliberate = 1;
		public static readonly float extraTime = 0.5f;
	#else
		public static readonly float brainstorm = 60;
		public static readonly float pitch = 60;
		public static readonly float deliberate = 60;
		public static readonly float extraTime = 30;
	#endif
}