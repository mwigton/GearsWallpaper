#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Gear))]
public class Gear_Inspector : Editor 
{
    bool isRotateSettingsFoldout = true;
    bool isDimenstionSettingsFoldout = true;
    bool isMeshSettingsFoldout = true;
    Gear _gear;

	public override void OnInspectorGUI() 
    {
        _gear = (Gear)target;		 
		GUILayout.BeginVertical("Box");

        EditorGUI.indentLevel++;
        _gear.settings.isDriveGear = EditorGUILayout.Toggle("Drive Gear", _gear.settings.isDriveGear);
        if (!_gear.settings.isDriveGear)
        {
            _gear.settings.parentGear = (Gear)EditorGUILayout.ObjectField("Parent Gear", _gear.settings.parentGear, typeof(Gear), true);
        }
        EditorGUI.indentLevel--;

        isDimenstionSettingsFoldout = EditorGUILayout.Foldout(isDimenstionSettingsFoldout, "Dimension Settings");
        if (isDimenstionSettingsFoldout)
        {
            EditorGUI.indentLevel++;
            EditorGUI.indentLevel++;
            _gear.settings.gearRadianPosition = EditorGUILayout.IntSlider("Gear Radian Position", _gear.settings.gearRadianPosition, 0, 360);
            _gear.settings.dimensions.gearRadius = EditorGUILayout.Slider("Gear Radius", _gear.settings.dimensions.gearRadius, 0, 2f);
            _gear.settings.dimensions.teethLength = EditorGUILayout.Slider("Gear Teeth Length", _gear.settings.dimensions.teethLength, 0, 3);
            _gear.settings.dimensions.spokeCount = EditorGUILayout.IntField("Spoke Count", _gear.settings.dimensions.spokeCount);
            EditorUtility.SetDirty(target);
            EditorGUI.indentLevel--;
        }


        if (_gear.settings.isDriveGear)
        {
            isRotateSettingsFoldout = EditorGUILayout.Foldout(isRotateSettingsFoldout, "Rotate Settings");
            if (isRotateSettingsFoldout)
            {
                EditorGUI.indentLevel++;
                EditorGUI.indentLevel++;
                _gear.settings.rotation.rotateDirection = (Gear.RotateDirection)EditorGUILayout.EnumPopup("Rotation Direction", _gear.settings.rotation.rotateDirection);
                _gear.settings.rotation.rotateAxis = (Gear.RotateAxis)EditorGUILayout.EnumPopup("Rotation Axis", _gear.settings.rotation.rotateAxis);
                _gear.settings.rotation.speed = EditorGUILayout.FloatField("Speed", _gear.settings.rotation.speed);
                _gear.settings.rotation.localSpaceRotation = EditorGUILayout.Toggle("Local Space Rotation", _gear.settings.rotation.localSpaceRotation);
                EditorGUI.indentLevel--;
            }
        }

        isMeshSettingsFoldout = EditorGUILayout.Foldout(isMeshSettingsFoldout, "Mesh Settings");
        if (isMeshSettingsFoldout)
        {
            EditorGUI.indentLevel++;
            EditorGUI.indentLevel++;
            _gear.settings.gearMesh = (GameObject)EditorGUILayout.ObjectField("Gear Mesh", _gear.settings.gearMesh, typeof(GameObject), true);
            _gear.settings.pinMesh = (GameObject)EditorGUILayout.ObjectField("Pin Mesh", _gear.settings.pinMesh, typeof(GameObject), true);
            EditorGUI.indentLevel--;
        }

		GUILayout.Space(5);
		GUILayout.EndVertical();
		
	}
}
#endif