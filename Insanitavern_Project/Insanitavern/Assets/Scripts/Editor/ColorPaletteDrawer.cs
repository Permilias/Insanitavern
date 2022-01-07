using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ColorPalette))]
public class ColorPaletteDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight * (lineAmount+1);
    }

    public int lineAmount;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);

        EditorGUI.indentLevel++;

        float lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        Rect titleRect = new Rect(position.x, position.y, position.width, lineHeight);
        EditorGUI.LabelField(titleRect, "Color Palette: " + label.text, EditorStyles.boldLabel);

        Rect typeRect = new Rect(position.x, position.y + lineHeight, position.width, lineHeight);

        EditorGUI.BeginChangeCheck();

        SerializedProperty typeProperty = property.FindPropertyRelative("type");

        EditorGUI.PropertyField(typeRect, typeProperty);

        

        int value = typeProperty.enumValueIndex;
        Rect colorRect = new Rect(position.x, position.y + (lineHeight * 2), position.width, lineHeight);
        switch (value)
        {
            case 0:
                lineAmount = 3;
                EditorGUI.PropertyField(colorRect, property.FindPropertyRelative("colors").GetArrayElementAtIndex(0));
                break;
            case 1:
                SerializedProperty colors = property.FindPropertyRelative("colors");
                lineAmount = 4 + colors.arraySize;
                EditorGUI.PropertyField(colorRect, colors);
                break;
            case 2:
                lineAmount = 3;
                EditorGUI.PropertyField(colorRect, property.FindPropertyRelative("gradients").GetArrayElementAtIndex(0));
                break;
            case 3:
                SerializedProperty gradients = property.FindPropertyRelative("gradients");
                lineAmount = 4 + gradients.arraySize;
                EditorGUI.PropertyField(colorRect, gradients);
                break;
        }

        EditorGUI.EndChangeCheck();
    }
}
