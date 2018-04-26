//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using UnityEditorInternal;

//[CustomEditor(typeof(NPCInfo))]
//public class NPCInfoEditor : Editor {
//    ReorderableList list;

//    void OnEnable()
//    {
//        SerializedProperty listProperty = serializedObject.FindProperty("WorkFlow");
//        list = new ReorderableList(serializedObject, listProperty);
//        list.elementHeight = EditorGUIUtility.singleLineHeight * 3 + 8;

//        //绘制元素
//        list.drawElementCallback = (rect, index, isActive, isFocused) => {
//            SerializedProperty element = listProperty.GetArrayElementAtIndex(index);
//            rect.height -= 4;
//            rect.y += 2;
//            EditorGUI.PropertyField(rect, element);
//        };
//        //绘制标签
//        list.drawHeaderCallback = (rect) =>
//        {
//            EditorGUI.LabelField(rect, listProperty.displayName);
//        };
//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();
//        list.DoLayoutList();
//        serializedObject.ApplyModifiedProperties();
//    }
//}
