using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatingManager : MonoBehaviour
{
    [SerializeField] private Text _ratingText;
    private void Start()
    {
        InitRating();
    }

    private void InitRating()
    {
        string uri = URLLibrary.MAIN + URLLibrary.GETRATING;
        var data = new Dictionary<string, string>()
        {
            {"userID", UserInfo.Instance.ID.ToString()}
        };
        Network.Instance.Post(uri,data,Success, Errorr);
    }

    private void Success(string obj)
    {
        string[] result = obj.Split('|');
        if (result.Length != 3)
        {
            Errorr("Длинна массива != 3 " + obj);
            return;
        }

        if (result[0] != "ok")
        {
            Errorr("Страный результат " + obj);
            return;
        }

        _ratingText.text = $"<color=green>{result[1]}</color> : <color=red>{result[2]}</color>";
    }

    private void Errorr(string obj)
    {
        Debug.LogError(obj);
    }
}
