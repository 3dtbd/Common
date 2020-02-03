using System;
using UnityEngine;

namespace WizardsCode.Controller {
    /// <summary>
    /// Camera Controller interface used to ensure that camera controllers are interchangeable across 
    /// proejects. Pull all common method descriptors up into this interface to make them available
    /// across all camera controllers (which should always inmplement this interface).
    /// </summary>
    [Serializable]
    public abstract class AbstractCameraController : MonoBehaviour
    {
        GameObject desiredCameraPosition;

        protected virtual void Awake()
        {
            desiredCameraPosition = new GameObject("Set Camera Position");
            desiredCameraPosition.SetActive(false);
        }

        /// <summary>
        /// Position the camera so that it is looking at a given game object. 
        /// The camera will attempt to position itself so that it has a good angle
        /// on the target. For more precise positioning of th camers see
        /// SetPosition(transform).
        /// </summary>
        /// <param name="target">The transform to look at.</param>
        public virtual void LookAt(Transform target)
        {
            desiredCameraPosition.SetActive(true);
            Vector3 offset = new Vector3(0, 15, 10);
            desiredCameraPosition.transform.position = target.position + offset;
            desiredCameraPosition.transform.LookAt(target.transform);
            SetPosition(desiredCameraPosition.transform);
            desiredCameraPosition.SetActive(false);
        }

        /// <summary>
        ///  Sets the position and rotation of the camera to match that of a supplied transform.
        /// </summary>
        /// <param name="transform">The position and rotation the camera is to take.</param>
        public abstract void SetPosition(Transform transform);
    }
}