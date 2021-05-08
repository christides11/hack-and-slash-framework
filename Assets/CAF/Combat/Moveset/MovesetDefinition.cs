using System;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace CAF.Combat
{
    public class MovesetDefinition : NodeGraph
    {
        [System.Serializable]
        public class CancelListConnection
        {
            public int ID;
            public CancelList list;
        }

        private Dictionary<int, CancelList> cancelListsDictionary = new Dictionary<int, CancelList>();
        private Dictionary<int, MovesetAttackNode> nodeDictionary = new Dictionary<int, MovesetAttackNode>();
        [SerializeField] private int nodeIDCounter = 0;

        public List<CancelListConnection> cancelLists = new List<CancelListConnection>();

        [Header("Normal Attacks")]
        public int groundIdleCancelListID = -1;
        public List<MovesetAttackNode> groundAttackStartNodes = new List<MovesetAttackNode>();
        public int airIdleCancelListID = -1;
        public List<MovesetAttackNode> airAttackStartNodes = new List<MovesetAttackNode>();

        public void OnEnable()
        {
            cancelListsDictionary = new Dictionary<int, CancelList>();
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

            for(int i = 0; i < cancelLists.Count; i++)
            {
                if (cancelListsDictionary.ContainsKey(cancelLists[i].ID))
                {
                    Debug.LogError($"{name} has two conflicting CancelList IDs. {cancelListsDictionary[cancelLists[i].ID].nickname} & {cancelLists[i].list.nickname}");
                    continue;
                }
                cancelListsDictionary.Add(cancelLists[i].ID, cancelLists[i].list);
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

        public CancelList GetCancelList(int identifier)
        {
            if(cancelListsDictionary.TryGetValue(identifier, out CancelList value))
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