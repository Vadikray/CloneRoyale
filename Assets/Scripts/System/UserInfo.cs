using UnityEngine;

public class UserInfo : MonoBehaviour
{
    #region Singleton

    public static UserInfo Instance { get; private set; }

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

    public int ID { get; private set; } = 13;


    public void SetID(int id)
    {
        ID = id;
    }
}