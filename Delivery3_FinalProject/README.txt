-----------------------------------------------------------------------------
Oriol Bech, Núria Chaves, Martí Vilaró - Sistemes Interactius
-----------------------------------------------------------------------------
Assets folder and subfolders contain all the files structured used for the project.
GAME_build contains the finalbuild of the game to be used with the corresponding sensor tracking.
(It also has a zip of the finalbuild with the tracking disabled, so it can be executed with the keyboard)
-----------------------------------------------------------------------------
[Scenes of the game]

First Scene:
Instructions of the game appear. Press the button to proceed.

Second scene (forest):
- Collect all trash from the floor and leave it in the car.
- Be aware of the snakes, if they collide with you you lose the trash you're carrying.
- Some trash can only be grabbed if the two players are colliding with it.
- If all trash is collected or 2:30minutes have passed, the next scene is loaded.

Third scene (waste deposit):
- Grab the trash from the car and leave it in the corresponding bin.
- If all trash is deposited, win scene is loaded.
- If 2:30minutes have passed and not all trash has been deposited, lose scene is loaded.

Win or Lose scene allow you to go back to First Scene.
-----------------------------------------------------------------------------
Features:
- Particles and effects:
	- In forest scene, the more objects you collect, the more vegetation appears. Vegetation = trees, plants, flowers.
	- Animated vegetation and animals.
	- Snakes do move around and steal your collected trash from you.

- Sounds:
	- Sound for grabbing, collecting, failed depositing, snake robbing or colliding with snake.
	- Ambient sounds and background music different for each scene.
- Timer Countdown at the top.
- Counter of grabbed objects.
-----------------------------------------------------------------------------
