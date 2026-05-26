using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Anvil
{
    public class ClipboardTableWraper
    {
        private List<string> _headerNames = new List<string>();
        private string[,] elements;
        private int _rowCount;
        private int _columnCount;

        public int RowCount=>_rowCount;
        public int ColumnCount=>_columnCount;

        public ClipboardTableWraper(string sourceString)
        {

            string[] lines = sourceString.Split('\n');
            string line0 = lines[0];
            _headerNames = line0.Split("\t").ToList();
            for (int i = 0; i < _headerNames.Count; i++)
            {
                _headerNames[i] = _headerNames[i].Trim();
            }
            _rowCount = lines.Length - 1; // excluding header line
            _columnCount = _headerNames.Count;
            elements = new string[_rowCount, _columnCount];
            for (int lineIndex = 1; lineIndex < lines.Length; lineIndex++)
            {
                Debug.Log($"Row{lineIndex}");
                string line = lines[lineIndex];
                string[] parts = line.Split('\t');
                for (int i = 0; i < parts.Length; i++)
                {
                    elements[lineIndex - 1, i] = parts[i];
                    Debug.Log($"Col{i}: {parts[i]}");
                }
            }
        }

        public string GetValueAt(int row, string columnName)
        {
            int columnIndex = _headerNames.IndexOf(columnName);
            if (columnIndex == -1)
            {
                Debug.LogError($"Column name {columnName} not found in the csv");
                return null;
            }
            if (row < 0 || row >= _rowCount)
            {
                Debug.LogError($"row index {row} out of range");
                return null;
            }
            return elements[row, columnIndex];
        }

        public string[] GetRow(int index)
        {
            if (index < 0 || index >= _rowCount)
            {
                Debug.LogError($"row index {index} out of range");
                return null;
            }
            string[] row = new string[_columnCount];
            for (int i = 0; i < _columnCount; i++)
            {
                row[i] = elements[index, i];
            }

            return row;
        }
    }
}
