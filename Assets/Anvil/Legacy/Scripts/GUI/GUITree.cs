using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public class GUITree : IGUIController
    {
        static readonly float NodeIndent = 20;

        List<GUINode> _nodes = new();

        public void AddNode(GUINode node)
        {
            _nodes.Add(node);
        }

        public void Clear()
        {
            _nodes.Clear();
        }

        public void OnGUI()
        {
            int nodeCount = _nodes.Count;
            if (nodeCount > 0)
            {
                GUIHelper.LayoutVertical(() =>
                {
                    for (int i = 0; i < nodeCount; i++)
                    {
                        _nodes[i].OnGUI(0, NodeIndent);
                    }
                });
            }
        }
    }
}