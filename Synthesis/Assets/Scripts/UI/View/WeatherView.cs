using Synthesis.EventBus;
using Synthesis.EventBus.Events.Weather;
using Synthesis.UI.View;
using Synthesis.Weather;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Synthesis.UI.Weather
{
    public class WeatherView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform timelineScrollView;
        [SerializeField] private RectTransform timelineRect;
        [SerializeField] private RectTransform timelineScrollContent;
        [SerializeField] private ScrollRect timelineScrollRect;
        [SerializeField] private GameObject weatherSegmentPrefab;
        [SerializeField] private HorizontalLayoutGroup segmentLayoutGroup;

        [Header("Fields")]
        [SerializeField] private int totalTicks = 100;
        private WeatherSegmentPool segmentPool;
        private List<GameObject> activeSegments;
        private List<WeatherPeriod> currentWeatherPeriods;

        [Header("Weather Colors")]
        [SerializeField] private Color clearColor;
        [SerializeField] private Color torrentColor;
        [SerializeField] private Color droughtColor;

        private EventBinding<WeatherUpdated> onUpdateWeather;
        private EventBinding<UpdateWeatherDuration> onUpdateWeatherDuration;

        private void Awake()
        {
            // Create the Weather Segment Pool
            segmentPool = new WeatherSegmentPool(timelineScrollContent.transform, weatherSegmentPrefab);

            // Initialzie the lists
            activeSegments = new List<GameObject>();
            currentWeatherPeriods = new List<WeatherPeriod>();
        }

        private void OnEnable()
        {
            onUpdateWeather = new EventBinding<WeatherUpdated>(UpdateWeatherTimeline);
            EventBus<WeatherUpdated>.Register(onUpdateWeather);

            onUpdateWeatherDuration = new EventBinding<UpdateWeatherDuration>(UpdateWeatherDuration);
            EventBus<UpdateWeatherDuration>.Register(onUpdateWeatherDuration);
        }

        private void OnDisable()
        {
            EventBus<WeatherUpdated>.Deregister(onUpdateWeather);
            EventBus<UpdateWeatherDuration>.Deregister(onUpdateWeatherDuration);
        }

        /// <summary>
        /// Event callback to update the Weather Timeline
        /// </summary>
        private void UpdateWeatherTimeline(WeatherUpdated eventData) => UpdateWeatherTimeline(eventData.WeatherPeriods);

        /// <summary>
        /// Update the Weather Timeline
        /// </summary>
        private void UpdateWeatherTimeline(List<WeatherPeriod> weatherPeriods)
        {
            // Store the current periods for tracking updates
            currentWeatherPeriods = new List<WeatherPeriod>(weatherPeriods);

            // Iterate through each Weather Segment that is active
            foreach (GameObject segment in activeSegments)
            {
                // Release it back to the pool
                segmentPool.Release(segment);
            }

            // Clear the list
            activeSegments.Clear();

            float tickWidth = timelineRect.rect.width / totalTicks;
            float positionOffset = 0f;

            // Disable the Horizontal Layout Group
            segmentLayoutGroup.enabled = false;

            // Iterate through each Weather Period
            for(int i =0; i < weatherPeriods.Count; i++)
            {
                WeatherPeriod period = weatherPeriods[i];

                // Get a segment from the pool
                GameObject newSegment = segmentPool.Get();
                RectTransform segmentRect = newSegment.GetComponent<RectTransform>();
                Image segmentImage = newSegment.GetComponent<Image>();

                // Set color based on weather type
                segmentImage.color = GetWeatherColor(period.WeatherType);

                // Calculate width based on duration
                float boxWidth = period.Duration * tickWidth;

                // Set width dynamically
                segmentRect.sizeDelta = new Vector2(boxWidth, segmentRect.sizeDelta.y);

                // Force correct ordering
                segmentRect.SetSiblingIndex(i);

                // Set the anchored position
                segmentRect.anchoredPosition = new Vector2(positionOffset, 0);

                // Move offset for next box
                positionOffset += boxWidth;

                // Add to tracking list
                activeSegments.Add(newSegment);
            }

            // Force a UI refresh to apply changes immediately
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(timelineScrollContent);

            // Enable the Horizontal Layout Group
            segmentLayoutGroup.enabled = true;
        }

        /// <summary>
        /// Update a Weather's duration
        /// </summary>
        private void UpdateWeatherDuration(UpdateWeatherDuration eventData)
        {
            // Find the period that matches the Weather Type
            for(int i = 0; i < currentWeatherPeriods.Count; i++)
            {
                if (currentWeatherPeriods[i].WeatherType == eventData.Type)
                {
                    // Update the duration
                    currentWeatherPeriods[i] = new WeatherPeriod(eventData.Type, eventData.Duration);
                    break;
                }
            }

            // Update the timeline
            UpdateWeatherTimeline(currentWeatherPeriods);
        }

        /// <summary>
        /// Assigns colors to weather types.
        /// </summary>
        private Color GetWeatherColor(WeatherType weatherType)
        {
            if (weatherType == WeatherSystem.Clear) return clearColor;
            if (weatherType == WeatherSystem.Drought) return droughtColor;
            if (weatherType == WeatherSystem.Torrent) return torrentColor;
            return clearColor; // Default fallback
        }
    }
}
