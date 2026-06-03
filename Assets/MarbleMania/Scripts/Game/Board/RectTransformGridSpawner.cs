using NaughtyAttributes;
using UnityEngine;

namespace MarbleMania
{
    public class RectTransformGridSpawner : MonoBehaviour
    {
        [SerializeField] private int rows;
        [SerializeField] private int columns;
        [SerializeField] private float gridWidth;
        [SerializeField] private float gridHeight;

        [Button]
        public void Spawn()
        {
            int index = 0;
            float left = -gridWidth / 2;
            float top = gridHeight / 2;
            float cellWidth = gridWidth/columns;
            float cellHeight = gridHeight/rows;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    index++;
                    GameObject go = new GameObject(index.ToString());
                    go.transform.SetParent(transform);
                    go.transform.localPosition = new Vector3(left + cellWidth * c + cellWidth/2, 0, top - cellHeight * r - cellHeight/2);
                }
            }
        }
    }
}