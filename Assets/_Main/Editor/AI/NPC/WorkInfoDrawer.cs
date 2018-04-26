//using UnityEngine;
//using UnityEditor;

//[CustomPropertyDrawer(typeof(WorkInfo))]
//public class WorkInfoDrawer : PropertyDrawer
//{
//    private WorkInfo work;
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        using (new EditorGUI.PropertyScope(position, label, property))
//        {
//            EditorGUIUtility.labelWidth = 100;
//            position.height = EditorGUIUtility.singleLineHeight;

//            //位置矩形
//            Rect locationRect = new Rect(position)
//            {
                
//            };
//            Rect workNameRect = new Rect(position)
//            {
//                y = locationRect.y + EditorGUIUtility.singleLineHeight + 2
//            };
//            Rect workTimeRect = new Rect(position)
//            {
//                y = workNameRect.y + EditorGUIUtility.singleLineHeight + 2
//            };

//            //找到每个属性的序列化值
//            SerializedProperty locationProperty = property.FindPropertyRelative("workshop");
//            SerializedProperty workNameProperty = property.FindPropertyRelative("workName");
//            SerializedProperty workTimeProperty = property.FindPropertyRelative("workTime");

//            //绘制GUI
//            locationProperty.objectReferenceValue = EditorGUI.ObjectField(locationRect, locationProperty.displayName, locationProperty.objectReferenceValue, typeof(GameObject), true);
//            workNameProperty.stringValue = EditorGUI.TextField(workNameRect, workNameProperty.displayName, workNameProperty.stringValue);
//            workTimeProperty.floatValue = EditorGUI.FloatField(workTimeRect, workTimeProperty.displayName, workTimeProperty.floatValue);
//        }
//    }
//}
