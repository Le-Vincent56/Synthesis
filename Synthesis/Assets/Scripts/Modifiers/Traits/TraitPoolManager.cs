using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthesis.Modifiers.Mutations
{
    // Singleton
    public class TraitPoolManager : MonoBehaviour
    {
        public TraitPool traitPool;
        
        private static TraitPoolManager _instance;
        public static TraitPoolManager Instance
        {
            get
            {
                if (_instance)
                {
                    return _instance;
                }
                else
                {
                    _instance = FindObjectOfType<TraitPoolManager>();
                    return _instance;
                }
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }
    }
}
