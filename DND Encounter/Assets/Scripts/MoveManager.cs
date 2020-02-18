using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MoveManager : MonoBehaviour
{
    #region Singleton
    public static MoveManager instance;

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


    Character selectedCharacter;
    GameObject selectedCharacterGameObject;
    [SerializeField] Text characterNameText;

    int normalMoveLength = 0;
    int dashingMoveLength = 0;
    [SerializeField] Text maximumMoveLengthText;
    [SerializeField] Text moveDistanceText;


    public Vector3 positionToMoveTo;

    bool isCharacterSelected = false;
    bool isDashing = false;
    bool isAllowedToMove = false;

    [SerializeField] GameObject dashOnText;
    [SerializeField] GameObject dashOffText;

    [SerializeField] GameObject infiniteMoveOnText;
    [SerializeField] GameObject infiniteMoveoffText;

    [SerializeField] GameObject indicatorGood;
    [SerializeField] GameObject indicatorBad;


    GameObject spawnedIndicator;


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
        if (selectedCharacter != null)
        {
            selectedCharacter.SetClickedOn(false);
        }
    }

    public bool CheckIfManagerIsOn()
    {
        return managerIsOn;
    }
    #endregion




    public void ResetMoveManager()
    {
        if (selectedCharacter != null)
        {
            selectedCharacter = null;
        }

        if (spawnedIndicator != null)
        {
            Destroy(spawnedIndicator);
        }

        selectedCharacterGameObject = null;

        characterNameText.text = "Character name";

        normalMoveLength = 0;
        dashingMoveLength = 0;
        maximumMoveLengthText.text = "0 feet";
        moveDistanceText.color = Color.white;
        moveDistanceText.text = "0 feet";


        positionToMoveTo = Vector3.zero;

        isCharacterSelected = false;

        SetIsDashing(false);

        HideMaxMovementCircle();

        TurnOffInfiniteMove();
    }

    public void SelectObject(GameObject input, int inputCharMaxMoveLength)
    {
        if (managerIsOn)
        {
            if (input != selectedCharacterGameObject) //if different object than already selected
            {
                isCharacterSelected = true;
                selectedCharacterGameObject = input;
                selectedCharacter = selectedCharacterGameObject.GetComponent<Character>();


                normalMoveLength = inputCharMaxMoveLength;
                dashingMoveLength = normalMoveLength * 2;
                maximumMoveLengthText.text = normalMoveLength.ToString() + " feet";

                characterNameText.text = selectedCharacterGameObject.GetComponent<Character>().GetCharacterName();

                positionToMoveTo = GridSystem.instance.GetNearestPointOnGrid(input.transform.position); //reset to char own position

                ShowMaxMovementCircle();
            }
            else
            {
                ResetMoveManager();
            }
        }
    }





    [SerializeField] GameObject maxMoveCircleBlueprint;
    GameObject spawnedMaxMoveIndicator = null;



    private void ShowMaxMovementCircle()
    {
        if (isCharacterSelected)
        {
            if (spawnedMaxMoveIndicator != null)
            {
                Destroy(spawnedMaxMoveIndicator);
            }

            spawnedMaxMoveIndicator = Instantiate(maxMoveCircleBlueprint);

            float radius;
            if (isDashing)
            {
                radius = dashingMoveLength;
            }
            else
            {
                radius = normalMoveLength;
            }
            radius /= GridSystem.instance.feetPerSquare;
            spawnedMaxMoveIndicator.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2); //radius is only half of the scale, scale == diameter of sphere
            spawnedMaxMoveIndicator.transform.position = selectedCharacterGameObject.transform.position;
        }
    }


    private void HideMaxMovementCircle()
    {
        if (spawnedMaxMoveIndicator != null)
        {
            Destroy(spawnedMaxMoveIndicator);
        }
    }





    public void ToggleIsDashing()
    {
        if (isDashing) //turn off
        {
            TurnOffDashing();
        }
        else //turn on
        {
            TurnOnDashing();
        }
    }

    public void SetIsDashing(bool input)
    {
        if (input)
        {
            TurnOnDashing();
        }
        else
        {
            TurnOffDashing();
        }
    }

    private void TurnOnDashing()
    {
        isDashing = true;
        dashOnText.SetActive(true);
        dashOffText.SetActive(false);

        maximumMoveLengthText.text = dashingMoveLength.ToString() + " feet";

        if (selectedCharacterGameObject != null)
        {
            SetDestination(positionToMoveTo);
        }
        ShowMaxMovementCircle();
    }


    bool infiniteMove = false;
    private void TurnOnInfiniteMove()
    {
        infiniteMoveoffText.SetActive(false);
        infiniteMoveOnText.SetActive(true);

        infiniteMove = true;
    }

    private void TurnOffInfiniteMove()
    {
        infiniteMoveoffText.SetActive(true);
        infiniteMoveOnText.SetActive(false);

        infiniteMove = false;

    }

    public void ToggleInfiniteMove()
    {
        infiniteMove = !infiniteMove;
         
        if(infiniteMove)
        {
            TurnOnInfiniteMove();
        }
        else
        {
            TurnOffInfiniteMove();
        }
    }

    private void TurnOffDashing()
    {
        isDashing = false;
        dashOnText.SetActive(false);
        dashOffText.SetActive(true);

        maximumMoveLengthText.text = normalMoveLength.ToString() + " feet";

        if (selectedCharacterGameObject != null)
        {
            SetDestination(positionToMoveTo);
        }
        ShowMaxMovementCircle();
    }

    private void Update()
    {
        if (managerIsOn)
        {
            if (isCharacterSelected)
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
                        SetDestination(hitInfo.point);
                    }
                }
            }
        }
    }

    public void SetDestination(Vector3 positionB)
    {
        positionB = GridSystem.instance.GetNearestPointOnGrid(positionB);
        int distanceToMove = DistanceCalculator.instance.CalculateDistance(GridSystem.instance.GetNearestPointOnGrid(selectedCharacterGameObject.transform.position), positionB);

        moveDistanceText.text = distanceToMove.ToString() + " feet";

        if (infiniteMove)
        {
            positionToMoveTo = positionB;

            ShowAllowedMove(positionB);
            return;
        }

        if (!isDashing)
        {
            if (distanceToMove <= normalMoveLength)
            {
                positionToMoveTo = positionB;

                ShowAllowedMove(positionB);
            }
            else
            {
                positionToMoveTo = positionB;

                ShowNotAllowedMove(positionB);
            }
        }
        else
        {
            if (distanceToMove <= dashingMoveLength)
            {
                positionToMoveTo = positionB;

                ShowAllowedMove(positionB);
            }
            else
            {
                positionToMoveTo = positionB;

                ShowNotAllowedMove(positionB);
            }
        }

    }

    private void ShowAllowedMove(Vector3 indicatorPos)
    {
        Debug.Log("Allowed distance");
        isAllowedToMove = true;

        if (spawnedIndicator != null)
        {
            Destroy(spawnedIndicator);
        }
        spawnedIndicator = Instantiate(indicatorGood);
        spawnedIndicator.transform.position = indicatorPos;

        moveDistanceText.color = Color.green;
    }

    private void ShowNotAllowedMove(Vector3 indicatorPos)
    {
        Debug.Log("Too far away");
        isAllowedToMove = false;

        if (spawnedIndicator != null)
        {
            Destroy(spawnedIndicator);
        }
        spawnedIndicator = Instantiate(indicatorBad);
        spawnedIndicator.transform.position = indicatorPos;

        moveDistanceText.color = Color.red;
    }

    public void MoveObject()
    {
        if (isAllowedToMove)
        {
            selectedCharacterGameObject.transform.position = positionToMoveTo;
            ShowMaxMovementCircle();

            int distanceToMove = 0;
            moveDistanceText.text = distanceToMove.ToString() + " feet";
        }
        else
        {
            ShowNotAllowedMove(positionToMoveTo);
        }
    }


}
