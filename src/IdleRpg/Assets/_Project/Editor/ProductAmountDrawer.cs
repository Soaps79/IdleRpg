using UnityEngine;
using QGame;
using UnityEditor;

[CustomPropertyDrawer(typeof(ProductAmount))]

public class ProductAmountDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);

		//// this looks good local, scaling not yet implemented
		EditorGUI.indentLevel = 1;
		position = EditorGUI.IndentedRect(position);
		var nameRect = new Rect(position.x, position.y, 80, position.height);
		EditorGUI.LabelField(nameRect, "Product");
		nameRect.x += 60;
		nameRect.width = 100;
		EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("Product"), GUIContent.none);

		EditorGUI.indentLevel = 5;
		position = EditorGUI.IndentedRect(position);

		var categoryRect = new Rect(position.x, position.y, 160, position.height);
		categoryRect.x += 20;
		EditorGUI.LabelField(categoryRect, "Amt");
		categoryRect.x += 30;
		categoryRect.width = 120;
		EditorGUI.PropertyField(categoryRect, property.FindPropertyRelative("_amount"), GUIContent.none);

		EditorGUI.EndProperty();
	}
}