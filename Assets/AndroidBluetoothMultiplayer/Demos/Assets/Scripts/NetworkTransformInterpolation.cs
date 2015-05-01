using UnityEngine;

namespace LostPolygon.AndroidBluetoothMultiplayer.Examples {
    public class NetworkTransformInterpolation {
        private double _interpolationBackTime = 0.11;

        // We store twenty states with "playback" information
        private State[] _bufferedStates = new State[20];

        // Keep track of what slots are used
        private int _timestampCount = 0;
        private bool _isReceivedFirstInfo = false;

        public double InterpolationBackTime {
            get {
                return _interpolationBackTime;
            }

            set {
                _interpolationBackTime = value > 1 ? 1 : (value < 0 ? 0 : value);
            }
        }

        public void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info, Vector3 position, Quaternion rotation) {
            // Always send transform (depending on reliability of the network view)
            if (stream.isWriting) {
                // When receiving, buffer the information
                stream.Serialize(ref position);
                stream.Serialize(ref rotation);
            } else {
                // Receive latest state information
                position = Vector3.zero;
                rotation = Quaternion.identity;
                stream.Serialize(ref position);
                stream.Serialize(ref rotation);

                // Shift buffer contents, oldest data erased, 18 becomes 19, ... , 0 becomes 1
                for (int i = _bufferedStates.Length - 1; i >= 1; i--) {
                    _bufferedStates[i] = _bufferedStates[i - 1];
                }

                // Save currect received state as 0 in the buffer, safe to overwrite after shifting
                State state;
                state.Timestamp = info.timestamp;
                state.Position = position;
                state.Rotation = rotation;
                _bufferedStates[0] = state;

                // Increment state count but never exceed buffer size
                _timestampCount = Mathf.Min(_timestampCount + 1, _bufferedStates.Length);

                // Check integrity, lowest numbered state in the buffer is newest and so on
                for (int i = 0; i < _timestampCount - 1; i++) {
                    if (_bufferedStates[i].Timestamp < _bufferedStates[i + 1].Timestamp)
                        Debug.Log("State inconsistent");
                }

                _isReceivedFirstInfo = true;
                // Debug.Log("stamp: " + info.Timestamp + "my time: " + Network.time + "delta: " + (Network.time - info.Timestamp));
            }
        }

        // This only runs where the component is enabled, which is only on remote peers (server/clients)
        public void Update(ref Vector3 position, ref Quaternion rotation) {
            if (!_isReceivedFirstInfo)
                return;

            double currentTime = Network.time;
            double interpolationTime = currentTime - _interpolationBackTime;

            // We have a window of interpolationBackTime where we basically play 
            // By having interpolationBackTime the average ping, you will usually use interpolation.
            // And only if no more data arrives we will use extrapolation

            // Use interpolation
            // Check if latest state exceeds interpolation time, if this is the case then
            // it is too old and extrapolation should be used
            if (_bufferedStates[0].Timestamp > interpolationTime) {
                for (int i = 0; i < _timestampCount; i++) {
                    // Find the state which matches the interpolation time (time+0.1) or use last state
                    if (_bufferedStates[i].Timestamp <= interpolationTime || i == _timestampCount - 1) {
                        // The state one slot newer (<100ms) than the best playback state
                        State rhs = _bufferedStates[Mathf.Max(i - 1, 0)];

                        // The best playback state (closest to 100 ms old (default time))
                        State lhs = _bufferedStates[i];

                        // Use the time between the two slots to determine if interpolation is necessary
                        double length = rhs.Timestamp - lhs.Timestamp;
                        float t = 0.0f;

                        // As the time difference gets closer to 100 ms t gets closer to 1 in 
                        // which case rhs is only used
                        if (length > 0.0001)
                            t = (float) ((interpolationTime - lhs.Timestamp) / length);

                        // if t=0 => lhs is used directly
                        position = Vector3.Lerp(lhs.Position, rhs.Position, t);
                        rotation = Quaternion.Slerp(lhs.Rotation, rhs.Rotation, t);
                        return;
                    }
                }
            } else {
                // Use extrapolation. Here we do something really simple and just repeat the last
                // received state. You can do clever stuff with predicting what should happen.
                State latest = _bufferedStates[0];
                position = latest.Position;
                rotation = latest.Rotation;
            }
        }

        private struct State {
            public double Timestamp;
            public Vector3 Position;
            public Quaternion Rotation;
        }
    }
}