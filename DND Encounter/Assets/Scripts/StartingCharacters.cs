using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingCharacters : MonoBehaviour
{
    public string[] characterNames;

    public int[] speed;

    public ColorIndex[] colorIndex;

    public Vector3 spawnPos;

    public GameObject[] characters;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CreateStartingCharacters()); 
    }

    IEnumerator CreateStartingCharacters()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < characterNames.Length; i++)
        {
            CreateCharacterSystem.instance.SetName(characterNames[i]);
            CreateCharacterSystem.instance.SetSpeed(speed[i]);
            CreateCharacterSystem.instance.SetColor((int)colorIndex[i]);
            CreateCharacterSystem.instance.SetPositionForSpawn(spawnPos + new Vector3(i * GridSystem.instance.feetPerSquare, 0, 0));
            CreateCharacterSystem.instance.SetCharacterStyle(0);

            CreateCharacterSystem.instance.CreateCharacter(characters[i]);
        }
    }


}
