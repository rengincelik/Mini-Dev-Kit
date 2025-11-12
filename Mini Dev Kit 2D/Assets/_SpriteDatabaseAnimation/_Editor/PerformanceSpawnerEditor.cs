#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PerformanceSpawner))]
public class PerformanceSpawnerEditor : Editor
{
    string targetState = "Idle";

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var spawner = (PerformanceSpawner)target;

        GUILayout.Space(10);
        GUILayout.Label("Spawn Controls", EditorStyles.boldLabel);

        if (Application.isPlaying)
        {
            if (GUILayout.Button("Start Spawning", GUILayout.Height(30)))
                spawner.StartSpawning();

            if (GUILayout.Button("Stop Spawning", GUILayout.Height(30)))
                spawner.StopSpawning();

            if (GUILayout.Button("Clear All", GUILayout.Height(30)))
                spawner.ClearAll();

            GUILayout.Space(10);
            GUILayout.Label("Change States", EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();
            targetState = GUILayout.TextField(targetState, GUILayout.Width(100));
            if (GUILayout.Button("Change All to →", GUILayout.Height(25)))
                spawner.ChangeAllStates(targetState);
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Change All to Random", GUILayout.Height(30)))
                spawner.ChangeAllStatesToRandom();
        }
        else
        {
            GUILayout.Label("Play mode'a gir butonları kullanmak için", EditorStyles.helpBox);
        }
    }
}
#endif
