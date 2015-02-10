@Stake
=======

@Stake is a game that fosters democracy, empathy, and creative problem solving for civic issues. Players take on a variety of roles and pitch ideas under a time pressure, competing to produce the best idea in the eyes of the table's “Decider.” 

This is a mobile (iOS & Android) version of the [tabletop game](http://engagementgamelab.org/games/@stake/) designed by the [Engagement Lab](http://elab.emerson.edu/) at Emerson College.

## Developers
This is a Unity3D 4.6 project. Clone the repository and open the project in Unity to make edits.

Players connect to an installation of Unity's Master Server at `54.149.47.87`. [Some edits were made](http://answers.unity3d.com/questions/429957/unity-master-server-ubuntu-build-problem.html) to the Master Server.

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