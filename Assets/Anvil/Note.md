sfx:

QuestManager:
 - Reward is not instant after completing quest. potential miss.

Potential issues:
 - powerup created will be floating while matching item is moving and its create animation is playing. will make other item fall in its place. when it completed entry it will init override the new item that fall in its place.
    observed on propeller

 - quest asset still holding on to the target reference after menu is reloaded