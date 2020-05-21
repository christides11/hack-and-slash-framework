using CAF.Input;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace CAF.Combat
{
    [System.Serializable]
    public class MovesetAttackNode : Node
    {
        [Input] public MovesetAttackNode lastNode;

        public List<InputDefinition> executeButton = new List<InputDefinition>();
        public List<InputDefinition> buttonSequence = new List<InputDefinition>();
        public AttackDefinition attackDefinition;

        public List<Vector2Int> cancelWindows = new List<Vector2Int>();
        [Output(dynamicPortList = true)] public List<MovesetAttackNode> nextNode = new List<MovesetAttackNode>();

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            base.OnCreateConnection(from, to);
            if(from.node == this)
            {
                nextNode[(int)from.GetOutputValue()] = (MovesetAttackNode)to.node;
            }
            if(to.node == this)
            {
                lastNode = (MovesetAttackNode)from.node;
            }
        }

        public override object GetValue(NodePort port)
        {
            for(int i = 0; i < nextNode.Count; i++)
            {
                if(port.fieldName == $"nextNode {i}")
                {
                    return i;
                }
            }
            return null;
        }
    }
}