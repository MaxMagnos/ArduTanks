# ArduTanks
This project was created by Max Busch as an assignment for the CGL BA4 Hardware Programming Class.
It consists of a Unity game (ArduTanks) as well as a custom built arcade machine including a control-panel powered by an Arduino Uno R4 Wifi that is required for the input. **Control methods other than these are not supported**


### [YouTube Short about project creation](https://youtube.com/shorts/AtszOTIaLDM?si=Mo6oWbbg-AOPVxbF)
<a href="https://www.youtube.com/watch?v=AtszOTIaLDM?si=Mo6oWbbg-AOPVxbF">
  <img src="_ReadMe%20Media/TiktokThumbnail.png" alt="Watch on YouTube" width="400"/>
</a>

# Hardware Used
- Arduino Uno R4 Wifi
- Intel N100 Mini-PC
- Portable 15.6" Monitor (1920x1080)
> 2 Each of the following:
- Rotary Encoder
- Sliding Potentiometer
- Toggle Switch
- Red LED

# Wiring Diagram
This diagram shows the wiring required for the project to work.
Note that to improve readability of the diagram only one pair of parts is connected. For the game to work a second pair needs to be connected, using pins that were purposely left free.

<img src="_ReadMe%20Media/WiringDiagram.png" alt="Wirind Diagram" width="800"/>


# Demo Video
[Watch the demo video](_ReadMe%20Media/ArduTanksDemo.mp4)

# Game Explanation
The game is a simple 2 player game based on shooting each other with tanks.
Main point of interest is the rather complicated control setup which is purposely unintuitivie, making the simple task of even hitting each other already a fun challenge.

## Controls
- Rotate Tank -> Rotary Encoder
- Rotate Head -> Potentiometer
- Fire -> Click Rotary Encoder
- Move -> Slide Potentiometer (NOTE: Mode-Switch needs to be set "up")
- Adjust shot distance -> Slide Potentiometer (NOTE: Mode-Switch needs to be set "down")

NOTE: Firing has a cooldown to prevent spamming. The LEDs show wether the cooldown has ended (LED ON = Ready to fire)
