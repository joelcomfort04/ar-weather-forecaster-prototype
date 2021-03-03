using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ForecastDay
{
    public string averageTemp;
    public string condition;
    public string date;
    
    public TextMeshProUGUI forecastDate;
    public TextMeshProUGUI forecastTemp;
    public TextMeshProUGUI forecastDesciption;

    public bool rain = false;
    public bool snow = false;
    public bool cloudy = false;
    public bool misting = false;

    public ParticleSystem rainParticles;
    public ParticleSystem snowParticles;
    public ParticleSystem mistParticles;

    public GameObject clouds;
   

}
