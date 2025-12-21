using UnityEngine;
using UnityEngine.UI;

public class RegistrationUI : MonoBehaviour
{
    [SerializeField] private Registration _registration;
    [SerializeField] private InputField _login;
    [SerializeField] private InputField _password;
    [SerializeField] private InputField _confirmPassword;
    [SerializeField] private Button _apply;
    [SerializeField] private Button _signIn;
    
    [SerializeField] private GameObject _authorizationCanvas;
    [SerializeField] private GameObject _registrationCanvas;

    private void Awake()
    {
        _login.onEndEdit.AddListener(_registration.SetLogin);
        _password.onEndEdit.AddListener(_registration.SetPassword);
        _confirmPassword.onEndEdit.AddListener(_registration.SetConfirmPassword);

        _apply.onClick.AddListener(SignUpClick);
        _signIn.onClick.AddListener(SignInClick);
        _registration.Error += () =>
        {
            _apply.gameObject.SetActive(true);
            _signIn.gameObject.SetActive(true);
        };
        _registration.Success += () =>
        {
            _signIn.gameObject.SetActive(true);
        };
    }
    private void SignInClick()
    {
        _registrationCanvas.SetActive(false);
        _authorizationCanvas.SetActive(true);
    }

    private void SignUpClick()
    {
        _apply.gameObject.SetActive(false);
        _signIn.gameObject.SetActive(false);
        _registration.SignUp();
    }

}
