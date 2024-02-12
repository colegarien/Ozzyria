﻿using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Components
{
    public class Armor : Component
    {
        private string _armorId;

        [Savable]
        public string ArmorId
        {
            get => _armorId; set
            {
                if (_armorId != value)
                {
                    _armorId = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
    }
}
