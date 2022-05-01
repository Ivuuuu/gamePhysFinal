using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// A Singleton is like a static class, but can be destroyed and such. These are usually reserved for running specific tasks, such as the Player or Camera.
/// You usually do not have to concern yourself with Singletons, as I would handle implementing Singletons during integration.
/// </summary>
public class Singleton : MonoBehaviour, IDisposable {
	public Maid Maid = new Maid();
	public virtual void Dispose() {}
}
