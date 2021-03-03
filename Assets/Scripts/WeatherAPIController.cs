using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;
using System.Linq;
using System.Collections.Generic;


public class WeatherAPIController : MonoBehaviour
{

    public GameObject forecastCube;


    public TMP_InputField postalInput;

    public List<ForecastDay> days = new List<ForecastDay>();

    [SerializeField]
    GameObject[] forecastSquares;





    public const string API_KEY = "ca14bca92a834da3a16171558211702";


    private readonly string baseWeatherURL = "http://api.weatherapi.com/v1/";


    

    ForecastDay forecast1;
    ForecastDay forecast2;
    ForecastDay forecast3;

    public void OnButtonGetWeather()
    {


        forecast1 = new ForecastDay();
        days.Add(forecast1);
        forecast2 = new ForecastDay();
        days.Add(forecast2);
        forecast3 = new ForecastDay();
        days.Add(forecast3);

        for(int k = 0; k <3; k++)
        {
            GameObject billboard = forecastSquares[k].transform.GetChild(0).gameObject;
            GameObject canvas = billboard.transform.GetChild(0).gameObject;
            GameObject date = canvas.transform.GetChild(0).gameObject;
            GameObject temp = canvas.transform.GetChild(1).gameObject;
            GameObject desc = canvas.transform.GetChild(2).gameObject;
            

            days[k].snowParticles = forecastSquares[k].transform.GetChild(1).GetComponent<ParticleSystem>();
            days[k].rainParticles = forecastSquares[k].transform.GetChild(2).GetComponent<ParticleSystem>();
            days[k].mistParticles = forecastSquares[k].transform.GetChild(4).GetComponent<ParticleSystem>();

            GameObject clouds = days[k].clouds = forecastSquares[k].transform.GetChild(3).gameObject;

            days[k].forecastDate = date.GetComponent<TextMeshProUGUI>();
            days[k].forecastTemp = temp.GetComponent<TextMeshProUGUI>();
            days[k].forecastDesciption = desc.GetComponent<TextMeshProUGUI>();
        }


        string zipCode = postalInput.text;


        int zipLength = zipCode.Length;

        bool zipHasLetters = zipCode.Any(x => char.IsLetter(x));


        if (zipCode == null || zipLength != 5 || zipHasLetters)
        {
            Debug.Log("Invalid Postal Code");
         
        }
        else
        {
           
            StartCoroutine(GetWeatherAtZip(zipCode));
        }

        

    }

    IEnumerator GetWeatherAtZip(string zipCode)
    {
        
        //http://api.weatherapi.com/v1/forecast.json?key=ca14bca92a834da3a16171558211702&q=55126&days=7


        //get weather info
        string weatherForecastURL = baseWeatherURL + "forecast.json?key=" + API_KEY + "&q=" + zipCode + "&days=3";
        Debug.Log(weatherForecastURL);



        UnityWebRequest weatherForecastRequest = UnityWebRequest.Get(weatherForecastURL);

        yield return weatherForecastRequest.SendWebRequest();


#pragma warning disable CS0618 // Type or member is obsolete
        if (weatherForecastRequest.isNetworkError || weatherForecastRequest.isHttpError)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            Debug.LogError(weatherForecastRequest.error);
            yield break;
        }

        
        JSONNode weatherForecastInfo = JSON.Parse(weatherForecastRequest.downloadHandler.text);

        


        for (int i = 0; i < 3; i++)
        {
            days[i].averageTemp = weatherForecastInfo["forecast"]["forecastday"][i]["day"]["avgtemp_f"];
            days[i].condition = weatherForecastInfo["forecast"]["forecastday"][i]["day"]["condition"]["text"];
            days[i].date = weatherForecastInfo["forecast"]["forecastday"][i]["date"];


        }

        foreach(ForecastDay day in days)
        {
            Debug.Log(day.condition);
        }

        for(int j = 0; j <3; j++)
        {
            
            days[j].forecastDate.text = days[j].date;
            days[j].forecastTemp.text = "Avg: "+ days[j].averageTemp + "F";
            days[j].forecastDesciption.text = days[j].condition;
            CheckWeatherDescription(days[j].condition.ToLower(), days[j]);
            DisplayWeatherParticles(days[j]);
        }



    }


    public void CheckWeatherDescription(string desc, ForecastDay day)
    {
        day.snow = false;
        day.misting = false;
        day.cloudy = false;
        day.rain = false;

        if (desc.Contains("cloud"))
        {
            day.cloudy = true;
        }
        if (desc.Contains("rain"))
        {
            day.rain = true;
            day.cloudy = true;
          
        }
        if (desc.Contains("snow"))
        {
            day.snow = true;
            day.cloudy = true;
           
        }
        if (desc.Contains("mist"))
        {
            day.cloudy = true;
            day.misting = true;
            
        }

    
        
    }

    public void DisplayWeatherParticles(ForecastDay day)
    {
        day.snowParticles.gameObject.SetActive(false);
        day.rainParticles.gameObject.SetActive(false);
        day.mistParticles.gameObject.SetActive(false);
        day.clouds.gameObject.SetActive(false);

        if (day.cloudy)
        {
            day.clouds.gameObject.SetActive(true);
        }
        if (day.snow)
        {
            day.snowParticles.gameObject.SetActive(true);

        }
        if (day.rain)
        {
            day.rainParticles.gameObject.SetActive(true);

        }
        if (day.misting)
        {
            day.mistParticles.gameObject.SetActive(true);

        }

    }



}
