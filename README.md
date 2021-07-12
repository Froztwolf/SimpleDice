# SimpleDice
Making simple configurable dice that can be rolled using physics, and automatically detect which face was rolled give their orientation when they stop.
- Done: Use arbitrary amounts of D6 and get a tally of values
- Done: Configurable values per face
 - TODO: Custom Inspector for Dice 
- Done: Allow re-rolling invalid or selected dice only
 - TODO: Roll invalids more sideways to resist clumping  
- Done: Optional and prefab-based UI with buttons for rolling and text that shows the dice values
 - TODO: Non-gizmo based Invalidity and Selection feedbacks
- Done: Create placeable dice-spawner prefabs instead of using the DiceMgr for locating the dice
 - TODO: Spawning FX
 - TODO: Scene-count of expected and current dice 
 - TODO: Allow freezing of XY coordinates
 - TODO? Spawnerless options?
- Done: D4 and D6 with proper art pipeline
 - TODO: Fix normals on symbols layer 
- TODO: Support a mix of dice types
- TODO: Collision Audio
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
- Original rolled face detection method has serious edge-cases that can't be solved. Must be replaced.
 - Investigating hand-crafted vectors, but 3D models don't seem to fit the mathematics. Needs further investigations
- Value update is only sent if there's a UI. Should always be sent if there's any listeners.  

![image](https://user-images.githubusercontent.com/46853782/124386994-ec2f3580-dcaa-11eb-8626-88554fb27b4f.png)
