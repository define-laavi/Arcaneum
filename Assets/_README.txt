Hello!

If you are looking at this readme that means you must be trying to use the Arcadeum project.
If so, here are some tips on how to use it.

Without coding path:
Currently in Arcadeum there's only one project "Asteroids", so you mostly can change design parts of that game.
Almost everything is available to you on the "Asteroids" scene in "Managers" GameObject.
There you will see three main scripts that will enable you to edit almost all the parameters in "Asteroids".
First and foremost - there's Gameplay:
Max lives - speaks for itself, set's the players starting lives value, all of the other things happen automatically, so you just have to set the value.
If you would like to change how the lives are display there's a header named "Life Display" the objects there are responible for displaying life data to user.
Life holder is a rect transform, and both life and lost indicators are prefabs.
Then you have "Destruction" which is responible for handling that blast effect that you see when the player dies. Of course you can change it too:
Player destrution blast is the particle system for, well, blast. 
Time of blast is how long it will take for the blast to finish in seconds. 
Time after blast is how much time will the screen be empty before respawning the player. 
And lastly, Image Fill Percentage shows how much of the texture used for the blast is the actual blast and not empty space.

You can also change asteroid point value with SharedInt in the AsteroidValueReference. Just open the scriptable object and change it's value inside. Warning:changing this shared int for some other won't actually change anything, it's just a quick reference to not look inside folders.

Last thing you can change here is the SpaceshipPrefab. More on spaceship later.

Next we have Simple Asteroid Spawner.
The values there are pretty straightforward. You have time between asteroids spawn (they spawn one after one). Time to start spawning (when entering the scene or after respawn it will wait thi time to start spawning the asteroids). And, Asteroid Variants, which are Asteroid prefabs. They are taken randomly from this list with equal likelyhood, so if you want to make something more probable, for now you will have to add more of the same prefab there.

At last, the Score and TopScoresDisplay:
Here you can change players count (N), but most importantly, where the current, highest, and topN scores are displayed;

Other parameter's:
You can change spaceship movement variables, death sound, and actions in it's prefab. The template on how to setup actions trails, and thruster is provided in "prefab_exampleShip". Additionally, you can change recoil and bullet parameters if you go into the action definition on the aforementioned prefab.

You can also change asteroid movement parameters, death sound and particle in their respectful prefab's. Each prefab has their own parameter's the only one that's shared is the score.

And that's mostly it, there are some other parameters but they are mostly straightforward. If you have further questions, feel free to contact me at mateusz.gawlik.kaluzny@gmail.com.
