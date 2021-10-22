using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Combat
{
    [CreateAssetMenu(fileName = "BoxCollectionDefinition", menuName = "HnSF/BoxCollectionDefinition")]
    public class BoxCollectionDefinition : ScriptableObject
    {
        [SerializeReference] public List<HurtboxGroup> hurtboxGroups = new List<HurtboxGroup>();
    }
}