using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldDependentItem : MonoBehaviour
{
        public abstract void HandleWorldUpdate();
        public WorldState currState;
}
