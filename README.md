# RequireComponentChecker
A unity editor script designed to find all of the components which are currently in the scene and have missing components marked by the RequireComponent attribute.

RequireComponent is nice for making game objects require certain components when adding new components, but if you later add a RequireComponent attribute to a class that was already on an existing prefab then it will not automatically add the component. This project just adds an editor script located in Tools -> RequireComponent Checker to either log, or add all of the missing components.

Documentation for RequireComponent:
https://docs.unity3d.com/ScriptReference/RequireComponent.html
