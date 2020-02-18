using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterManager : MonoBehaviour
{
    #region Singleton
    public static CharacterManager instance;

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


    public List<GameObject> spawnedCharactersInOrganizedList = new List<GameObject>();
    List<int> initiativeList = new List<int>();
    //List<int> hpList = new List<int>();

    GameObject[] characterUIElements;
    [SerializeField] Transform characterUIListParent;
    [SerializeField] GameObject activeCharacterInListIndicator;

    float activePlayerIndicatorStartYpos;

    int activeCharacterIndex = 0;

    int round;
    [SerializeField] Text roundText;

    private void Start()
    {
        characterUIElements = new GameObject[characterUIListParent.childCount];
        for (int i = 0; i < characterUIListParent.childCount; i++)
        {
            characterUIElements[i] = characterUIListParent.GetChild(i).gameObject;
        }
        activePlayerIndicatorStartYpos = activeCharacterInListIndicator.GetComponent<RectTransform>().localPosition.y;
    }

    public void ShowAllNametags()
    {
        for (int i = 0; i < spawnedCharactersInOrganizedList.Count; i++)
        {
            spawnedCharactersInOrganizedList[i].GetComponent<Character>().ShowHideNametag(true);
        }
    }

    public void HideAllNametags()
    {
        for (int i = 0; i < spawnedCharactersInOrganizedList.Count; i++)
        {
            spawnedCharactersInOrganizedList[i].GetComponent<Character>().ShowHideNametag(false);
        }
    }

    public void AddCharacter(GameObject newCharacter)
    {
        spawnedCharactersInOrganizedList.Add(newCharacter);

        AddCharacterToInitiativeList();
    }

    public void RemoveCharacter(GameObject characterToRemove)
    {
        GameObject activeCharacter = spawnedCharactersInOrganizedList[activeCharacterIndex];

        int charIndex = 0;

        for (int i = 0; i < spawnedCharactersInOrganizedList.Count; i++)
        {
            if (spawnedCharactersInOrganizedList[i] == characterToRemove)
            {
                charIndex = i;
                break;
            }
        }

        initiativeList.Remove(initiativeList[charIndex]);
        spawnedCharactersInOrganizedList.Remove(spawnedCharactersInOrganizedList[charIndex]);

        ReOrganizeInitiativeList();

        for (int i = 0; i < spawnedCharactersInOrganizedList.Count; i++)
        {
            if (activeCharacter == spawnedCharactersInOrganizedList[i])
            {
                activeCharacterIndex = i;
            }
        }


        IndicateActiveCharacter();
    }

    private void AddCharacterToInitiativeList()
    {
        initiativeList.Add(0);
        //hpList.Add(0);

        characterUIElements[spawnedCharactersInOrganizedList.Count - 1].SetActive(true);

        characterUIElements[spawnedCharactersInOrganizedList.Count - 1].transform.GetChild(0).GetComponent<Image>().color = spawnedCharactersInOrganizedList[spawnedCharactersInOrganizedList.Count - 1].GetComponent<Character>().GetMyColor();
        characterUIElements[spawnedCharactersInOrganizedList.Count - 1].transform.GetChild(1).GetComponent<Text>().text = spawnedCharactersInOrganizedList[spawnedCharactersInOrganizedList.Count - 1].GetComponent<Character>().GetCharacterName();
        //allCharactersInList[allSpawnedCharacters.Count - 1].transform.GetChild(2).GetComponent<Text>().text = allSpawnedCharacters[allSpawnedCharacters.Count - 1].GetComponent<Character>().GetHP().ToString();
        characterUIElements[spawnedCharactersInOrganizedList.Count - 1].transform.GetChild(3).GetComponent<Text>().text = spawnedCharactersInOrganizedList[spawnedCharactersInOrganizedList.Count - 1].GetComponent<Character>().GetInitiative().ToString();

        ReOrganizeInitiativeList();
    }

    //public void ChangeHP(GameObject characterToChange, int newHP)
    //{
    //    int charIndex = 0;

    //    for (int i = 0; i < allSpawnedCharacters.Count; i++)
    //    {
    //        if (allSpawnedCharacters[i] == characterToChange)
    //        {
    //            charIndex = i;
    //            break;
    //        }
    //    }
    //    hpList[charIndex] = newHP;
    //    allSpawnedCharacters[charIndex].GetComponent<Character>().SetHP(newHP);
    //    allCharactersInList[charIndex].transform.GetChild(2).GetComponent<Text>().text = newHP.ToString();
    //}

    public void ChangeInitiative(GameObject characterToChange, int newInitiative)
    {
        int charIndex = 0;

        for (int i = 0; i < spawnedCharactersInOrganizedList.Count; i++)
        {
            if (spawnedCharactersInOrganizedList[i] == characterToChange)
            {
                charIndex = i;
                break;
            }
        }
        initiativeList[charIndex] = newInitiative;

        //spawnedCharactersInOrganizedList[charIndex].GetComponent<Character>().SetInitiative(newInitiative);

        characterUIElements[charIndex].transform.GetChild(3).GetComponent<Text>().text = newInitiative.ToString();

        ReOrganizeInitiativeList();
    }

    private void ReOrganizeInitiativeList()
    {
        List<int> sortedInitList = new List<int>();
        List<GameObject> sortedObjectList = new List<GameObject>();

        for (int i = 0; i < initiativeList.Count; i++)
        {
            int indexWithHighestInitiative = 0;
            int highestInitiative = 0;

            for (int j = 0; j < initiativeList.Count; j++)
            {
                if (initiativeList[j] > highestInitiative)
                {
                    indexWithHighestInitiative = j;
                    highestInitiative = initiativeList[j];
                }
            }

            sortedInitList.Add(initiativeList[indexWithHighestInitiative]);
            initiativeList.Remove(initiativeList[indexWithHighestInitiative]);

            sortedObjectList.Add(spawnedCharactersInOrganizedList[indexWithHighestInitiative]);
            spawnedCharactersInOrganizedList.Remove(spawnedCharactersInOrganizedList[indexWithHighestInitiative]);

            i--;
        }

        for (int i = 0; i < characterUIElements.Length; i++)
        {
            characterUIElements[i].SetActive(false);
        }

        for (int i = 0; i < sortedObjectList.Count; i++)
        {
            spawnedCharactersInOrganizedList.Add(sortedObjectList[i]);
            initiativeList.Add(sortedInitList[i]);

            characterUIElements[i].SetActive(true);

            characterUIElements[i].transform.GetChild(0).GetComponent<Image>().color = spawnedCharactersInOrganizedList[i].GetComponent<Character>().GetMyColor();
            characterUIElements[i].transform.GetChild(1).GetComponent<Text>().text = spawnedCharactersInOrganizedList[i].GetComponent<Character>().GetCharacterName();
            //allCharactersInList[i].transform.GetChild(2).GetComponent<Text>().text = allSpawnedCharacters[i].GetComponent<Character>().GetHP().ToString();
            characterUIElements[i].transform.GetChild(3).GetComponent<Text>().text = spawnedCharactersInOrganizedList[i].GetComponent<Character>().GetInitiative().ToString();
        }
    }


    public void ResetList()
    {

    }



    public void NextCharacter()
    {
        activeCharacterIndex++;

        if (activeCharacterIndex >= spawnedCharactersInOrganizedList.Count)
        {
            IncreaseRound();
            activeCharacterIndex = 0;
        }

        IndicateActiveCharacter();
    }

    private void IndicateActiveCharacter()
    {
        activeCharacterInListIndicator.GetComponent<RectTransform>().localPosition = new Vector3(0, activePlayerIndicatorStartYpos - (activeCharacterIndex * 40), 0);
    }

    public void ResetRoundCounter()
    {
        round = 0;
        roundText.text = round.ToString();
        activeCharacterIndex = 0;
        IndicateActiveCharacter();
    }

    private void IncreaseRound()
    {
        round++;
        roundText.text = round.ToString();
    }

}
