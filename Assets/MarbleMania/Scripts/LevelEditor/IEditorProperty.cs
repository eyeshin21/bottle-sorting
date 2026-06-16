using UnityEngine;

namespace MarbleMania.LevelEditor
{
    public interface IEditorProperty
    {
        public void CreatePropertyPanel(Transform parent);
    }
}