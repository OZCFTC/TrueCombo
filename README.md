# TrueCombo

TrueCombo is a fast-paced arcade game prototype focused on combo mechanics, timing precision, and score optimization.

The project is designed to demonstrate gameplay system architecture, clean state management, and UI-driven feedback loops implemented in Unity.

Rather than content quantity, the focus is on **system clarity, scalability, and correctness**.

---

## Core Gameplay Concept
- Players build score by maintaining uninterrupted action chains
- Mistimed actions or failures reset the combo state
- Strategic card-based buffs temporarily alter gameplay rules
- Risk–reward balance encourages aggressive but precise play

---

## Key Features
- Combo-based scoring system with multiplier logic
- Time-limited power-ups and card mechanics
- Centralized buff and duration management
- UI-driven feedback (timers, pulse effects, visual cues)
- Modular and scalable gameplay architecture

---

## Architecture Overview

### Gameplay Systems
- **Combo System**
  - Tracks consecutive successful actions
  - Dynamically adjusts score multipliers
  - Resets safely on failure or state exit

- **Buff / Card System**
  - Time-based effects (e.g. score multiplier, duration extension)
  - Centralized lifecycle control to prevent state leaks
  - Automatic reset on expiration

- **Score Management**
  - Event-driven updates
  - Decoupled from UI layer
  - Supports easy expansion (leaderboards, analytics)

---

### UI Systems
- Real-time duration bars for active effects
- Visual pulse feedback for critical timing states
- UI logic separated from gameplay logic
- Mobile-oriented readability and performance

---

## Technical Highlights
- Event-driven architecture for gameplay interactions
- Clear separation of concerns (Gameplay / UI / State)
- Lifecycle-safe state reset logic
- Scalable system design for future mechanics
- Git-based version control workflow

---

## Technologies
- Unity (2D)
- C#
- TextMeshPro
- Git / GitHub

---

## Project Scope
- Arcade game prototype
- Focused on gameplay mechanics and system design
- Built as a foundation for expansion (new cards, modes, difficulty scaling)

---

## What This Project Demonstrates
- Gameplay system design thinking
- State management and bug-resistant logic
- UI–gameplay synchronization
- Clean and maintainable Unity project structure

---

## Project Status
Active development
