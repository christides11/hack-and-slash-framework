using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Sample.TDAction
{
    [CreateAssetMenu(fileName = "FighterDefinition", menuName = "2dAction/Fighter Definition")]
    public class FighterDefinition : ScriptableObject
    {
        public MovesetDefinition[] movesets = new MovesetDefinition[1];
    }
}