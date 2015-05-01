using UnityEngine;

namespace LostPolygon.AndroidBluetoothMultiplayer.Examples {
    /// <summary>
    /// A very simple object. Moves to the position of the touch with interpolation.
    /// </summary>
    public class TestActor : MonoBehaviour {
        public float Speed = 100f;
        public double NetworkInterpolationBackTime = 0.11;
        private Vector3 _destination;
        private Transform _transform;
        private Renderer _renderer;
        private NetworkView _networkView;
        private NetworkTransformInterpolation _transformInterpolation;

        private readonly Color[] kColors = {
            Color.blue,
            Color.cyan,
            Color.green,
            Color.magenta,
            Color.red,
            Color.white,
            Color.yellow
        };

        private void Awake() {
            _transform = GetComponent<Transform>();
            _renderer = GetComponent<Renderer>();
            _networkView = GetComponent<NetworkView>();
            _destination = transform.position;
            _renderer.material.color = kColors[Random.Range(0, kColors.Length)];

            _transformInterpolation = new NetworkTransformInterpolation();
            _transformInterpolation.InterpolationBackTime = NetworkInterpolationBackTime;
        }

        private void Update() {
            if (_networkView.isMine) {
                _destination.z = 0f;
                Vector3 direction = _destination - transform.position;

                if (direction.magnitude > 1f)
                    transform.Translate(Speed * direction.normalized * Time.deltaTime);

                if (Input.GetMouseButtonDown(0)) {
                    _destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
            } else {
                Vector3 interpolatedPosition = _transform.position;
                Quaternion interpolatedRotation = _transform.rotation;
                _transformInterpolation.Update(ref interpolatedPosition, ref interpolatedRotation);

                _transform.position = interpolatedPosition;
                _transform.rotation = interpolatedRotation;
            }
        }

        private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
            // Serialize the position and color
            if (stream.isWriting) {
                Color color = _renderer.material.color;

                stream.Serialize(ref color.r);
                stream.Serialize(ref color.g);
                stream.Serialize(ref color.b);
                stream.Serialize(ref color.a);
            } else {
                Color color = Color.white;

                stream.Serialize(ref color.r);
                stream.Serialize(ref color.g);
                stream.Serialize(ref color.b);
                stream.Serialize(ref color.a);

                _renderer.material.color = color;
            }

            _transformInterpolation.OnSerializeNetworkView(stream, info, _transform.position, _transform.rotation);
        }
    }
}