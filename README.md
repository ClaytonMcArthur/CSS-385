# One-Pager: Performance Plan — Cipher of the Deep (Top-Down 2D Dungeon RPG)

## 1) Likely performance pain points
1. **Screen drawing load**  
   When lots of tiles, effects, and transparent sprites are on screen, the game ends up “drawing” the same pixels many times. Switching between many small images also slows things down.

2. **Collisions and pathfinding growing fast**  
   If every enemy checks itself against everything nearby and constantly searches for paths through a detailed grid, work adds up quickly as the room gets busier.

3. **Too many updates and short-lived objects**  
   Updating every enemy every frame and repeatedly creating/destroying things like projectiles and particles can cause hiccups and brief slowdowns.

---

## 2) Common ways to fix or reduce these

### A. Rendering (what’s drawn on screen)
- **Group images together** so the game switches less between files, and **draw only what the camera can see** by splitting the map into chunks.
- **Keep layers simple** where possible and limit heavy full-screen effects; use tools to spot areas that are being redrawn more than needed.

### B. Collisions & Pathfinding (movement and “can I hit this?”)
- **Check nearby areas first** using a simple grid so characters don’t test against the whole room.
- **Use quick “box” checks** before any precise checks.
- **Plan routes on a coarser grid** and **limit how often paths are recalculated**, reusing recent results when possible.

### C. Updates & Object Lifetimes (what runs each frame)
- **Update off-screen or low-priority enemies less often** and pause distant rooms.
- **Reuse common objects** (like bullets and particle effects) instead of creating new ones each time.
- **Run movement/physics on a steady rhythm** and smooth the visuals in between to keep frames even.

---

## 3) How I’ll apply this to Cipher of the Deep
**Goal:** Stay near **120 FPS (8.3 ms)** on my dev machine and **60 FPS (16.7 ms)** on mid-range laptops while moving through a 4-room dungeon with ~12 active enemies and light effects.

### A. Rendering plan
- Use **one image atlas per theme** (UI, characters, environment).
- Split the tilemap into **32×32-tile chunks** and **draw only the chunks near the camera**.
- **Limit on-screen particles to about 60**; favor small per-sprite glows over heavy full-screen effects.  
**Success check:** Screen drawing time under ~5 ms in the test scene, with at least **50% fewer draw calls** than the starting point.

### B. Collision & pathfinding plan
- Use a **simple grid** sized to the biggest enemy so checks stay local.
- Only check **the current cell and the 8 neighbors** for collisions.
- Do pathfinding on a **coarser grid** and **space out recalculations** (about once every 0.25 s per enemy; at most two path jobs per frame).  
**Success check:** AI work usually under ~3 ms, with short spikes under ~6 ms when things get busy.

### C. Updates & churn plan
- **Slow down updates** for things off-screen (~10 times per second) and keep on-screen updates between **30–60 times per second** depending on load.
- **Reuse** projectiles, damage numbers, and common particle effects from pools.
- Run movement/physics on a **steady step** (around 50–60 times per second) and **smooth visuals** between steps.  
**Success check:** Memory growth under **~0.5 MB per minute** and **no visible stutters** from cleanup.
