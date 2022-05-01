/// This util script is imported from a previous game that I've made in Roblox.
/// @author  Ryan Vo
/// @version 1.0

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Similar to .Update() Unity has, but utilizes my custom Listener class that easily allows for disconnections preventing memory leaks.
/// Kinda useless with Chronos, but still used in general.
/// </summary>
/// <remarks>
/// <para>
/// Allows for FPS limits but kinda wonky and a more reliable way to adjust tick speed.
/// </para>
/// </remarks>
public sealed class Runservice : Singleton {
	private static List<Tuple<int, List<Listener<float>>>> _HeartbeatFuncList = new List<Tuple<int, List<Listener<float>>>>();
	private static List<Tuple<int, List<Listener<float>>>> _RenderStepFuncList = new List<Tuple<int, List<Listener<float>>>>();

	private static List<Tuple<int, List<Listener<float>>>> _FixedUpdateFuncList = new List<Tuple<int, List<Listener<float>>>>();
	private static List<Tuple<int, List<Listener<float>>>> _UpdateFuncList = new List<Tuple<int, List<Listener<float>>>>();
	private static List<Tuple<int, List<Listener<float>>>> _LateUpdateFuncList = new List<Tuple<int, List<Listener<float>>>>();

	private static float fixedDeltaTime;

	private static Listener<float> BindToEither(int priority, Func<float, bool> func, List<Tuple<int, List<Listener<float>>>> funcList) {
		Listener<float> self;
		List<Listener<float>> list = new List<Listener<float>>(); //Redundant cause complier doesn't like it if i didn't put this.
		list = null; //yea

		if (funcList.Count == 0) {
			list = new List<Listener<float>>();
			funcList.Add(new Tuple<int, List<Listener<float>>>(priority, list));
		} else {
			bool found = false;
			for (int i = 0; i < funcList.Count; i++) {
				Tuple<int, List<Listener<float>>> value = funcList[i];
				if (priority < value.Item1) {
					found = true;
					list = new List<Listener<float>>();

					funcList.Insert(i, new Tuple<int, List<Listener<float>>>(priority, list));
					break;
				} else if (priority == value.Item1) {
					found = true;
					list = value.Item2;
				}
			}
			if (!found) {
				list = new List<Listener<float>>();
				funcList.Add(new Tuple<int, List<Listener<float>>>(priority, list));
			}
		}

		self = new Listener<float>(func, list);

		return self;
	}

	/// <summary>
	/// Binds a function to run after Camera renders. Check priority list in Global.cs to see priorities.
	/// </summary>
	/// <remarks>
	/// Don't use this, preferably use BindToUpdate() since this uses a loop separate from Unity's event system.
	/// </remarks>
	/// <param name="priority"></param>
	/// <param name="func"></param>
	/// <returns>A Listener object.</returns>
	public static Listener<float> BindToHeartBeat(int priority, Func<float, bool> func) {
		return BindToEither(priority, func, _HeartbeatFuncList);
	}

	/// <summary>
	/// NOTE: This binds the function to fire BEFORE the camera renders. ONLY use this if needed, as making many functions fire before camera renders will make the game run slower.
	/// </summary>
	/// <remarks>
	/// Don't use this, preferably use BindToLateUpdate() since this uses a loop separate from Unity's event system.
	/// </remarks>
	/// <param name="priority"></param>
	/// <param name="func"></param>
	/// <returns></returns>
	public static Listener<float> BindToRenderStep(int priority, Func<float, bool> func) {
		return BindToEither(priority, func, _RenderStepFuncList);
	}

	/// <summary>
	/// Binds a function to run in Unity's FixedUpdate, which runs during physics.
	/// </summary>
	/// <param name="priority"></param>
	/// <param name="func"></param>
	/// <returns>A Listener object.</returns>
	public static Listener<float> BindToFixedUpdate(int priority, Func<float, bool> func) {
		return BindToEither(priority, func, _FixedUpdateFuncList);
	}

	/// <summary>
	/// Binds a function to run after a set amount of time using FixedUpdate. Think of it as BindToFixedUpdate(), but after a period of time.
	/// </summary>
	/// <param name="priority"></param>
	/// <param name="length"></param>
	/// <param name="func"></param>
	/// <returns>A Listener object.</returns>
	public static Listener<float> RunAfter(int priority, float time, Func<float, bool> func) {
		float counter = 0;
		return BindToEither(priority, (float dt) => {
			if (counter >= time) {
				return func.Invoke(dt);
			}
			counter += dt;
			return true;
		}, _FixedUpdateFuncList);
	}
	/// <summary>
	/// Binds a function to run for a set amount of time using FixedUpdate, and then destroyed. Think of it as BindToFixedUpdate(), but destroyed after a period of time.
	/// </summary>
	/// <param name="priority"></param>
	/// <param name="length"></param>
	/// <param name="func"></param>
	/// <returns>A Listener object.</returns>
	public static Listener<float> RunFor(int priority, float time, Func<float, bool> func) {
		float counter = 0;
		return BindToEither(priority, (float dt) => {
			if (counter <= time) {
				counter += dt;
				return func.Invoke(dt);
			}
			return false;
		}, _FixedUpdateFuncList);
	}

	public static Listener<float> RunEvery(int priority, float timer, Func<float, bool> func) {
		float counter = 0;
		return BindToEither(priority, (float dt) => {
			counter += dt;
			if (counter >= timer) {
				counter = 0;
				return func.Invoke(dt);
			}
			return true;
		}, _FixedUpdateFuncList);
	}

	/// <summary>
	/// Binds a function to run Unity's Update(), which runs after physics.
	/// </summary>
	/// <param name="priority"></param>
	/// <param name="func"></param>
	/// <returns>A Listener object.</returns>
	public static Listener<float> BindToUpdate(int priority, Func<float, bool> func) {
		return BindToEither(priority, func, _UpdateFuncList);
	}

	/// <summary>
	/// Binds a function to run to Unity's LateUpdate(), which runs after Update().
	/// </summary>
	/// <param name="priority"></param>
	/// <param name="func"></param>
	/// <returns></returns>
	public static Listener<float> BindToLateUpdate(int priority, Func<float, bool> func) {
		return BindToEither(priority, func, _LateUpdateFuncList);
	}

	private static void UpdateEither(List<Tuple<int, List<Listener<float>>>> funcList, float dt) {
		int i = 0;
		int size_ = funcList.Count;

		while (i < size_) {
			Tuple<int, List<Listener<float>>> v = funcList[i];

			int a = 0;
			int size = v.Item2.Count;

			if (size == 0) {
				funcList.RemoveAt(i);
				size_--;
			} else {
				while (a < size) {
					Listener<float> b = v.Item2[a];
					if (!b.Destroyed) {
						if (!b.Call(dt)) {
							b.Disconnect();
						}
					} else {
						v.Item2.RemoveAt(a);
						size--;
						continue;
					}
					a++;
				}
			}
			i++;
		}

	}

	private static float PrevTime_RenderStep;
	private static void BeforeCameraRender(Camera cam) {
		UpdateEither(_RenderStepFuncList, (Time.realtimeSinceStartup - PrevTime_RenderStep) * Global.Environment.TimeScale);
		PrevTime_RenderStep = Time.realtimeSinceStartup;
	}

	public void Awake() {
		PrevTime_RenderStep = Time.time;
		PrevTime_Heartbeat = Time.time;

		fixedDeltaTime = Time.fixedDeltaTime;
	}


	private static float PrevTime_Heartbeat;
	///<summary>
	/// Can only be called once.
	///</summary>
	public void Start() {
		PrevTime_RenderStep = Time.time;
		PrevTime_Heartbeat = Time.time;

		Maid.GiveTask(_HeartbeatFuncList);
		Maid.GiveTask(_RenderStepFuncList);

		Camera.onPreRender += Runservice.BeforeCameraRender;

		//Cleaning up binded Renderstep
		Maid.GiveTask(new Action(delegate () {
			Camera.onPreRender -= Runservice.BeforeCameraRender;
		}));

		if (Global.Performance.EnableFPSLimit) {
			Application.targetFrameRate = Global.Performance.FPSLimit;
		}

		Maid.GiveTask(Runservice.BindToFixedUpdate(Global.RunservicePriority.Heartbeat.First, (float dt) => {
			Time.timeScale = Global.Environment.TimeScale;
			Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
			return true;
		}));
	}

	public void FixedUpdate() {
		UpdateEither(_FixedUpdateFuncList, Time.deltaTime);
	}

	public void Update() {
		UpdateEither(_UpdateFuncList, Time.deltaTime);
	}

	public void LateUpdate() {
		UpdateEither(_LateUpdateFuncList, Time.deltaTime);
	}

	public override void Dispose() {
		Maid.Dispose();
	}
}
