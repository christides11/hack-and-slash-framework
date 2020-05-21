using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace CAF.Combat
{
    [CreateAssetMenu(fileName = "MovesetDefinition", menuName = "CAF/Combat/Moveset")]
    public class MovesetDefinition : NodeGraph
    {
        [Header("Normal Attacks")]
        public List<MovesetAttackNode> groundAttackCommandNormals = new List<MovesetAttackNode>();
        public List<MovesetAttackNode> groundAttackStartNodes = new List<MovesetAttackNode>();
        public List<MovesetAttackNode> airAttackCommandNormals = new List<MovesetAttackNode>();
        public List<MovesetAttackNode> airAttackStartNodes = new List<MovesetAttackNode>();
    }
}