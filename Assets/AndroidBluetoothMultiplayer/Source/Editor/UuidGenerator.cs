using System;
using UnityEditor;
using UnityEngine;

namespace LostPolygon.AndroidBluetoothMultiplayer.Editor {
    public class UuidGenerator : EditorWindow {
        [MenuItem("Tools/Lost Polygon/Android Bluetooth Multiplayer/UUID generator")]
        private static void ShowWindow() {
#if UNITY_ANDROID
            GetWindow<UuidGenerator>(true, "UUID generator");
#else
            EditorUtility.DisplayDialog("Wrong build platform", "Build platform was not set to Android. Please choose Android as build Platform in File - Build Settings...", "OK");
#endif
        }

#if UNITY_ANDROID
        private string _uuid = "";

        private void OnGUI() {
            minSize = new Vector2(260f, 80f);
            maxSize = minSize;

            EditorGUILayout.LabelField("Randomly generated UUID: ");
            _uuid = EditorGUILayout.TextField(_uuid);
            if (GUILayout.Button("Generate new UUID") || _uuid == "") {
                _uuid = Guid.NewGuid().ToString();
                Repaint();
            }
        }
#endif
    }
}
