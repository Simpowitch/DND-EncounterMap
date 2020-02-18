using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SendMyValue : MonoBehaviour
{

    [SerializeField] Text radiusText;
    [SerializeField] Text degreeText;

    [SerializeField] int indexInCharList;


    public void SendRadiusToAOEManager()
    {
        AreaOfEffectManager.instance.SetRadius(Mathf.RoundToInt(GetComponent<Slider>().value));
        radiusText.text = Mathf.RoundToInt(GetComponent<Slider>().value).ToString();
    }

    public void SendDegreeToAOEManager()
    {
        AreaOfEffectManager.instance.SetConeDegree(Mathf.RoundToInt(GetComponent<Slider>().value));
        degreeText.text = Mathf.RoundToInt(GetComponent<Slider>().value).ToString();
    }

    public void SendShapeSelectionToAOEManager()
    {
        AreaOfEffectManager.instance.SetShape(GetComponent<Dropdown>().value);
    }

    public void SendNameToCharacterCreationSystem()
    {
        CreateCharacterSystem.instance.SetName(GetComponent<InputField>().text);
    }

    public void SendSpeedToCharacterCreationSystem()
    {
        CreateCharacterSystem.instance.SetSpeed(int.Parse(GetComponent<InputField>().text));
    }

    public void SendCharacterStyleToCreation()
    {
        CreateCharacterSystem.instance.SetCharacterStyle(GetComponent<Dropdown>().value);
    }

    public void SendColorToCreation()
    {
        CreateCharacterSystem.instance.SetColor(GetComponent<Dropdown>().value);
    }

    public void SendInitChange()
    {
        //CharacterManager.instance.ChangeInitiative(transform.parent.parent.gameObject, int.Parse(GetComponent<InputField>().text));
        CharacterManager.instance.spawnedCharactersInOrganizedList[indexInCharList].GetComponent<Character>().SetInitiative(int.Parse(GetComponent<InputField>().text));
    }

    //public void SendHPChange()
    //{
    //    CharacterManager.instance.ChangeHP(transform.parent.parent.gameObject, int.Parse(GetComponent<InputField>().text));
    //}

    public void SendGroundChoice()
    {
        GroundCreation.instance.SetMapType((MapType)GetComponent<Dropdown>().value);
    }

    public void SendObjectChoice()
    {
        ObjectSpawner.instance.SetObjectToSpawn((WorldObject)GetComponent<Dropdown>().value);
    }

    public void SendTimeOfDay()
    {
        TimeOfDay.instance.SetTimeOfDay((PartOfTheDay)GetComponent<Dropdown>().value);
    }

    public void SendWeatherDay()
    {
        TimeOfDay.instance.SetWeather((Weather)GetComponent<Dropdown>().value);
    }

}
