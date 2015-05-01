#if UNITY_ANDROID

using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace LostPolygon.AndroidBluetoothMultiplayer.Editor {
    [InitializeOnLoad]
    public class ManifestChecker : EditorWindow {
        static ManifestChecker() {
            EditorApplication.playmodeStateChanged += GenerateManifestIfAbsent;
            GenerateManifestIfAbsent();
        }

        [PostProcessScene]
        private static void GenerateManifestIfAbsent() {
            if (File.Exists(ManifestGenerator.GetManifestPath())) {
                ManifestGenerator.PatchOldManifest();
                return;
            }

            ManifestGenerator.GenerateManifest();
            Debug.Log("AndroidManifest.xml was missing, generated new");
        }
    }
}

#endif