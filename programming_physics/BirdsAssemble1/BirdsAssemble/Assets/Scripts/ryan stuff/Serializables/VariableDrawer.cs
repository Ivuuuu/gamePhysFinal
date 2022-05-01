using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
/*
// IngredientDrawerUIE
[CustomPropertyDrawer(typeof(Variable<T>))]
public class IngredientDrawerUIE : PropertyDrawer {
	public override VisualElement CreatePropertyGUI(SerializedProperty property) {
		// Create property container element.
		var container = new VisualElement();

		// Create property fields.
		var amountField = new PropertyField(property.FindPropertyRelative("amount"));
		var unitField = new PropertyField(property.FindPropertyRelative("unit"));
		var nameField = new PropertyField(property.FindPropertyRelative("name"), "Fancy Name");

		// Add fields to the container.
		container.Add(amountField);
		container.Add(unitField);
		container.Add(nameField);

		return container;
	}
}
*/