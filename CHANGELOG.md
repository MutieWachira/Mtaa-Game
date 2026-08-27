# Changelog

## [0.1.0] - 2026-08-20

### Added
- Initial Unity project structure.
- Git version control.
- GameBootstrap runtime component.
- Initial GameState enum.
- First EditMode automated test.

### Testing
- GameState test passes.
- GameBootstrap startup verified in Test scene.

### Known Issues
- Player controller not implemented.
- Main menu not implemented.
- Gameplay world not implemented.

## [0.2.0] - 2026-08-20

### Added
- Unity Input System integration.
- Player input reader.
- Character Controller based player movement.
- Gravity handling.
- Temporary player capsule.
- Test ground.
- Player movement EditMode tests.

### Testing
- Keyboard movement verified.
- Controller movement verified.
- Diagonal movement normalization verified.
- Movement tests passing.

### Known Issues
- Camera does not follow player yet.
- No animations.
- No jumping.
- No Android touch controls.
- Temporary capsule used instead of final character.

## [0.3.0] - 2026-08-21

### Added
- Third-person camera controller.
- Camera target system.
- Camera orbit.
- Camera smoothing.
- Camera pitch limits.
- Camera collision detection.
- Camera-relative player movement.
- Smooth player rotation.

### Testing
- Camera orbit tested.
- Camera-relative movement tested.
- Diagonal movement tested.
- Camera collision tested.
- Vertical camera limits tested.

### Known Issues
- Camera still uses temporary legacy mouse input.
- No player animation.
- No jumping.
- No Android camera controls.
- Temporary capsule character.

## [0.4.0] - 2026-08-21

### Added
- Camera input migrated to Unity Input System.
- Sprint input.
- Jump input.
- Movement state system.
- Walking state.
- Running state.
- Jumping state.
- Falling state.
- Player animation controller architecture.
- Movement state tests.

### Changed
- Player movement now supports walk/run speeds.
- Player movement now supports jumping.
- Camera no longer directly reads legacy input.
- Player movement remains independent from input hardware.

### Testing
- Movement state tests passing.
- Walking tested.
- Running tested.
- Jumping tested.
- Falling tested.
- Camera input tested.

### Known Issues
- Character still uses temporary capsule.
- Animator is not connected to a character model yet.
- Android controls not implemented.
- Camera settings need further tuning.

## [0.6.0] - 2026-08-22

### Added
- First Mtaa playable block.
- Environment scene structure.
- Road intersection.
- Prototype buildings.
- Shop landmark.
- Player spawn point.
- Initial lighting.
- Environment layers.
- Gameplay/debug scene organization.

### Architecture
- Separated environment and gameplay objects.
- Introduced world scene organization.
- Added dedicated player spawn marker.
- Established greybox level-design workflow.

### Testing
- Player movement tested within city block.
- Camera tested around buildings.
- Road traversal tested.
- Player scale tested against environment.

### Known Issues
- Prototype geometry only.
- No NPCs.
- No traffic.
- No interactions.
- No missions.
- No economy.
- No final materials.
- No navigation system.

## [0.7.0] - 2026-08-27

### Added
- AI Navigation integration.
- NavMesh-based NPC movement.
- NPC state system.
- NPC movement state system.
- Civilian NPC prefab.
- NPC spawning system.
- Configurable NPC spawn points.
- Spawn position validation.
- Basic NPC roaming behaviour.
- NPC state tests.
- Initial NPC debug visualization.

### Architecture
- Separated NPC decision-making from movement.
- Separated NPC movement from animation.
- NPC navigation uses NavMeshAgent.
- Player continues using CharacterController.
- NPC spawning centralized through NPCSpawner.

### Performance
- NPC population is capped through configurable maximum count.
- Spawn positions are validated before instantiation.
- Expensive scene-wide object searches are avoided.

### Testing
- Navigation mesh tested.
- NPC spawning tested.
- NPC pathfinding tested.
- Destination arrival tested.
- NPC state tests passing.

### Known Issues
- NPC behaviour is currently simple roaming.
- NPC schedules not implemented.
- NPC needs system not implemented.
- NPC relationships not implemented.
- Traffic not implemented.
- NPC simulation LOD not implemented.
- NPC animation needs final integration.

## [0.7.0] - 2026-08-27
## Fixed
- Debugged player movement and camera to replicate a Triple A game.
