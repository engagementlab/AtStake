using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Prime31;



#if UNITY_IPHONE
public class MultiPeer
{
	[DllImport("__Internal")]
	private static extern void _multiPeerAdvertiseCurrentDevice( bool shouldAdvertise, string serviceType );

	// Starts/stops advertising the current device. This method will require the user to accept the connection.
	// Note that serviceType must be 1–15 characters long and can contain only ASCII lowercase letters, numbers, and hyphens
	public static void advertiseCurrentDevice( bool shouldAdvertise, string serviceType )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_multiPeerAdvertiseCurrentDevice( shouldAdvertise, serviceType );
	}


	[DllImport("__Internal")]
	private static extern void _multiPeerAdvertiseCurrentDeviceWithNearbyServiceAdvertiser( bool shouldAdvertise, string serviceType );

	// Starts/stops advertising the current device. This method will auto-connect all peers as they are found.
	// Note that serviceType must be 1–15 characters long and can contain only ASCII lowercase letters, numbers, and hyphens
	public static void advertiseCurrentDeviceWithNearbyServiceAdvertiser( bool shouldAdvertise, string serviceType )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_multiPeerAdvertiseCurrentDeviceWithNearbyServiceAdvertiser( shouldAdvertise, serviceType );
	}


	[DllImport("__Internal")]
	private static extern void _multiPeerShowPeerPicker();

	// iOS only. Shows the peer picker browser so users can select peers to connect with
	public static void showPeerPicker()
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_multiPeerShowPeerPicker();
	}


	[DllImport("__Internal")]
	private static extern void _multiPeerStartNearbyServiceBrowser();

	// Starts up the nearby service browser. This works much like the peer picker except it automatically invites any devices that it finds with no user interation required.
	public static void startNearbyServiceBrowser()
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_multiPeerStartNearbyServiceBrowser();
	}


	[DllImport("__Internal")]
	private static extern void _multiPeerStopNearbyServiceBrowser();

	// Stops the nearby service browser
	public static void stopNearbyServiceBrowser()
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_multiPeerStopNearbyServiceBrowser();
	}


	[DllImport("__Internal")]
	private static extern string _multiPeerGetLocalPeerID();

	// Gets the peerID of the local player
	public static string getLocalPeerId()
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			return _multiPeerGetLocalPeerID();

		return string.Empty;
	}


	[DllImport("__Internal")]
	private static extern string _multiPeerGetConnectedPeers();

	// Gets all the currently connected peers
	public static List<string> getConnectedPeers()
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			return Json.decode<List<string>>( _multiPeerGetConnectedPeers() );

		return new List<string>();
	}


	[DllImport("__Internal")]
	private static extern void _multiPeerDisconnectAndEndSession();

	// Disconnects from the current session and ends it completely
	public static void disconnectAndEndSession()
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_multiPeerDisconnectAndEndSession();
	}


	[DllImport("__Internal")]
	private static extern bool _multiPeerSendMessageToPeers( string peerIds, string gameObject, string method, string param, bool reliably );

	// Sends a message to all the peers present in peerIds. Works much like SendMessage with regard to the GameObject, method and parameter
	public static bool sendMessageToPeers( string[] peerIds, string gameObject, string method, string param, bool reliably = false )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			return _multiPeerSendMessageToPeers( Json.encode( peerIds ), gameObject, method, param, reliably );

		return false;
	}


	[DllImport("__Internal")]
	private static extern bool _multiPeerSendMessageToAllPeers( string gameObject, string method, string param, bool reliably = false );

	// Sends a message to all the connected peers. Works much like SendMessage with regard to the GameObject, method and parameter
	public static bool sendMessageToAllPeers( string gameObject, string method, string param, bool reliably = false )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			return _multiPeerSendMessageToAllPeers( gameObject, method, param, reliably );

		return false;
	}


	[DllImport("__Internal")]
	private static extern bool _multiPeerSendRawMessageToAllPeers( byte[] bytes, int length, bool reliably );

	// Sends a raw byte array message to all connected devices
	public static bool sendRawMessageToAllPeers( byte[] bytes, bool reliably = false )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			return _multiPeerSendRawMessageToAllPeers( bytes, bytes.Length, reliably );

		return false;
	}


	[DllImport("__Internal")]
	private static extern bool _multiPeerSendRawMessageToPeers( string peerIds, byte[] bytes, int length, bool reliably );

	// Sends a raw byte array message to all peerIds
	public static bool sendRawMessageToPeers( string[] peerIds, byte[] bytes, bool reliably = false )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			return _multiPeerSendRawMessageToPeers( Json.encode( peerIds ), bytes, bytes.Length, reliably );

		return false;
	}



}
#endif
