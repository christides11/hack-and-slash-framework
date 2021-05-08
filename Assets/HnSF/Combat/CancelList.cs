using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Combat
{
    [CreateAssetMenu(fileName = "CancelList", menuName = "HnSF/CanceList")]
    public class CancelList : ScriptableObject
    {
        public string nickname;
        public List<MovesetAttackNode> nodes = new List<MovesetAttackNode>();
    }
}