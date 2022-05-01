using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;

[AttributeUsage(AttributeTargets.Field)]
public class SerializeProperty : PropertyAttribute {
	public string PropertyName { get; private set; }

	public SerializeProperty(string propertyName) {
		PropertyName = propertyName;
	}
}
