# One-Pager: Performance Plan — Cipher of the Deep (Top-Down 2D Dungeon RPG)

## 1) Likely performance pain points
1. **Rendering pressure (overdraw + draw calls)**  
   Dense tilemaps, layered sprites/FX, and transparency drive overdraw; many materials/textures increase draw calls. Use batching/atlases and watch overdraw hotspots. :contentReference[oaicite:0]{index=0}

2. **Collision & pathfinding scale**  
   Naive collision checks and frequent A* on fine grids scale poorly; broad-phase spatial partitioning (grid/quadtree) and coarser nav graphs reduce work. :contentReference[oaicite:1]{index=1}

3. **Update frequency & object lifetime churn**  
   Ticking every entity every frame and constantly allocating/destroying short-lived objects (projectiles/FX) causes CPU & GC spikes; object pooling and fixed-timestep simulation stabilize performance. :contentReference[oaicite:2]{index=2}

---

## 2) Common mitigation strategies

### A. Rendering
- **Sprite batching + texture atlases** to cut draw calls; **tilemap chunking + camera/frustum culling** to avoid drawing unseen tiles; audit **overdraw** with tooling and prefer opaque layers where possible. :contentReference[oaicite:3]{index=3}

### B. Collision & Pathfinding
- **Spatial partitioning** (uniform grid or quadtree) for broad-phase; **AABB first** before narrow-phase; **coarser nav-graphs** and **path budgets** (cap nodes/frame, reuse paths). :contentReference[oaicite:4]{index=4}

### C. Updates & Object Lifetimes
- **Tick decimation/LOD** for off-screen actors; **object pooling** for projectiles/FX; **fixed timestep** physics with visual interpolation to smooth frames. :contentReference[oaicite:5]{index=5}

---

## 3) How I’ll apply this to Cipher of the Deep
**Goal:** Hold **120 FPS (8.3 ms)** on my dev machine and **60 FPS (16.7 ms)** on mid-range laptops while traversing a 4-room dungeon with 12 enemies + light FX.

### A. Rendering plan
- One **atlas per theme** (UI/characters/environment).  
- Convert tilemap to **32×32 tile chunks**; render only chunks in/near camera.  
- Cap **particles ≤ 60**; prefer per-sprite glow over full-screen post.  
**Acceptance:** GPU frame < 5 ms; **≥50%** fewer draw calls vs baseline. :contentReference[oaicite:6]{index=6}

### B. Collision & pathfinding plan
- **Uniform spatial grid** (cell ≈ largest enemy).  
- Query only 9 neighboring cells; **A\*** on **coarse nav-grid** (e.g., 1 node per 2×2 tiles).  
- **Budget:** ≤1 recompute/enemy every 0.25 s; global cap **2 A\*** jobs/frame.  
**Acceptance:** AI step < 3 ms; spikes < 6 ms during swarms. :contentReference[oaicite:7]{index=7}

### C. Updates & churn plan
- **Decimate updates**: off-screen at ~10 Hz; on-screen at 30–60 Hz under load.  
- **Pool** projectiles, damage text, particle emitters.  
- **Fixed timestep** (e.g., 50–60 Hz) + visual interpolation.  
**Acceptance:** Allocation < 0.5 MB/min; **no GC spikes > 2 ms**. :contentReference[oaicite:8]{index=8}
