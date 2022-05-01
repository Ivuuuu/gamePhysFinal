using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// A class that "welds" two <c>Transform</c> objects together, like one of them is parented to the other. <para/> Don't use this if one is already parented to the other, make funky things.
/// </summary>
[Serializable]
public class Weld : IDisposable {
	//private Maid Maid = new Maid();
	private bool _Destroyed = false;

	/// <summary>
	/// The first Transform that the joint connects.
	/// </summary>
	public Transform Part0;
	/// <summary>
	/// The second Transform that the joint connects.
	/// </summary>
	public Transform Part1;

	/// <summary>
	/// Determines how the offset point is attached to Part0.
	/// </summary>
	[NonSerialized] public CFrame C0 = new CFrame();
	/// <summary>
	/// Is subtracted from the C0 property to create an offset point for Part1.
	/// </summary>
	[NonSerialized] public CFrame C1 = new CFrame();

	/// <summary>
	/// Determines if the joint is currently active in the world.
	/// </summary>
	public bool Active = true;

	private static Listener<float> _UpdateWelds;
	private static readonly List<Weld> _ListOfWelds = new List<Weld>();

	private void BindWelds() {
		//Maid.GiveTask(this);

		if (_UpdateWelds == null) {
			_UpdateWelds = Runservice.BindToUpdate(Global.RunservicePriority.Heartbeat.Physics, (float dt) => {
				int i = 0;
				int size = _ListOfWelds.Count;
				while (i < size) {
					Weld weld = _ListOfWelds[i];
					if (weld._Destroyed) {
						_ListOfWelds.RemoveAt(i);
						size--;
						continue;
					}

					if (weld.Active && weld.Part0 != null && weld.Part1 != null) {
						//weld.Part0.CFrame * weld.C0 = weld.Part1.CFrame * weld.C1
						weld.Part1.UpdateFromCFrame(weld.Part0.transform.GetCFrame() * weld.C0 * weld.C1.Inverse());
					}
					i++;
				}

				return true;
			});
			_UpdateWelds.Name = "_UpdateWelds";
		}

		_ListOfWelds.Add(this);
	}

	/// <summary>
	/// Constructs a Weld with the defined parameters.
	/// </summary>
	/// <param name="Part0"></param>
	/// <param name="Part1"></param>
	/// <param name="C0"></param>
	/// <param name="C1"></param>
	public Weld(Transform Part0, Transform Part1, CFrame C0, CFrame C1) {
		this.Part0 = Part0;
		this.Part1 = Part1;
		this.C0 = C0;
		this.C1 = C1;
		BindWelds();
	}

	/// <summary>
	/// Constructs a Weld with blank C0 and C1.
	/// </summary>
	/// <param name="Part0"></param>
	/// <param name="Part1"></param>
	public Weld(Transform Part0, Transform Part1) {
		this.Part0 = Part0;
		this.Part1 = Part1;
		BindWelds();
	}

	/// <summary>
	/// Constructs a Weld with blank fields for use later.
	/// </summary>
	public Weld() {
		BindWelds();
	}

	/// <summary>
	/// Destroys the Weld.
	/// </summary>
	public void Dispose() {
		this._Destroyed = true;
	}
}