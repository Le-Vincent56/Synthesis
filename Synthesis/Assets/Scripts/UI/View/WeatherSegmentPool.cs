using UnityEngine;
using UnityEngine.Pool;

namespace Synthesis.UI.View
{
    public class WeatherSegmentPool
    {
        private readonly Transform parentObject;
        private readonly GameObject weatherSegmentPrefab;
        private readonly ObjectPool<GameObject> pool;
        
        public WeatherSegmentPool(Transform parentObject, GameObject weatherSegmentPrefab)
        {
            this.parentObject = parentObject;
            this.weatherSegmentPrefab = weatherSegmentPrefab;

            // Instantiate the object pool
            pool = new ObjectPool<GameObject>(
                OnCreateSegment,
                OnTakeSegmentFromPool,
                OnReturnSegmentToPool,
                OnDestroySegment,
                true,
                5,
                10
            );
        }

        /// <summary>
        /// Get a Weather Segment from the Pool
        /// </summary>
        public GameObject Get() => pool.Get();

        /// <summary>
        /// Release a Weather Segment back to the Pool
        /// </summary>
        public void Release(GameObject weatherSegment) => pool.Release(weatherSegment);

        /// <summary>
        /// Create a Weather Segment within the Pool
        /// </summary>
        private GameObject OnCreateSegment()
        {
            // Create a weather segment
            GameObject weatherSegment = Object.Instantiate(weatherSegmentPrefab, parentObject);

            return weatherSegment;
        }

        /// <summary>
        /// Take a Weather Segment from the Pool
        /// </summary>
        private void OnTakeSegmentFromPool(GameObject weatherSegment)
        {
            // Set the weather segment as active
            weatherSegment.SetActive(true);
        }

        /// <summary>
        /// Return a Weather Segment to the Pool
        /// </summary>
        private void OnReturnSegmentToPool(GameObject weatherSegment)
        {
            // Set the weather segment as inactive
            weatherSegment.SetActive(false);
        }

        /// <summary>
        /// Destroy a Weather Segment
        /// </summary>
        private void OnDestroySegment(GameObject weatherSegment)
        {
            // Destroy the weather segment
            Object.Destroy(weatherSegment);
        }
    }
}
