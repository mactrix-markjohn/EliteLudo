using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Authenticate : MonoBehaviour
{
    /// <summary>
    /// - The code implements the new Login and SignUp using Email and Password
    /// </summary>

    public string urlLoginEndpoint = "https://api.buylink.app/users/auth-login";
    public string urlSignUpEndpoint = "https://api.buylink.app/users/auth-signup";
    public static string nameKey = "name";
    public static string emailKey = "email";
    public static string passwordKey = "password";
    
    
    
    
    //Login panel gameobjects
    [SerializeField] private InputField loginEmailField;
    [SerializeField] private InputField loginPasswordField;
    [SerializeField] private GameObject loginPanel;
    
    
    //SignUp panel gameobjects
    [SerializeField] private InputField signUpNameField;
    [SerializeField] private InputField signUpEmailField;
    [SerializeField] private InputField signUpPasswordField;
    [SerializeField] private GameObject signUpPanel;
    
    //Error Text
    [SerializeField] private Text messageText;
    
    
    
    
    
    /// <summary>
    /// The code below implement a onBottonClick listeners for the Login and SignUp buttons
    /// </summary>
    
    //Login button onClick Listener
    public void LoginUsingEmailPassword()
    {

        //Get the values of the input field
        string email = loginEmailField.text;
        string password = loginPasswordField.text;

        StartCoroutine(LoginCoroutine(email,password));
        
    }

    IEnumerator LoginCoroutine(string email, string password)
    {
        WWWForm form = new WWWForm();
        
        form.AddField(emailKey, email);
        form.AddField(passwordKey, password);
        
        
        using (UnityWebRequest handshake = UnityWebRequest.Post(urlLoginEndpoint, form))
        {
            yield return handshake.SendWebRequest();

            string handshakeText = handshake.downloadHandler.text;
            
            print(handshakeText);
            
            
            if (handshake.isHttpError || handshake.isNetworkError || handshake.isNetworkError)
            {
                DisplayMessage("Request error", true);
                
            }else {
                
                if (handshakeText.Contains("false"))
                {
                    DisplayMessage(handshakeText,true);
                }
                else
                {
                    DisplayMessage(handshakeText);
                }
                JSONNode jsonNode = JSON.Parse(handshakeText);
            }
        }
    }


    //SignUp button onClick Listener
    public void SignUpUsingEmailPassword()
    {
        //Get the value of the input field
        string username = signUpNameField.text;
        string email = signUpEmailField.text;
        string password = signUpPasswordField.text;

        StartCoroutine(SignUpCoroutine(username, email, password));

    }
    
    IEnumerator SignUpCoroutine(string username, string email, string password)
    {
        WWWForm form = new WWWForm();
        
        form.AddField(nameKey, username);
        form.AddField(emailKey, email);
        form.AddField(passwordKey, password);
        
        
        using (UnityWebRequest handshake = UnityWebRequest.Post(urlSignUpEndpoint, form))
        {
            yield return handshake.SendWebRequest();

            string handshakeText = handshake.downloadHandler.text;
            
            print(handshakeText);
            
            
            if (handshake.isHttpError || handshake.isNetworkError || handshake.isNetworkError)
            {
                DisplayMessage("Request error", true);
                
            }else {


                if (handshakeText.Contains("false"))
                {
                    DisplayMessage(handshakeText,true);
                }
                else
                {
                    DisplayMessage(handshakeText);
                }

               
                JSONNode jsonNode = JSON.Parse(handshakeText);
            }
        }
    }
    
    //Switch to Login panel
    public void LoginPanelActivate()
    {
        loginPanel.SetActive(true);
        signUpPanel.SetActive(false);
    }
    
    //Switch to SignUp panel
    public void SignUpPanelActivate()
    {
        loginPanel.SetActive(false);
        signUpPanel.SetActive(true);
    }

    void DisplayMessage(string message, bool error = false)
    {
        StartCoroutine(Message(message, error));
    }
    
    IEnumerator Message(string message, bool error)
    {
        messageText.gameObject.SetActive(true);
        
        if (error)
        {
            messageText.color = Color.red;
        }
        else
        {
            messageText.color = Color.cyan;
        }

        messageText.text = message;

       
        
        
        yield return new WaitForSeconds(5f);
        messageText.gameObject.SetActive(false);
    }


}
