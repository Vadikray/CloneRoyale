using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Network : MonoBehaviour
{
    #region Singleton

    public static Network Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    public void Post(
        string url,
        Dictionary<string, string> data,
        Action<string> success,
        Action<string> error = null
    ) => StartCoroutine(PostAsync(url, data, success, error));

    private IEnumerator PostAsync(string url, Dictionary<string, string> data, Action<string> success,
        Action<string> error = null)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(url, data))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) error?.Invoke(www.error);
            else success?.Invoke(www.downloadHandler.text);
        }
    }
}