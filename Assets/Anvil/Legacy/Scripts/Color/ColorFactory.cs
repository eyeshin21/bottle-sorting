using UnityEngine;

namespace Anvil.Legacy
{
    public class ColorFactory
    {
        Color[][] _colors;
        int _groupCount;
        int _currentGroupIndex;
        int[] _colorIndices;

        public ColorFactory(params Color[][] colors)
        {
            _colors = colors;
            _groupCount = colors.Length;
            _colorIndices = new int[_groupCount];
            Reset();
        }

        public Color GetNextColor()
        {
            _currentGroupIndex++;
            if (_currentGroupIndex >= _groupCount)
            {
                _currentGroupIndex = 0;
            }

            var colors = _colors[_currentGroupIndex];
            int colorCount = colors.Length;
            int colorIndex = _colorIndices[_currentGroupIndex];
            colorIndex++;
            if (colorIndex >= colorCount)
            {
                colorIndex = 0;
            }
            _colorIndices[_currentGroupIndex] = colorIndex;

            return colors[colorIndex];
        }

        public void Reset()
        {
            _currentGroupIndex = -1;
            for (int i = 0; i < _groupCount; i++)
            {
                _colorIndices[i] = -1;
            }
        }
    }
}