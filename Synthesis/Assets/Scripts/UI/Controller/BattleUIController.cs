using Synthesis.Modifiers.Traits;
using Synthesis.UI.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthesis.UI.Controller
{
    public class BattleUIController
    {
        public class Builder
        {
            private readonly BattleUIModel model = new BattleUIModel();

            public Builder WithTraitPool(TraitPool traitPool)
            {
                model.SetTraitPool(traitPool);
                return this;
            }
        }
    }
}
