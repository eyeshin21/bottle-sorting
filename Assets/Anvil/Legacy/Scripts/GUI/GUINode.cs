using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public class GUINode
    {
        GUIContent _content;
        GUIContent _contentExpand;
        GUIContent _contentCollapse;
        List<GUINode> _children;
        bool _isExpand;

        static GUIContent ContentExpandAll = new("\u25BD");
        static GUIContent ContentCollapseAll = new("\u25B3");

        public bool Expand
        {
            get => _isExpand;
            set => _isExpand = value;
        }

        public GUINode(string text)
        {
            _content = new GUIContent(text);
            _contentExpand = new GUIContent($"\u25BC {text}");
            _contentCollapse = new GUIContent($"\u25B6 {text}");
        }

        public GUINode(string text, int listCount) : this(listCount > 0 ? $"{text} ({listCount})" : text)
        {

        }

        public void AddChild(object obj)
        {
            AddChild(new GUINode($"{obj}"));
        }

        public void AddChild(string name, object value)
        {
            AddChild(new GUINode($"{name}: {value}"));
        }

        public void AddChild(GUINode child)
        {
            Helper.CheckAdd(ref _children, child);
        }

        bool HasDescendant()
        {
            int childCount = _children.GetCount();
            if (childCount > 0)
            {
                for (int i = 0; i < childCount; i++)
                {
                    if (_children[i]._children.GetCount() > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        void ExpandAll()
        {
            _isExpand = true;
            int childCount = _children.GetCount();
            for (int i = 0; i < childCount; i++)
            {
                _children[i].ExpandAll();
            }
        }

        void CollapseAll()
        {
            _isExpand = false;
            int childCount = _children.GetCount();
            for (int i = 0; i < childCount; i++)
            {
                _children[i].CollapseAll();
            }
        }

        public void Clear()
        {
            _children = null;
            _isExpand = false;
        }

        public void OnGUI(float left, float indent)
        {
            int childCount = _children.GetCount();
            if (childCount == 0)
            {
                GUIHelper.LabelLeft(left, _content);
            }
            else
            {
                //if (GUIHelper.ButtonLeft(left, _isExpand ? _contentExpand : _contentCollapse))
                //{
                //    _isExpand = !_isExpand;
                //}

                GUIHelper.LayoutLeft(left, () =>
                {
                    if (_isExpand)
                    {
                        if (GUIHelper.Button(_contentExpand))
                        {
                            _isExpand = !_isExpand;
                        }
                        if (HasDescendant() && GUIHelper.Button(ContentCollapseAll))
                        {
                            CollapseAll();
                        }
                    }
                    else
                    {
                        if (GUIHelper.Button(_contentCollapse))
                        {
                            _isExpand = !_isExpand;
                        }
                        if (HasDescendant() && GUIHelper.Button(ContentExpandAll))
                        {
                            ExpandAll();
                        }
                    }
                });

                if (_isExpand)
                {
                    GUIHelper.LayoutVertical(() =>
                    {
                        left += indent;
                        for (int i = 0; i < childCount; i++)
                        {
                            _children[i].OnGUI(left, indent);
                        }
                    });
                }
            }
        }
    }
}