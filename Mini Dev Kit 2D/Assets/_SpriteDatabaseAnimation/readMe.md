🌟 Sprite Database Animator
Purpose: A Unity system to drive dynamic, code-based animations using the SpriteLibrary and SpriteResolver components, playing sequential sprite series grouped by Categories.

🚀 Key Features
Dynamic Animation: Plays all sprites within a defined Category sequentially, acting as an animation clip.

Frame Rate Control: Animation speed is controlled by the frameRate variable.

Editor Tools: Custom Inspector adds quick test buttons for every available category in the assigned Sprite Library Asset (visible in the Editor).

⚙️ How to Use
Setup: Attach SpriteDatabaseAnimator.cs to the GameObject that already has SpriteLibrary and SpriteResolver components.

Configuration: Ensure your Sprite Library Asset has sequential sprites defined under unique Categories (e.g., "Idle" containing "frame0", "frame1", etc.).

Start Animation: Call the SetCategory method from an external script:

C#

// Starts playing the sprite sequence defined under the "Run" category.
GetComponent<SpriteDatabaseAnimator>().SetCategory("Run");
