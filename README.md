# Cannon-Brain-Game---Game-Dev

-- Basic Game Mechanics
1. Cannon rotation direction is controlled by the mouse movement
2. A key click (S,D, and F) fires a bullet with a value (1,2 and 3) in the direction the cannon points at
3. Spawn crates with random values at random positions from the top of the screen at certain time intervals.
4. When the bullet collides with the crate, the bullet's value is subtracted from the one on the crate. The goal is to substract the crate number to a zero.
5. When the crate's number equals zero, points will be awarded and the crate is destroyed.
6. If the crate's number becomes less than zero, points will be deducted and the crate is destroyed.
7. As long as the crate's number is greater than zero, the crate will continue to move down the screen. When the crate goes below the bottom of the screen, it will be destroyed and the points will be deducted.

-- UI: Proper UI elements to represent
1. current level
2. current points score
3. remaining time
4. remaining bullets and crates
5. number on the crate
6. number on the projectile

-- Others
1. audio and graphics when level up and down
2. graphics when a crate is destroyed successfully
3. graphics when a crate value is <0
4. audio on/off button to turn on/off audio
5. pause button to pause the game
6. variation in the balloon's movement
