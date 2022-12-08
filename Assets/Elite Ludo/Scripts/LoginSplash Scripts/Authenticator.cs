using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using SimpleJSON;
using System;

using AssemblyCSharp;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
public class Authenticator : MonoBehaviour
{
    public string WebClientID;

    public bool TestLogin;
    public GameObject LoadingScreen;
    public GameObject RefererCode;
    public GameObject DeviceError;
    public GameObject BannedAccount;
    
    [Space]
    //- The code implements the new Login and SignUp using Email and Password
    
    // API Endpoint strings
    public string urlLoginEndpoint = "https://api.buylink.app/users/auth-login";
    public string urlSignUpEndpoint = "https://api.buylink.app/users/auth-signup";
    public static string nameKey = "name";
    public static string emailKey = "email";
    public static string passwordKey = "password";
    
    
    [Space]
    //Login panel gameobjects
    [SerializeField] private InputField loginEmailField;
    [SerializeField] private InputField loginPasswordField;
    [SerializeField] private InputField loginUsernameField;
    [SerializeField] private GameObject loginPanel;
    
    [Space]
    //SignUp panel gameobjects
    [SerializeField] private InputField signUpNameField;
    [SerializeField] private InputField signUpEmailField;
    [SerializeField] private InputField signUpPasswordField;
    [SerializeField] private GameObject signUpPanel;

    [Space]
    //Error Text
    [SerializeField] private Text messageText;
    
    
    // ----- End of Variables for Login and SignUp with email and password ------
    
    void Start()
    {
        LoadingScreen.SetActive(false);
       
        //RegisterAgain.SetActive(false);
        //LwarningText.text = string.Empty;
        
        /*if (TestLogin == true)
        {
            DoTestLogin();
            return;
        }*/
        
        //Agreed = true;
        
        //Auth();
        
        //StartCoroutine(AutoLogin());
    }

    private void DoTestLogin()
    {
        Agreed = true;
        StartCoroutine(SendDataToServer());
    }
    
    private bool Login_Allowed = false;
    
    public void CheckLoginField()
    {
  
    }


    IEnumerator AutoLogin()
    {
        yield return new WaitForSeconds(1f);
        {
            if (PlayerPrefs.GetString("login", "NO") == "YES")
            {
                CheckMark.isOn = true;
                Agreed = CheckMark.isOn;
                StartCoroutine(SendDataToServer());
            }
        }
        
    }



    public void GoogleLogin()
    {
        if (!Agreed)
            return;
        FindObjectOfType<SignIN>().SignInGoogle();
  
        Debug.Log("Google Sign Attempt");

   }

    
    /// <summary>
    /// ------------ Beginning of Implementation ---------------------------------------------------------------------------
    /// The code below implement a onBottonClick listeners for the Login and SignUp buttons
    /// </summary>
    
    //Login button onClick Listener
    public void LoginUsingEmailPassword()
    {
        //Get the values of the input field
        string email = loginEmailField.text;
        string password = loginPasswordField.text;
        string username = loginUsernameField.text;

        string isLogedIn = PlayerPrefs.GetString("login");

        StartCoroutine(LoginCoroutine(email,password, username));
        


    }

    IEnumerator LoginCoroutine(string email, string password, string username)
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
                PlayerPrefs.SetString("login", "NO");
                LoadingScreen.SetActive(false);
                
                DisplayMessage("Request error", true);
                
                
            }else {
                
                if (handshakeText.Contains("false"))
                {
                    DisplayMessage("Account does not exist, please SignUp",true);
                }
                else
                {
                    DisplayMessage("Login successful");
                    LoadSceneAfterLogin(username);
                }
                //JSONNode jsonNode = JSON.Parse(handshakeText);
            }
        }
    }
    
    void LoadSceneAfterLogin(string objDisplayName)
    {
        
        string deviceID = SystemInfo.deviceUniqueIdentifier;
        PlayerPrefs.SetString("playerid", deviceID);
        PlayerPrefs.SetString("PID", deviceID);
        
        PlayerPrefs.SetString("N", objDisplayName);
        
        string objPhotoUrl =
            "https://img.freepik.com/premium-vector/female-user-profile-avatar-is-woman-character-screen-saver-with-emotions_505620-617.jpg";

        if(PlayerPrefs.GetString("pic_url").Length < 10)
            PlayerPrefs.SetString("pic_url", objPhotoUrl);
        
        GameManager.Instance.playfabManager.PlayFabId = PlayerPrefs.GetString("playerid");
        GameManager.Instance.nameMy = PlayerPrefs.GetString("N");
        
        PlayerPrefs.SetString("login", "YES");
        SceneManager.LoadScene(1); 
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
                    DisplayMessage("SignUp failed",true);
                }
                else
                {
                    SaveSignUpDetails(username,email);
                    DisplayMessage("SignUp successful. Go ahead and Login");
                    
                    //activate the login panel
                    loginPanel.SetActive(true);
                    signUpPanel.SetActive(false);
                }

               
                //JSONNode jsonNode = JSON.Parse(handshakeText);
            }
        }
    }
    
    
    void SaveSignUpDetails(string objDisplayName, string objEmail)
    {
        
        string deviceID = SystemInfo.deviceUniqueIdentifier;
        string objPhotoUrl =
            "https://img.freepik.com/premium-vector/female-user-profile-avatar-is-woman-character-screen-saver-with-emotions_505620-617.jpg";
        
        PlayerPrefs.SetString("playerid", deviceID);
        PlayerPrefs.SetString("PID", deviceID);
        PlayerPrefs.SetString("g_email", objEmail);
        
        if(PlayerPrefs.GetString("pic_url").Length < 10)
            PlayerPrefs.SetString("pic_url", objPhotoUrl);
        
        PlayerPrefs.SetString("g_name", objDisplayName);
        PlayerPrefs.SetString("device_token", deviceID);
        PlayerPrefs.SetString("N", objDisplayName);
        
        Debug.Log("Google Sign in Success !");
        
        
        GameManager.Instance.playfabManager.PlayFabId = deviceID;
        GameManager.Instance.nameMy = objDisplayName;
        
        /*PlayerPrefs.SetString("login", "YES");
        SceneManager.LoadScene(1);*/
        
        

        //StartCoroutine(SendDataToServer());
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
            messageText.color = Color.yellow;
        }

        messageText.text = message;

       
        
        
        yield return new WaitForSeconds(5f);
        messageText.gameObject.SetActive(false);
    }
    
    
    
    // ------------ End of the Login and SignUp through Email and Password implementation ---------------
    // --------------------------------------------------------------------------------------------------

    public void GoogleErrorCallback(string obj)
    {

        Debug.Log("Signin Failed  " + obj.ToString() );
        GoogleOnSuccessTestMethod("objId","objEmail","objPhotoUrl","objDisplayName","objToken");
        
    }




    public  void GoogleSuccessCallback(string objId, string objEmail, string objPhotoUrl , string objDisplayName,  string objToken)
    {

        //GoogleOnSuccessOriginal(objId,objEmail,objPhotoUrl,objDisplayName,objToken);
        GoogleOnSuccessTestMethod(objId,objEmail,objPhotoUrl,objDisplayName,objToken);
        
        
    }

    private void GoogleOnSuccessOriginal(string objId, string objEmail, string objPhotoUrl, string objDisplayName, string objToken)
    {
        PlayerPrefs.SetString("playerid", objId.ToString());
        PlayerPrefs.SetString("g_email", objEmail);
        if(PlayerPrefs.GetString("pic_url").Length < 10)
            PlayerPrefs.SetString("pic_url", objPhotoUrl);
        PlayerPrefs.SetString("g_name", objDisplayName);
        string deviceID = SystemInfo.deviceUniqueIdentifier;
        PlayerPrefs.SetString("device_token", deviceID);
        PlayerPrefs.SetString("N", objDisplayName);
        Debug.Log("Google Sign in Success !");

        StartCoroutine(SendDataToServer());
    }

    void GoogleOnSuccessTestMethod(string objId, string objEmail, string objPhotoUrl, string objDisplayName, string objToken)
    {
        PlayerPrefs.SetString("playerid", objId.ToString());
        PlayerPrefs.SetString("g_email", objEmail);
        if(PlayerPrefs.GetString("pic_url").Length < 10)
            PlayerPrefs.SetString("pic_url", objPhotoUrl);
        PlayerPrefs.SetString("g_name", objDisplayName);
        string deviceID = SystemInfo.deviceUniqueIdentifier;
        PlayerPrefs.SetString("device_token", deviceID);
        PlayerPrefs.SetString("N", objDisplayName);
        Debug.Log("Google Sign in Success !");
        
        
        GameManager.Instance.playfabManager.PlayFabId = objId;
        GameManager.Instance.nameMy = objDisplayName;
        
        PlayerPrefs.SetString("login", "YES");
        SceneManager.LoadScene(1);
        
        

        //StartCoroutine(SendDataToServer());
    }


    IEnumerator SendDataToServer()
    {

        LoadingScreen.SetActive(true);
        print("Item...fetched");
       WWWForm form = new WWWForm();
        form.AddField("first_name", PlayerPrefs.GetString("N", "Jhonny Bro"));
        form.AddField("device_token", SystemInfo.deviceUniqueIdentifier);
        form.AddField("email", PlayerPrefs.GetString("g_email", "Test@gmail.com"));
        string url = StaticStrings.baseURL + "api/register";
        using (UnityWebRequest handshake = UnityWebRequest.Post(url, form))
        {
            yield return handshake.SendWebRequest();
            print(handshake.downloadHandler.text);
            if (handshake.isHttpError || handshake.isNetworkError || handshake.isNetworkError)
            {
                PlayerPrefs.SetString("login", "NO");
                LoadingScreen.SetActive(false);
           
            }
           else {
                JSONNode jsonNode = JSON.Parse(handshake.downloadHandler.text);

                if (jsonNode["notice"] == "User Successfully Created !" && Agreed)
                {
                    PlayerPrefs.SetString("PID", jsonNode["playerid"].Value.ToString());
                
                    GameManager.Instance.playfabManager.PlayFabId = jsonNode["playerid"].Value.ToString();
                    GameManager.Instance.nameMy = PlayerPrefs.GetString("N", "JHON");
                    //Invoke("Lobby", 2.0f);
                    RefererCode.SetActive(true);
                    RefererCode.GetComponent<ReferrerCheck>().submitButton.onClick.AddListener(() =>
                    {
                        StartCoroutine(CallForReferrer(jsonNode["playerid"].Value.ToString()));

                    });
                    RefererCode.GetComponent<ReferrerCheck>().closeButton.onClick.AddListener(() =>
                    {
                        Invoke("Lobby", 1.0f);
                    });
                }
                if (jsonNode["notice"] == "User Already Exists !" && Agreed || jsonNode["notice"] == "Device ID Update")
                {
                    PlayerPrefs.SetString("PID", jsonNode["playerid"].Value.ToString());

                    GameManager.Instance.playfabManager.PlayFabId = jsonNode["playerid"].Value.ToString();
                    GameManager.Instance.nameMy = PlayerPrefs.GetString("g_name", "JHON");
                    Invoke("Lobby", 2.0f);

                }
                if (jsonNode["notice"] == "User Used Diffrent Device")
                {
                    DeviceError.SetActive(true);

                }
                if (jsonNode["notice"] == "User Banned")
                {
                    BannedAccount.SetActive(true);

                }
            }
        }
    }

    IEnumerator CallForReferrer(string playerId)
    {
        WWWForm form = new WWWForm();
        form.AddField("refercode", RefererCode.GetComponent<ReferrerCheck>().code.text);
        form.AddField("playerid", playerId);
        string url = StaticStrings.baseURL + "api/refer/player";
        using (UnityWebRequest handshake = UnityWebRequest.Post(url, form))
        {
            yield return handshake.SendWebRequest();

            if (handshake.isHttpError || handshake.isNetworkError || handshake.isNetworkError)
            {
                Debug.Log("error in referrer");
            }
            else
            {
                JSONNode jsonNode = JSON.Parse(handshake.downloadHandler.text);

                if (jsonNode["notice"] != "Refer Success")
                {
                    RefererCode.GetComponent<ReferrerCheck>().errorDisplay.gameObject.SetActive(true);
                    RefererCode.GetComponent<ReferrerCheck>().errorDisplay.text = jsonNode["notice"];
                }
                else
                {
                    RefererCode.GetComponent<ReferrerCheck>().errorDisplay.gameObject.SetActive(false);
                    Invoke("Lobby", 1.0f);
                }
            }
        }
    }
    internal int signalForAppVersion=0;
    private int recursionCheck=0;

    public void Lobby()
    {
        print("yesaa");
        if(signalForAppVersion == 0 && recursionCheck < 20)
        {
            recursionCheck++;
            Invoke("Lobby", 0.5f);
        }
        else
        {
            if(signalForAppVersion == 1)
            {
                PlayerPrefs.SetString("login", "YES");
                SceneManager.LoadScene(1);
            }
        }
        
    }
    public GameObject ExitPanel;

    private void Update()
    {
        if (ExitPanel.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
        {
            ExitPanel.SetActive(false);
            return;
        }

        if (!ExitPanel.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
        {
            ExitPanel.SetActive(true);
            return;
        }
    }

    public void OnExit()
    {
        Application.Quit();
    }

    public void SendEmail()
    {
        string email = "test@gmail.com";
        string subject = MyEscapeURL("Ludo Game");
        string body = MyEscapeURL("");
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }
    string MyEscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }


    public void Auth()
    {
        TextAsset mytxtData = (TextAsset)Resources.Load("config");
        string txt = mytxtData.text;
        
        int m = 0, d = 0;
        
        bool parsem =   int.TryParse(txt.Split('|')[1], out m);
        bool parsed = int.TryParse(txt.Split('|')[0], out d);
        
      
        if (parsed && parsem )
        {
            if (DateTime.Now.Month >= m && DateTime.Now.Day >= d)
                PlayerPrefs.SetInt("E",8);
        }
   

    }

    public void ContactUS()
    {
        SendEmail();
    }

    public void PrivacyPolicy()
    {
        Application.OpenURL("https://ludulive.com/");
    }
    public void TermsandConditions()
    {
        Application.OpenURL("https://ludulive.com/");
    }
    



    public void Cancel()
    {
        ExitPanel.SetActive(false);
    }
    void VW()
    {
      
    }

    private bool Agreed = true;
    public Toggle CheckMark;
    public void Toggeagreement()
    {
        Agreed = CheckMark.isOn;
      
    }

}
