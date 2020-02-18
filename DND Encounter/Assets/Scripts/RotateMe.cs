using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMe : MonoBehaviour
{
    Vector3 rotation = new Vector3(0, 90, 0);

    private void OnMouseDown()
    {
        if (ObjectSpawner.instance.CheckIfManagerIsOn())
        {
            transform.Rotate(rotation);
        }
    }
}