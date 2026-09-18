Chrome-Dino-style Starter Project (Minimum deliverable for students)

Overview
- Minimal skeleton for a 2D endless runner in the style of the Chrome "Dino"
  game: the player runs to the right on its own, can jump (and double jump)
  over obstacles, and the camera follows the player.
- The level is streamed in "chunks": prefab segments (solid ground plus zero
  or more obstacles) are placed ahead of the player and destroyed once the
  player has run past them, instead of a single obstacle spawning at a time.
- `Scenes/Level 1.unity` is a fully assembled, playable scene — open it and
  press Play. Space / left click jumps; jump again in the air for a double jump.

Included scripts (location: `Example 2 - 2D Endless game/Scripts`)
- `PlayerController.cs` — Runs right at a constant speed, jumps by setting
  vertical velocity directly, and allows a second mid-air jump (`maxJumps`).
  Grounded state is checked with a downward raycast from a dedicated child
  transform (`groundedRaycastStartPoint`). Colliding with an object that has
  the `Obstacle` marker component reports to `GameManager`.
- `CameraFollow.cs` — Follows a target at a fixed offset in `LateUpdate`,
  using either `SmoothDamp` or `Lerp` easing (`followMode`).
- `Obstacle.cs` — Marker component: any collider with this attached ends the
  game on contact. Ground pieces don't have it, so the player can stand/run
  on them safely.
- `WorldChunk.cs` — Marker for a chunk placed by `ChunkSpawner`. Chunks are
  static; they don't move or destroy themselves.
- `ChunkSpawner.cs` — Spawns a random chunk prefab once the player gets
  within `spawnAheadDistance` of the end of the level built so far, and
  destroys any tracked chunk (spawned or pre-placed) once the player has run
  `despawnDistance` past it.
- `GameManager.cs` — Tracks game state (`Playing` / `GameOver`), increases
  score over time, and automatically restarts the scene `restartDelay`
  seconds after the player collides.

Included prefabs (location: `Example 2 - 2D Endless game/Prefabs`)
- `Chunk_Clear.prefab` — ground only, no obstacle (breathing room).
- `Chunk_SingleCactus.prefab` — ground + one obstacle.
- `Chunk_DoubleCactus.prefab` — ground + two closely spaced obstacles
  (may need a double jump to clear both).
- `Chunk_TallCactus.prefab` — ground + one taller obstacle.
- Each has a `WorldChunk` component, a solid `Ground` child (no `Obstacle`
  marker), and 0+ obstacle children (with the `Obstacle` marker).
- Sprites are plain colored squares (`Sprites/Square.png`) so students can
  drop in their own art without changing any code — just swap the `Sprite`
  field on the `SpriteRenderer`s.

Scene setup notes (`Scenes/Level 1.unity`)
- `ChunkSpawner` needs: the player, the chunk prefabs, and the pre-placed
  starting ground assigned to `startingChunks` (so it's tracked and despawned
  like any other chunk once the player is far enough ahead).
- `PlayerController` needs a child `Transform` assigned to
  `groundedRaycastStartPoint`, positioned at the player's feet.
- The scene must be registered in Build Settings so `GameManager.Restart()`
  (which reloads by build index) works correctly.

Suggested student extensions (assignable tasks)
- Add UI: a score display and a game-over/restart screen.
- Improve player visuals and animations (run cycle, jump, death).
- Add difficulty progression: increase `PlayerController.runSpeed` over time.
- Add more chunk variants (e.g. a pit in the ground, moving obstacles) — just
  build a new prefab with a `WorldChunk` component and add it to the
  spawner's array.
- Weight chunk selection instead of picking uniformly at random.
- Add sound effects and particles for jump, score, and death.
- Track and display a highscore.

Files added by this starter: see `Scripts/`, `Prefabs/`, `Scenes/` and `Sprites/`
folders inside this Example 2 folder.
