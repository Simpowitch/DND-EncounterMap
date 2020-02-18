using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AreaOfEffectManager : MonoBehaviour
{

    #region Singleton
    public static AreaOfEffectManager instance;

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

    enum Mode { None, SetOrigin, SetTarget }
    Mode mode;


    Vector3 pointOfOrigin;
    Vector3 targetPosition;
    float radius = 1;
    enum Shape { Circle, Line, Cone }
    Shape shape;
    int coneDegree = 54;

    GameObject spawnedIndicator;

    [SerializeField] GameObject circle = null;
    [SerializeField] GameObject line = null;
    [SerializeField] GameObject cone = null;

    GameObject originIndicator = null;
    GameObject targetIndicator = null;


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
        if (spawnedIndicator != null)
        {
            Destroy(spawnedIndicator);
            spawnedIndicator = null;
        }
        managerIsOn = false;

        if (originIndicator != null)
        {
            Destroy(originIndicator);
        }

        if (targetIndicator != null)
        {
            Destroy(targetIndicator);
        }
    }
    #endregion


    public void SetMode(int input)
    {
        mode = (Mode)input;
    }

    public void SetRadius(int feet)
    {
        radius = feet / GridSystem.instance.feetPerSquare;
    }

    public void SetShape(int index)
    {
        shape = (Shape)index;
    }

    public void SetConeDegree(int degree)
    {
        coneDegree = degree;
    }

    public void SetPointOfOrigin(Vector3 point)
    {
        pointOfOrigin = GridSystem.instance.GetNearestPointOnGrid(point);

        if (originIndicator != null)
        {
            Destroy(originIndicator);
        }
        originIndicator = Instantiate(Resources.Load("Prefabs/IndicatorAllowed") as GameObject);
        originIndicator.transform.position = pointOfOrigin;
    }

    public void SetTargetPosition(Vector3 point)
    {
        targetPosition = GridSystem.instance.GetNearestPointOnGrid(point);

        if (targetIndicator != null)
        {
            Destroy(targetIndicator);
        }
        targetIndicator = Instantiate(Resources.Load("Prefabs/IndicatorAllowed") as GameObject);
        targetIndicator.transform.position = targetPosition;
    }

    public void ShowIndicator()
    {
        if (spawnedIndicator != null)
        {
            Destroy(spawnedIndicator);
        }

        switch (shape)
        {
            case Shape.Circle:
                spawnedIndicator = Instantiate(circle);
                spawnedIndicator.transform.position = targetPosition;
                spawnedIndicator.transform.localScale = new Vector3(radius*2, radius*2, radius*2);
                break;
            case Shape.Line:
                spawnedIndicator = Instantiate(line);
                spawnedIndicator.transform.localScale = new Vector3(radius*2, 0.5f, Vector3.Distance(pointOfOrigin, targetPosition));
                spawnedIndicator.transform.position = ((pointOfOrigin+targetPosition)/2);
                spawnedIndicator.transform.LookAt(targetPosition);
                break;
            case Shape.Cone:
                spawnedIndicator = Instantiate(cone);
                spawnedIndicator.transform.position = pointOfOrigin;
                spawnedIndicator.GetComponent<Cone>().SetLength(radius);
                spawnedIndicator.GetComponent<Cone>().SetDegree(coneDegree);
                spawnedIndicator.GetComponent<Cone>().SetTarget(targetPosition);
                break;
        }
    }


    private void Update()
    {
        if (managerIsOn)
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
                        case Mode.None:
                            break;
                        case Mode.SetOrigin:
                            SetPointOfOrigin(hitInfo.point);
                            break;
                        case Mode.SetTarget:
                            SetTargetPosition(hitInfo.point);
                            break;
                    }
                }
            }
        }
    }
}