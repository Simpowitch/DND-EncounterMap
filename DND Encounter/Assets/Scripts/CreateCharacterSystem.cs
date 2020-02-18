using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

   public enum ColorIndex { Black, Red, Yellow, Green, Blue, Magenta, Cyan, Orange, DarkBlue, Purple, Pink }
public class CreateCharacterSystem : MonoBehaviour
{
    #region Singleton
    public static CreateCharacterSystem instance;

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
        ResetManager();
    }
    #endregion

    string characterName;
    int speed = 30;
    ColorIndex colorIndex;
    Color colorToSpawn = Color.black;

    enum CharacterStyle { BasicFigure }
    CharacterStyle style;

    Vector3 positionToSpawn;

    [SerializeField] GameObject indicatorGood;
    GameObject spawnedIndicator;

    [SerializeField] GameObject characterSpawnParent;

    [SerializeField] GameObject basicFigureBlueprint;
    //more chars




    GameObject characterToSpawn = null;
    GameObject lastSpawnedObject;


    public void ResetManager()
    {
        characterName = "";
        speed = 30;
        positionToSpawn = Vector3.zero;
        lastSpawnedObject = null;
        if (spawnedIndicator != null)
        {
            Destroy(spawnedIndicator);
        }
        SetColor(0);
    }

    public void SetName(string input)
    {
        characterName = input;
    }

    public void SetSpeed(int input)
    {
        speed = input;
    }

    public void SetColor(int input)
    {
        colorIndex = (ColorIndex)input;
        colorToSpawn = Color.black;
        switch (colorIndex)
        {
            case ColorIndex.Black:
                colorToSpawn = Color.black;
                break;
            case ColorIndex.Red:
                colorToSpawn = Color.red;
                break;
            case ColorIndex.Yellow:
                colorToSpawn = Color.yellow;
                break;
            case ColorIndex.Green:
                colorToSpawn = Color.green;
                break;
            case ColorIndex.Blue:
                colorToSpawn = Color.blue;
                break;
            case ColorIndex.Magenta:
                colorToSpawn = Color.magenta;
                break;
            case ColorIndex.Cyan:
                colorToSpawn = Color.cyan;
                break;
            case ColorIndex.Orange:
                colorToSpawn = new Color(1.0f, 0.64f, 0.0f);
                break;
            case ColorIndex.DarkBlue:
                colorToSpawn = new Color(0.23f, 0.26f, 0.4f);
                break;
            case ColorIndex.Purple:
                colorToSpawn = new Color(0.5f, 0.0f, 0.5f);
                break;
            case ColorIndex.Pink:
                colorToSpawn = new Color(1f, 0.6f, 0.6f);
                break;
        }
    }

    public void SetCharacterStyle(int characterStyleIndex)
    {
        style = (CharacterStyle)characterStyleIndex;
    }

    public void SetPositionForSpawn(Vector3 input)
    {
        if (spawnedIndicator != null)
        {
            Destroy(spawnedIndicator);
        }

        positionToSpawn = GridSystem.instance.GetNearestPointOnGrid(input);

        spawnedIndicator = Instantiate(indicatorGood);
        spawnedIndicator.transform.position = positionToSpawn;
    }

    public void CreateCharacter()
    {
        switch (style)
        {
            case CharacterStyle.BasicFigure:
                characterToSpawn = basicFigureBlueprint;
                break;
        }

        SpawnCharacter();
    }

    public void CreateCharacter(GameObject gameObjectToSpawn)
    {
        characterToSpawn = gameObjectToSpawn;
        SpawnCharacter();
    }

    private void SpawnCharacter()
    {
        if (CheckIfReadyForSpawn())
        {
            lastSpawnedObject = Instantiate(characterToSpawn);
            lastSpawnedObject.transform.position = positionToSpawn;

            lastSpawnedObject.GetComponent<Character>().SetMovementLength(speed);
            lastSpawnedObject.GetComponent<Character>().SetCaracterName(characterName);
            lastSpawnedObject.GetComponent<Character>().SetMyColor(colorToSpawn);

            lastSpawnedObject.transform.SetParent(characterSpawnParent.transform);

            CharacterManager.instance.AddCharacter(lastSpawnedObject);

            if (spawnedIndicator != null)
            {
                Destroy(spawnedIndicator);
            }
            positionToSpawn = Vector3.zero;
        }
        else
        {
            Debug.LogError("Character not ready to be spawned");
        }
    }

    private bool CheckIfReadyForSpawn()
    {
        if (characterName != "" && speed != 0 && positionToSpawn != Vector3.zero && characterToSpawn != null)
        {
            return true;
        }
        else
        {
            return false;
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
                }
            }
        }
    }
}