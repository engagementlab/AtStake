#if UNITY_ANDROID

using System;
using System.Globalization;
using UnityEngine;
using LostPolygon.AndroidBluetoothMultiplayer.Internal;

namespace LostPolygon.AndroidBluetoothMultiplayer {
    /// <summary>
    /// Represents a Bluetooth device.
    /// </summary>
    [Serializable]
    public sealed class BluetoothDevice {
        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the Bluetooth address of the device.
        /// </summary>
        public string Address { get; private set; }

        /// <summary>
        /// Gets the <see cref="DeviceBondState"/> between this device and the current one.
        /// </summary>
        public DeviceBondState BondState { get; private set; }

        /// <summary>
        /// Gets the <see cref="BluetoothDeviceClass.Class"/>.
        /// </summary>
        public BluetoothDeviceClass.Class DeviceClass { get; private set; }

        /// <summary>
        /// Gets the <see cref="BluetoothDeviceClass.MajorClass"/>.
        /// </summary>
        public BluetoothDeviceClass.MajorClass DeviceMajorClass { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the device can be connected to.
        /// </summary>
        public bool IsConnectable { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BluetoothDevice"/> class.
        /// </summary>
        /// <param name="deviceString">
        /// The device string.
        /// </param>
        internal BluetoothDevice(string deviceString) {
            try {
                string[] tokens =
                    deviceString.Split(
                        new[] { AndroidBluetoothMultiplayer.kDataDelimiter },
                        StringSplitOptions.None
                        );

                Name = tokens[0].Trim();
                Address = tokens[1];
                BondState = (DeviceBondState) byte.Parse(tokens[2]);
                int deviceClassFull = int.Parse(tokens[3]);

                DeviceClass = (BluetoothDeviceClass.Class) deviceClassFull;
                DeviceMajorClass = DeviceClass.GetMajorClass();
                IsConnectable = DeviceClass.IsProbablyHandheldDataCapableDevice();

                if (Name == string.Empty) {
                    Name = Address;
                }
            } catch {
                Debug.LogError(string.Format("Exception while parsing BluetoothDevice, string was: {0}", deviceString));
                throw;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BluetoothDevice"/> class from the Java BluetoothDevice.
        /// </summary>
        /// <param name="bluetoothDeviceJavaObject">
        /// The Java object that is an instance of android.bluetooth.BluetoothDevice.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="bluetoothDeviceJavaObject"/> is null.
        /// </exception>
        internal BluetoothDevice(AndroidJavaObject bluetoothDeviceJavaObject) {
            try {
                if (bluetoothDeviceJavaObject.IsNull()) {
                    throw new ArgumentNullException("bluetoothDeviceJavaObject");
                }

                Name = bluetoothDeviceJavaObject.Call<string>("getName").Trim();
                Address = bluetoothDeviceJavaObject.Call<string>("getAddress");
                BondState = (DeviceBondState) bluetoothDeviceJavaObject.Call<int>("getBondState");

                AndroidJavaObject deviceClassJavaObject = bluetoothDeviceJavaObject.Call<AndroidJavaObject>("getBluetoothClass");
                int deviceClassFull = deviceClassJavaObject.Call<int>("getDeviceClass");
                DeviceClass = (BluetoothDeviceClass.Class) deviceClassFull;
                DeviceMajorClass = DeviceClass.GetMajorClass();
                IsConnectable = DeviceClass.IsProbablyHandheldDataCapableDevice();

                if (Name == string.Empty) {
                    Name = Address;
                }
            } catch {
                Debug.LogError("Exception while converting BluetoothDevice");
                throw;
            }
        }

        #region Comparison methods and operators
        private bool Equals(BluetoothDevice other) {
            return Address.ToLower(CultureInfo.InvariantCulture) == other.Address.ToLower(CultureInfo.InvariantCulture);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(BluetoothDevice)) return false;
            return Equals((BluetoothDevice) obj);
        }

        public override int GetHashCode() {
            return (Address != null ? Address.GetHashCode() : 0);
        }

        public static bool operator ==(BluetoothDevice left, BluetoothDevice right) {
            return Equals(left, right);
        }

        public static bool operator !=(BluetoothDevice left, BluetoothDevice right) {
            return !Equals(left, right);
        }
        #endregion Comparison methods and operators

        /// <summary>
        /// The bond state between the current Bluetooth device and
        /// the other Bluetooth device.
        /// </summary>
        public enum DeviceBondState : byte {
            /// <summary>
            /// No bond is established between the Bluetooth devices.
            /// </summary>
            None = 10,

            /// <summary>
            /// Bluetooth devices are currently establishing a bond.
            /// </summary>
            Bonding = 11,

            /// <summary>
            /// Bluetooth devices have an established bond.
            /// </summary>
            Bonded = 12
        }
    }
}

#endif