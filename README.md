# Mixed-Reality-Dart-Game

A spatially-aware AR dart-throwing game built in Unity using the Meta XR SDK. Anchor the board on your wall, grab and throw realistic physics darts, and compete to beat your high score! 

---

## Features

- **Spatial anchoring** of the dart board on any wall 
- **20-slice scoring** plus 50-point bull’s-eye hits  
- **Persistent best score** across sessions  
- **Realistic physics darts** with velocity-based force 
- **Haptic & audio feedback** on grabs, throws, hits, and misses 
- **Start / Restart / Reset** UI controls  

---

## Game Mechanics

### Dart Board  
A traditional dart-board background is overlaid with 20 triangular models—one per slice—to detect hits. Any slice hit awards its face value; the bull’s-eye awards 50 points. Plays an impact sound on each valid hit. 

### Spatial Anchoring  
On first launch, play `welcome.mp3` and prompt the player to grab and place the board on a wall. The board’s position persists across sessions. Grabbing and moving the board automatically re-anchors it. Handled in **CustomSpatialAnchor.cs**. 

### Scoring & Timer  
Below the board are three displays:  
- **Best Score** (highest ever)  
- **Current Score** (this session)  
- **Timer** (60-second countdown)  

Gameplay stops when time reaches zero. If the player beats the best score, play a victory sound; otherwise, play a defeat sound. Managed by **ScoreManager.cs** and **TimeManager.cs**. 

### Dart Behavior  
Players begin with three darts in a holder on the left controller.  
- Grab with right controller only (light haptic feedback on pickup)  
- Apply throw force proportional to controller velocity  
- Play whoosh sound on throw (3D audio blend)  
- Stick only when the dart **tip** collides with the board  
- Auto-destroy after 5 s if they miss or hit another object  

All in **DartBehavior.cs**. 

### Dart Spawner  
**DartSpawner.cs** ensures there are always up to three darts in the holder. Darts stuck on the board aren’t counted; new darts spawn as others are thrown or expire. 

### Helper UI  
- **START GAME** button appears at launch (no scoring before start)  
- Once started, **RESTART** reloads the scene; **RESET** clears best score and anchor data, and returns board to default position ((0, 1.5, 0.5)), replaying the welcome instructions on next run. 

---

## Installation

1. **Clone** this repo  
   ```bash
   git clone https://github.com/MondayMonster/Mixed-Reality-Dart-Game.git
   cd Mixed-Reality-Dart-Game
