using UnityEngine;


namespace LostPolygon.AndroidBluetoothMultiplayer.Examples {
    /// <summary>
    /// Base GUI used for demos.
    /// </summary>
    public abstract class BluetoothDemoGuiBase : MonoBehaviour {
        protected virtual void OnLevelWasLoaded(int level) {
            Screen.sleepTimeout = 500;
            CameraFade.StartAlphaFade(Color.black, true, 0.25f, 0.0f);
        }

        protected virtual void OnDestroy() {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }

        protected virtual void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                GoBackToMenu();
            }
        }

        protected void DrawBackButton(float scaleFactor) {
            GUI.contentColor = Color.white;
            if (GUI.Button(new Rect(Screen.width / scaleFactor - (100f + 15f), Screen.height / scaleFactor - (40f + 15f), 100f, 40f), "Back")) {
                GoBackToMenu();
            }
        }

        protected void GoBackToMenu() {
#if UNITY_ANDROID
            OnBackToMenu();
#endif
            CameraFade.StartAlphaFade(Color.black, false, 0.25f, 0f, () => Application.LoadLevel("BluetoothDemoMenu"));
        }

#if UNITY_ANDROID
        protected abstract void OnBackToMenu();
#endif
    }
}