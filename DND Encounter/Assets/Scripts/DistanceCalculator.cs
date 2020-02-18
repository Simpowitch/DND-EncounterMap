using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DistanceCalculator : MonoBehaviour
{
    #region Singleton
    public static DistanceCalculator instance;

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


    public enum CalculatorMode { None, SetA, SetB }
    CalculatorMode mode;

    [SerializeField] GameObject indicatorObject;

    GameObject pointIndicatorA;
    GameObject pointIndicatorB;

    [SerializeField] Text distanceText;

    Vector3 pointA;
    Vector3 pointB;


    #region ManagerOnOff

    bool managerIsOn = false;
    public void ToggleManagerOnOff()
    {
        if (managerIsOn)
        {
            TurnManagerOff();
        }
        else
        {
            TurnManagerOn();
        }
    }

    public void SetManagerOnOff(bool input)
    {
        if (input)
        {
            TurnManagerOn();
        }
        else
        {
            TurnManagerOff();
        }
    }

    private void TurnManagerOn()
    {
        managerIsOn = true;
    }

    private void TurnManagerOff()
    {
        managerIsOn = false;
    }
    #endregion



    public void ResetCalculator()
    {
        if (pointIndicatorA != null)
        {
            Destroy(pointIndicatorA);
        }
        if (pointIndicatorB != null)
        {
            Destroy(pointIndicatorB);
        }
        mode = CalculatorMode.None;
        distanceText.text = "0 feet";
    }
    


    public void SetCalculatorMode(int input)
    {
        mode = (CalculatorMode)input;
    }

    private void Update()
    {
        if (managerIsOn)
        {
            if (mode != CalculatorMode.None)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (EventSystem.current.IsPointerOverGameObject())
                    {
                        return;
                    }

                    RaycastHit hitInfo;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        switch (mode)
                        {
                            case CalculatorMode.SetA:
                                SetPointA(hitInfo.point);
                                break;
                            case CalculatorMode.SetB:
                                SetPointB(hitInfo.point);
                                break;
                        }
                    }
                }
            }
        }
    }


    public void SetPointA(Vector3 pos)
    {
        pointA = pos;
        pointA = GridSystem.instance.GetNearestPointOnGrid(pointA);

        if (pointIndicatorA != null)
        {
            Destroy(pointIndicatorA);
        }

        pointIndicatorA = Instantiate(indicatorObject);
        pointIndicatorA.transform.position = pointA;

        if (pointIndicatorA != null && pointIndicatorB != null)
        {
            CalculateDistance();
        }
    }

    public void SetPointB(Vector3 pos)
    {
        pointB = pos;
        pointB = GridSystem.instance.GetNearestPointOnGrid(pointB);

        if (pointIndicatorB != null)
        {
            Destroy(pointIndicatorB);
        }

        pointIndicatorB = Instantiate(indicatorObject);
        pointIndicatorB.transform.position = pointB;

        if (pointIndicatorA != null && pointIndicatorB != null)
        {
            CalculateDistance();
        }
    }

    void CalculateDistance()
    {
        float distanceFloat = Vector3.Distance(pointA, pointB);

        int distanceInt = Mathf.RoundToInt(distanceFloat);
        distanceText.text = "Distance: " + (distanceInt * GridSystem.instance.feetPerSquare).ToString() + " feet";
    }


    //external method
    public int CalculateDistance(Vector3 a, Vector3 b)
    {
        float distanceFloat = Vector3.Distance(a, b);

        int distanceInt = Mathf.RoundToInt(distanceFloat);
        int distanceFeet = distanceInt * GridSystem.instance.feetPerSquare;
        return distanceFeet;
    }
}