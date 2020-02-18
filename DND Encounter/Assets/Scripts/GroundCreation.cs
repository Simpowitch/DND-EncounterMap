using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapType { Grass1, Grass2, GrassLeaves, GrassMuddy, Muddy, Pavement, PavementMuddy, Stone, Wood, Water}
public class GroundCreation : MonoBehaviour
{

    #region Singleton
    public static GroundCreation instance;

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


    [SerializeField] Transform groundParent;


    [SerializeField] Material[] mapMaterials;

    [SerializeField] MapType testMapType;

    private void Start() //debug
    {
        SetMapType(testMapType);
    }

    public void SetMapType(MapType input)
    {
        for (int i = 0; i < groundParent.childCount; i++)
        {
            for (int j = 0; j < groundParent.GetChild(i).childCount; j++)
            {
                groundParent.GetChild(i).GetChild(j).GetComponent<Renderer>().material = mapMaterials[(int)input];
            }
        }
    }
}
