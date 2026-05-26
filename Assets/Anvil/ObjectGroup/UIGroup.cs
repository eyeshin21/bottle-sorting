using System.Collections.Generic;
using Anvil.Legacy;
using UnityEngine;

namespace Anvil
{
    public enum PresetGroupId
    {
        None,
        MainGame,
    }
    public class UIGroup
    {
        private static Dictionary<string,UIGroup> _groupDictionary = new Dictionary<string, UIGroup>();

        public static void RegisterCommonGroup(string groupId,UIGroup group)
        {
            if (_groupDictionary.ContainsKey(groupId))
            {
                Debug.LogError("Registering duplicate group id: " + groupId);
                _groupDictionary[groupId] = group;
            }
            else
            {
                _groupDictionary.Add(groupId,group);
            }
        }
        public static void RegisterCommonUIElement(string groupId,IGlobalUIElement element)
        {
            if (_groupDictionary.TryGetValue(groupId,out var group))
            {
                group.Register(element);
            }
            else
            {
                UIGroup newGroup = new UIGroup();
                newGroup.Register(element);
                _groupDictionary.Add(groupId,newGroup);
            }
        }
        public static UIGroup FindStaticGroup(string groupId)
        {
            return _groupDictionary.GetValueOrDefault(groupId);
        }
        public static void Clear()
        {
            _groupDictionary.Clear();
        }
        public static IGlobalUIElement FindElement(string groupId,string id)
        {
            if (_groupDictionary.TryGetValue(groupId,out var group))
            {
                return group.FindElement(id);
            }
            return null;
        }

        //Ultility functions
        public static void HideGroup(string groupId)
        {
            if (_groupDictionary.TryGetValue(groupId,out var group))
            {
                group.HideAllImidiate();
            }
        }
        public static void ShowGroup(string groupId)
        {
            if (_groupDictionary.TryGetValue(groupId,out var group))
            {
                group.ShowAllImidiate();
            }
        }
        public static void HideGroupSequence(string groupId, FxParams fxParams = null)
        {
            if (_groupDictionary.TryGetValue(groupId,out var group))
            {
                group.StartHideSequence(fxParams);
            }
        }
        public static void ShowGroupSequence(string groupId, FxParams fxParams = null)
        {
            if (_groupDictionary.TryGetValue(groupId,out var group))
            {
                group.StartShowSequence(fxParams);
            }
        }




        private List<IGlobalUIElement> _elements = new List<IGlobalUIElement>();
        // private List<string> _preferedOrder = new List<string>();
        public void Register(IGlobalUIElement element, string id = "")
        {
            if (id != "")
            {
                element.idString = id;
            }
            for (int i = 0; i < _elements.Count; i++)
            {
                var occupant = _elements[i];
                if (occupant.PreferedIndex > element.PreferedIndex)
                {
                    _elements.Insert(i,element);
                    return;
                }
            }
            _elements.Add(element);
        }
        public void Unregister(IGlobalUIElement element)
        {
            _elements.Remove(element);
        }
        public IGlobalUIElement FindElement(string id)
        {
            for (int i = 0; i < _elements.Count; i++)
            {
                if (_elements[i].idString == id)
                {
                    return _elements[i];
                }
            }

            Debug.LogWarning($"element with id {id} not found in group");
            return null;
        }
        public void ShowAllImidiate()
        {
            for (int i = 0; i < _elements.Count; i++)
            {
                _elements[i].Show();
            }
        }
        public void HideAllImidiate()
        {
            for (int i = 0; i < _elements.Count; i++)
            {
                _elements[i].Hide();
            }
        }
        public void ShowElement(string id)
        {
            IGlobalUIElement element = FindElement(id);
            if (element != null)
            {
                element.Show();
            }
        }

        ScriptedEvent _playingSequence;
        public void StartHideSequence(FxParams fxParams = null)
        {
            if (_playingSequence != null)
            {
                _playingSequence.OnComplete();
            }
            ScriptedEvent sequence = new ScriptedEvent();
            if (fxParams == null)
            {
                fxParams = FxConfig.GetFxParams(CollectFxType.DefaultUISequence);
            }
            sequence.SetPrimaryAction((callback)=>
            {
                float delay = 0;
                for (int i = 0; i < _elements.Count; i++)
                {
                    int index = i;
                    IGlobalUIElement element = _elements[i];
                    Manager.DelayCall(delay, () =>
                    {
                        element.Hide();
                        if (index == _elements.Count - 1)
                        {
                            callback?.Invoke();
                        }
                    });
                    delay += fxParams.EvaluateDelaySpawn(i, _elements.Count);
                }
            });
            _playingSequence = sequence;
            sequence.Execute();
        }
        public void StartShowSequence(FxParams fxParams = null)
        {
            if (_playingSequence != null)
            {
                _playingSequence.OnComplete();
            }
            ScriptedEvent sequence = new ScriptedEvent();
            if (fxParams == null)
            {
                fxParams = FxConfig.GetFxParams(CollectFxType.DefaultUISequence);
            }
            sequence.SetPrimaryAction((callback)=>
            {
                float delay = 0;
                for (int i = 0; i < _elements.Count; i++)
                {
                    int index = i;
                    IGlobalUIElement element = _elements[i];
                    Manager.DelayCall(delay, () =>
                    {
                        element.Show();
                        if (index == _elements.Count - 1)
                        {
                            callback?.Invoke();
                        }
                    });
                    delay += fxParams.EvaluateDelaySpawn(i, _elements.Count);
                }
            });
            _playingSequence = sequence;
            sequence.Execute();
        }
    }
}
