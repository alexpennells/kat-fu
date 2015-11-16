using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (MeshGen), true)]
public class MeshGenEditor : Editor {
  public override void OnInspectorGUI () {
    DrawDefaultInspector();

    if (GUILayout.Button("Create Mesh")) {
      (target as MeshGen).Create();
    }

    if (GUILayout.Button("Destroy Mesh")) {
      (target as MeshGen).Remove();
    }
  }
}
