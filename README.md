<table>
  <tr>
    <td align="left" width="50%">
      <img width="100%" alt="gif1" src="https://github.com/AlbertNugroho/Project-DungeonCrawler/blob/main/prev1.gif">
    </td>
    <td align="right" width="50%">
      <img width="100%" alt="gif2" src="https://github.com/AlbertNugroho/Project-DungeonCrawler/blob/main/prev2.gif">
    </td>
  </tr>
</table>

<p align="center">
  <img width="100%" alt="gif3" src="https://github.com/AlbertNugroho/Project-DungeonCrawler/blob/main/prev3.gif">
</p>

## 📜Scripts and Features

the many stuff in the game like attacking, using skill, procedural dungeon generation, enemies, health, stamina, and so much more is thanks to tons of scripting has been implemented to the game

| Script                         | Description                                                                              |
| ------------------------------ | ---------------------------------------------------------------------------------------- |
| `RoomFirstDungeonGenerator.cs` | Responsible for all the Dungeon Generation in game like rooms, corridors, platforms, etc |
| `PlayerMovement.cs`            | Responsible for all the Movement,attacking,dashing and using skills                      |
| `Staminawork.cs`               | Responsible for all the Stamina usage in the game                                        |
| `PlayerHealth.cs`              | Responsible for all the Health system in the game                                        |
| `EnemyAI.cs`                   | Responsible for the enemy's AI like attacking, and following the player                  |
| `etc`                          |                                                                                          |

<br>

## 🔴About

"Ruby's way to grandma's" is a 2D dungeon crawler where you fight monsters, collect keys, and brave procedurally generated levels on your journey to Grandma's House. This was my first ever game project
<br>

## 🕹️Play Game

<a href="https://drive.google.com/file/d/1RfUs3W-JSsMddPK0DM_iMcK8wRjTsRDV/view?usp=drive_link">Play Now</a>
<br>

## 👤Developer & Contributions

- Albert Nugroho (Game Programmer & Player Sprite Animator)
  <br>

## 📂Files description

```
├── Project-DungeonCrawler            # Contain everything needed for Ruby's way to grandma's to work.
   ├── Assets                         # Contains every assets that have been worked with unity to create the game like the scripts and the art.
      ├── Animation                   # Contains every animation clip and animator controller that played when the game start.
      ├── Material                    # Contains all the material for the game.
      ├── Sounds                      # Contains every sound used for the game like music and sound effects.
      ├── Scripts                     # Contains all scripts needed to make the gane work like PlayerMovement scripts.
      ├── Sprites                     # Contains every sprites used in the game.
      ├── Prefab                      # Contains every Prefab, reusable game object that you can instantiate (create copies of) in your game scene.
      ├── Scenes                      # Contains all scenes that exist in the game for it to interconnected with each other like MainMenu and Game.
   ├── Packages                       # Contains game packages that responsible for managing external libraries and packages used in your project.
      ├── Manifest.json               # Contains the lists of all the packages that your project depends on and their versions.
      ├── Packages-lock.json          # Contains packages that ensuring your project always uses the same versions of all dependencies and their sub-dependencies.
   ├── Project Settings               # Contains the configuration of your game to control the quality settings, icon, or even the cursor settings
├── README.md                         # The description of Ruby's way to grandma's file from About til the developers and the contribution for this game.
```

<br>

## 🕹️Game controls

The following controls are bound in-game, for gameplay and testing.

| Key Binding | Function          |
| ----------- | ----------------- |
| W,D         | Standard movement |
| Esc         | Pause             |
| Space       | Jump              |
| 1           | Use 1st Skill     |
| 2           | Use 2nd Skill     |
| Left Click  | Attack            |
| Right Click | Dash              |

<br>
