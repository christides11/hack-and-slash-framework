using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Combat
{
    [CreateAssetMenu(fileName = "CancelList", menuName = "CAF/CanceList")]
    public class CancelList : ScriptableObject
    {
        public string nickname;
        public List<MovesetAttackNode> nodes = new List<MovesetAttackNode>();
    }
}