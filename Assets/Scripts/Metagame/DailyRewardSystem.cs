using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DailyRewardSystem : MonoBehaviour
{
    public string url = "www.google.com";
    public string urlDate = "http://worldclockapi.com/api/json/est/now";
    public string sDate = "";

    public TMP_Text testCoinText;
    public List<int> testRewardCoins;
    public List<Button> testRewardButtons;
    public bool delete;
    private void Start()
    {
        if (delete) PlayerPrefs.DeleteAll();
        testCoinText.text = PlayerPrefs.GetInt("TestCoin").ToString();
        StartCoroutine(CheckInternetConnection());
    }
    private IEnumerator CheckInternetConnection()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Success Internet");
                StartCoroutine(CheckDate());
            }
            else
            {
                Debug.Log("No Net");
            }
        }
    }
    private IEnumerator CheckDate()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(urlDate))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Processing");
                string[] splitDate = webRequest.downloadHandler.text.Split(new string[] { "currentDateTime\":\"" }, StringSplitOptions.None);
                sDate = splitDate[1].Substring(0, 10);
                Debug.Log(sDate);
            }
            else
            {
                Debug.Log("Error on API");
            }      
        }
        Debug.Log(sDate);
        //dailyButton.interactable = true;
    }

    public void DailyCheck()
    {
        string dateOld = PlayerPrefs.GetString("PlayDateOld");
        if (string.IsNullOrEmpty(dateOld))
        {
            Debug.Log("First Game");
            Debug.Log("First Reward");
            testRewardButtons[0].interactable = true;
            PlayerPrefs.SetString("PlayDateOld", sDate);
            PlayerPrefs.SetInt("PlayDayGameCount", 1);
        }
        else
        {
            DateTime _dateNow = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime _dateOld = Convert.ToDateTime(dateOld);

            TimeSpan diff = _dateNow.Subtract(_dateOld);
            if(diff.Days >= 1 && diff.Days < 2)
            {
                int gameCount = PlayerPrefs.GetInt("PlayeDayGameCount");
                if(gameCount == 1)
                {
                    testRewardButtons[1].interactable = true;
                    PlayerPrefs.SetInt("PlayerGameCount", 2);
                }else if(gameCount == 2)
                {
                    testRewardButtons[2].interactable = true;
                    //We Can Add more Rewards with More games here!
                }
                Debug.Log("Other Day");
                PlayerPrefs.SetString("PlayDateOld", _dateNow.ToString());
            }else if (diff.Days > 2)
            {
                testRewardButtons[0].interactable = true;
                PlayerPrefs.SetInt("PlayerGameCount", 1);
                PlayerPrefs.SetString("PlayDateOld", _dateNow.ToString());
            }
        }
    }

    public void Reward(int count)
    {
        int testCoins = PlayerPrefs.GetInt("TestCoin");
        testCoins += testRewardCoins[count];
        PlayerPrefs.SetInt("TestCoin", testCoins);
        testCoinText.text = testCoins.ToString();

        Button clickedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        clickedButton.interactable = false;

    }

}
