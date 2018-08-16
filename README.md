== 3rd Party Products Used
* 3D Game Kit
* AllStarCharacterLibrary Superhero Construction Kit
* PARTIAL Simple Vector Icons (UI_Icon_Aim in icons folder)
* PARTIAL Virtual Joystick Texture Vol1 (Joystick_04A in icons folder)
* PARTIAL UIPlatformer (Climbing animations in animations folder)

== Movement
* To move correctly, floors, stairs, and similar objects should be placed on the Terrain layer.
* Other collidable objects (ones the player does not walk on) should be placed in the Level layer.
* Climbable objects should use the Climbable tag.

== MovementPad
* Requires an Event System in the project.
* Requires that some other component call its methods to get and process user input. This will probably be the PlayerController.

== Known Bugs
