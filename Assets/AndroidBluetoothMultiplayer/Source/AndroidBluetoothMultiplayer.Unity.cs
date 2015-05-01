#if UNITY_ANDROID
using UnityEngine;
using LostPolygon.AndroidBluetoothMultiplayer.Internal;

namespace LostPolygon.AndroidBluetoothMultiplayer {
    /// <summary>
    /// A core class that wraps Java methods of Android plugin.
    /// </summary>
    public sealed partial class AndroidBluetoothMultiplayer : MonoBehaviour {
        /// <summary>
        /// Java Name of main plugin facade class.
        /// </summary>
        private const string kPluginClassName = "com.lostpolygon.unity.bluetoothmediator.BluetoothMediator";

        /// <summary>
        /// The name of the GameObject, used for receiving messages from Java side.
        /// </summary>
        private static readonly string kGameObjectName = typeof(AndroidBluetoothMultiplayer).Name;

        /// <summary>
        /// A reference to the Java BluetoothMediator object .
        /// </summary>
        private static readonly AndroidJavaObject _plugin;

        /// <summary>
        /// Whether the plugin is available and was loaded successfully.
        /// </summary>
        private static readonly bool _isPluginAvailable;

        /// <summary>
        /// A reference to singleton instance.
        /// </summary>
        private static AndroidBluetoothMultiplayer _instance;

        /// <summary>
        /// Initializes <see cref="AndroidBluetoothMultiplayer"/> class.
        /// Retrieves a pointer to the Java plugin object.
        /// Initalizes the singleton instance on the first usage of the class.
        /// </summary>
        static AndroidBluetoothMultiplayer() {
            _plugin = null;
            _isPluginAvailable = false;

            try {
                UpdateInstance();
            } catch {
                // Happens when this static constructor is called from a GameObject being created.
                // Just ignoring, as this is intended.
            }

#if !UNITY_EDITOR && UNITY_ANDROID
            // Retrieve BluetoothMediator singleton instance
            try {
                using (AndroidJavaClass mediatorClass = new AndroidJavaClass(kPluginClassName)) {
                    if (!mediatorClass.IsNull()) {
                        _plugin = mediatorClass.CallStatic<AndroidJavaObject>("getSingleton");
                        _isPluginAvailable = !_plugin.IsNull();
                    }
                }
            } catch {
                Debug.LogError("AndroidBluetoothMultiplayer initialization failed. Probably .jar not present?");
                throw;
            }
#endif
        }

        /// <summary>
        /// Tries to find an existing instance in the scene, 
        /// and creates one if there were none.
        /// </summary>
        private static void UpdateInstance() {
            if (_instance != null)
                return;

            // Trying to find an existing instance in the scene
            _instance = (AndroidBluetoothMultiplayer) FindObjectOfType(typeof(AndroidBluetoothMultiplayer));

            // Creating a new instance in case there are no instances present in the scene
            if (_instance != null)
                return;

            GameObject gameObject = new GameObject(kGameObjectName);
            _instance = gameObject.AddComponent<AndroidBluetoothMultiplayer>();

            // Make it hidden and indestructible
            gameObject.hideFlags = HideFlags.NotEditable | HideFlags.HideInHierarchy;
        }

        private void Awake() {
            // Kill other instances
            if (FindObjectsOfType(typeof(AndroidBluetoothMultiplayer)).Length > 1) {
                Debug.LogError("Multiple " + kGameObjectName + " instances found, destroying...");
                DestroyImmediate(gameObject);
                return;
            }

            _instance = this;

            // Set the GameObject name to the class name for UnitySendMessage
            gameObject.name = kGameObjectName;

            // We want this object to persist across scenes
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(this);
        }
    }
}

#endif