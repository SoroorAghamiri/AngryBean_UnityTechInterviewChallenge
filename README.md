# AngryBean_UnityTechInterviewChallenge
Welcome to the Angry Bean game! Where you are a little cute angry bean fighting the crusher to save all other beans beyond the mountains. Shoot the crusher before it shoots you dead or crushing you. But also be careful! The battlefield is a dangerous place. Stay away from the root fires.

## How-To-Play
Use WADS keys to move forward, left, right and backwards. Hold left shift key while moving to run.<br />
Space key makes you jump.<br />
Moving the mouse helps you looking around the field. To aim, keep holding the right mouse button and use left mouse button to shoot.<br />
Pay attention to the notification message on top right corner. The crusher will chase you down and shoot you and will jump and crush you if you are too close to it. Try to shoot it down while avoiding the bullets.<br />
Pay attention to the field. You need to jump over the hills to keep moving smoothly and avoid or jump over the root fires to stay alive.

## Additional-Features
A Particle system is added to the root fires, simulating the smoke.<br />
Upon crusher's jump over the bean's head, the bean will shrink and grow, giving a sense of animated crushing.<br />
When either of the characters get shot, a particle system is instantiated and played to deliver the sense of being shot.<br />
Aiming and running is added as an extra feature to the character, allowing it to move faster and to shoot precisely.<br />
A main menu is presented in the beginning, forwarding the player to the game and allowing it to quit the game.<br />
A notification text is displayed on top left corner that warns the player when they are getting too close to the bean or when they are being chased down. Another text on top right corner shows the health value for both the player and crusher, informing the player of their progress in the game.<br />

## Issues-And-Bugs
When the player is crushed, the camera is supposed to show how the animation of player being shrunk and growing, however in some special rotations this feature does not work which must be corrected by addressing the rotation of camera during the "crush" act.<br />
When the bean is placed within the crushing range of the crusher, crusher attemps to jump over the bean. However, when it fails, it does not continue with moving or shooting, causing it to stand still. This bug can be fixed by randomizing the next act after a jump failure.<br />
