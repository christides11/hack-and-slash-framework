using CAF.Input;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace CAF.Combat
{
    [System.Serializable]
    public class MovesetAttackNode : Node
    {
        [System.Serializable]
        public class nextNodeDefinition
        {
            public Vector2Int cancelWindow;
            public MovesetAttackNode node;
        }

        [Input] public MovesetAttackNode lastNode;

        public List<InputDefinition> executeInputs = new List<InputDefinition>();
        public List<InputDefinition> inputSequence = new List<InputDefinition>();
        public AttackDefinition attackDefinition;

        [Output(dynamicPortList = true)] public List<nextNodeDefinition> nextNode = new List<nextNodeDefinition>();

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            base.OnCreateConnection(from, to);
            if(from.node == this)
            {
                nextNode[(int)from.GetOutputValue()].node = (MovesetAttackNode)to.node;
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