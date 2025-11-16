# Media Integration Plan – Top-Down 2D RPG Dungeon Explorer

## 1. Common Uses of Media in Similar Games

In 2D top-down dungeon crawlers, media is typically used for:

- **Environment tiles & props**  
  Floors, walls, doors, pits, and decorative props (barrels, rubble, torches) using repeating tiles.

- **Character & enemy sprites**  
  4-direction walk cycles, attack, hit, death, and idle animations; sometimes separate weapon/armor layers.

- **Visual effects (VFX)**  
  Animated sprites/particles for attacks, damage, pickups, traps, and status effects (poison, burning, low HP overlay).

- **UI elements**  
  Health/stamina bars, minimap, inventory grid, hotbar icons, pause menu, and simple menu buttons.

- **Audio**  
  Ambient dungeon loops, footsteps, attack sounds, door open/close, pickup sounds, and short stingers (level clear, boss intro, death).

---

## 2. Strategies to Integrate Media + Pros/Cons

### Strategy A: Direct Scene/Prefab Assignment

Media is assigned directly in the engine editor or code for each scene/prefab (enemies, rooms, UI).

**Pros:**
- Quick to set up for prototypes and small projects.  
- Easy to tweak visually (drag-and-drop sprites/audio in the editor).

**Cons:**
- Lots of **duplication** across scenes/prefabs.  
- **Refactors are painful** if asset paths change.  
- Easy to end up with inconsistent or broken references.

### Strategy B: Data-Driven Asset Registry

Use a central registry (e.g., JSON/scriptable objects) mapping IDs (e.g., `enemy.slime`) to sprite, VFX, and audio paths.

**Pros:**
- **Single source of truth** for core assets.  
- Easy to **reskin** or update art/audio by changing data.  
- Scales better as you add more enemies, items, and media.

**Cons:**
- Needs initial design (IDs, structure).  
- Slightly more complex to debug (missing IDs instead of obvious missing sprites).

---

## 3. Chosen Approach for My Game

I’ll use a **hybrid approach**:

1. **Data-driven for core entities**  
   - Enemies and items will use IDs (e.g., `enemy.slime`, `item.health_potion`) tied to sprite sheets, sounds, and optional VFX via a registry.  
   - This supports easy expansion and swapping placeholder art with final assets.

2. **Direct assignment for dungeon themes**  
   - Each dungeon scene will directly reference its tileset and music/ambient audio.  
   - This keeps level-building simple and visual for me.

**Why this fits my game:**  
My project is small but content-heavy (multiple rooms, enemies, and items). The hybrid model keeps setup simple now while preventing a mess as I add more media later, balancing quick iteration with maintainability.
