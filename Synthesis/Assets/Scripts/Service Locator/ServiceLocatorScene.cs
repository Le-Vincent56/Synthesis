using UnityEngine;

namespace Synthesis.ServiceLocators
{
    [AddComponentMenu("Service Locator/Scene Service Locator")]
    public class ServiceLocatorScene : Bootstrapper
    {
        /// <summary>
        /// Bootstrap by configuring the Service Locator for scene use
        /// </summary>
        protected override void Bootstrap() => Container.ConfigureForScene();
    }
}