using Synthesis.UI.View;
using UnityEngine;
using UnityEngine.Pool;

namespace Synthesis
{
    public class MutationCardPool
    {
        private MutationCard mutationCardPrefab;
        private Transform mutationCardParent;
        private ObjectPool<MutationCard> mutationCardPool;

        public MutationCardPool(MutationCard prefab, Transform parent)
        {
            mutationCardPrefab = prefab;
            mutationCardParent = parent;

            mutationCardPool = new ObjectPool<MutationCard>(
                CreateCard,
                OnTakeCardFromPool,
                OnReturnCardToPool,
                OnDestroyCard,
                true,
                5,
                10
            );
        }

        /// <summary>
        /// Get a Mutation Card from the Mutation Card Pool
        /// </summary>
        public MutationCard Get() => mutationCardPool.Get();

        /// <summary>
        /// Release a Mutation Card back to the Mutation Card Pool
        /// </summary>
        public void Release(MutationCard card) => mutationCardPool.Release(card);

        /// <summary>
        /// Instantiate a Mutation Card within the Mutation Card Pool
        /// </summary>
        private MutationCard CreateCard()
        {
            // Instantiate the Particle System
            MutationCard card = Object.Instantiate(mutationCardPrefab, mutationCardParent);

            // Initialize the Mutation Card
            card.Initialize();

            return card;
        }

        /// <summary>
        /// Take a Mutation Card System from the Mutation Card Pool
        /// </summary>
        private void OnTakeCardFromPool(MutationCard card)
        {
            // Set the Particle System to be active
            card.gameObject.SetActive(true);
        }

        /// <summary>
        /// Return a Mutation Card to the Mutation Card Pool
        /// </summary>
        private void OnReturnCardToPool(MutationCard card)
        {
            // Reset the card data
            card.ResetData();

            // Deactivate the Particle System
            card.gameObject.SetActive(false);
        }

        /// <summary>
        /// Destroy a Mutation Card within the Mutation Card Pool
        /// </summary>
        private void OnDestroyCard(MutationCard card)
        {
            // Destroy the Particle System
            Object.Destroy(card.gameObject);
        }
    }
}
