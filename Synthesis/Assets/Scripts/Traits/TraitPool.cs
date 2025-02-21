using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthesis.Traits
{
    [CreateAssetMenu(fileName = "TraitPool", menuName = "ScriptableObjects/TraitPool", order = 0)]
    public class TraitPool : ScriptableObject
    {
        public Trait[] traits;

        /// <summary>
        /// Get a random trait from the pool.
        /// </summary>
        /// <returns></returns>
        public Trait GetRandomTrait()
        {
            // pick a random trait, repeat if the reference is null up to a max number of times.
            int iterations = traits.Length * 2;
            do
            {
                int i = Random.Range(0, traits.Length);
                if (traits[i] != null)
                {
                    return traits[i];
                }

                iterations--;
            } while (iterations > 0);

            return null;
        }
        
        /// <summary>
        /// Find a trait by its string name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Trait GetTraitByName(string name)
        {
            foreach (var trait in traits)
            {
                if (trait != null)
                {
                    if (trait.Name.Equals(name))
                    {
                        return trait;
                    }
                }
            }

            return null;
        }
    }
}
