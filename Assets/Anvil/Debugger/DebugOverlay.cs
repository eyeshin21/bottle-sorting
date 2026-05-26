using TMPro;
using UnityEngine;

namespace Anvil
{
    public class DebugOverlay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _sessionIdText;
        [SerializeField] private TMP_Text _logText;
        public void ShowSessionId(string sessionId)
        {
            _sessionIdText.text = $"Debug Session ID\n{sessionId}";
        }
        public void ShowLogs(string logs)
        {
            _logText.text = logs;
        }
        
    }
}