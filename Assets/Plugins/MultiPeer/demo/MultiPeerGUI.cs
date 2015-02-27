using UnityEngine;
using System.Collections.Generic;
using Prime31;


public class MultiPeerGUI : MonoBehaviourGUI
{
#if UNITY_IPHONE

	void OnEnable()
	{
		MultiPeerManager.receivedRawDataEvent += multiPeerRawMessageReceiver;
	}


	void OnDisable()
	{
		MultiPeerManager.receivedRawDataEvent -= multiPeerRawMessageReceiver;
	}


	void OnGUI()
	{
		beginColumn();


		GUILayout.Label( "First we need to advertise ourself" );

		if( GUILayout.Button( "Advertise Device" ) )
		{
			MultiPeer.advertiseCurrentDevice( true, "prime31-MyGame" );
		}


		if( GUILayout.Button( "Advertise Device (no UI)" ) )
		{
			MultiPeer.advertiseCurrentDeviceWithNearbyServiceAdvertiser( true, "prime31-MyGame" );
		}


		GUILayout.Label( "Then we can use Apple's built in UI" );

		if( GUILayout.Button( "Show Peer Picker" ) )
		{
			MultiPeer.showPeerPicker();
		}


		GUILayout.Label( "Or no UI at all" );

		if( GUILayout.Button( "Start Service Browser" ) )
		{
			MultiPeer.startNearbyServiceBrowser();
		}


		if( GUILayout.Button( "Stop Service Browser" ) )
		{
			MultiPeer.stopNearbyServiceBrowser();
		}


		GUILayout.Label( "Once connected, we can do more" );

		if( GUILayout.Button( "Get Connected Peers" ) )
		{
			var peers = MultiPeer.getConnectedPeers();
			Utils.logObject( peers );
		}


		if( GUILayout.Button( "Get Local PeerID" ) )
		{
			Debug.Log( "local peerID: " + MultiPeer.getLocalPeerId() );
		}


		if( GUILayout.Button( "Disconnect and End Session" ) )
		{
			MultiPeer.disconnectAndEndSession();
		}


		endColumn( true );


		GUILayout.Label( "Here are the different ways to send data" );

		if( GUILayout.Button( "Send Time to All Peers" ) )
		{
			var result = MultiPeer.sendMessageToAllPeers( "ui", "multiPeerMessageReceiver", Time.timeSinceLevelLoad.ToString() );
			Debug.Log( "send result: " + result );
		}


		if( GUILayout.Button( "Send Time to First Peer" ) )
		{
			var peers = MultiPeer.getConnectedPeers();
			if( peers.Count == 0 )
			{
				Debug.Log( "aborting send since there are no connected peers" );
				return;
			}

			var result = MultiPeer.sendMessageToPeers( new string[] { peers[0] }, "ui", "multiPeerMessageReceiver", Time.timeSinceLevelLoad.ToString() );
			Debug.Log( "send result: " + result );
		}


		if( GUILayout.Button( "Send Raw Message to All Peers" ) )
		{
			// we will just send some text across the wire encoded into a byte array for demonstration purposes
			var theStr = "im a string sent by MultiPeer magic";
			var bytes = System.Text.UTF8Encoding.UTF8.GetBytes( theStr );

			var result = MultiPeer.sendRawMessageToAllPeers( bytes );
			Debug.Log( "send result: " + result );
		}


		if( GUILayout.Button( "Send Raw Message to First Peers" ) )
		{
			var peers = MultiPeer.getConnectedPeers();
			if( peers.Count == 0 )
			{
				Debug.Log( "aborting send since there are no connected peers" );
				return;
			}

			// we will just send some text across the wire encoded into a byte array for demonstration purposes
			var theStr = "im a string sent by MultiPeer magic";
			var bytes = System.Text.UTF8Encoding.UTF8.GetBytes( theStr );

			var result = MultiPeer.sendRawMessageToPeers( new string[] { peers[0] }, bytes );
			Debug.Log( "send result: " + result );
		}

		endColumn( false );
	}


	#region Message receivers

	void multiPeerMessageReceiver( string param )
	{
		Debug.Log( "received a message: " + param );
	}


	void multiPeerRawMessageReceiver( string peerId, byte[] bytes )
	{
		var theStr = System.Text.UTF8Encoding.UTF8.GetString( bytes );
		Debug.Log( "received raw message from peer: " + peerId );
		Debug.Log( "message: " + theStr );
	}

	#endregion

#endif
}
