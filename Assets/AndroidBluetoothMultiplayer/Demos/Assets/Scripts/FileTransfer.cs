using System;
using System.IO;
using UnityEngine;

namespace LostPolygon.AndroidBluetoothMultiplayer.Examples {
    /// <summary>
    /// A very basic script for transferring files via RPC.
    /// Not very reliable and well-tested, but a good starting point
    /// and demonstrates the main concepts.
    /// The file is split into pieces and transmitted piece-by-piece over network.
    /// </summary>
    public class FileTransfer : MonoBehaviour {
        private MemoryStream _fileMemoryStream;
        private FileTransferState _fileTransferState;
        private NetworkView _networkView;

        // Data receival events
        public event Action<int> ReceiveFileStarted;
        public event Action<int> ReceiveFileChunk;
        public event Action<byte[]> ReceiveFileFinished;

        // Data transmission events
        public event Action<int> TransmitFileStarted;
        public event Action<int> TransmitFileChunk;
        public event Action TransmitFileFinished;

        public FileTransferState TransferState {
            get {
                return _fileTransferState;
            }
        }

        private void OnEnable() {
            _networkView = GetComponent<NetworkView>();
        }

        /// <summary>
        /// Initiates transfer of <paramref name="data"/> to all other players
        /// with default chunk size.
        /// </summary>
        /// <param name="data">
        /// The data to transfer.
        /// </param>
        public void TransmitFile(byte[] data) {
            TransmitFile(data, 16 * 1024);
        }

        /// <summary>
        /// Initiates transfer of <paramref name="data"/> to all other players.
        /// </summary>
        /// <param name="data">
        /// The data to transfer.
        /// </param>
        /// <param name="chunkSize">
        /// The size of chunks to split the file into.
        /// </param>
        public void TransmitFile(byte[] data, int chunkSize) {
            if (_fileTransferState != FileTransferState.None) {
                throw new IOException("File transfer already in progress");
            }

            _fileTransferState = FileTransferState.Transmitting;

            int dataSize = data.Length;
            int transmittedSize = 0;

            // Calculate number of chunks
            int chunksCount = data.Length / chunkSize + 1;

            // Notify listeners that transfer has started
            if (TransmitFileStarted != null)
                TransmitFileStarted(dataSize);

            // Send an RPC to notify other players about start of transfer
            _networkView.RPC("OnStartFileTransfer", RPCMode.Others, dataSize);
            for (int i = 0; i < chunksCount; i++) {
                // Calculate size of current chunk. It is usually smaller than chunkSize 
                // for the last chunk
                int currentChunkSize = chunkSize;
                if (transmittedSize + chunkSize > dataSize) {
                    currentChunkSize = dataSize - transmittedSize;
                    if (currentChunkSize == 0)
                        break;
                }

                // Allocate an array for a chunk, fill it and send over network
                byte[] chunk = new byte[currentChunkSize];
                Array.Copy(data, transmittedSize, chunk, 0, currentChunkSize);
                _networkView.RPC("ProcessFileChunk", RPCMode.Others, chunk);

                transmittedSize += currentChunkSize;

                // Notify listeners about new chunk transfer
                if (TransmitFileChunk != null)
                    TransmitFileChunk(dataSize);
            }

            // Send an RPC to notify other players about end of transfer
            _networkView.RPC("OnEndFileTransfer", RPCMode.Others);

            _fileTransferState = FileTransferState.None;

            // Notify event listeners about end of transfer
            if (TransmitFileFinished != null)
                TransmitFileFinished();
        }

        [RPC]
        private void OnStartFileTransfer(int fileSize) {
            _fileTransferState = FileTransferState.Receiving;

            // Notify event listener about start of file receival
            if (ReceiveFileStarted != null)
                ReceiveFileStarted(fileSize);

            // Allocate a temporary memory stream for incoming file
            _fileMemoryStream = new MemoryStream(fileSize);
        }

        [RPC]
        private void OnEndFileTransfer() {
            _fileTransferState = FileTransferState.None;

            // Returns reconstructed received file data to event listeners
            if (ReceiveFileFinished != null) {
                byte[] data = _fileMemoryStream.ToArray();
                ReceiveFileFinished(data);
            }
        }

        [RPC]
        private void ProcessFileChunk(byte[] data) {
            // Check if we can receive a chunk
            if (_fileTransferState != FileTransferState.Receiving) {
                _fileTransferState = FileTransferState.None;
                Debug.LogError("Received a file chunk when not in receival mode");
                return;
            }

            // Notify event listeners about new incoming chunk
            if (ReceiveFileChunk != null)
                ReceiveFileChunk(data.Length);

            // Save chunk data
            _fileMemoryStream.Write(data, 0, data.Length);
        }

        public enum FileTransferState {
            None,
            Transmitting,
            Receiving
        }
    }
}