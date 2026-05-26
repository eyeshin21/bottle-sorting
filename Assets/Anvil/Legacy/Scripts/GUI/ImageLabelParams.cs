using UnityEngine;

namespace Anvil.Legacy
{
    public class ImageLabelParams
    {
        float _leftPadding;
        float _space;
        GUIDebugger _debugger;

        public float LeftPadding => _leftPadding;
        public float Space => _space;
        public GUIDebugger Debugger => _debugger;

        public ImageLabelParams()
        {

        }

        public ImageLabelParams(float leftPadding = 0, float space = 0, GUIDebugger debugger = null)
        {
            _leftPadding = leftPadding;
            _space = space;
            _debugger = debugger;
        }

        public ImageLabelParams SetLeftPadding(float leftPadding)
        {
            _leftPadding = leftPadding;
            return this;
        }

        public ImageLabelParams SetSpace(float space)
        {
            _leftPadding = space;
            return this;
        }

        public ImageLabelParams SetDebugger(GUIDebugger debugger)
        {
            _debugger = debugger;
            return this;
        }

        void Clear()
        {
            _leftPadding = 0;
            _space = 0;
            _debugger = null;
        }

        static Pool<ImageLabelParams> _pool = new Pool<ImageLabelParams>();
        public static ImageLabelParams Get()
        {
            var @params = _pool.Get();
            return @params;
        }

        public static void Return(ImageLabelParams @params)
        {
            if (@params != null)
            {
                @params.Clear();
                _pool.Return(@params);
            }
        }
    }
}