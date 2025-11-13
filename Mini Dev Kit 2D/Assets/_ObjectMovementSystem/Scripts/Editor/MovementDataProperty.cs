#if UNITY_EDITOR
using DG.Tweening;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(MovementData))]
public class MovementDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Foldout için ana başlık

        EditorGUI.indentLevel++;
        float yPos = position.y + EditorGUIUtility.singleLineHeight + 4;

        // Movement Type
        SerializedProperty doTypeProp = property.FindPropertyRelative("doType");
        if(doTypeProp == null)
            Debug.Log("doTypeProp null!");

        yPos = DrawProperty(position, property, "doType", "Movement Type", yPos);

        DOType doType = (DOType)doTypeProp.enumValueIndex;

        // Seçilen tipe göre parametreleri göster
        switch (doType)
        {
            case DOType.Move:
                yPos = DrawProperty(position, property, "targetValue", "Target Position", yPos);
                break;

            case DOType.MoveX:
                yPos = DrawProperty(position, property, "toEnd", "Target X Position", yPos);
                break;

            case DOType.MoveY:
                yPos = DrawProperty(position, property, "toEnd", "Target Y Position", yPos);
                break;

            case DOType.Rotate:
                yPos = DrawProperty(position, property, "toEnd", "Target Rotation (Z)", yPos);
                break;

            case DOType.Jump:
                yPos = DrawProperty(position, property, "targetValue", "End Position", yPos);
                yPos = DrawProperty(position, property, "jumpPower", "Jump Power", yPos);
                yPos = DrawProperty(position, property, "jumpCount", "Jump Count", yPos);
                break;

            case DOType.Path:
            case DOType.LocalPath:
                yPos = DrawArrayProperty(position, property, "pathPoints", "Waypoints", yPos);
                yPos = DrawProperty(position, property, "pathType", "Path Type", yPos);
                yPos = DrawProperty(position, property, "pathMode", "Path Mode", yPos);
                break;
        }

        yPos += 8;

        // Timing (her zaman görünsün)
        EditorGUI.LabelField(new Rect(position.x, yPos, position.width, EditorGUIUtility.singleLineHeight),
            "Timing", EditorStyles.boldLabel);
        yPos += EditorGUIUtility.singleLineHeight + 4;
        yPos = DrawProperty(position, property, "duration", "Duration", yPos);
        yPos = DrawProperty(position, property, "ease", "Ease", yPos);
        yPos = DrawProperty(position, property, "delay", "Delay", yPos);

        yPos += 8;

        // Loop (her zaman görünsün)
        EditorGUI.LabelField(new Rect(position.x, yPos, position.width, EditorGUIUtility.singleLineHeight),
            "Loop", EditorStyles.boldLabel);
        yPos += EditorGUIUtility.singleLineHeight + 4;
        yPos = DrawProperty(position, property, "loopType", "Loop Type", yPos);
        yPos = DrawProperty(position, property, "loopCount", "Loop Count", yPos);

        yPos += 8;

        // Options (her zaman görünsün)
        EditorGUI.LabelField(new Rect(position.x, yPos, position.width, EditorGUIUtility.singleLineHeight),
            "Options", EditorStyles.boldLabel);
        yPos += EditorGUIUtility.singleLineHeight + 4;
        yPos = DrawProperty(position, property, "useRelativeValues", "Use Relative Values", yPos);

        EditorGUI.indentLevel--;


        EditorGUI.EndProperty();
    }

    private float DrawProperty(Rect position, SerializedProperty parentProperty, string propertyName, string label, float yPos)
    {
        SerializedProperty prop = parentProperty.FindPropertyRelative(propertyName);
        float height = EditorGUI.GetPropertyHeight(prop);
        EditorGUI.PropertyField(new Rect(position.x, yPos, position.width, height), prop, new GUIContent(label));
        return yPos + height + 2;
    }

    private float DrawArrayProperty(Rect position, SerializedProperty parentProperty, string propertyName, string label, float yPos)
    {
        SerializedProperty prop = parentProperty.FindPropertyRelative(propertyName);
        float height = EditorGUI.GetPropertyHeight(prop, true);
        EditorGUI.PropertyField(new Rect(position.x, yPos, position.width, height), prop, new GUIContent(label), true);
        return yPos + height + 2;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isExpanded)
            return EditorGUIUtility.singleLineHeight;

        float height = EditorGUIUtility.singleLineHeight + 4; // Foldout - OnGUI ile aynı
        SerializedProperty doTypeProp = property.FindPropertyRelative("doType");
        height += EditorGUIUtility.singleLineHeight + 4; // Movement Type - OnGUI ile aynı

        DOType doType = (DOType)doTypeProp.enumValueIndex;
        height += EditorGUIUtility.singleLineHeight + 4;
        // Seçilen tipe göre yükseklik hesapla
        switch (doType)
        {
            case DOType.Move:
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("targetValue")) + 2;
                break;

            case DOType.MoveX:
            case DOType.MoveY:
            case DOType.Rotate:
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("toEnd")) + 2;
                break;

            case DOType.Jump:
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("targetValue")) + 2;
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("jumpPower")) + 2;
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("jumpCount")) + 2;
                break;

            case DOType.Path:
            case DOType.LocalPath:
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("pathPoints"), true) + 2;
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("pathType")) + 2;
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("pathMode")) + 2;
                break;
        }

        height += 8; // Space - OnGUI ile aynı

        // Timing section
        height += EditorGUIUtility.singleLineHeight + 4; // Label - OnGUI ile aynı
        height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("duration")) + 2;
        height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("ease")) + 2;
        height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("delay")) + 2;

        height += 8; // Space - OnGUI ile aynı

        // Loop section
        height += EditorGUIUtility.singleLineHeight + 4; // Label - OnGUI ile aynı
        height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("loopType")) + 2;
        height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("loopCount")) + 2;

        height += 8; // Space - OnGUI ile aynı

        // Options section
        height += EditorGUIUtility.singleLineHeight + 4; // Label - OnGUI ile aynı
        height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("useRelativeValues")) + 2;

        return height;
    }
}
#endif

