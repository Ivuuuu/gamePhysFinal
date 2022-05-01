/// This util script is imported from a previous game that I've made in Java.
/// @author  Ryan Vo
/// @version 1.0

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wrapper class used to bind functions to value changes.
/// </summary>
/// <remarks>
/// <para>
/// The reason why I wanted to use this is to make it easier to listen to variable changes, running functions whenever a variable changes or whatever.
/// I can also lock a variable to prevent any changes without modifying any code that sets the variable.
/// The functions that connect to variable changes are <c>Listeners</c>.
/// </para>
/// </remarks>
[Serializable]
public class Variable<T> {
	[SerializeField] private T _Value;
	
	///<summary>Get or set values. When setting a value, it first sets the internal value then it calls upon all of its Listeners depending on if it calls on value change or when value is set even to the same value</summary>
	public T Value {
		get => _Value;
		set {
			if (!this.Locked) { //Checks if the function is locked.
				if (CheckIfSame && (EqualityComparer<T>.Default.Equals(this._Value, value))) {
					return;
				}
				this._Value = value;

				int i = 0; //Dynamic counter and size since iterators or for loops cannot be used since _FuncList is going to change.
				int size = _FuncList.Count;
				while (i < size) { //Checks if reach at end of list.
					Listener<T> func = _FuncList[i];
					if (func.Destroyed) { //Checks if the function should be garbaged collected and if so remove itself from _FuncList.
						this._FuncList.RemoveAt(i);
						size--; //Size decreases since it removes well a connected function
						continue;
					} else if (func.PlayOnce) { //If playonce is true then just call the function and set it as destroyed
						func.Disconnect();
					};
					func.Call(value);
					i++;
				};
			}
		}
	}

	private List<Listener<T>> _FuncList = new List<Listener<T>>();

	[SerializeField] public bool Locked = false;
	[SerializeField] public bool CheckIfSame = true;

	/// <summary>
	/// Creates and returns a Variable class with the stored parameter as its Value.
	/// </summary>
	/// See <see cref="Variable(T, bool)"/> to define if the Variable should check whenever a new value assigned to it should be checked if same.
	/// <typeparam name="T">The type of the stored value of Variable.</typeparam>
	/// <param name="value">The Variable object's stored value.</param>
	public Variable(T value) {
		this._Value = value;
		this.Locked = false;
	}

	/// <summary>
	/// Creates and returns a Variable class with the stored parameter as its Value. Its second parameter defines if the Variable should check whenever a new value assigned to it should be checked if same.
	/// </summary>
	/// <typeparam name="T">The type of the stored value of Variable.</typeparam>
	/// <param name="value">The Variable object's stored value.</param>
	public Variable(T value, bool CheckIfSame) {
		this._Value = value;
		this.CheckIfSame = CheckIfSame;
		this.Locked = false;
	}

	public Variable() {
		this.Locked = false;
	}

	/// <summary>
	/// Connects the defined function in parameter to be called whenever the variable changes.
	/// </summary>
	/// <param name="func">A lambda function that takes the Variable's stored value's type and returns a boolean.</param>
	/// <returns>A Listener object. This can be used separately to call the function or disconnect it manually.</returns>
	public Listener<T> Connect(Func<T, bool> func) {
		return new Listener<T>(func, this._FuncList, false);
	}

	/// <summary>
	/// Calls every single connected Listeners. Useful in edge cases.
	/// </summary>
	public void Call() {
		int i = 0; //Dynamic counter and size since iterators or for loops cannot be used since _FuncList is going to change.
		int size = _FuncList.Count;
		while (i < size) { //Checks if reach at end of list.
			Listener<T> func = _FuncList[i];
			if (func.Destroyed) { //Checks if the function should be garbaged collected and if so remove itself from _FuncList.
				this._FuncList.RemoveAt(i);
				size--; //Size decreases since it removes well a connected function
				continue;
			} else if (func.PlayOnce) { //If playonce is true then just call the function and set it as destroyed
				func.Disconnect();
			};
			func.Call(_Value);
			i++;
		};
	}

	/// <summary>
	/// Disconnects all currently connected functions.
	/// </summary>
	public void Disconnect() {
		_FuncList.Clear();
	}
}

[Serializable]
public class DesiredAction<T> : Variable<T> {
	public T Actual { get => Value; }

	//[SerializeProperty("Desired")]
	private T _Desired;
	public T Desired {
		get => _Desired;
		set {
			_Desired = value;
			if (!_Locked) {
				this.Value = _Desired;
			}
		}
	}

	//[SerializeProperty("Locked")]
	private bool _Locked;
	public new bool Locked {
		get => _Locked;
		set {
			_Locked = value;
			if (!_Locked) {
				this.Value = _Desired;
			}
		}
	}

	public DesiredAction(T value) : base(value) {
		Desired = value;
	}

	public DesiredAction() : base() {}
}