using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Little things I've added onto Unity's classes to make it easier to do some stuff.
/// </summary>
public static class Extensions {
	/// <summary>
	/// Function that finds a child with the defined name through a deep search. Don't use this often since it's intensive.
	/// </summary>
	/// <param name="aParent">The gameObject which you're trying to find the child in.</param>
	/// <param name="aName">The string name of the child.</param>
	/// <returns></returns>
	public static Transform FindDeepChild(this Transform aParent, string aName) {
		Queue<Transform> queue = new Queue<Transform>();
		queue.Enqueue(aParent);
		while (queue.Count > 0) {
			var c = queue.Dequeue();
			if (c.name == aName)
				return c;
			foreach (Transform t in c)
				queue.Enqueue(t);
		}
		return null;
	}

	/// <summary>
	/// Function returns CFrame from a transform.
	/// </summary>
	/// <param name="obj">The Transform object.</param>
	/// <returns></returns>
	public static CFrame GetCFrame(this Transform obj) {
		return new CFrame(obj);
	}

	/// <summary>
	/// Updates position and rotation of a Transform using a cframe.
	/// </summary>
	/// <param name="aParent">The gTransform being updated.</param>
	/// <returns></returns>
	public static void UpdateFromCFrame(this Transform aParent, CFrame cframe) {
		aParent.position = cframe.p;
		aParent.rotation = (cframe - cframe.p).ToQuaternion();
	}

	/// <summary>
	/// Changes the Vector3 representation as a Vector2 representation, with the z axis ignored. (x,y,z) -> (x,y).
	/// </summary>
	/// <param name="_v"></param>
	/// <returns></returns>
	public static Vector2 AsVector2(this Vector3 _v) {
		return new Vector2(_v.x, _v.y);
	}

	/// <summary>
	/// Changes the Vector2 representation as a Vector3 representation, with the z axis defaulted to 0. (x,y) -> (x,y,0).
	/// </summary>
	/// <param name="_v"></param>
	/// <returns></returns>
	public static Vector3 AsVector3(this Vector2 _v) {
		return new Vector3(_v.x, _v.y, 0);
	}

	/*
	//Depth-first search
	public static Transform FindDeepChild(this Transform aParent, string aName)
	{
		foreach(Transform child in aParent)
		{
			if(child.name == aName )
				return child;
			var result = child.FindDeepChild(aName);
			if (result != null)
				return result;
		}
		return null;
	}
	*/

	/// <summary>
	/// Creates an audio source gameObject at a point which will destroy itself after the audio clip finishes playing.
	/// </summary>
	/// <param name="audioSource"></param>
	/// <param name="pos"></param>
	/// <returns></returns>
	public static AudioSource PlayClipAtPoint(this AudioSource audioSource, Vector3 pos) {
		GameObject tempGO = new GameObject("TempAudio"); // create the temp object
		tempGO.transform.position = pos; // set its position
		AudioSource tempASource = tempGO.AddComponent<AudioSource>(); // add an audio source
		tempASource.clip = audioSource.clip;
		tempASource.outputAudioMixerGroup = audioSource.outputAudioMixerGroup;
		tempASource.mute = audioSource.mute;
		tempASource.bypassEffects = audioSource.bypassEffects;
		tempASource.bypassListenerEffects = audioSource.bypassListenerEffects;
		tempASource.bypassReverbZones = audioSource.bypassReverbZones;
		tempASource.playOnAwake = audioSource.playOnAwake;
		tempASource.loop = audioSource.loop;
		tempASource.priority = audioSource.priority;
		tempASource.volume = audioSource.volume;
		tempASource.pitch = audioSource.pitch;
		tempASource.panStereo = audioSource.panStereo;
		tempASource.spatialBlend = audioSource.spatialBlend;
		tempASource.reverbZoneMix = audioSource.reverbZoneMix;
		tempASource.dopplerLevel = audioSource.dopplerLevel;
		tempASource.rolloffMode = audioSource.rolloffMode;
		tempASource.minDistance = audioSource.minDistance;
		tempASource.spread = audioSource.spread;
		tempASource.maxDistance = audioSource.maxDistance;
		// set other aSource properties here, if desired
		tempASource.Play(); // start the sound
		MonoBehaviour.Destroy(tempGO, tempASource.clip.length); // destroy object after clip duration (this will not account for whether it is set to loop)
		return tempASource; // return the AudioSource reference
	}
	private static object GetValue_Imp(object source, string name, int index) {
		var enumerable = GetValue_Imp(source, name) as System.Collections.IEnumerable;
		if (enumerable == null) return null;
		var enm = enumerable.GetEnumerator();
		//while (index-- >= 0)
		//    enm.MoveNext();
		//return enm.Current;

		for (int i = 0; i <= index; i++) {
			if (!enm.MoveNext()) return null;
		}
		return enm.Current;
	}


	private static object GetValue_Imp(object source, string name) {
		if (source == null)
			return null;
		var type = source.GetType();

		while (type != null) {
			var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			if (f != null)
				return f.GetValue(source);

			var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
			if (p != null)
				return p.GetValue(source, null);

			type = type.BaseType;
		}
		return null;
	}

}