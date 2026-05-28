using UnityEngine;

public class TrayGrid : MonoBehaviour
{
    [SerializeField] private int _rowCount;
    [SerializeField] private int _columnCount;
    [SerializeField] private GameObject _trayPrefab;
    [SerializeField] private Tray[] _trays;
    [SerializeField] private Tray[,] _trayMap;

    public Tray[]  Trays => _trays;
    
    private void Construct()
    {
        _trayMap = new Tray[_rowCount, _columnCount];
    }
    // public void 
}