using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public enum WorldObject { Tree1, Tree2, Tree3, Tree4, House1, House2, House3, House4, House1Large, House2Large, House3Large, House4Large, Cliff, Rock1, Rock2, Rock3, Rock4, Wall }
public class ObjectSpawner : MonoBehaviour
{
    #region Singleton
    public static ObjectSpawner instance;

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
        SetIsDeletingObjects(false);
        if (spawnedIndicator != null)
        {
            Destroy(spawnedIndicator);
        }
    }

    public bool CheckIfManagerIsOn()
    {
        return managerIsOn;
    }
    #endregion

    WorldObject objectToSpawn;

    Vector3 spawnPosition = Vector3.zero;


    [SerializeField] GameObject indicatorGood;
    GameObject spawnedIndicator;

    [SerializeField] Transform worldObjectParent;

    [SerializeField] GameObject[] blueprints;

    List<GameObject> allSpawnedObjects = new List<GameObject>();

    bool isDeletingObjects = false;
    [SerializeField] GameObject onText;
    [SerializeField] GameObject offText;


    public void ToggleIsDeletingObjects()
    {
        if (isDeletingObjects)
        {
            isDeletingObjects = false;
            ShowHideIsDeletingObjectsText(false);
        }
        else
        {
            isDeletingObjects = true;
            ShowHideIsDeletingObjectsText(true);
        }
    }

    public void SetIsDeletingObjects(bool input)
    {
        isDeletingObjects = input;
        ShowHideIsDeletingObjectsText(input);
    }

    private void ShowHideIsDeletingObjectsText(bool input)
    {
        if (input)
        {
            onText.SetActive(true);
            offText.SetActive(false);
        }
        else
        {
            onText.SetActive(false);
            offText.SetActive(true);
        }
    }


    public void SetPositionForSpawn(Vector3 input)
    {
        if (spawnedIndicator != null)
        {
            Destroy(spawnedIndicator);
        }

        spawnPosition = GridSystem.instance.GetNearestPointOnGrid(input);

        spawnedIndicator = Instantiate(indicatorGood);
        spawnedIndicator.transform.position = spawnPosition;
    }

    public void SetObjectToSpawn(WorldObject objectInput)
    {
        objectToSpawn = objectInput;
    }


    public void SpawnObject()
    {
        if (CheckIfReadyForSpawn())
        {
            GameObject lastSpawnedObject = Instantiate(blueprints[(int)objectToSpawn]);
            lastSpawnedObject.transform.position = spawnPosition;


            lastSpawnedObject.transform.SetParent(worldObjectParent);

            lastSpawnedObject.transform.gameObject.tag = "World Object";

            lastSpawnedObject.AddComponent<RotateMe>();

            allSpawnedObjects.Add(lastSpawnedObject);

            spawnPosition = Vector3.zero;
            Destroy(spawnedIndicator);
        }
        else
        {
            Debug.LogError("Object not ready to be spawned");
        }
    }

    private bool CheckIfReadyForSpawn()
    {
        if (spawnPosition != Vector3.zero)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetWorld()
    {
        for (int i = 0; i < allSpawnedObjects.Count; i++)
        {
            Destroy(allSpawnedObjects[i]);
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
                    if (hitInfo.collider.gameObject.tag == "Ground")
                    {
                        SetPositionForSpawn(hitInfo.point);
                    }
                    else if (hitInfo.collider.gameObject.tag == "World Object")
                    {
                        if (isDeletingObjects)
                        {
                            allSpawnedObjects.Remove(hitInfo.collider.gameObject);
                            Destroy(hitInfo.collider.gameObject);
                        }
                    }
                }
            }
        }
    }
}
