using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FourthMouse.Tools
{
    public delegate void ActionToTake(GameObject go, System.Type t);

    public class RequireComponentChecker
    {
        private static ActionToTake action;
        private static List<GameObject> gameObjects;

        [MenuItem("Tools/RequireComponent Checker/Add Missing -- Check Assets %#g")]
        private static void AddMissingComponentsFromAssets()
        {
            action = new ActionToTake(AddMissing);
            gameObjects = GetEditableGameObjectsFromAssets();
            ActOnMissingComponents();
        }

        [MenuItem("Tools/RequireComponent Checker/Add Missing -- Check Loaded Components")]
        private static void AddMissingComponentsFromMemory()
        {
            action = new ActionToTake(AddMissing);
            gameObjects = GetEditableGameObjectsFromMemory();
            ActOnMissingComponents();
        }

        [MenuItem("Tools/RequireComponent Checker/Log -- Check Assets")]
        private static void LogMissingComponentsFromAssets()
        {
            action = new ActionToTake(LogMissing);
            gameObjects = GetEditableGameObjectsFromAssets();
            ActOnMissingComponents();
        }

        [MenuItem("Tools/RequireComponent Checker/Log -- Check Loaded Components")]
        private static void LogMissingComponentsFromMemory()
        {
            action = new ActionToTake(LogMissing);
            gameObjects = GetEditableGameObjectsFromMemory();
            ActOnMissingComponents();
        }

        private static void LogMissing(GameObject go, System.Type t)
        {
            Debug.Log($"{go.name} is missing a required component of type {t.Name}.");
        }

        private static void AddMissing(GameObject go, System.Type t)
        {
            Undo.AddComponent(go, t);
            Debug.Log($"{go.name} now has a component of type {t.Name}.");
        }

        private static bool ValidEditableGameObject(GameObject go)
        {
            return go != null
                && go.transform.root.gameObject != null
                && go.hideFlags == HideFlags.None;
        }

        private static List<GameObject> GetEditableGameObjectsFromAssets()
        {
            var guids = AssetDatabase.FindAssets("t:GameObject");
            var editableObjects = new List<GameObject>();
            foreach (string guid in guids)
            {
                var component = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(GameObject));
                var go = component as GameObject;
                if (ValidEditableGameObject(go))
                {
                    editableObjects.Add(go);
                }
            }

            return editableObjects;
        }

        private static List<GameObject> GetEditableGameObjectsFromMemory()
        {
            var editableObjects = new List<GameObject>();

            // This checks objects that may be in the scenes loaded in memory, but not necessarily added as an asset of the project yet.
            (Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
                .ToList<GameObject>()
                .Where(go => ValidEditableGameObject(go) && !editableObjects.Contains(go))
                .ToList<GameObject>()
                .ForEach(go => editableObjects.Add(go));

            return editableObjects;
        }

        private static void ActOnMissingComponents()
        {
            Debug.Log($"Found {gameObjects.Count} objects.");

            gameObjects.ForEach(go =>
            {
                var components = go.GetComponents(typeof(Component));
                for (var childIndedx = 0; childIndedx < components.Length; childIndedx++)
                {
                    var child = components[childIndedx];

                    // Get all of the attributes via reflection matching RequireComponent belonging to the child component's type
                    var reqComps = System.Attribute.GetCustomAttributes(child.GetType()).ToList().Where(a => a is RequireComponent).ToList();
                    reqComps.ForEach(a =>
                    {
                        var rc = a as RequireComponent;

                        // The RequireComponent decorator has a possibility of having 3 types assigned to it.
                        List<System.Type> types = new List<System.Type>() { rc.m_Type0, rc.m_Type1, rc.m_Type1 };
                        types.ForEach(t =>
                        {
                            if (t != null)
                            {
                                Component component = go.GetComponent(t);
                                if (component == null)
                                    action(go, t);
                            }
                        });
                    });
                }
            });
        }
    }
}
