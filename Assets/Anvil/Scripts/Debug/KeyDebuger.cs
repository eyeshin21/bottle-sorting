#if DEBUG_MODE
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;
#if NaughtyAttributes
using NaughtyAttributes;
#endif

namespace Anvil.Legacy.Utility
{
    public class KeyDebuger : ScriptableObject,IMonoBevhaviourMessageHandler
    {
        protected static KeyDebuger _instance;

        public static KeyDebuger Instance
        {
            get
            {
                if (!_inited)
                {
                    Init();
                }

                return _instance;
            }
        }

        private static bool _inited = false;
        private static MonoBehaviourMessageForwarder _messageForwarder;

        public static void Init()
        {
            if (_inited)
            {
                return;
            }

            if (!Input.multiTouchEnabled)
            {
                Input.multiTouchEnabled = true;
            }

            KeyDebuger instance = Resources.Load<KeyDebuger>("KeyDebuger");
            _instance = instance;

            if (_messageForwarder != null)
            {
                Destroy(_messageForwarder.gameObject);
                _messageForwarder = null;
            }

            // _messageForwarder = MonoBehaviourMessageForwarder.Create(_instance);
            MonoBehaviourMessageForwarder.RegisterCommonUpdate(()=>{ Instance.OnUpdate(Time.deltaTime); });
            // Debug.Log("[KeyDebuger] Inited");
            _inited = true;
        }

        private void Reset()
        {
            // _debugUIObj.Destroy();
            // _debugUI = null;
            _inited = false;
        }

        public static GameObject selfObj = null;
        [SerializeField] private bool _allowLog = false;
        [SerializeField] GameObject _indicator;
        [SerializeField] int lagCycle;
        [SerializeField] float delayParam;
        [SerializeField] float commonUpdateInterval;
        [SerializeField] bool _scrollEnabled = false;
        //[SerializeField] int int01 = 0;
        [SerializeField] GameObject Obj1;
        [SerializeField] GameObject hierachySelectionObj;
        [SerializeField] bool _autoOrderEnabled = false;
        Dictionary<KeyCode,Action> keyMap = new Dictionary<KeyCode,Action>();
        public static KeyDebuger MainInstance=>Instance;
        private static bool _lag = false;
        private static string temporaryStoragePath = "KeyDebuger/";
        public static bool AutoOrderEnabled=>MainInstance._autoOrderEnabled;

        public static void RegisterAtKey(KeyCode keyCode,Action action,bool replace = false)
        {
            if (MainInstance.keyMap.ContainsKey(keyCode))
            {
                if (replace)
                {
                    MainInstance.keyMap[keyCode] = action;
                }
                else
                {
                    MainInstance.keyMap[keyCode] += action;
                }
            }
            else
            {
                MainInstance.keyMap.Add(keyCode,action);
            }
        }

        public static void UnregisterAtKey(KeyCode keyCode,Action action)
        {
            if (MainInstance.keyMap.ContainsKey(keyCode))
            {
                MainInstance.keyMap[keyCode] -= action;
            }
        }

        public static void Log(string msg)
        {
            if (MainInstance != null && MainInstance._allowLog)
            {
                Debug.Log(msg);
            }
        }

        public static void StartLag()
        {
            _lag = true;
        }

        public static void StopLag()
        {
            _lag = false;
        }

        public static double seed = 13242314;
        public static readonly double divider = 2.56789;

        /// <summary>
        /// Memory safe cpu lagger
        /// </summary>
        private void UpdateLagMachine()
        {
            if (!_lag)
            {
                return;
            }

            // string seed = Hash128.Parse(";lfjladjf;laksdfj;asdkfjas;l").ToString();
            int cycle = lagCycle * 10;

            for (int i = 0; i < cycle; i++)
            {
                seed /= divider;
            }
        }

        public void OnAwake()
        {
            temporaryStoragePath = Application.persistentDataPath + "/KeyDebuger/";
            _instance = this;
            // StartCoroutine(CMD2("begin KeyDebug"));
            // RefreshUI();
        }

        public void OnStart()
        {
        }

        public void OnUpdate(float deltaTime)
        {
            ProccessTouch();
            UpdateLagMachine();
            ProccessKey();
            ProccessScroll();
            // UpdateDebug();
        }

        public void OnEnable()
        {
        }

        public void OnDisable()
        {
        }

        public void OnDestroy()
        {
            Reset();
        }

        public void OnApplicationPause(bool state)
        {
        }

        // private void Awake()
        // {
        //
        // }

        // private void Update()
        // {
        //
        // }

#region InputProccess
        private void ProccessTouch()
        {
            int touchCount = Input.touchCount;
            if (touchCount >= 2)
            {
                Debug.Log("on multi touch");
                OnMultiTouchAny(touchCount);
            }
        }

        float _preTouchCount = 0;

        private void OnMultiTouchAny(int touchCount)
        {
            if (_preTouchCount == touchCount)
            {
                return;
            }

            _preTouchCount = touchCount;
            switch (touchCount)
            {
                case 2:
                    break;
                case 3:
                    // ToggleDebugUI();
                    break;
            }
        }

        private void ProccessScroll()
        {
            if (!_scrollEnabled)
            {
                return;
            }

            float scrollY = Input.mouseScrollDelta.y;
            if (Input.GetKey(KeyCode.LeftControl) && scrollY != 0)
            {
            }
            else if (scrollY != 0)
            {
            }
        }

        private void ProccessKey()
        {
            // if (Input.GetKeyDown(KeyCode.L))
            // {
            //     PlayerProgressManager.DebugLevelUp();
            // }
            foreach (var pair in keyMap)
            {
                KeyCode keyCode = pair.Key;
                if (Input.GetKeyDown(keyCode))
                {
                    keyMap[keyCode]?.Invoke();
                }
            }

            AutoClicker();
            updateInterval += Time.deltaTime;
            bool isCommonUpdate = updateInterval > commonUpdateInterval;

            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                // UserPrefs.NewDayFlags = 0;
                Helper.ReloadScene();
            }

            if (Input.GetKeyDown(KeyCode.Backslash))
            {
            }

            if (Input.GetKey(KeyCode.LeftAlt) && isCommonUpdate)
            {
            }

            if (Input.GetKeyDown(KeyCode.F1))
            {
                F1();
            }

            if (Input.GetKey(KeyCode.Space))
            {
                Space();
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                F2();
            }

            if (Input.GetKeyDown(KeyCode.F3))
            {
                F3();
            }

            if (Input.GetKey(KeyCode.F4))
            {
                F4();
            }

            if (Input.GetKey(KeyCode.F5))
            {
                F5();
            }

            if (Input.GetKeyDown(KeyCode.F6))
            {
                // StartCoroutine(CMD2("begin reward event"));
                F6();
            }

            if (Input.GetKeyDown(KeyCode.F7))
            {
                F7();
            }

            if (Input.GetKeyDown(KeyCode.F8))
            {
                F8();
            }

            if (Input.GetKeyDown(KeyCode.F10))
            {
                F10();
            }

            if (Input.GetKeyDown(KeyCode.F12))
            {
                F12();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                C();
            }
        }

        private static void C()
        {
            ClearIndicator();
        }

        private static void F12()
        {
            _lag = !_lag;
            Debug.LogWarning($"Lag {_lag}");
        }

        private static void F10()
        {
        }

        private static void F8()
        {
        }

        private static void F7()
        {
        }

        private void F6()
        {
        }

        private static void F5()
        {

           }

        private static void F4()
        {
        }

        private void F3()
        {
        }

        private static void F2()
        {
        }

        private void Space()
        {
        }

        private void F1()
        {
        }

        public static void LogOrderTable()
        {
            // Manager.Instance.StartCoroutine(CMD2(GameContext.OrderManager.logTable()));
        }
#endregion

        // private SimulatedInputModule _simulatedInputModule;

        public static string LoadTemporaryFile(string fileName)
        {
            string path = temporaryStoragePath + fileName + ".txt";
            return LoadText(path,true);
        }

        public static void SaveTemporaryFile(string fileName,string data)
        {
            string path = $"{temporaryStoragePath}{fileName}.txt";
            SaveText(data,path,true);
        }

        public static string LoadText(string path,bool isAbsolutePath = false)
        {
            if (!isAbsolutePath)
            {
                path = FileHelper.GetAbsolutePath(path);
            }

            string text = "";
            if (File.Exists(path))
            {
                var fileInfo = new FileInfo(path);
                using (var reader = fileInfo.OpenText())
                {
                    text = reader.ReadToEnd();
                    reader.Close();
                }
            }
            else
            {
                Debug.LogWarning($"\"{path}\" not found!");
            }

            return text;
        }

        public static bool SaveText(string text,string path,bool isAbsolutePath = false,
            bool createDirectoryIfNotExists = true)
        {
            if (!isAbsolutePath)
            {
                path = FileHelper.GetAbsolutePath(path);
            }

            if (createDirectoryIfNotExists)
            {
                var path2 = Path.GetDirectoryName(path);
                if (!Directory.Exists(path2))
                {
                    Directory.CreateDirectory(path2);
                }
            }

            try
            {
                var writer = new StreamWriter(path);
                writer.Write(text);
                writer.Close();
                // Debug.Log($"data written to {path}.\n{text}");
#if UNITY_EDITOR
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
#endif
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex.Message);
                return false;
            }

            return true;
        }

        //private float _autoplayCoolDown = 0.4f;
        //private float _autoplayTime = 0f;

        //private bool _clickState = false;

        private void AutoClicker()
        {
            // if (!Input.GetMouseButton(2))
            // {
            //     return;
            // }
            //
            // if (UserPrefs.Energy == 0) return;
            //
            // Vector3 mousePosition = Context.MainCamera.ScreenToWorldPoint(Input.mousePosition);
            // ClickAt(mousePosition);
        }

        public void ClickAt(Vector3 pos)
        {
            // _clickState = !_clickState;
            // // _simulatedInputModule.ClickAt(pos, _clickState);
            // if (_clickState)
            // {
            //     board.OnTouchPressed(pos);
            //     board.OnTouchReleased(pos);
            // }
            // else
            // {
            //     board.OnTouchReleased(pos);
            //     ClickAt(pos);
            // }
        }

        float updateInterval = 0;
#if NaughtyAttributes
        [NaughtyAttributes.Button]
#endif
        public void CMD()
        {
            using (Process process = new Process())
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.WorkingDirectory = @"C:\";
                process.StartInfo.FileName = Path.Combine(Environment.SystemDirectory,"cmd.exe");

                // Redirects the standard input so that commands can be sent to the shell.
                process.StartInfo.RedirectStandardInput = true;
                // Runs the specified command and exits the shell immediately.
                //process.StartInfo.Arguments = @"/c ""dir""";

                process.OutputDataReceived += ProcessOutputDataHandler;
                process.ErrorDataReceived += ProcessErrorDataHandler;

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                // Send a directory command and an exit command to the shell
                process.StandardInput.WriteLine("echo test");
                process.StandardInput.WriteLine("timeout 2");
                process.StandardInput.WriteLine("pause");

                // process.WaitForExit();
            }
        }
#if NaughtyAttributes
        [Button]
#endif
        public static IEnumerator CMD2(string value = "")
        {
#if DEBUG_MODE

            string DebugServerIP = "http://192.168.0.34";
            string DebugServeruri = $"{DebugServerIP}:5500";
            bool hasData = false;
            if (!string.IsNullOrEmpty(value))
            {
                hasData = true;
                DebugServeruri = $"{DebugServerIP}:5500/upload";
            }

            UnityWebRequest webRequest = UnityWebRequest.Get(DebugServeruri);
            if (hasData)
            {
                WWWForm form = new WWWForm();
                form.AddField("string",value);
                webRequest.Dispose();
                webRequest = UnityWebRequest.Post(DebugServeruri,form);
            }

            yield return webRequest.SendWebRequest();
            // Request and wait for the desired page.
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    // Debug.LogError(": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    // Debug.LogError(": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    // Debug.Log(":Received: " + webRequest.downloadHandler.text);
                    break;
            }

            webRequest.Dispose();
#else
            yield break;
#endif
        }

        public static void ProcessOutputDataHandler(object sendingProcess,DataReceivedEventArgs outLine)
        {
            Console.WriteLine(outLine.Data);
        }

        public static void ProcessErrorDataHandler(object sendingProcess,DataReceivedEventArgs outLine)
        {
            Console.WriteLine(outLine.Data);
        }

        [SerializeField] private List<GameObject> _indicators = new List<GameObject>();

        public static void ClearIndicator()
        {
            GameObjectPool.ClearPool(MainInstance._indicator);
            List<GameObject> indicators = MainInstance._indicators;

            for (int i = indicators.Count - 1; i >= 0; i--)
            {
                GameObject indicator = indicators[i];
                if (indicator == null)
                {
                    indicators.RemoveAt(i);
                    continue;
                }

                indicator.Destroy();
            }
        }

        public static void PlaceIndicator(Vector3 position,Color color,Transform parent = null,float lifeTime = 0f)
        {
            GameObject indicator = PlaceIndicator(position,parent,lifeTime);
            if (indicator == null)
            {
                return;
            }

            // IColorChangable colorControl = indicator.GetComponent<IColorChangable>();
            // colorControl.ChangeColor(color);
        }

        public static GameObject PlaceIndicator(Vector3 position,Transform parent = null,float lifeTime = 0f)
        {
            GameObject indicator = GameObjectPool.CreateObject(parent,MainInstance._indicator);
            if (indicator == null)
            {
                return null;
            }

            indicator.transform.position = position;
            if (lifeTime > 0)
            {
                Manager.DelayCall(lifeTime,()=>{ GameObjectPool.RemoveObject(indicator); });
            }

            Instance._indicators.Add(indicator);
            return indicator;
        }

        public static void Visualize(Easer easer,float yPos = 0)
        {
            Vector3 start = new Vector3(-5,yPos,0);
            Vector3 end = new Vector3(5,yPos,0);
            int sample = 20;
            for (int i = 1; i <= sample; i++)
            {
                float val = easer(i / (float)sample);
                Vector3 pos = Vector3.Lerp(start,end,val);
                PlaceIndicator(pos);
            }
        }

#region DebugUI
//         [SerializeField] private GameObject _debugUIPrefab;
//         [SerializeField] private bool _debugUIEnabled = false;
//         [SerializeField] private GameObject _debugUIObj;
//         private GameObject _auxiliaryDebugUIContainer;
//         private GameObject DebugUIObj
//         {
//             get
//             {
//                 if (_debugUIObj == null)
//                 {
//                     Canvas canvas = FindObjectOfType<Canvas>();
//                     if (canvas == null)
//                     {
//                         GameObject canvasObj = new GameObject("DebugCanvas");
//                         canvas = canvasObj.AddComponent<Canvas>();
//                         canvas.renderMode = RenderMode.ScreenSpaceCamera;
//                         canvasObj.AddComponent<CanvasScaler>();
//                         canvasObj.AddComponent<GraphicRaycaster>();
//                     }
//                     GameObject debugUI = GameObjectPool.CreateObject(canvas.transform,CommonUIConfig.DebuggerUIPrefab,false);
//                     debugUI.transform.localScale = Vector3.one;
//                     _debugUIObj = debugUI;
//                     _auxiliaryDebugUIContainer = new GameObject("AuxiliaryDebugUIContainer");
//                     _auxiliaryDebugUIContainer.transform.SetParent(debugUI.transform);
//                     _auxiliaryDebugUIContainer.transform.localScale = Vector3.one;
//                     // _debugUI = debugUI.GetComponent<KeyDebugerUI>();
//                 }
//
//                 return _debugUIObj;
//             }
//         }
//
//         private CommonDebugUI _debugUI;
//         private CommonDebugUI DebugUI
//         {
//             get
//             {
//                 if (_debugUI == null)
//                 {
//                     _debugUI = DebugUIObj.GetComponent<CommonDebugUI>();
//                     InitDebugUI();
//                 }
//
//                 return _debugUI;
//             }
//         }
//
//         public static void RegisterDebugUI(GameObject uiObj)
//         {
//             if (Instance._auxiliaryDebugUIContainer == null)
//             {
//                 GameObjectPool.RemoveObject(uiObj);
//                 return;
//             }
//
//             uiObj.transform.SetParent(Instance._auxiliaryDebugUIContainer.transform);
//         }
//
//         private void InitDebugUI()
//         {
//             AddDebugButton("120FPS",()=>{ Application.targetFrameRate = 120; });
//             AddDebugButton("60FPS",()=>{ Application.targetFrameRate = 60; });
//             AddDebugButton("30FPS",()=>{ Application.targetFrameRate = 30; });
//         }
//
//         public static bool DebugUIEnabled=>Instance._debugUIEnabled;
//
//
//         [FormerlySerializedAs("_text")] [SerializeField]
//         private GameObject _textObj;
//
//         private GameObject _statusTextObj;
//         public static int fps = 0;
//         public static int frameCount = 0;
//         public static float frameTimeMilSec = 0;
//         public static float fpsAvg = 0;
//         public static float frameTimeAvg = 0;
//
//         public void UpdateDebug()
//         {
//             if (frameCount < 10000)
//             {
//                 frameCount++;
//             }
//
//             frameTimeMilSec = Time.deltaTime * 1000;
//             if (frameTimeAvg == 0)
//             {
//                 frameTimeAvg = frameTimeMilSec;
//             }
//             else
//             {
//                 frameTimeAvg = (frameTimeAvg + frameTimeMilSec) / 2;
//             }
//
//             fps = (int)(1f / Time.deltaTime);
//             if (fpsAvg == 0)
//             {
//                 fpsAvg = fps;
//             }
//             else
//             {
//                 fpsAvg = fpsAvg * (frameCount - 1) + fps;
//                 fpsAvg /= frameCount;
//             }
//
//             UpdateStatusText();
//         }
//
//         private void UpdateStatusText()
//         {
//             if (!_debugUIEnabled)
//             {
//                 return;
//             }
//
//             if (Instance._statusTextObj == null)
//             {
//                 GameObject container = Instance.DebugUI.statusContainer;
//                 Instance.DebugUI.PoolTable.GetPrefab("SubText",out GameObject textprefab);
//                 GameObject textObj = GameObjectPool.CreateObject(container.transform,textprefab);
//                 Instance._statusTextObj = textObj;
//             }
//
//             if (Instance._statusTextObj == null)
//             {
//                 return;
//             }
//
//             Instance._statusTextObj.SetActive(true);
//             string text = $"FPS: {fps} FPS avg: {fpsAvg:0.0} Frame Time: {frameTimeMilSec:0.00} FrameTime avg: {frameTimeAvg:0.00}";
//             TextAdapter.SetText(Instance._statusTextObj,text);
//         }
//
//         public static void AddDebugButton(string label,Action onClick)
//         {
//             Instance.DebugUI.CreateButton(label,onClick);
//         }
//         public static void ShowText(string text)
//         {
//             text = $"\n{Color.red.ToRichTextTag()}3 FINGER TAP TO CLOSE</color>\n{text}";
//             GameObject container = Instance.DebugUI.mainContainer;
//             if (Instance._textObj == null)
//             {
//                 Instance.DebugUI.PoolTable.GetPrefab("Text",out GameObject textprefab);
//                 GameObject textObj = GameObjectPool.CreateObject(container.transform,textprefab);
//                 textObj.transform.SetSiblingIndex(0);
//                 Instance._textObj = textObj;
//             }
//
//             if (Instance._textObj == null)
//             {
//                 return;
//             }
//             Instance._textObj.SetActive(true);
//             TextAdapter.SetText(Instance._textObj,text);
//             LayoutRebuilder.MarkLayoutForRebuild(container.transform as RectTransform);
//
//         }
//
//         public static void ToggleDebugUI()
//         {
//             Instance._debugUIEnabled = !Instance._debugUIEnabled;
//             Instance.RefreshUI();
//         }
//
//         public static void ShowDebugUI()
//         {
//             Instance._debugUIEnabled = true;
//             Instance.RefreshUI();
//         }
//
//         public static void HideDebugUI()
//         {
//             Instance._debugUIEnabled = false;
//             Instance.RefreshUI();
//         }
//
//         public void RefreshUI()
//         {
//             if (DebugUIObj == null)
//             {
//                 return;
//             }
//
//             if (_debugUIEnabled)
//             {
//                 DebugUIObj.SetActive(true);
//             }
//             else
//             {
//                 DebugUIObj.SetActive(false);
//             }
//             LayoutRebuilder.ForceRebuildLayoutImmediate(DebugUIObj.transform as RectTransform);
//         }
#endregion
    }

    public class TableWriter
    {
        public int col;
        public int row;
        public List<string> data = new List<string>();

        public TableWriter(int row,int col)
        {
            this.col = col;
            this.row = row;
        }

        public void InsertDataAt(int row,int col,string value)
        {
            int index = row * this.col + col;
            data.Insert(index,value);
        }

        public string CreateTableString(System.Func<int,int,string> preElement = null, System.Func<int,int,string> posElement = null)
        {
            return Helper.CreateString((sb)=>
            {
                List<int> charLenByCol = new List<int>();
                sb.Append("<mspace=mspace=12>");
                for (int c = 0; c < col; c++)
                {
                    int maxChar = 1;
                    for (int r = 0; r < row; r++)
                    {
                        int index = r * col + c;
                        if (index < data.Count)
                        {
                            string s = data[index];
                            maxChar = Math.Max(maxChar,s.Length);
                        }
                    }

                    charLenByCol.Add(maxChar);
                }

                for (int r = 0; r < row; r++)
                {
                    for (int c = 0; c < col; c++)
                    {
                        int maxChar = charLenByCol[c];
                        if (preElement != null)
                        {
                            sb.Append(preElement(r,c));
                        }

                        sb.Append("|");
                        int index = r * col + c;
                        if (index < data.Count)
                        {
                            string s = data[index];
                            int wDiff = maxChar - s.Length;

                            sb.Append(s);
                            for (int i = 0; i < wDiff; i++)
                            {
                                sb.Append(" ");
                            }

                            if (posElement != null)
                            {
                                sb.Append(posElement(r,c));
                            }
                            // sb.Append("\t");
                        }
                        else
                        {
                            // sb.Append("_");
                            for (int i = 0; i < maxChar; i++)
                            {
                                sb.Append("_");
                            }
                        }
                    }

                    sb.AppendLine();
                }

                sb.Append("</mspace>");
            });
        }
    }
}
#endif