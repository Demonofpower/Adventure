# Adventure
This is a custom server implementation of the play-to-hack game [Pwn Adventure 3: Pwnie Island](https://www.pwnadventure.com/)  
The server is inside folder Adv.Server, everything other is protocol analysis related.

# Progress
The implementation of the protocol and backend is quite advanced.  
Currently you can go ingame and explore the world.

#Setup
Insert
<Server host IP> master.pwn3
<Server host IP> game.pwn3
Into C:\Windows\System32\drivers\etc\hosts
  
## MasterServer
![Progress](https://progress-bar.dev/80/?title=MasterServer)  
- Handshake :heavy_check_mark:
- Login :heavy_check_mark:
- Register :heavy_check_mark:
- Character :heavy_check_mark:
  - Creation :heavy_check_mark:
  - Choosing :heavy_check_mark:
  - Deletion :heavy_check_mark:
- Join game :heavy_check_mark:
- Validate token/session :x:
- Questsystem :x:
- Flagsystem :x:

## GameServer
![Progress](https://progress-bar.dev/35/?title=GameServer)
- Handshake :heavy_check_mark:
- Join game :heavy_check_mark:
- Move :heavy_check_mark:
- Jump :heavy_check_mark:
- Chat :heavy_check_mark:
- Enable PvP :heavy_check_mark:
- See others :heavy_check_mark:
- Pickup items :white_check_mark:
- Everything else :) :x:
