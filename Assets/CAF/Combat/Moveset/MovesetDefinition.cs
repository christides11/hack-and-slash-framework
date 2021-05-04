using System;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace CAF.Combat
{
    public class MovesetDefinition : NodeGraph
    {
        private Dictionary<int, MovesetAttackNode> nodeDictionary = new Dictionary<int, MovesetAttackNode>();
        [SerializeField] private int nodeIDCounter = 0;

        [Header("Normal Attacks")]
        public List<MovesetAttackNode> groundAttackCommandNormals = new List<MovesetAttackNode>();
        public List<MovesetAttackNode> groundAttackStartNodes = new List<MovesetAttackNode>();
        public List<MovesetAttackNode> airAttackCommandNormals = new List<MovesetAttackNode>();
        public List<MovesetAttackNode> airAttackStartNodes = new List<MovesetAttackNode>();

        public void OnEnable()
        {
            nodeDictionary = new Dictionary<int, MovesetAttackNode>();

            for(int i = 0; i < nodes.Count; i++)
            {
                MovesetAttackNode currentNode = (MovesetAttackNode)nodes[i];
                if (nodeDictionary.ContainsKey(currentNode.Identifier))
                {
                    Debug.LogError($"{name} has two conflicting MovesetAttackNode IDs. {nodeDictionary[currentNode.Identifier].name} & {currentNode.Identifier}");
                    continue;
                }
                nodeDictionary.Add(currentNode.Identifier, currentNode);
            }
        }

        public MovesetAttackNode GetAttackNode(int identifier)
        {
            if(nodeDictionary.TryGetValue(identifier, out MovesetAttackNode value))
            {
                return value;
            }
            return null;
        }

        public override Node AddNode(Type type)
        {
            var n = base.AddNode(type);
            if(type == typeof(MovesetAttackNode) || type.IsSubclassOf(typeof(MovesetAttackNode)))
            {
                ((MovesetAttackNode)n).Identifier = nodeIDCounter++;
            }
            return n;
        }
    }
}