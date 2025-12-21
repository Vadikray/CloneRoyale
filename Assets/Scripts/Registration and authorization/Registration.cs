using System;
using System.Collections.Generic;
using UnityEngine;

public class Registration : MonoBehaviour
{
    private const string LOGIN = "login";
    private const string PASSWORD = "password";

    private string _login;
    private string _password;
    private string _confirmPassword;

    public event Action Error;
    public event Action Success;

    public void SetLogin(string login)
    {
        _login = login;
    }

    public void SetPassword(string password)
    {
        _password = password;
    }

    public void SetConfirmPassword(string confirmPassword)
    {
        _confirmPassword = confirmPassword;
    }

    public void SignUp()
    {
        if (string.IsNullOrEmpty(_login) || string.IsNullOrEmpty(_password) || string.IsNullOrEmpty(_confirmPassword))
        {
            ErrorMessage("Логин и/или пароль пустые");
            return;
        }

        if (_password != _confirmPassword)
        {
            ErrorMessage($"пароли не совпадают: {_password} != { _confirmPassword}");
            return;
        }

        string url = URLLibrary.MAIN + URLLibrary.REGISTRATION;
        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            {LOGIN, _login},
            {PASSWORD, _password}
        };
        Network.Instance.Post(url, data, SuccessMessage, ErrorMessage);
    }

    private void SuccessMessage(string data)
    {
        if (data != "ok")
        {
            ErrorMessage("Ответ с сервера пришел вот такоей: " + data);
            return;
        }
        Debug.Log("Успешная регистрация");
        Success?.Invoke();
    }

    public void ErrorMessage(string error)
    {
        Debug.LogError(error);
        Error?.Invoke();
    }
}