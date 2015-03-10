# Server
Players connect to an installation of Unity's Master Server at `54.149.47.87`. [Some edits were made](http://answers.unity3d.com/questions/429957/unity-master-server-ubuntu-build-problem.html) to the Master Server, so if you need to install it on a new server, use the two zip files here rather than the ones hosted on Unity's website. 

#### Setting up a server (recommended steps)
**1.** Use Amazon Web Services to set up an EC2 instance & associate an elastic IP

**2.** Upload the Master Server and Facilitator zips to the EC2 instance and unzip 'em

**3.** In the game, set the new IP address and ports [here](https://github.com/engagementgamelab/AtStake/blob/master/Assets/Scripts/Settings/ServerSettings.cs). (You probably won't need to change the ports)

#### Starting the server
**1.** You must have the SSH key `atstake.pem`

**2.** In the Terminal: 
```
chmod 400 atstake.pem
```

**3.** Open two Terminal windows and in each of them enter: 
```
ssh -i atstake.pem ec2-user@54.149.47.87
```

**4.** In the first window, start the Master Server: 
```
cd MasterServer-2.0.1f1
./MasterServer
```

**5.** In the second window, start the Facilitator: 
```
cd Facilitator-2.0.1f1
./Facilitator
```
