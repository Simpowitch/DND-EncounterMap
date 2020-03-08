using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public GameObject gridVisual;

    public int feetPerSquare = 5;
    public int mapSize = 100;

    #region Singleton
    public static GridSystem instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Instance of " + this + " was tried to be instantiated again");
        }
    }
    #endregion

    public void ToggleGridOnOff()
    {
        if (gridVisual.activeSelf)
        {
            HideGrid();
        }
        else
        {
            ShowGrid();
        }
    }

    public void ShowGrid()
    {
        gridVisual.SetActive(true);
    }

    public void HideGrid()
    {
        gridVisual.SetActive(false);
    }



    [SerializeField] float metersPerSquare = 1f;

    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        position -= transform.position;

        int xCount = Mathf.RoundToInt(position.x / metersPerSquare);
        //int yCount = Mathf.RoundToInt(position.y / size);
        int zCount = Mathf.RoundToInt(position.z / metersPerSquare);

        Vector3 result = new Vector3(
            (float)xCount * metersPerSquare,
            //(float)yCount * size,
            0,
            (float)zCount * metersPerSquare);

        result += transform.position;

        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (float x = 0; x < mapSize; x += metersPerSquare)
        {
            for (float z = 0; z < mapSize; z += metersPerSquare)
            {
                var point = GetNearestPointOnGrid(new Vector3(x- mapSize/2, 0f, z- mapSize/2));
                Gizmos.DrawWireCube(point, new Vector3 (0.1f, 0.1f, 0.1f));
            }

        }
    }
}