# SimpleDice
Making simple configurable dice that can be rolled using physics, and automatically detect which face was rolled give their orientation when they stop.
- Done: Use arbitrary amounts of D6 and get a tally of values
- Done: Configurable values per face
 -- TODO: Custom Inspector for Dice 
- Done: Allow re-rolling invalid or selected dice only
 -- TODO: Roll invalids more sideways to resist clumping  
- Done: Optional and prefab-based UI with buttons for rolling and text that shows the dice values
 -- TODO: Non-gizmo based Invalidity and Selection feedbacks
- Done: Create placeable dice-spawner prefabs instead of using the DiceMgr for locating the dice
 -- TODO: Scene-count of expected and current dice 
 -- TODO: Allow freezing of XY coordinates
- Done: D4 and D6 with proper art pipeline
 -- TODO: Fix normals on symbols layer
 -- TODO: LoDs
- TODO: Support a mix of dice types
- TODO: Collision Audio
- TODO: VFX
- TODO: Results Logging
- TODO: Test using the assembly in a different project
- TODO: Write Unit-Tests
- TODO: Design showcase scenes and add them to this list
- TODO: Allow drag-and-drop customization of dice faces (investigate using texture arrays and splatting)
- TODO: Make high-quality textures
- TODO: Optimize Rendering
- TODO: Create D2, D8, D10 (single and double-digit), D12 and D20
- TODO: Get some user feedback
- TODO: Write documentation
- TODO: Package for Unity Asset store

Issues to fix:
- Value update is only sent if there's a UI. Should always be sent if there's any listeners.  

![image](https://user-images.githubusercontent.com/46853782/126040638-17b4133d-5818-4cbd-b1a3-51a5974a1180.png)
