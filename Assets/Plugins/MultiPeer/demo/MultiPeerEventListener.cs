using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class MultiPeerEventListener : MonoBehaviour
{
#if UNITY_IPHONE
	void OnEnable()
	{
		// Listen to all events for illustration purposes
		MultiPeerManager.browserFinishedEvent += browserFinishedEvent;
		MultiPeerManager.peerDidChangeStateToConnectedEvent += peerDidChangeStateToConnectedEvent;
		MultiPeerManager.peerDidChangeStateToNotConnectedEvent += peerDidChangeStateToNotConnectedEvent;
		MultiPeerManager.receivedRawDataEvent += receivedRawDataEvent;
	}


	void OnDisable()
	{
		// Remove all event handlers
		MultiPeerManager.browserFinishedEvent -= browserFinishedEvent;
		MultiPeerManager.peerDidChangeStateToConnectedEvent -= peerDidChangeStateToConnectedEvent;
		MultiPeerManager.peerDidChangeStateToNotConnectedEvent -= peerDidChangeStateToNotConnectedEvent;
		MultiPeerManager.receivedRawDataEvent -= receivedRawDataEvent;
	}



	void browserFinishedEvent( string param )
	{
		Debug.Log( "browserFinishedEvent: " + param );
	}


	void peerDidChangeStateToConnectedEvent( string param )
	{
		Debug.Log( "peerDidChangeStateToConnectedEvent: " + param );
	}


	void peerDidChangeStateToNotConnectedEvent( string param )
	{
		Debug.Log( "peerDidChangeStateToNotConnectedEvent: " + param );
	}


	void receivedRawDataEvent( string peerId, byte[] data )
	{
		Debug.Log( "receivedRawDataEvent from: " + peerId + ", data.Length: " + data.Length );
	}


#endif
}


