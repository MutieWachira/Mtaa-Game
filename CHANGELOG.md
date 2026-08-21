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