# RequireComponentChecker

## Description

A unity editor script designed to find all of the components which are currently in the scene and have missing components marked by the RequireComponent attribute.

RequireComponent is nice for making game objects require certain components when adding new components, but if you later add a RequireComponent attribute to a class that was already on an existing prefab then it will not automatically add the component. This project just adds an editor script located in Tools -> RequireComponent Checker to either log, or add all of the missing components.

Documentation for RequireComponent:
https://docs.unity3d.com/ScriptReference/RequireComponent.html

## Usage

In most cases, you would likely want to run the tool on the assets first, and then any additional loaded components that were not yet added as assets.

### Tools -> RequireComponent Checker -> Add Missing -- Check Assets (ctrl + shift + g)

Checks all "asset" GameObjects in the project using the AssetDatabase and adds any missing components denoted by the RequireComponent attribute.

### Tools -> RequireComponent Checker -> Add Missing -- Check Loaded Components

Checks all loaded GameObjects in the project using Resources and adds any missing components denoted by the RequireComponent attribute.

### Tools -> RequireComponent Checker -> Log -- Check Assets

Checks all "asset" GameObjects in the project using the AssetDatabase and  logs the GameObject to the console.

### Tools -> RequireComponent Checker -> Log -- Check Loaded Components

Checks all loaded GameObjects in the project using Resources and logs the GameObject to the console.
