using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CylinderCollider))]
public class CylinderCollderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CylinderCollider script = (CylinderCollider)target;
        if(GUILayout.Button("Build Object")) 
        {
            script.BuildCollider();
        }
        if(GUILayout.Button("Clear Collider")) 
        {
            script.ClearCollider();
        }
    }
}
