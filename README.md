# 🎮ParallelDuad

**ParallelDuad** is an arcade-style card matching game built in Unity.  
It focuses on clean gameplay systems, efficient code structure, and smooth user feedback while keeping visuals minimal, as requested in the assignment.

---

## 🧠 Game Concept

ParallelDuad is a **card flip memory game** where players match card pairs while managing combos, accuracy, and flip count.

Performance depends on:
- Accuracy  
- Combo chains  
- Number of flips  

The game UI is designed as a single integrated screen to give a classic arcade console feel.

---

## 🕹️ Core Gameplay

- Click / Tap cards to flip  
- Match pairs to gain points  
- Each correct match increases your combo  
- Wrong matches reset (or reduce) the combo  
- Score depends on combo chain and total performance  
- Game ends when all pairs are matched  

---

## 🧩 UI Overview

### Left Panel

**1. Mode Selector**  
Allows switching between different game modes.

**2. Score Display**  
Displays the current player score.

**3. Flip Counter**  
Shows number of flips in the current session.

**4. Combo Indicator**  
Displays current combo multiplier (x0, x1, x2...).

---

### Action Buttons

| Button | Function |
|--------|----------|
| 💾 Save  | Saves current game |
| 📂 Load  | Loads last saved game |
| 🔇 Mute  | Toggle sound on/off |
| ℹ️ Info  | Opens How To Play panel |

---

### ▶️ Play Button

Located at the bottom  
- Starts a new game  
- Regenerates card layout  

---

### Board Panel

Located on the right side  
- Contains all playable cards  
- Adjusts dynamically based on screen resolution  

---

## 📖 How To Play

1. Click **Play** to start a new match.  
2. Flip two cards by tapping or clicking them.  
3. If they match:
   - Cards disappear
   - Score and combo increase  
4. If not:
   - Cards flip back
   - Combo decreases  
5. Complete the board with the highest possible combo and lowest flip count.

---

## Technical Details

| Item | Value |
|------|-------|
| Engine | Unity 6 |
| Language | C# |
| Platforms | Windows (EXE), Android (APK), WebGL |
| Frameworks | No third-party frameworks used |

---

## 🎨 Features

- Smooth card flipping animations  
- Match animation  
- Dynamic layout scaling  
- Combo-based scoring system  
- Save/Load system  
- Sound effects (flip, match, mismatch, game over)  
- Arcade style UI  

---

## 🚀 Build Instructions

### Unity

1. Clone the repository:
```bash
git clone https://github.com/ChamathNad/parallelduad.git
```
2. Open Unity Hub and load the project.
3. Open the main scene:
```
Assets/Scenes/Main.unity
```
4. Press Play▶️.

---

## 📦 Builds

| Platform | Link |
|------|-------|
| PC Build | (coming soon) |
| Android Build | (coming soon) |
| WebGL Build | (coming soon) |

---

## 🧑‍💻 Developer

ParallelDuad
- Developed by: Chamath Nadeeshan
- Location: Sri Lanka
- Game Dev | Unity Developer

---

## 📜 License

- This project was created as a technical test.
- Assets and code are not allowed for commercial redistribution without permission.
