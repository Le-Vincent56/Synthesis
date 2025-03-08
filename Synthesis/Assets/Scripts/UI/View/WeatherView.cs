using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthesis.UI.Weather
{
    public class WeatherView : MonoBehaviour
    {
        [SerializeField] private RectTransform timelineRect;
        [SerializeField] private GameObject weatherSegmentPrefab;

        private List<GameObject> activeSegments;

        private void Awake()
        {
            // Initialzie the list
            activeSegments = new List<GameObject>();
        }

    }
}
