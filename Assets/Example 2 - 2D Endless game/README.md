Chrome-Dino-style Starter Project (Minimum deliverable for students)

Overview
- Minimal skeleton for a 2D endless runner in the style of the Chrome "Dino"
  game: the player runs to the right on its own, can jump (and double jump)
  over obstacles, and the camera follows the player instead of the world
  scrolling past a static camera.
- Built to match the code patterns from the "3. Unity Basics II" lecture:
  a hybrid physics controller (set `linearVelocity` directly, lecture
  "Moving Rigidbodies: set velocity"), a `LateUpdate`/`SmoothDamp` camera
  follow (lecture "3.1 Camera Follow"), a central `GameManager` driving game
  state, and prefab-based level streaming.
- The level is streamed in "chunks": pre-built prefab segments (solid ground
  plus zero or more obstacles) are placed ahead of the player and destroyed
  once the player has run past them, instead of a single obstacle spawning
  at a time.
- `Scenes/Level 1.unity` is a fully assembled, playable scene — open it and
  press Play. Space / left click jumps; jump again in the air for a double jump.

Included scripts (location: `Example 2 - 2D Endless game/Scripts`)
- `PlayerController.cs` — Constantly runs right (`rb.linearVelocity.x` fixed
  to `runSpeed` in `FixedUpdate`), jumps by setting `linearVelocity.y`, and
  allows a second mid-air jump (`maxJumps`). Grounded state is checked with a
  downward raycast (same pattern as Example 1's `Player.IsGrounded()`).
  Colliding with an object that has the `Obstacle` marker component reports
  to `GameManager`.
- `CameraFollow.cs` — Follows a target at a fixed offset using
  `Vector3.SmoothDamp` in `LateUpdate`, straight from the lecture's Camera
  Follow example.
- `Obstacle.cs` — Marker component (same idea as Example 1's `KillZone.cs`):
  any collider with this attached ends the game on contact. Ground pieces do
  **not** have it, so the player can stand/run on them safely.
- `WorldChunk.cs` — Attached to each chunk prefab's root. The chunk itself
  doesn't move (the player does); it just destroys itself once the player has
  run far enough past it.
- `ChunkSpawner.cs` — Picks a random chunk prefab and places it immediately
  after the last one once the player gets within `spawnAheadDistance` of the
  end of the level built so far, so ground tiles edge-to-edge with no gaps.
- `GameManager.cs` — Tracks game state (`Playing` / `GameOver`), increases
  score over time, keeps an in-memory (not persisted) highscore, and
  automatically restarts the scene `restartDelay` seconds (default 2) after
  the player collides.

Included prefabs (location: `Example 2 - 2D Endless game/Prefabs`)
- `Chunk_Clear.prefab` — ground only, no obstacle (breathing room).
- `Chunk_SingleCactus.prefab` — ground + one obstacle.
- `Chunk_DoubleCactus.prefab` — ground + two closely spaced obstacles
  (may need a double jump to clear both).
- `Chunk_TallCactus.prefab` — ground + one taller obstacle.
- Each is 6 units wide with a `WorldChunk` component, a solid `Ground` child
  (no `Obstacle` marker), and 0+ `Cactus` children (with the `Obstacle`
  marker). All four are already assigned to the `ChunkSpawner` in the scene.
- Sprites are plain colored squares (`Sprites/Square.png`) so students can
  drop in their own art without changing any code — just swap the `Sprite`
  field on the `SpriteRenderer`s.

How the scene is set up (`Scenes/Level 1.unity`)
- `Player` — SpriteRenderer, `Rigidbody2D` (gravity scale 1, rotation frozen),
  `CircleCollider2D`, `PlayerController`.
- `Main Camera` — Orthographic, size 5, positioned slightly ahead of the
  player (so more of the upcoming level is visible) with `CameraFollow`
  targeting the player.
- `GameManager` — empty GameObject with `GameManager`.
- `StartingGround` — a fixed 12-unit-wide ground platform under the player's
  spawn point, so there's solid footing before the first procedural chunk.
- `ChunkSpawner` — positioned at the right edge of `StartingGround`, with the
  player, all four chunk prefabs, and `chunkWidth = 6` assigned.
- The scene is registered in Build Settings so `GameManager.Restart()` (which
  reloads by build index) works correctly.

Suggested student extensions (assignable tasks)
- Add UI (score text, highscore, restart button) following lecture Section 8:
  a `Canvas` in Screen Space - Overlay, a `TextMeshProUGUI` bound to the score,
  and a `Button` wired to `GameManager.Restart()`. `GameManager.OnPlayerCollided()`
  already has a commented-out `uiManager.ShowGameOverScreen(...)` call as a hook.
- Improve player visuals and animations (run cycle, jump, death).
- Add difficulty progression: increase `PlayerController.runSpeed` over time.
- Add more chunk variants (e.g. a pit in the ground, moving obstacles) — just
  build a new prefab with a `WorldChunk` component and add it to the
  spawner's array.
- Weight chunk selection instead of picking uniformly at random.
- Add sound effects and particles for jump, score, and death.
- Award bonus score for narrowly clearing an obstacle using a trigger zone
  that calls the existing `GameManager.AddScore(amount)` hook.

Files added by this starter: see `Scripts/`, `Prefabs/`, `Scenes/` and `Sprites/`
folders inside this Example 2 folder.
