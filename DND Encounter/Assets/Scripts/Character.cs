using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Character : MonoBehaviour
{
    bool clickedOn = false;
    int movementLength = 30;
    string characterName = "";

    [SerializeField] GameObject nameTag;
    [SerializeField] Text nameTagName;
    [SerializeField] Image nameTagBackground;
    Color myColor;
    int initiative = 0;
    int hp;

    public void DeleteMe()
    {
        CharacterManager.instance.RemoveCharacter(this.gameObject);
        Destroy(this.gameObject);
    }

    private void Start()
    {
        nameTag.SetActive(false);
    }

    public void ToggleClickedOn()
    {
        if (clickedOn)
        {
            SetClickedOn(false);
        }
        else
        {
            SetClickedOn(true);
        }
    }

    public void SetClickedOn(bool input)
    {
        if (!input)
        {
            clickedOn = false;
            nameTag.SetActive(false);
            if (MoveManager.instance.CheckIfManagerIsOn())
            {
                MoveManager.instance.SelectObject(this.gameObject, movementLength);
            }
        }
        else
        {
            clickedOn = true;
            if (MoveManager.instance.CheckIfManagerIsOn())
            {
                MoveManager.instance.SelectObject(this.gameObject, movementLength);
            }
            else
            {
                nameTag.SetActive(true);
            }
        }
    }

    public void ShowHideNametag(bool turnOn)
    {
        if (turnOn)
        {
            nameTag.SetActive(true);
        }
        else
        {
            nameTag.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        ToggleClickedOn();
    }

    public void SetCaracterName(string input)
    {
        characterName = input;
        nameTagName.text = input;
    }

    public string GetCharacterName()
    {
        return characterName;
    }

    public void SetMovementLength(int input)
    {
        movementLength = input;
    }

    public void SetMyColor(Color input)
    {
        nameTagBackground.color = input;
        GetComponent<Renderer>().material.SetColor("_Color", input);
        myColor = input;
    }

    public Color GetMyColor()
    {
        return myColor;
    }

    public void SetInitiative(int input)
    {
        initiative = input;
        CharacterManager.instance.ChangeInitiative(this.gameObject, input);
    }

    public int GetInitiative()
    {
        return initiative;
    }

    public void SetHP(int input)
    {
        hp = input;
    }

    public int GetHP()
    {
        return hp;
    }
}
