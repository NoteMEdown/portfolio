using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoginScript : MonoBehaviour {

    [SerializeField]
    private InputField user, pass;
    [SerializeField]
    private Button loginButton;
    [SerializeField]
    private GameObject InvUser, InvPass, TakenUser, UserLength, PassLength;
    [SerializeField]
    Client client;

    // Use this for initialization
    void Start () {
        user.Select();
        user.ActivateInputField();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Tab)) // tab moves to password input field
        {
            if (user.isFocused)
            {
                pass.Select();
                pass.ActivateInputField();
            }
            else
            {
                user.Select();
                user.ActivateInputField();
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            var pointer = new PointerEventData(EventSystem.current); // no idea what this does
            ExecuteEvents.Execute(loginButton.gameObject, pointer, ExecuteEvents.submitHandler);
        }
    }

    public void Login()
    {
        this.gameObject.SetActive(false);
    }

    public void TryLogin()
    {
        if (user.text.Length<1)
        {
            PassLength.SetActive(false);
            InvPass.SetActive(false);
            TakenUser.SetActive(false);
            InvUser.SetActive(false);
            UserLength.SetActive(true);
            return;
        }
        if (pass.text.Length<1)
        {
            UserLength.SetActive(false);
            InvPass.SetActive(false);
            TakenUser.SetActive(false);
            InvUser.SetActive(false);
            PassLength.SetActive(true);
            return;
        }
        client.Login(user.text, pass.text);
    }

    public void LoginError(int choice) // 1=wrong username, 2=wrong password, 3=username taken
    {
        switch (choice)
        {
            case 1:
                UserLength.SetActive(false);
                PassLength.SetActive(false);
                InvPass.SetActive(false);
                TakenUser.SetActive(false);
                InvUser.SetActive(true);
                break;
            case 2:
                UserLength.SetActive(false);
                PassLength.SetActive(false);
                InvUser.SetActive(false);
                TakenUser.SetActive(false);
                InvPass.SetActive(true);
                break;
            case 3:
                UserLength.SetActive(false);
                PassLength.SetActive(false);
                InvPass.SetActive(false);
                InvUser.SetActive(false);
                TakenUser.SetActive(true);
                break;
            default:
                Debug.Log("Invalid Error code");
                break;
        }
    }
}
