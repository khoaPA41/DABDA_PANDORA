# UNITY PROJECT : DABDA PANDORA
> Indevelopment

> A 3D single-player - adventure - puzzle game developed with Unity and C#.

## 🎮 Demo
- [Download](https://pakbot4124.itch.io/dabda-pandora)
- [Gameplay Video](https://youtu.be/3XokwoHEwiI)
- [Play Demo](https://pakbot4124.itch.io/dabda-pandora-web)
## 📖 Overview
DABDA PANDORA is a 3D action, adventure, puzzle game developed with Unity and C#.

The project focuses on implementing the core gameplay systems, including
player movement, puzzle, overcoming obstacles, enemy behavior, game progression and multiple gameplay mechanics.

This project was developed as a portfolio project to demonstrate my
Unity development and C# programming skills.

## ✨ Features

- Multiple gameplay styles and mechanics.
- Dynamic Camera System — Designed and implemented trigger-based camera transitions for different gameplay sections.
- Item Interactions.
- Dynamic gameplay progression

## 🛠️ Technical Highlights
### Engine & Tools
- Unity 6.3 (6000.3.8f1)
- C#
- Unity Input System
- Universal Render Pipeline (URP)
- Animator
- Timeline
- Unity UI (Canvas)
### Gameplay & Architecture
- Multiple gameplay mechanics - Different sections introduce unique gameplay mechanics and challenges.
- Trigger-based gameplay events - Used to control gameplay transitions between sections.
- Dynamic camera transitions - To change perspective based on gameplay.
- State Machine Pattern - Organizes gameplay behaviors into independent states for clearer and more maintainable logic.
- Object Pooling Pattern- Used to efficiently reuse frequently spawned objects.
- ScriptableObject-based Data** — Used to separate gameplay data from runtime logic.
- Timeline & Signals** — Used to orchestrate cinematic sequences and gameplay events.

## 🎯 Controls

| Action   | Input     |
|----------|-----------|
| Move     | WASD      |
| Jump     | Space     |
| Run      | Left Shift|
| Crouch   | C         |
| Interact | E         |
| Shoot    | Left Mouse|

## 📂 Project Structure

```text
Assets/
├── Script/
│   ├── Attack/          # Combat systems
│   ├── Camera/          # Camera systems and transitions
│   ├── Design Pattern/  # State Machine and Object Pool
│   ├── Dragon/          # Dragon gameplay
│   ├── Input/           # Input handling
│   ├── Interact/        # Interaction systems
│   ├── Managers/        # Global/gameplay managers
│   ├── Obstacle/        # Environmental obstacles
│   ├── Physics/         # Physics systems
│   ├── Settings/        # Game and graphics settings
│   ├── UI/              # UI systems
│   └── Zombie/          # Zombie gameplay
│
├── ScriptableObject/
│   └── Enemy/           # Enemy data and configuration
│
├── Settings/            # URP and rendering configuration
├── Timeline/            # Cutscene sequences
├── Prefab/              # Reusable game objects
└── Scene/               # Game scenes
```
## 🚀 How to Run

1. Clone the repository.
2. Open it with Unity 6000.3.8f1.
3. Open the main scene.
4. Press Play.

## 👨‍💻 Developer

Phạm Anh Khoa        
