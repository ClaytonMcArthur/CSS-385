# Interaction Model Write-Up

## Game Summary
A top-down 2D prototype where:
- The **Player** moves using **W/A/S/D**.
- A **Slime enemy** continuously **seeks** the player’s position and moves toward it.
- Tilemap terrain constrains movement.

---

## Interaction Model & Objective

**Interaction model:**  
A simple **agent–agent** loop built from two interacting systems:

1. **Player Locomotion (direct control)**  
   - Input: keyboard (`W/A/S/D` via legacy Input Manager).
   - Mapping: raw axes → a 2D normalized direction vector.  
   - Effect: the Rigidbody2D’s velocity is set each physics tick, with a small deadzone to eliminate drift.

2. **Enemy Pursuit (autonomous control)**  
   - Perception: reads the player’s transform when within an awareness radius.
   - Decision: compute a vector to the player; if outside a small stopping distance, face and move toward the target.
   - Effect: velocity eases toward a desired velocity for smooth acceleration/stop.

**Objective:**  
Create a minimal, readable **chase/avoid** loop where player intention (movement) immediately changes enemy behavior (pursuit). This demonstrates core interactive feedback: *input → world state change → AI reaction → new player decision*.

---

## Two Examples Implemented

### Example 1 — Player Movement (Direct Control)
- **What:** Continuous 2D movement using `W/A/S/D`.
- **How:**  
  - Read `Horizontal`/`Vertical` axes, normalize to avoid diagonal speed boost.  
  - Apply exact velocity without `deltaTime`.  
  - Use a configurable deadzone to guarantee full stop when no input.

**Why it matters:** Immediate, predictable control creates tight feedback; the deadzone prevents subtle sliding.

### Example 2 — Enemy Pursuit (Autonomous Control)
- **What:** Slime rotates to face and moves toward the player until within a small stopping radius.
- **How:**  
  - Awareness controller exposes `AwareOfPlayer`, `DirectionToPlayer`, and the player’s `Transform`.  
  - Enemy computes a desired velocity toward the player, eases with accel/decel, and rotates smoothly toward its movement direction.

**Why it matters:** Demonstrates reactive AI driven by simple perception and kinematics—small, composable pieces that scale into patrol/stealth/formation behaviors.

---

## Video Demonstration

- **Video — Player Movement & Enemy Pursuit:**  
  - Link: https://www.youtube.com/watch?v=nZSUNYh1lM0

---

## How This Interaction Model Reuses Across the Game (and Why)

1. **Patrol & Guard Posts**  
   - Reuse the awareness controller but switch target source (patrol waypoints when not aware; switch to player when spotted).  
   - *Why:* Same chase kinematics, different “where to go” logic.

2. **Collectibles & Quests**  
   - Player locomotion stays identical; interactions become proximity-based triggers (e.g., pick-ups).  
   - *Why:* Consistent movement makes item interactions predictable and readable.

3. **Boss & Minion Behaviors**  
   - Minions use the same pursuit with different speed/accel; bosses add states (charge, retreat) but still rely on the same seek + stop foundation.  
   - *Why:* Parameterization scales difficulty without new systems.

4. **Stealth / Line-of-Sight**  
   - Replace radius checks with LOS raycasts or FOV cones; keep the same pursuit movement when detected.  
   - *Why:* Evolve perception while preserving tested movement code.
