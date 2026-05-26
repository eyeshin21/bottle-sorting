// using System;
// using Gametamin;
// using Gametamin.Utility;
// using UnityEngine;
// using UnityEngine.UI;
// using Object = UnityEngine.Object;
//
// namespace Gametamin.Utility
// {
//     public class CommonDebugUI : MonoBehaviour
//     {
//         public GameObject topUIObj;
//         public GameObject bottomUIObj;
//         public GameObject leftUIObj;
//         public GameObject rightUIObj;
//
//         public GameObject statusContainer;
//
//         [SerializeField] GameObject _textObject;
//         private TextAdapter _textAdapter;
//
//         public Action onUpdate;
//
//         private void Update()
//         {
//             onUpdate?.Invoke();
//         }
//
//         public void SetDebugText(string text)
//         {
//             if (_textObject == null)
//             {
//                 return;
//             }
//             if (_textAdapter == null)
//             {
//                 _textAdapter = TextAdapter.Create(_textObject);
//             }
//             _textAdapter.SetText(text);
//         }
//
//         //TEMPORARY
//         public GameObject mainContainer;
//         public GameObject buttonContainer;
//
//         public IPoolTable PoolTable
//         {
//             get
//             {
//                 return GetComponent<IPoolTable>();
//             }
//         }
//
//         public UIButton CreateButton(string text, Action onClick)
//         {
//             PoolTable.GetPrefab("Button", out GameObject prefab);
//             UIButton uiButton = Object.Instantiate(prefab, buttonContainer.transform).GetComponent<UIButton>();
//             uiButton.SetText(text);
//             uiButton.SetOnClick(onClick);
//             LayoutRebuilder.ForceRebuildLayoutImmediate(buttonContainer.transform as RectTransform);
//
//             return uiButton;
//         }
//     }
// }
