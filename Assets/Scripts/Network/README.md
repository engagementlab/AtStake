Networking
==========

The scripts here handle hosting and joining games and passing messages between players.

## Hosting and joining
There are two types of connections: Wifi and Bluetooth. When the app is opened the game immediately performs a check to see if it can connect to the server.  If it can, it defaults to Wifi (which is preferable); otherwise, it enables Bluetooth.

There are a few differences in how the game performs depending on which connection type has been established, though I've tried to minimize these differences as much as possible. The game interfaces with the networking through three classes: `MultiplayerManager`, `MessageSender`, and `MessageMatcher`. 

### MultiplayerManager
The MultiplayerManager wraps the NetworkingManager class. It accepts requests to host, join, and start a game, and keeps track of the names of players in a game. It also has a few frequently refrenced properties: 

1. `Hosting` - *whether or not this device is the host*
2. `Connected` - *(Wifi) whether or not this device is connected to a host*
3. `PlayerCount` - *the number of players in the game*
4. `UsingWifi` - *false means the app is using Bluetooth*


The NetworkingManager class is a dispatcher between `ServerManager` and `BluetoothManager`- it keeps track of the connection type (Wifi or Bluetooth) and sends requests from the MultiplayerManager to the appropriate place.

### MessageSender
All messages passed between devices are sent and received through the `MessageSender`. This probably should be (*should have been* -_-) split into two classes, one for Wifi and one for Bluetooth- but oh well. There are a few different types of messages that can be passed:

1. **Messages to the host:** *only the hosting device will receive this message*
2. **Messages to the Decider:** *only the current Decider will receive this message*
3. **Messages to all:** *everyone hears this message*
4. **Scheduled messages:** *these messages are queued and only sent once the previously scheduled messages have been received*

### MessageMatcher
Regularly throughout the game it's necessary to make sure players are in agreement about what to do before advancing. This happens when selecting the first decider and when pressing 'next' buttons. The `MessageMatcher` receives the name of a message to match, and then listens as players submit their responses. When all players have submitted a response and all responses match, the `MessageMatcher` raises a "Messages Match" event.