using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideShowToggle : MonoBehaviour
{
    [SerializeField] Animator animatorController;

    [SerializeField] bool hidden = false;

    public void ToggleHideAnimation()
    {
        if (!hidden)
        {
            animatorController.SetBool("Hide", true);
            hidden = true;
        }
        else
        {
            animatorController.SetBool("Hide", false);
            hidden = false;
        }
    }

    [SerializeField] List<GameObject> objectsToHide = new List<GameObject>();
    [SerializeField] List<GameObject> objectsToShow = new List<GameObject>();
    [SerializeField] List<GameObject> objectsToToggle = new List<GameObject>();



    public void HideObjects()
    {
        for (int i = 0; i < objectsToHide.Count; i++)
        {
            objectsToHide[i].SetActive(false);
        }
    }

    public void ShowObjects()
    {
        for (int i = 0; i < objectsToShow.Count; i++)
        {
            objectsToShow[i].SetActive(true);
        }
    }

    public void HideAndShowObjects()
    {
        ShowObjects();
        HideObjects();
    }

    public void ToggleObjectsOnOff()
    {
        for (int i = 0; i < objectsToToggle.Count; i++)
        {
            if (objectsToToggle[i].activeInHierarchy)
            {
                objectsToToggle[i].SetActive(false);
            }
            else
            {
                objectsToToggle[i].SetActive(true);
            }
        }
    }

}
