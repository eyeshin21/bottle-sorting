//     using Anvil;
//     using UnityEngine;
//     using UnityEngine.Rendering.Universal;
// namespace Anvil
// {
//     public enum GraphicQuality
//     {
//         High,
//         Low,
//     }
//     public class GraphicQualityController : SingletonBehaviour<GraphicQualityController>
//     {
//         private GraphicQuality _quality;
//         public static GraphicQuality Quality => Instance._quality;
//         public UniversalRenderPipelineAsset highQualityAsset;
//         public UniversalRenderPipelineAsset lowQualityAsset;
//
//         protected override void OnAwake()
//         {
//             if (SystemInfo.systemMemorySize < 3000 || SystemInfo.graphicsMemorySize < 1000)
//             {
//                 SetLowQuality();
//             }
//             else
//             {
//                 SetHighQuality();
//             }
//             QualitySettings.vSyncCount = 0;
//             int refresh = Screen.currentResolution.refreshRate;
//             Debug.Log("screen refresh rate: " + refresh);
//             if (refresh >= 120)
//                 Application.targetFrameRate = 120;
//             else if (refresh >= 90)
//                 Application.targetFrameRate = 90;
//             else
//                 Application.targetFrameRate = 60;
//         }
//
//         public void SetLowQuality()
//         {
//             _quality = GraphicQuality.Low;
//             QualitySettings.SetQualityLevel((int)GraphicQuality.Low);
//             Debug.Log("Low graphic set");
//         }
//
//         public void SetHighQuality()
//         {
//             _quality = GraphicQuality.High;
//             QualitySettings.SetQualityLevel((int)GraphicQuality.High);
//             Debug.Log("High graphic set");
//         }
//     }
// }