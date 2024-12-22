# Jump Pad Mayhem

**Jump Pad Mayhem** is an action-packed game where the objective is to make a ball jump between all the jump pads in the scene, forming a continuous loop. The game involves precise control and timing to create a perfect loop using various gameplay mechanics, including level generation, trajectory prediction, and dynamic ball interactions with jump pads.

## Table of Contents

1. [Features](#features)
   - [Level Generation](#level-generation)
   - [Trajectory Simulation](#trajectory-simulation)
   - [Game Flow](#game-flow)
2. [Coding Architecture](#coding-architecture)
3. [Recordings](#recordings)

---

## Features

### Level Generation

The game utilizes **dynamic level generation** to create diverse grid layouts, where **jump pads** and other elements are spawned in random positions with varying orientations. The grid is configurable, and elements such as **cannons** and **jump pads** can be customized with different properties for each level.

- **Grid Setup**: A grid is dynamically initialized, and elements (such as **CANNON** and **JUMPPAD**) are spawned at specific slots within the grid.
- **Variation Data**: Each level can have different configurations based on **VariationData**, such as the number of projectiles, jump pad force, and other environmental settings.
- **Modular Level Design**: Each level's grid and environment can be customized, making each gameplay session unique and challenging.

### Trajectory Simulation

**Trajectory simulation** is key to understanding how the ball moves between jump pads. The game visualizes the ball's path using a trajectory simulation system, which shows how the ball will travel based on its current speed, direction, and trajectory.

- **Trajectory Line**: The ball’s projected path is visualized using a **LineRenderer**, simulating the ball's movement through the environment. This helps players plan their jumps to successfully complete the loop.
- **Physics-based Simulation**: A dedicated physics scene is used for simulating the ball's motion and interactions with jump pads, ensuring accurate calculations and predictions.

### Game Flow

The **game flow** is structured around the player's ability to navigate through jump pads to complete a loop. The flow includes:

- **Ball Firing**: The ball is fired from a cannon, either manually or automatically, based on the player’s input.
- **Jump Pad Interactions**: As the ball collides with jump pads, it is launched towards the next pad, and the player must control the ball's trajectory to reach the correct pad in sequence.
- **Loop Completion**: Once the ball successfully completes a loop across all pads, the game triggers a victory state.
- **Game Over**: If the ball fails to land on a valid pad or the loop is not completed, the game transitions to a failure state.

---

## Coding Architecture

**Jump-Pad-Mayhem** incorporates a well-structured coding architecture and design patterns that enhance the game's organization, performance, and gameplay experience.

### Object Pooling

**Object pooling** is used for managing projectiles, particularly the ball and cannon projectiles, which are frequently instantiated and destroyed during gameplay. This optimization technique helps improve performance by reusing objects rather than creating new ones each time a projectile is fired.

- **Projectile Pool**: The **Pool<T>** class is used to store a set of projectiles (balls) that can be reused. This prevents unnecessary memory allocation and improves the game's performance, particularly during intense gameplay sequences where multiple projectiles may be fired at once.

### Singleton Patterns

The **Singleton** design pattern is applied to key game management systems that require global access throughout the game.

- **GameManager**: Manages overall game state, progression, and interaction between different gameplay systems. It tracks the ball's progress, handles game win and fail states, and controls the flow of the game.
- **TrajectorySimulationSceneManager**: Manages the dedicated simulation scene where the trajectory of projectiles is calculated and visualized. The singleton ensures that only one instance of the simulation scene is created and maintained during gameplay.
- **CreativeConfig**: Stores and manages the configuration data for various game elements (like jump pads, cannons, and variations) to ensure consistency across levels and gameplay modes.

### Observer Pattern

The **Observer** pattern is utilized for handling events and communication between different game components, promoting loose coupling and easier management of game state changes.

- **GameplayEvents**: The **Observer** pattern is applied through events like **OnGameFail**, **OnGameWon**, and **OnBallIsLooping**. These events allow components like **Cannon**, **Ball**, and **GameManager** to react to changes in the game state without being directly dependent on each other. For example, when the ball loops through all jump pads, the **GameManager** listens for the **OnBallIsLooping** event and triggers the win condition.

### Abstract Classes

The use of **abstract classes** allows for a flexible and extensible design. Common behaviours are defined in base classes, with specific element types inheriting and extending these behaviours.

- **GhostObject**: This abstract class defines the common behaviour for all ghost-like objects (e.g., ghost projectiles or ghost jump pads). Derived classes like **JumpSurface** and **Projectile** implement specific logic for their respective behaviours, but they all share the basic functionality of being "ghosted" or inactive in the main game scene.
- **GhostSlotElement**: This class is used for grid elements in the **LooperGrid**. It provides the base functionality for configuring slot elements (like jump pads or cannons) and ensures that each element can be placed, rotated, and positioned within the grid.

### Template Classes (Generics)

**Template classes** (or **generic classes**) provide a flexible and reusable approach to managing different types of objects.

- **Pool<T>**: The **Pool<T>** class is a generic class used for object pooling, allowing for the efficient reuse of game objects, such as projectiles. This class is able to work with any type of object, making it versatile and efficient across different game systems.
- **GenericConfig<T>**: The **GenericConfig** class is a base template for managing configuration data. The **CreativeConfig** class inherits from this template, enabling the management of various configurations (such as grid data, jump pad properties, and cannon configurations) across the game.

### Factory Pattern

The **Factory** pattern is used to create different types of game elements (such as cannons, jump pads, etc.) dynamically during level generation. This ensures that new elements can be added easily without modifying the core logic.

- **Element Factory**: The **LevelGenerator** uses a simplified factory method to spawn different types of game elements (like **CANNON** and **JUMPPAD**) based on the current variation data. The factory pattern helps decouple the element creation logic from the rest of the game, promoting flexibility and ease of maintenance.

---

## Recordings

**_Click on GIF To view Enchanced Version_**

1. Level Generator
   <br>
   [![Video](https://github.com/Bhawesh02/Jump-Pad-Mayhem/blob/main/GIFS/LevelCreator.gif)](https://youtu.be/FOFXl6VMl-A)
   <br>
2. Trajectory Simulation Testing
   <br>
   [![Video](https://github.com/Bhawesh02/Jump-Pad-Mayhem/blob/main/GIFS/Trajectory%20Line.gif)](https://youtu.be/uk4Y_k5YZRk)
   <br>
<!-- 3. Gameplay
   <br>
   [![Video](https://github.com/Bhawesh02/Jump-Pad-Mayhem/blob/main/GIFS/Gameplay.gif)](https://youtube.com/shorts/zajEYe4CAg0)
   <br> -->

---

## Contribution

This project represents my individual development efforts, where I solely created the code base for a game creative in Kwalee, with art being provided by various colleagues. Feedback, suggestions, and collaborative contributions are highly encouraged.

---

## Contact

You can connect with me on LinkedIn: [Bhawesh Agarwal](https://www.linkedin.com/in/bhawesh-agarwal-70b98b113). Feel free to reach out if you're interested in discussing the game's mechanics, and development process, or if you simply want to talk about game design and development.
