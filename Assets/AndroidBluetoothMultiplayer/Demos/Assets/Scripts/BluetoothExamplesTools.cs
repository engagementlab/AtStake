using UnityEngine;

namespace LostPolygon.AndroidBluetoothMultiplayer.Examples {
    /// <summary>
    /// Internal examples tools.
    /// </summary>
    public static class BluetoothExamplesTools {
        public static float UpdateScaleMobile() {
            if (Application.platform != RuntimePlatform.Android)
                return 1f;

            float scaleFactor = (Screen.width - 20f) / 500f;
            if (scaleFactor < 1f) {
                scaleFactor = 1f;
            }

            Vector3 scale;
            scale.x = scaleFactor;
            scale.y = scaleFactor;
            scale.z = 1f;

            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);

            return scaleFactor;
        }

        public static void TouchScroll(ref Vector2 scrollPosition) {
            if (Input.touchCount > 0) {
                Touch touch = Input.touches[0];
                if (touch.phase == TouchPhase.Moved) {
                    scrollPosition.y += touch.deltaPosition.y; 
                    scrollPosition.y = Mathf.Max(0f, scrollPosition.y);
                }
            }
        }

#if UNITY_ANDROID
        public static string FormatDevice(BluetoothDevice device) {
            return string.Format("{0} [{1}], class: {2}, connectable: {3}", device.Name, device.Address, device.DeviceClass, device.IsConnectable);
        }
#endif
    }
}
