/// This util script is imported from a previous game that I've made in Java.
/// @author  Ryan Vo
/// @version 1.0
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that's analagous to UnityEvent. This one just allows for automatic memory management by destroying itself.
/// </summary>
public abstract class BindableEvent : IDisposable {
	/// <summary>Protected bool that defines if the function is destroyed right after calling.</summary>
	protected bool _PlayOnce = false;

	///<summary>Protected bool that defines if the function is destroyed right after calling.</summary>
	public bool PlayOnce {
		get => this._PlayOnce;
	}

	/// <summary>Defines if the function is currently running. (Useless as of now)</summary>
	public bool Running = false;
	
	/// <summary>Unique name of Listener that makes debugging easier.</summary>
	public string Name = null;
	/// <summary>Protected bool that if the function is queued for garbage collection</summary>
	protected bool _Destroyed = false;

	/// <summary>If the function is queued for garbage collection</summary>
	public bool Destroyed {
		get {
			return this._Destroyed;
		}
	}

	/// <summary>
	/// Disconnects function from variable and queues it for garbage collection.
	/// </summary>
	public void Disconnect() {
		this._Destroyed = true;
	}

	/// <summary>
	/// Same as .Disconnect()
	/// </summary>
	public void Destroy() {
		this.Disconnect();
	}

	/// <summary>
	/// Same as .Disconnect()
	/// </summary>
	public void Dispose() {
		this.Disconnect();
	}
}

/// <summary>
/// This class is used as a way to encapsulate functions in order to use them as variables for function parameters, etc.
/// </summary>
/// <remarks>
/// It would encapsulate a function and attach itself to an outside list that contains all connected functions.
/// Since <c>_FuncList</c> contains all the connected functions, it's easy to just call them or disconnect them. A direct reference is not even needed.
/// <para>
/// IMPORTANT: One important caveat to this is that the function interface MUST return a bool, as this is used to detect when a function should be garbage collected.
/// For example if it returns true, then the function is able to run again. If it returns false, the function is removed from its parent list (funcList) and would not run again.
/// Basically, function return true = good, function return false = bad and destroyed/disconnected immediately.
/// </para>
/// </remarks>
public class Listener<T> : BindableEvent {
	/// <summary>Private property that stores the actual function.</summary>
	private Func<T, bool> _Func;
	/// <summary>Private reference to List that stores this object.</summary>
	private List<Listener<T>> _Parent;

	/// <summary>
	/// Initialize Listener.
	/// </summary>
	/// <param name="Func"> Lambda function to wrap. </param>
	/// <param name="ParentList"> Reference to List that this function appends to. </param>
	/// <param name="PlayOnce"> Defines if the function will be destroyed right after calling. </param>
	public Listener(Func<T, bool> Func, List<Listener<T>> ParentList, bool PlayOnce) {
		this._Func = Func;
		this._PlayOnce = PlayOnce; //If PlayOnce is true, this function only runs once, and then it disconnects itself from the variable
		this._Parent = ParentList;
		ParentList.Add(this); //How this works is that this class appends itself to an array that belongs to another class. Whenever that class wants to, it goes through this array and run every function attached.
	}
	public Listener(Func<T, bool> Func, List<Listener<T>> ParentList) {
		this._Func = Func;
		this._Parent = ParentList;
		ParentList?.Add(this); //How this works is that this class appends itself to an array that belongs to another class. Whenever that class wants to, it goes through this array and run every function attached.
	}

	public Listener(Func<T, bool> Func) {
		this._Func = Func;
	}

	/// <summary>
	/// Calls function. Was to lazy to find a way to actually make this use () like a function.
	/// </summary>
	/// <param name="v"> Value to send to wrapped function. </param>
	/// <returns>A boolean defining if it should be run again. </returns>
	public bool Call(T v) {
		if (!this._Destroyed) {
			if (!this._Func(v)) {
				this._Destroyed = true;
			}
			return !this._Destroyed;
		}
		return false;
	}
}