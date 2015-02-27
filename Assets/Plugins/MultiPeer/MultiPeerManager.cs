using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Prime31;


#if UNITY_IPHONE
public class MultiPeerManager : AbstractManager
{
	// Fired when the browser finishes. Possible parameter values are "done" or "cancelled"
	public static event Action<string> browserFinishedEvent;

	// Fired when a peer connects
	public static event Action<string> peerDidChangeStateToConnectedEvent;

	// Fired when a peer disconnects
	public static event Action<string> peerDidChangeStateToNotConnectedEvent;

	// Fired when a raw message is received. Includes the peerId and the raw data
	public static event Action<string,byte[]> receivedRawDataEvent;




	private delegate void receivedDataCallback( string playerId, IntPtr dataBuf, int dataSize );

	[DllImport("__Internal")]
	private static extern void _multiPeerSetReceivedDataCallback( receivedDataCallback callback );


	static MultiPeerManager()
	{
		AbstractManager.initialize( typeof( MultiPeerManager ) );

		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_multiPeerSetReceivedDataCallback( didReceivedData );
	}


	[AOT.MonoPInvokeCallback( typeof( receivedDataCallback ) )]
	private static void didReceivedData( string playerId, IntPtr dataBuf, int dataSize )
	{
		var data = new byte[dataSize];
		Marshal.Copy( dataBuf, data, 0, dataSize );

		if( receivedRawDataEvent != null )
			receivedRawDataEvent( playerId, data );
	}


	public void browserFinished( string param )
	{
		browserFinishedEvent.fire( param );
	}


	public void peerDidChangeStateToConnected( string param )
	{
		peerDidChangeStateToConnectedEvent.fire( param );
	}


	public void peerDidChangeStateToNotConnected( string param )
	{
		peerDidChangeStateToNotConnectedEvent.fire( param );
	}

}
#endif
