The outline of a little boardgame framework for Unity3D. Began as a mini-project to challenge myself. It might become a package in the distant future.

Rather than an object-oriented approach, it tries to treat state and actions as only data, while behaviour is handled through static functions, in an attempt to emulate functional programming. However, for the sake of practicality, the state and history objects of a game are always assumed to be mutable. 

The project currently contains two simple games - tic-tac-toe, and chess (unfinished), serving as examples, and allowing for comparison in order to find common patterns.

# Setup

1. After cloning the repository, you must first place `OdinInspector` files into the folder `Assets/Plugins/Sirenix/` (You don't need Odin Validator). The code depends upon it, but the plugin's contents have of course been added in the `.gitignore`.
2. Open the project folder with the latest Unity 2020 LTS version (as of this writing, `2020.03.29f1`).

# NB
I do not own *any* of the code found in the `Plugins` folder. The `TextMeshPro` essentials folder is included for the sake of convenience.
