using Cinemachine;
using System.Collections.Generic;
using Synthesis.ServiceLocators;
using UnityEngine;

namespace Synthesis
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera initialCamera;
        [SerializeField] private List<CinemachineVirtualCamera> virtualCameras;
        [SerializeField] private CinemachineImpulseSource impulse;

        private void Awake()
        {
            // Get components
            impulse = GetComponent<CinemachineImpulseSource>(); 

            // Store all the virtual cameras
            virtualCameras = new List<CinemachineVirtualCamera>();
            GetComponentsInChildren(virtualCameras);

            // Set the initial camera
            PrioritizeCamera(initialCamera);

            // Register this as a service
            ServiceLocator.ForSceneOf(this).Register(this);
        }

        /// <summary>
        /// Prioritize the UI Camera
        /// </summary>
        public void PrioritizeUICamera()
        {
            // Arrange priorities
            virtualCameras[0].Priority = 15;
            virtualCameras[1].Priority = 10;
        }

        /// <summary>
        /// Prioritize the Battle Camera
        /// </summary>
        public void PrioritizeBattleCamera()
        {
            // Arrange priorities
            virtualCameras[1].Priority = 15;
            virtualCameras[0].Priority = 10;
        }

        /// <summary>
        /// Prioritize a Cinemachine Virtual Camera
        /// </summary>
        private void PrioritizeCamera(CinemachineVirtualCamera cameraToPrioritize)
        {
            // Set the initial camera as the highest priority
            initialCamera.Priority = 15;

            // Set all other cameras to a lower priority
            foreach (CinemachineVirtualCamera camera in virtualCameras)
            {
                // Skip if the camera is the one to prioritize
                if (camera == cameraToPrioritize) continue;

                // Set the camera to a lower priority
                camera.Priority = 10;
            }
        }

        /// <summary>
        /// Generate impulse for camera shake
        /// </summary>
        public void GenerateImpulse() => impulse.GenerateImpulse();
    }
}
