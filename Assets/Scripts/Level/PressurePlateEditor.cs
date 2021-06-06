using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(PressurePlate))]
public class PressurePlateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        PressurePlate plate = (PressurePlate)target;
        bool autoConfig = GUILayout.Button("Auto config");
        if(autoConfig)
        {
            plate.renderer = plate.GetComponent<Renderer>();
            plate.pressedSfx = Resources.Load<AudioClip>("snd/click");
            plate.pressedTexture = Resources.Load<Texture>("tex/PressurePlate2");
            plate.unpressedSfx = Resources.Load<AudioClip>("snd/click2");
            plate.unPressedTexture = Resources.Load<Texture>("tex/PressurePlate");
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            plate.GetComponent<AudioSource>().minDistance = 1f;
            plate.GetComponent<AudioSource>().maxDistance = 3f;
            plate.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Linear;
            plate.GetComponent<AudioSource>().spatialBlend = 1f;
        }
    }
}
