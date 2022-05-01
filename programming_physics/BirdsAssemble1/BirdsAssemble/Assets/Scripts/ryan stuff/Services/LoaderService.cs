using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

/// <summary>
/// A "static" class that loads <c>Singletons</c> into the game. Don't mess with this since this handles integration.
/// </summary>
public sealed class LoaderService : Singleton {
	//Starting singletons.
	public void Start() {
		if (SceneManager.GetActiveScene().name == "Title Screen") {
			PlayerPrefs.SetFloat("TotalScore", 0);
		}
	}

	//Disposing every singleton.
	public void OnApplicationQuit() {
		Maid.KillAll();
	}
}