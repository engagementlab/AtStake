using UnityEngine;

namespace LostPolygon.AndroidBluetoothMultiplayer.Examples {
    public class BluetoothDemoMenu : MonoBehaviour {
        private void OnLevelWasLoaded(int level) {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
            CameraFade.StartAlphaFade(Color.black, true, 0.25f, 0.0f);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        }

        private void OnGUI() {
            const float width = 250f;
            const float buttonHeight = 35f;
            const float height = 100f + buttonHeight * 3f;

            float scaleFactor = BluetoothExamplesTools.UpdateScaleMobile();
            GUI.color = Color.white;
            GUILayout.BeginArea(
                new Rect(
                    Screen.width / 2f / scaleFactor - width / 2f,
                    Screen.height / 2f / scaleFactor - height / 2f,
                    width,
                    height),
                "Android Bluetooth Multiplayer Demo",
                "Window");
            GUILayout.BeginVertical();

            GUILayout.Space(10);
            if (GUILayout.Button("Basic multiplayer", GUILayout.Height(buttonHeight)))
                CameraFade.StartAlphaFade(Color.black, false, 0.25f, 0f, () => Application.LoadLevel("BluetoothMultiplayerDemo"));

            if (GUILayout.Button("Device discovery", GUILayout.Height(buttonHeight)))
                CameraFade.StartAlphaFade(Color.black, false, 0.25f, 0f, () => Application.LoadLevel("BluetoothDiscoveryDemo"));

            if (GUILayout.Button("Basic RPC file transfer", GUILayout.Height(buttonHeight)))
                CameraFade.StartAlphaFade(Color.black, false, 0.25f, 0f, () => Application.LoadLevel("BluetoothFileTransferDemo"));

            GUILayout.Space(15);
            GUI.backgroundColor = new Color(1f, 0.6f, 0.6f, 1f);
            if (GUILayout.Button("Quit", GUILayout.Height(buttonHeight))) {
                CameraFade.StartAlphaFade(Color.black, false, 0.25f, 0f, Application.Quit);
            }

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

    }
}