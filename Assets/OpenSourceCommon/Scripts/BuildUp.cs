using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using WizardsCode.Tools.DocGen;

namespace WizardsCode.Animation
{
    [DocGen("Using Buildup it is possible to automatically create an animation of a game object buing built. " +
        "This is useful in games such as Tower Defense or an RTS game.\n\n" + 
        "To use BuildUp add this component to the game object to build or to another game object and identify the object to be built in the `objectToBuild` field.\n\n" +
        "When the object is instantiated this script will turn off all renderers, other than those belonging to an object whose name matches a pattern in the `ObjectsToIgnoreInBuild`, and then run through the build cycle.\n\n" +
        "The build cycle consists of a number of build phases in which parts of the game object will be 'built'. " +
        "Define as many build phases as you would like in the `objectBuildPhases`, each phase will be executed in order. " +
        "During each phase any children mathcing the regular expression defining the phase will be built up. " +
        "Once all the defined phases have been completed any remaining inactive children, that are not excluded from the build using `objectsToIgnoreInBuild`, will be built.")]
    public class BuildUp : MonoBehaviour
    {
        [Tooltip("The parent game object to build. If null then it is assumed that this script is attached to the desired game object.")]
        [SerializeField]
        // TODO: provide a custom editor that allows us to clearly indicate that the selected object is this or an explicitly supplied object
        GameObject objectToBuild;

        [Tooltip("An ordered list of regular expressions that will match the names of child objects that should be built in each step.")]
        [SerializeField]
        // TODO: make this a sortable list in the inspector
        string[] objectBuildPhases;

        [Tooltip("A list of regular expressions that will match the names of child objects that should not be a part of the build phases. These items will not be marked as inactive/invisible at the start of the build cycle.")]
        [SerializeField]
        string[] objectsToIgnoreInBuild;

        enum axis { X, Y, Z }
        [Tooltip("The axis along which the object is to be built.")]
        [SerializeField]
        axis buildAxis = axis.Y;

        [Tooltip("The time in seconds that each phase of a build lasts. Each phase is defined by a single pattern in the `Object Build Order`.")]
        float phaseTime = 2f;

        bool isBuilt = false;
        bool isBuilding = false;
        int currentPhase = 0;

        private void Start()
        {
            if (objectToBuild == null)
            {
                objectToBuild = gameObject;
            }
            // turn off all children that do not have children of their own
            DeactivateBuildComponents(objectToBuild.transform);
        }

        private void DeactivateBuildComponents(Transform parent)
        {
            if (parent.childCount == 0)
            {
                parent.gameObject.SetActive(IsExcluded(parent));
            }
            else
            {
                for (int i = 0; i < parent.childCount; i++)
                {
                    DeactivateBuildComponents(parent.GetChild(i));
                }
            }
        }

        private bool IsExcluded(Transform parent)
        {
            bool excluded = false;
            for (int i = 0; i < objectsToIgnoreInBuild.Length; i++)
            {
                if (Regex.IsMatch(parent.name, objectsToIgnoreInBuild[i]))
                {
                    excluded = true;
                    break;
                }
            }
            return excluded;
        }

        void Update()
        {
            if (!isBuilt && !isBuilding)
            {
                StartCoroutine(ExecuteBuildPhases());
            }
            else
            {
                this.enabled = false;
            }
        }

        private IEnumerator ExecuteBuildPhases()
        {
            isBuilding = true;
            while (currentPhase < objectBuildPhases.Length)
            {
                // Get the first regular expression
                string re = objectBuildPhases[currentPhase];
                // find all objects that match the regular expression and activate them
                BuildMatching(objectToBuild.transform, re);
                yield return new WaitForSeconds(phaseTime);
                currentPhase++;
            }

            BuildRemaining(objectToBuild.transform);

            isBuilding = false;
            isBuilt = true;
        }

        /// <summary>
        /// Build all the leaves on or below an object that have names matching a regulare expression.
        /// </summary>
        /// <param name="obj">The object to consider building. If this object is a parent and matches the RE then all children will be built. If it is a leaf then it will be built if the RE matches or if it is the child of a parent whose RE matches.</param>
        /// <param name="re">A regular expression to use in matching the name of the parent</param>
        /// <param name="forceBuild">Build even if the RE does not match?</param>
        private void BuildMatching(Transform obj, string re, bool forceBuild = false)
        {
            if (!obj.gameObject.activeSelf && (forceBuild || (obj.childCount == 0 && Regex.IsMatch(obj.name, re))))
            {
                StartCoroutine(Build(obj, phaseTime));
            }
            else
            {
                for (int i = 0; i < obj.childCount; i++)
                {
                    BuildMatching(obj.GetChild(i), re, Regex.IsMatch(obj.name, re));
                }
            }
        }

        /// <summary>
        /// Build all leaves and parents that are still inactive and are not in the exclude list.
        /// </summary>
        private void BuildRemaining(Transform parent)
        {
            if (parent.transform.childCount == 0 && !parent.gameObject.activeSelf && !IsExcluded(parent))
            {
                StartCoroutine(Build(parent, phaseTime));
            } else
            {
                for (int i = 0; i < parent.childCount; i++)
                {
                    BuildRemaining(parent.GetChild(i));
                }
            }
        }

        private IEnumerator Build(Transform obj, float duration)
        {
            Vector3 currentScale = obj.localScale;
            float min = 0;
            float max = 0;
            switch (buildAxis) {
                case axis.X:
                    max = obj.localScale.x;
                    currentScale.x = min;
                    break;
                case axis.Y:
                    max = obj.localScale.y;
                    currentScale.y = min;
                    break;
                case axis.Z:
                    max = obj.localScale.z;
                    currentScale.z = min;
                    break;
            }
            
            obj.transform.localScale = currentScale;
            obj.gameObject.SetActive(true);

            int steps = 50;
            float pause = duration / steps;
            float inc = (max - min) / steps;
            for (int i = 0; i < steps; i++)
            {
                yield return new WaitForSeconds(pause);
                switch (buildAxis)
                {
                    case axis.X:
                        currentScale.x += inc;
                        break;
                    case axis.Y:
                        currentScale.y += inc;
                        break;
                    case axis.Z:
                        currentScale.z += inc;
                        break;
                }
                obj.transform.localScale = currentScale;
            }

            switch (buildAxis)
            {
                case axis.X:
                    currentScale.x = max;
                    break;
                case axis.Y:
                    currentScale.y = max;
                    break;
                case axis.Z:
                    currentScale.z = max;
                    break;
            }
            obj.transform.localScale = currentScale;
        }
    }
}
