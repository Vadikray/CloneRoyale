using System;
using UnityEngine;
using UnityEngine.UI;

public class AuthorizationUI : MonoBehaviour
{
    [SerializeField] private Authorization _authorization;
    [SerializeField] private InputField _login;
    [SerializeField] private InputField _password;
    [SerializeField] private Button _signIn;
    [SerializeField] private Button _signUp;
    [SerializeField] private GameObject _authorizationCanvas;
    [SerializeField] private GameObject _registrationCanvas;

    private void Awake()
    {
        _login.onEndEdit.AddListener(_authorization.SetLogin);
        _password.onEndEdit.AddListener(_authorization.SetPassword);

        _signIn.onClick.AddListener(SignInClick);
        _signUp.onClick.AddListener(SignUpClick);
        _authorization.Error += () =>
        {
            _signIn.gameObject.SetActive(true);
            _signUp.gameObject.SetActive(true);
        };
    }

    private void SignUpClick()
    {
        _authorizationCanvas.SetActive(false);
        _registrationCanvas.SetActive(true);
    }

    private void SignInClick()
    {
        _signIn.gameObject.SetActive(false);
        _signUp.gameObject.SetActive(false);
        _authorization.SignIn();
    }
}