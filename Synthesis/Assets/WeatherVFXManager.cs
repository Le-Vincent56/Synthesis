using System;
using System.Collections;
using System.Collections.Generic;
using Synthesis.EventBus;
using Synthesis.EventBus.Events.Weather;
using Synthesis.Weather;
using UnityEngine;
using UnityEngine.VFX;

namespace Synthesis
{
    public class WeatherVFXManager : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer land;

        [SerializeField] private VisualEffect rainVFX;

        [SerializeField] private SpriteRenderer sunVFX;

        [SerializeField] private Material rainLandMat;

        [SerializeField] private Material sunLandMat;

        [SerializeField] private float transitionTime = 2f;
        
        // events
        private EventBinding<ClearWeather> onClearWeather;
        private EventBinding<StartDrought> onStartDrought;
        private EventBinding<StartTorrent> onStartTorrent;
        private static readonly int T = Shader.PropertyToID("_T");

        private void OnEnable()
        {
            onClearWeather = new EventBinding<ClearWeather>(OnClearWeather);
            EventBus<ClearWeather>.Register(onClearWeather);

            onStartDrought = new EventBinding<StartDrought>(OnDrought);
            EventBus<StartDrought>.Register(onStartDrought);

            onStartTorrent = new EventBinding<StartTorrent>(OnTorrent);
            EventBus<StartTorrent>.Register(onStartTorrent);
        }

        private void OnDisable()
        {
            EventBus<ClearWeather>.Deregister(onClearWeather);
            EventBus<StartDrought>.Deregister(onStartDrought);
            EventBus<StartTorrent>.Deregister(onStartTorrent);
        }

        private void OnClearWeather()
        {
            StartCoroutine(ClearWeather(null));
        }

        private void OnDrought()
        {
            StartCoroutine(ClearWeather(Drought()));
        }

        private void OnTorrent()
        {
            StartCoroutine(ClearWeather(Torrent()));
        }

        IEnumerator ClearWeather(IEnumerator newWeather)
        {
            float t = 0;
            float a = land.material.GetFloat(T);
            float b = sunVFX.material.GetFloat(T);

            float mT = transitionTime / 2;

            while (t < mT)
            {
                land.material.SetFloat(T, Mathf.Lerp(a, 0, t));
                sunVFX.material.SetFloat(T, Mathf.Lerp(b, 0, t));
                t += Time.deltaTime;
                
                rainVFX.Stop();
                
                yield return null;
            }
            
            land.material.SetFloat(T, 0);
            sunVFX.material.SetFloat(T, 0);

            if (newWeather != null)
            {
                StartCoroutine(newWeather);
            }
        }

        IEnumerator Drought()
        {
            float t = 0;
            land.material = sunLandMat;

            float mT = transitionTime / 2;

            while (t < mT)
            {
                land.material.SetFloat(T, Mathf.Lerp(0, 1f, t));
                sunVFX.material.SetFloat(T, Mathf.Lerp(0, 0.4f, t));
                t += Time.deltaTime;
                
                yield return null;
            }
            
            yield return null;
        }

        IEnumerator Torrent()
        {
            float t = 0;
            land.material = rainLandMat;

            float mT = transitionTime / 2;

            while (t < mT)
            {
                land.material.SetFloat(T, Mathf.Lerp(0, 1f, t));
                rainVFX.Play();
                t += Time.deltaTime;
                
                yield return null;
            }
            
            yield return null;
        }
    }
}
