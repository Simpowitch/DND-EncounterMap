using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PartOfTheDay { Morning, Day, Evening, Night};
public enum Weather { Clear, Cloudy, Rainy, Snowy, Foggy };


public class TimeOfDay : MonoBehaviour
{
    public GameObject[] lights;
    public GameObject[] weather;


    #region Singleton
    public static TimeOfDay instance;

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


    public void SetTimeOfDay(PartOfTheDay input)
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(false);
        }
        lights[(int)input].SetActive(true);
    }

    public void SetWeather(Weather input)
    {
        for (int i = 0; i < weather.Length; i++)
        {
            weather[i].SetActive(false);
        }
        weather[(int)input].SetActive(true);
    }
}