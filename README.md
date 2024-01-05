# RPG-Project-Unity
This is an academic project aimed at creating a self-learning AI for the Press-Turn System RPG, inspired by Shin Megami Tensei III.

Unity version: 2022.2.11f1

The project includes copyrighted assets to which I don't have ownership:
- Grassland.png image from RPG Maker MV
- Click.ogg sound from RPG Maker MV
- Sound Test.ogg song from Shin Megami Tesei III
- FOE.ogg song from Persona Q: Shadow of Labyrinth 

# How to install? (Windows only)
1. Download the repository
2. Install Unity version 2022.2.11f1 from Unity Hub (it should prompt you to download that version after adding the project to Unity Hub)
3. Install Python 3.9.13 (other versions might work but it's not guaranteed)
5. Open the project folder in the command prompt (cmd)
6. Check if you have Python installed by typing:
   ```
   python
   ```
   If it's installed correctly, you'll see your Python version and run the Python CLI, which you should close by entering:
   ```python
   exit()
   ```
8. Create a virtual environment by entering the command:
   ```
   python -m venv venv
   ```
9. Activate the virtual environment in the command prompt:
   ```
   venv\Scripts\activate
   ```
10. Install or update pip by entering:
    ```
    python -m pip install --upgrade pip
    ```
12. Install ML-Agents by entering:
    ```
    pip install mlagents
    ```
13. Install PyTorch by entering:
    ```
    pip install torch torchvision torchaudio
    ```
14. Change the protobuf version (because ML-Agents installs the wrong version) by entering: 
    ```
    pip install protobuf==3.20.3
    ```
15. Install ONNX by entering:
    ```
    pip install onnx
    ```
16. Check if ML-Agents is installed correctly by running the command:
    ```
    mlagents-learn -h
    ```
# How to use?
1. Open the Unity project and then open the TrainingScene, which is in the Scenes folder
2. In the hierarchy, search for the Environments object
3. Clone the Environment object as many times as you want to have different training scenarios
4. Configure objects in Environments
   - In MLAgentsTrainer, you can change reward values from actions
   - In TrainingManager, you can change max turns, points, stats, and actor count
     - Max turns represent the maximum number of turns after which the environment will reset if the battle is not determined
     - Points min/max is for HP and MP randomization
     - Stats min/max is for Strength, Magic, Dexterity, Agility, and Luck randomization
     - Actor count is for randomizing the number of actors in the non-bot team
5. Run ML-Agents from the command prompt by entering the command:
   ```
   mlagents-learn
   ```
   Or, if you were already training the agent, you can force learning by entering the command:
   ```
   mlagents-learn --force
   ```
6. When ML-Agents is waiting for Unity (it will be written in the command prompt), run a playtest of the TrainingScene in Unity
7. Train your agent, and when you are done, exit playtest in Unity and exit ML-Agents training in the command prompt by pressing CTRL+C. Remember the name of the .onxx file, which will be prompted after closing ML-Agents.
8. Copy the brain file (.onxx file) from [ProjectPath]/results/ppo/EnemyBehaviour folder into [ProjectPath]/Assets/Data/Brains folder.
9. Open the BattleScene from the Scenes folder in Unity
10. In the hierarchy, find the MLAgentsAI object and assign your brain from Data/Brains to the Model value in the Inspector.
11. Open the Main Menu scene and run your simulation.
