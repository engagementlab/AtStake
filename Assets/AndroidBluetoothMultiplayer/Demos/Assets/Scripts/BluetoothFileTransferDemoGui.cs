using System.Collections;
using System.IO;
using UnityEngine;
using LostPolygon.AndroidBluetoothMultiplayer;

namespace LostPolygon.AndroidBluetoothMultiplayer.Examples {
    public class BluetoothFileTransferDemoGui : BluetoothDemoGuiBase {
        public GameObject FileTransferPrefab;
#if !UNITY_ANDROID
        private void Awake() {
            Debug.LogError("Build platform is not set to Android. Please choose Android as build Platform in File - Build Settings...");
        }

        private void OnGUI() {
            GUI.Label(new Rect(10, 10, Screen.width, 100), "Build platform is not set to Android. Please choose Android as build Platform in File - Build Settings...");
        }
#else
        private const string kLocalIp = "127.0.0.1"; // An IP for Network.Connect(), must always be 127.0.0.1
        private const int kPort = 28000; // Local server IP. Must be the same for client and server
        private bool _initResult;
        private string _log;
        private Vector2 _logPosition = Vector2.zero;

        private BluetoothMultiplayerMode _desiredMode = BluetoothMultiplayerMode.None;

        private MemoryStream _fileMemoryStream;
        private FileTransfer _fileTransfer;
        private Texture2D _receivedTexture;
        private int _transferSize;
        private int _transferTotalSize;
        private const int kTextureSize = 256;

        private void HandleLog(string logString, string stackTrace, LogType logType) {
            if (logType == LogType.Error || logType == LogType.Exception) {
                _log += string.Format("Error: {0}, stacktrace: \n {1}", logString, stackTrace);
            } else {
                _log += logString + "\r\n";
            }
        }

        private void Awake() {
            Application.RegisterLogCallback(HandleLog);

            // Setting the UUID. Must be unique for every application
            _initResult = AndroidBluetoothMultiplayer.Initialize("8ce255c0-200a-11e0-ac64-0800200c9a66");

            // Enabling verbose logging. See log cat!
            AndroidBluetoothMultiplayer.SetVerboseLog(true);

            // Registering the event delegates
            AndroidBluetoothMultiplayer.ListeningStarted += OnBluetoothListeningStarted;
            AndroidBluetoothMultiplayer.ListeningStopped += OnBluetoothListeningStopped;
            AndroidBluetoothMultiplayer.AdapterEnabled += OnBluetoothAdapterEnabled;
            AndroidBluetoothMultiplayer.AdapterEnableFailed += OnBluetoothAdapterEnableFailed;
            AndroidBluetoothMultiplayer.AdapterDisabled += OnBluetoothAdapterDisabled;
            AndroidBluetoothMultiplayer.DiscoverabilityEnabled += OnBluetoothDiscoverabilityEnabled;
            AndroidBluetoothMultiplayer.DiscoverabilityEnableFailed += OnBluetoothDiscoverabilityEnableFailed;
            AndroidBluetoothMultiplayer.ConnectedToServer += OnBluetoothConnectedToServer;
            AndroidBluetoothMultiplayer.ConnectionToServerFailed += OnBluetoothConnectionToServerFailed;
            AndroidBluetoothMultiplayer.DisconnectedFromServer += OnBluetoothDisconnectedFromServer;
            AndroidBluetoothMultiplayer.ClientConnected += OnBluetoothClientConnected;
            AndroidBluetoothMultiplayer.ClientDisconnected += OnBluetoothClientDisconnected;
            AndroidBluetoothMultiplayer.DevicePicked += OnBluetoothDevicePicked;
        }

        // Don't forget to unregister the event delegates!
        protected override void OnDestroy() {
            base.OnDestroy();

            AndroidBluetoothMultiplayer.ListeningStarted -= OnBluetoothListeningStarted;
            AndroidBluetoothMultiplayer.ListeningStopped -= OnBluetoothListeningStopped;
            AndroidBluetoothMultiplayer.AdapterEnabled -= OnBluetoothAdapterEnabled;
            AndroidBluetoothMultiplayer.AdapterEnableFailed -= OnBluetoothAdapterEnableFailed;
            AndroidBluetoothMultiplayer.AdapterDisabled -= OnBluetoothAdapterDisabled;
            AndroidBluetoothMultiplayer.DiscoverabilityEnabled -= OnBluetoothDiscoverabilityEnabled;
            AndroidBluetoothMultiplayer.DiscoverabilityEnableFailed -= OnBluetoothDiscoverabilityEnableFailed;
            AndroidBluetoothMultiplayer.ConnectedToServer -= OnBluetoothConnectedToServer;
            AndroidBluetoothMultiplayer.ConnectionToServerFailed -= OnBluetoothConnectionToServerFailed;
            AndroidBluetoothMultiplayer.DisconnectedFromServer -= OnBluetoothDisconnectedFromServer;
            AndroidBluetoothMultiplayer.ClientConnected -= OnBluetoothClientConnected;
            AndroidBluetoothMultiplayer.ClientDisconnected -= OnBluetoothClientDisconnected;
            AndroidBluetoothMultiplayer.DevicePicked -= OnBluetoothDevicePicked;

            Application.RegisterLogCallback(null);
        }


        // Update is called once per frame
        private void OnGUI() {
            float scaleFactor = BluetoothExamplesTools.UpdateScaleMobile();

            // If initialization was successfull, showing the buttons
            if (_initResult) {
                // Show log
                float logY = 80f;
                if (_receivedTexture != null)
                    logY += kTextureSize;

                GUILayout.Space(logY);
                BluetoothExamplesTools.TouchScroll(ref _logPosition);
                _logPosition = GUILayout.BeginScrollView(
                    _logPosition,
                    GUILayout.MaxHeight(Screen.height / scaleFactor - logY),
                    GUILayout.MinWidth(Screen.width / scaleFactor - 10f / scaleFactor),
                    GUILayout.ExpandHeight(false));
                GUI.contentColor = Color.black;
                GUILayout.Label(_log, GUILayout.ExpandHeight(true), GUILayout.MaxWidth(Screen.width / scaleFactor));
                GUI.contentColor = Color.white;
                GUILayout.EndScrollView();

                // If there is no current Bluetooth connectivity
                BluetoothMultiplayerMode currentMode = AndroidBluetoothMultiplayer.GetCurrentMode();
                if (currentMode == BluetoothMultiplayerMode.None) {
                    if (GUI.Button(new Rect(10, 10, 150, 50), "Create server")) {
                        // If Bluetooth is enabled, then we can do something right on
                        if (AndroidBluetoothMultiplayer.GetIsBluetoothEnabled()) {
                            AndroidBluetoothMultiplayer.RequestEnableDiscoverability(120);
                            Network.Disconnect(); // Just to be sure
                            AndroidBluetoothMultiplayer.StartServer(kPort);
                        } else {
                            // Otherwise we have to enable Bluetooth first and wait for callback
                            _desiredMode = BluetoothMultiplayerMode.Server;
                            AndroidBluetoothMultiplayer.RequestEnableDiscoverability(120);
                        }
                    }

                    if (GUI.Button(new Rect(170, 10, 150, 50), "Connect to server")) {
                        // If Bluetooth is enabled, then we can do something right on
                        if (AndroidBluetoothMultiplayer.GetIsBluetoothEnabled()) {
                            Network.Disconnect(); // Just to be sure
                            AndroidBluetoothMultiplayer.ShowDeviceList(); // Open device picker dialog
                        } else {
                            // Otherwise we have to enable Bluetooth first and wait for callback
                            _desiredMode = BluetoothMultiplayerMode.Client;
                            AndroidBluetoothMultiplayer.RequestEnableBluetooth();
                        }
                    }
                } else {
                    // Stop all networking
                    if (GUI.Button(new Rect(10, 10, 150, 50), currentMode == BluetoothMultiplayerMode.Client ? "Disconnect" : "Stop server")) {
                        if (Network.peerType != NetworkPeerType.Disconnected)
                            Network.Disconnect();
                    }

                    // Display file transfer UI
                    if (Network.peerType != NetworkPeerType.Disconnected && (Network.isClient || Network.connections.Length > 0) && _fileTransfer != null) {
                        GUI.enabled = _fileTransfer.TransferState == FileTransfer.FileTransferState.None;
                        if (GUI.Button(new Rect(Screen.width / scaleFactor - 160, 10, 150, 50), "Send file")) {
                            Color32[] image = GenerateTexture(kTextureSize);

                            byte[] colorBytes = Color32ArrayToByteArray(image);
                            _fileTransfer.TransmitFile(colorBytes);
                        }
                        GUI.enabled = true;

                        // Display file transfer status
                        string status = null;
                        switch (_fileTransfer.TransferState) {
                            case FileTransfer.FileTransferState.Receiving:
                                status =
                                    string.Format("Receiving: {0:F1}% ({1} out of {2} bytes)",
                                        _transferSize / (float) _transferTotalSize * 100f,
                                        _transferSize,
                                        _transferTotalSize);
                                break;
                            case FileTransfer.FileTransferState.Transmitting:
                                status =
                                    string.Format("Transmitting: {0:F1}% ({1} out of {2} bytes)",
                                        _transferSize / (float) _transferTotalSize * 100f,
                                        _transferSize,
                                        _transferTotalSize);
                                break;
                            case FileTransfer.FileTransferState.None:
                                status = "Idle.";
                                break;
                        }

                        GUI.contentColor = Color.black;
                        GUI.Label(new Rect(10, 65, Screen.width, 20), status);

                        GUI.contentColor = Color.white;
                        if (_receivedTexture != null) {
                            GUI.Label(new Rect(10, 80, kTextureSize, kTextureSize), _receivedTexture);
                        }
                    }
                }
            } else {
                // Show a message if initialization failed for some reason
                GUI.contentColor = Color.black;
                GUI.Label(
                    new Rect(10, 10, Screen.width / scaleFactor - 10, 50),
                    "Bluetooth not available. Are you running this on Bluetooth-capable " +
                    "Android device and AndroidManifest.xml is set up correctly?");
            }

            DrawBackButton(scaleFactor);
        }

        protected override void OnBackToMenu() {
            // Gracefully closing all Bluetooth connectivity and loading the menu
            try {
                AndroidBluetoothMultiplayer.StopDiscovery();
                AndroidBluetoothMultiplayer.Stop();
            } catch {
                // 
            }
        }

        #region Bluetooth events

        private void OnBluetoothListeningStarted() {
            Debug.Log("Event - ListeningStarted");

            // Starting Unity networking server if Bluetooth listening started successfully
            Network.InitializeServer(2, kPort, false);
        }

        private void OnBluetoothListeningStopped() {
            Debug.Log("Event - ListeningStopped");

            // For demo simplicity, stop server if listening was canceled
            AndroidBluetoothMultiplayer.Stop();
        }

        private void OnBluetoothDevicePicked(BluetoothDevice device) {
            Debug.Log("Event - DevicePicked: " + BluetoothExamplesTools.FormatDevice(device));

            // Trying to connect to a device user had picked
            AndroidBluetoothMultiplayer.Connect(device.Address, kPort);
        }

        private void OnBluetoothClientDisconnected(BluetoothDevice device) {
            Debug.Log("Event - ClientDisconnected: " + BluetoothExamplesTools.FormatDevice(device));
        }

        private void OnBluetoothClientConnected(BluetoothDevice device) {
            Debug.Log("Event - ClientConnected: " + BluetoothExamplesTools.FormatDevice(device));
        }

        private void OnBluetoothDisconnectedFromServer(BluetoothDevice device) {
            Debug.Log("Event - DisconnectedFromServer: " + BluetoothExamplesTools.FormatDevice(device));

            // Stopping Unity networking on Bluetooth failure
            Network.Disconnect();
        }

        private void OnBluetoothConnectionToServerFailed(BluetoothDevice device) {
            Debug.Log("Event - ConnectionToServerFailed: " + BluetoothExamplesTools.FormatDevice(device));
        }

        private void OnBluetoothConnectedToServer(BluetoothDevice device) {
            Debug.Log("Event - ConnectedToServer: " + BluetoothExamplesTools.FormatDevice(device));

            // Trying to negotiate a Unity networking connection, 
            // when Bluetooth client connected successfully
            Network.Connect(kLocalIp, kPort);
        }

        private void OnBluetoothAdapterDisabled() {
            Debug.Log("Event - AdapterDisabled");
        }

        private void OnBluetoothAdapterEnableFailed() {
            Debug.Log("Event - AdapterEnableFailed");
        }

        private void OnBluetoothAdapterEnabled() {
            Debug.Log("Event - AdapterEnabled");

            // Resuming desired action after enabling the adapter
            switch (_desiredMode) {
                case BluetoothMultiplayerMode.Server:
                    Network.Disconnect();
                    AndroidBluetoothMultiplayer.StartServer(kPort);
                    break;
                case BluetoothMultiplayerMode.Client:
                    Network.Disconnect();
                    AndroidBluetoothMultiplayer.ShowDeviceList();
                    break;
            }

            _desiredMode = BluetoothMultiplayerMode.None;
        }

        private void OnBluetoothDiscoverabilityEnableFailed() {
            Debug.Log("Event - DiscoverabilityEnableFailed");
        }

        private void OnBluetoothDiscoverabilityEnabled(int discoverabilityDuration) {
            Debug.Log(string.Format("Event - DiscoverabilityEnabled(): {0} seconds", discoverabilityDuration));
        }
        #endregion Bluetooth events

        #region Multiplayer events

        private void OnPlayerDisconnected(NetworkPlayer player) {
            Debug.Log("Player disconnected: " + player.GetHashCode());
            Network.RemoveRPCs(player);
            Network.DestroyPlayerObjects(player);
        }

        private void OnFailedToConnect(NetworkConnectionError error) {
            Debug.Log("Can't connect to the networking server");

            // Stopping all Bluetooth connectivity on Unity networking disconnect event
            AndroidBluetoothMultiplayer.Stop();
        }

        private void OnDisconnectedFromServer() {
            Debug.Log("Disconnected from server");
            // Stopping all Bluetooth connectivity on Unity networking disconnect event
            AndroidBluetoothMultiplayer.Stop();

            Destroy(_receivedTexture);
            UnregisterFileTransfer();

            FileTransfer[] objects = FindObjectsOfType(typeof(FileTransfer)) as FileTransfer[];
            if (objects != null) {
                foreach (FileTransfer obj in objects) {
                    Destroy(obj.gameObject);
                }
            }
        }

        private void OnConnectedToServer() {
            Debug.Log("Connected to server");

            StartCoroutine(WaitForConnection());
        }

        private void OnServerInitialized() {
            Debug.Log("Server initialized");

            UnregisterFileTransfer();
            if (Network.isServer) {
                RegisterFileTransfer();
            }
        }

        #endregion Multiplayer events

        #region File transfer

        // Registers file transfer
        private IEnumerator WaitForConnection() {
            yield return new WaitForEndOfFrame();
            UnregisterFileTransfer();
            RegisterFileTransfer();
        }

        // Returns instance of FileTransfer or creates a GameObject with one if it is not present
        private FileTransfer GetFileTransferInstance() {
            
            FileTransfer[] objects = FindObjectsOfType(typeof(FileTransfer)) as FileTransfer[];
            if (objects != null) {
                foreach (FileTransfer transfer in objects) {
                    if (transfer != null)
                        return transfer;
                }
            }

            FileTransfer fileTransfer =
                ((GameObject) Network.Instantiate(FileTransferPrefab, Vector3.zero, Quaternion.identity, 0))
                    .GetComponent<FileTransfer>();
            return fileTransfer;
        }

        // Attaches FileTransfer event listeners to the methods
        private void RegisterFileTransfer() {
            _fileTransfer = GetFileTransferInstance();

            _fileTransfer.ReceiveFileFinished += ReceiveFileFinishedHandler;
            _fileTransfer.ReceiveFileStarted += ReceiveFileStartedHandler;
            _fileTransfer.ReceiveFileChunk += ReceiveFileChunkHandler;

            _fileTransfer.TransmitFileFinished += TransmitFileFinishedHandler;
            _fileTransfer.TransmitFileStarted += TransmitFileStartedHandler;
            _fileTransfer.TransmitFileChunk += TransmitFileChunkHandler;
        }

        // Deattaches FileTransfer event listeners to the methods
        private void UnregisterFileTransfer() {
            if (_fileTransfer == null)
                return;

            _fileTransfer.ReceiveFileFinished -= ReceiveFileFinishedHandler;
            _fileTransfer.ReceiveFileStarted -= ReceiveFileStartedHandler;
            _fileTransfer.ReceiveFileChunk -= ReceiveFileChunkHandler;

            _fileTransfer.TransmitFileFinished -= TransmitFileFinishedHandler;
            _fileTransfer.TransmitFileStarted -= TransmitFileStartedHandler;
            _fileTransfer.TransmitFileChunk -= TransmitFileChunkHandler;
        }

        // Called when a file chunk is sent
        private void TransmitFileChunkHandler(int chunkSize) {
            _transferSize += chunkSize;
        }

        // Called before file sending is started
        private void TransmitFileStartedHandler(int dataSize) {
            _transferSize = 0;
            _transferTotalSize = dataSize;
        }

        // Called when file sending is finished
        private void TransmitFileFinishedHandler() {
            Debug.Log("File transfer finished.");
        }

        // Called when receiving a file chunk
        private void ReceiveFileChunkHandler(int chunkSize) {
            _transferSize += chunkSize;
        }

        // Called when file receival is started
        private void ReceiveFileStartedHandler(int dataSize) {
            Debug.Log(string.Format("Start receiving file, size: {0} bytes", dataSize));
            _transferSize = 0;
            _transferTotalSize = dataSize;
        }

        // Called when file is received
        private void ReceiveFileFinishedHandler(byte[] data) {
            Color32[] colors = ByteArrayToColor32Array(data);
            _receivedTexture = new Texture2D(kTextureSize, kTextureSize, TextureFormat.RGB24, false, true);
            _receivedTexture.SetPixels32(colors);
            _receivedTexture.Apply();
        }

        #endregion File transfer

        #region Helper methods for texture generation

        private Color32[] GenerateTexture(int size) {
            Color32[] buffer = new Color32[size * size];
            int wavesCount = Random.Range(2, 5);
            for (int w = 0; w < wavesCount; w++) {
                float frequency = Random.Range(2f, 7f);
                Color color =
                    (new Vector4(
                        Random.Range(0f, 1f),
                        Random.Range(0f, 1f),
                        Random.Range(0f, 1f))
                        )
                        .normalized * 0.7f;

                int xShift = Random.Range(-size / 2, size / 2);
                int yShift = Random.Range(-size / 2, size / 2);

                for (int x = 0; x < size; x++) {
                    for (int y = 0; y < size; y++) {
                        int index = y * size + x;

                        float xValue = (x + xShift - size / 2f) / size;
                        float yValue = (y + yShift - size / 2f) / size;
                        float distValue = Mathf.Sqrt(xValue * xValue + yValue * yValue);
                        float sineValue = Mathf.Abs(Mathf.Sin(2f * frequency * distValue * Mathf.PI));
                        sineValue = Mathf.Pow(sineValue, 8f);

                        buffer[index] += sineValue * color;
                    }
                }
            }

            return buffer;
        }

        private byte[] Color32ArrayToByteArray(Color32[] colors) {
            byte[] bytes = new byte[colors.Length * 4];
            for (int i = 0; i < colors.Length; i ++) {
                int index = i * 4;
                bytes[index] = colors[i].r;
                bytes[index + 1] = colors[i].g;
                bytes[index + 2] = colors[i].b;
                bytes[index + 3] = colors[i].a;
            }

            return bytes;
        }

        private Color32[] ByteArrayToColor32Array(byte[] bytes) {
            Color32[] colors = new Color32[bytes.Length / 4];
            for (int i = 0; i < colors.Length; i++) {
                colors[i].r = bytes[i * 4];
                colors[i].g = bytes[i * 4 + 1];
                colors[i].b = bytes[i * 4 + 2];
                colors[i].a = bytes[i * 4 + 3];
            }

            return colors;
        }

        #endregion
#endif
    }
}