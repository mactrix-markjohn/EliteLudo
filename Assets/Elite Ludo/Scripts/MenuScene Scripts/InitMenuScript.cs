using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using ExitGames.Client.Photon.Chat;
using UnityEngine.SceneManagement;

using SimpleJSON;
using UnityEngine.Networking;
using System.Collections.Generic;
//using Garlic.Plugins.Webview;
//using Garlic.Plugins.Webview.Utils;
#if UNITY_ANDROID || UNITY_IOS
using UnityEngine.Advertisements;
#endif
using AssemblyCSharp;

public class InitMenuScript : MonoBehaviour
{
    public GameObject botNotice;
    public GameObject botTimer;
    public GameObject rateWindow;
    public GameObject FacebookLinkReward;
    public GameObject rewardDialogText;
    public GameObject Usernotice;
    public GameObject FacebookLinkButton;
    public GameObject playerName;
    public GameObject videoRewardText;
    public GameObject playerAvatar;
    public GameObject fbFriendsMenu;
    public GameObject matchPlayer;
    public GameObject backButtonMatchPlayers;
    public GameObject MatchPlayersCanvas;
    public GameObject menuCanvas;
    public GameObject tablesCanvas;
    public GameObject gameTitle;
    public GameObject changeDialog;
    public GameObject inputNewName;
    public GameObject tooShortText;
    public GameObject coinsText;
    public GameObject coinsTextShop;
    public GameObject coinsTab;
    public GameObject TheMillButton;
    public GameObject dialog;
    public GameObject termsbtn;
    public GameObject privacyBtn;
    public GameObject dialogNew;
    public GameObject dialogfailedtofindplayers;
    public GameObject dialogPlayers;
    public GameObject easyPlayLoginDialog;
    public GameObject easyPlayWalletDialog;
    public GameObject addCoinsNofitications;
    public GameObject addMoneyView;
    // Use this for initialization
    public GameObject GameConfigurationScreen;
    public GameObject computerGame;
    public GameObject FourPlayerMenuButton;
    public String urlText;
    public String termsUrl;
    public String privacyUrl;
    public AudioSource BackgroundSound;
    public AudioClip BackgroundSfx;
    public Font f;
    public Sprite DefAvatar;
    public Text coinsInfo;
    public Text NoticeDetails;
    public Text TwoplayerOnline;
    public Text FourplayerOnline;
    public Text NoticeText;

    public int botEnebal;

    public static InitMenuScript inst;


    void Start()
    {
        Usernotice.SetActive(true);
        inst = this;

        Text[] fonts = FindObjectsOfType<Text>();
        for (int i = 0; i < fonts.Length; i++)
            fonts[i].font = f;
        // GarlicWebview.Instance.SetCallbackInterface(new GarlicWebviewCallbackReceiver());
        playerName.GetComponent<Text>().text = PlayerPrefs.GetString("g_name", "JHon");
        if (PlayerPrefs.GetInt(StaticStrings.SoundsKey, 0) == 0)
        {
            AudioListener.volume = 1;
        }
        else
        {
            AudioListener.volume = 0;
        }
        GameManager.Instance.isLocalMultiplayer = false;
        GameManager.Instance.isLocalPLay = false;
        GameManager.Instance.GameScene = "GameScene";
        GameManager.Instance.isPlayingWithComputer = false;

        // PlayerPrefs.SetInt(StaticStrings.SoundsKey,0);
        // PlayerPrefs.SetInt(StaticStrings.MusicKey,0);
        // PlayerPrefs.Save();
        // if (PlayerPrefs.GetInt(StaticStrings.MusicKey, 0) == 0)
        // {
        //     AudioListener.volume = 1;
        // }
        // else
        // {
        //     AudioListener.volume = 0;
        // }

        FacebookLinkReward.GetComponent<Text>().text = "+ " + StaticStrings.CoinsForLinkToFacebook;

        if (!StaticStrings.isFourPlayerModeEnabled)
        {
            FourPlayerMenuButton.SetActive(false);
        }

        GameManager.Instance.FacebookLinkButton = FacebookLinkButton;
        
        // GameManager.Instance.avatarMy = DefAvatar;
        GameManager.Instance.type = MyGameType.TwoPlayer;
        GameManager.Instance.dialog = dialog;
        if (GameManager.Instance.avatarMy != null)
        {
            playerAvatar.GetComponent<Image>().sprite = GameManager.Instance.avatarMy;

            PanelAvatar.sprite = GameManager.Instance.avatarMy;
        }
        GameManager.Instance.easyLoginDialog = easyPlayLoginDialog;
        GameManager.Instance.easyWalletDialog = easyPlayWalletDialog;
        GameManager.Instance.objectGame = dialogNew;
        GameManager.Instance.dialogPlayer = dialogPlayers;
        GameManager.Instance.dialogFails = dialogfailedtofindplayers;
        videoRewardText.GetComponent<Text>().text = "+" + StaticStrings.rewardForVideoAd;
        GameManager.Instance.tablesCanvas = tablesCanvas;
        // GameManager.Instance.facebookFriendsMenu = fbFriendsMenu.GetComponent<FacebookFriendsMenu>(); ;
        GameManager.Instance.matchPlayerObject = matchPlayer;
        GameManager.Instance.backButtonMatchPlayers = backButtonMatchPlayers;
        playerName.GetComponent<Text>().text = GameManager.Instance.nameMy;
        GameManager.Instance.MatchPlayersCanvas = MatchPlayersCanvas;

        //if (PlayerPrefs.GetString("LoggedType").Equals("Facebook"))
        //{
        //    FacebookLinkButton.SetActive(false);
        //}
        GameManager.Instance.Name1 = "";
        GameManager.Instance.Name2 = "";
        GameManager.Instance.Name3 = "";

        if (GameManager.Instance.avatarMy != null)
            playerAvatar.GetComponent<Image>().sprite = GameManager.Instance.avatarMy;

        GameManager.Instance.myAvatarGameObject = playerAvatar;
        GameManager.Instance.myNameGameObject = playerName;

        GameManager.Instance.coinsTextMenu = coinsText;
        GameManager.Instance.coinsTextShop = coinsTextShop;
        GameManager.Instance.initMenuScript = this;

        if (StaticStrings.hideCoinsTabInShop)
        {
            coinsTab.SetActive(false);
        }

#if UNITY_WEBGL
        coinsTab.SetActive(false);
#endif

        rewardDialogText.GetComponent<Text>().text = "1 Video = " + StaticStrings.rewardForVideoAd + " Coins";
        coinsText.GetComponent<Text>().text = GameManager.Instance.myPlayerData.GetCoins() + "";
        //Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        //Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;



        //AdsManager.Instance.adsScript.ShowAd(AdLocation.GameStart);

        if (PlayerPrefs.GetInt("GamesPlayed", 1) % 8 == 0 && PlayerPrefs.GetInt("GameRated", 0) == 0)
        {
            // rateWindow.SetActive(true);
            PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed", 1) + 1);
        }

        //Call Api to get bid values
        /*    string url = StaticStrings.baseURL + "get-bid-values.php";
            WWW www = new WWW(url);
            StartCoroutine(WaitForRequest(www));
            Debug.Log("Full Url to control : " + url);
        */
        // Call Api to get commision value
        //getPayoutCommision();
        if (PlayerPrefs.GetString("pic_url").Length > 12)
            StartCoroutine(LoadPro_pic(PlayerPrefs.GetString("pic_url")));
        StartCoroutine(Details());

       
    }
    public Transform CoinList;
    public GameObject LoadingScreen,CoinLister;
    public IEnumerator Details()
    {
     
        WWWForm form = new WWWForm();
        form.AddField("playerid", PlayerPrefs.GetString("PID", ""));
        string url = StaticStrings.baseURL + "api/player/details";

        using (UnityWebRequest handshake = UnityWebRequest.Post(url, form))
        {
            yield return handshake.SendWebRequest();

          
            if (handshake.isHttpError || handshake.isNetworkError || handshake.isNetworkError)
            {
                Debug.Log(handshake.error.ToString()); LoadingScreen.SetActive(false);
            }
           
            else 
            {
                JSONNode jsonNode = JSON.Parse(handshake.downloadHandler.text);

                Debug.Log("log=======" + handshake.downloadHandler.text);
                LoadingScreen.SetActive(false);
                if (jsonNode["message"] == "All Details Fetched Successfully")
                {
                    int playersTwo = UnityEngine.Random.Range(50, 99);
                    TwoplayerOnline.text = "Online Player - 10" + playersTwo;
                    int playersfour = UnityEngine.Random.Range(50, 99);
                    FourplayerOnline.text = "Online Player - 4" + playersfour;

                    int TotalCoins = 0;
                    string str = jsonNode["playerdata"]["playcoin"].Value;
                   

                    if (str.Equals("null"))
                    {
                       
                        TotalCoins = int.Parse(jsonNode["playerdata"]["totalcoin"].Value.ToString());

                    }
                    else
                    {
                       
                        TotalCoins = int.Parse( jsonNode["playerdata"]["playcoin"].Value);
                    }


                   //     int TotalCoins = jsonNode["playerdata"]["playcoin"].Value == null ? int.Parse(jsonNode["playerdata"]["totalcoin"].Value.ToString())
                     //   : int.Parse(jsonNode["playerdata"]["playcoin"].Value.ToString());
                    coinsText.GetComponent<Text>().text = TotalCoins.ToString();
                    GameManager.Instance.TotalCoins = TotalCoins;
                    coinsText.GetComponent<Text>().text = TotalCoins.ToString();
                    PlayerPrefs.SetInt("Coins", TotalCoins);
                    int Refercode = jsonNode["playerdata"]["refer_code"];
                    int SignupBonus = jsonNode["gameconfig"]["signup_bonus"];

                    // Debug.Log("This is player refer code" + Refercode);
                    PlayerPrefs.SetInt("ReferCode", Refercode);
                    PlayerPrefs.SetInt("SignupBonus", SignupBonus);
                    PlayerPrefs.SetString("GameName", jsonNode["gameconfig"]["website_name"]);
                    NoticeDetails.GetComponent<Text>().text = jsonNode["gameconfig"]["notification"].Value;
                    for (int f = 0; f < CoinList.childCount; f++)
                        Destroy(CoinList.GetChild(f).gameObject);
                    for (int s = 0; s < jsonNode["shop_coin"].Count; s++)
                    {
                    GameObject Cl =     Instantiate(CoinLister,CoinList);
                        Cl.GetComponent<BuyItemControl>().SetBuyData(jsonNode["shop_coin"][s]["shop_coin"].Value);
                   
                    }

                    if (jsonNode["gameconfig"]["min_withdraw"].Value.ToString() != "null" && !string.IsNullOrEmpty(jsonNode["gameconfig"]["min_withdraw"].Value.ToString()))
                        PlayerPrefs.SetInt("MW", int.Parse(jsonNode["gameconfig"]["min_withdraw"].Value.ToString()));
                    else
                    {
                        PlayerPrefs.SetInt("MW", 50);
                    }
                    if (!GameManager.Instance.Links)
                    {
                        YoutubeLink = jsonNode["gameconfig"]["youtube_link"].Value.ToString();
                        WhatsappLink = jsonNode["gameconfig"]["whatsapp_link"].Value.ToString();
                        FacebookLink = jsonNode["gameconfig"]["telegram_link"].Value.ToString();
                        PrivacyPolicyLink = jsonNode["gameconfig"]["website_url"].Value.ToString();
                        ContactUSLink = jsonNode["gameconfig"]["pemail"].Value.ToString();
                    }
                    botEnebal = int.Parse( jsonNode["gameconfig"]["bot_status"].Value);
                    if (!string.IsNullOrEmpty(jsonNode["playerdata"]["accountHolder"].Value) && jsonNode["playerdata"]["accountHolder"].Value != "null")
                    {
                        PlayerPrefs.SetString("HN", jsonNode["playerdata"]["accountHolder"].Value);
                        PlayerPrefs.SetString("AN", jsonNode["playerdata"]["accountNumber"].Value);
                        PlayerPrefs.SetString("IFSC", jsonNode["playerdata"]["ifsc"].Value);
                        PlayerPrefs.SetString("Bank", "YES");
                    }
                    else
                    {
                        PlayerPrefs.SetString("Bank", "NO");

                    }
                   
                    if (jsonNode["gameconfig"]["commission"].Value != "null" && !string.IsNullOrEmpty(jsonNode["gameconfig"]["commission"].Value))
                        PlayerPrefs.SetFloat("Comm", float.Parse(jsonNode["gameconfig"]["commission"].Value));
                    int[] bidVals = new int[jsonNode["bidvalues"].Count];
                    for (int i = 0; i < jsonNode["bidvalues"].Count; i++)
                    {
                   
                        bidVals[i] = int.Parse(jsonNode["bidvalues"][i]["bid_value"].Value);
                        StaticStrings.bidValues = bidVals;
                        StaticStrings.bidValuesStrings[i] = bidVals[i].ToString();
                    }

                    if (jsonNode["playerdata"]["photo"].Value.ToString() != "null")
                    {       PlayerPrefs.SetString("pic_name", jsonNode["playerdata"]["photo"].Value.ToString());
                        FindObjectOfType<EditProfileController>().LoadImageProfile();
                    }
                    playerName.GetComponent<Text>().text = jsonNode["playerdata"]["username"].Value.ToString();
                    GameManager.Instance.nameMy = playerName.GetComponent<Text>().text;
                    DataArray[0] = jsonNode["playerdata"]["wincoin"].Value.ToString();
                    DataArray[1] = jsonNode["playerdata"]["GamePlayed"].Value.ToString();
                    DataArray[2] = jsonNode["playerdata"]["totalcoin"].Value.ToString();

                    DataArray[3] = jsonNode["playerdata"]["twoPlayWin"].Value.ToString();
                    DataArray[4] = jsonNode["playerdata"]["FourPlayWin"].Value.ToString();
                    DataArray[5] = DataArray[1];
                    NoticeText.text = jsonNode["gameconfig"]["notification"].Value;

                    if (jsonNode["playerdata"]["refrelCoin"].Value == "null")
                    {
                        PlayerPrefs.SetString("refrelCoin", "0");
                    }
                    else
                    {
                        PlayerPrefs.SetString("refrelCoin", jsonNode["playerdata"]["refrelCoin"].Value.ToString());
                    }

                }


            }
        }
        yield return new WaitForSeconds(4f);
        StartCoroutine(Details());
        }




    public string[] DataArray = new string[7];

    public IEnumerator LoadPro_pic(string url)
    {
        using (UnityWebRequest handshake = UnityWebRequestTexture.GetTexture(url))
        {
            yield return handshake.SendWebRequest();

            if (handshake.isHttpError || handshake.isNetworkError || handshake.isNetworkError)
            {
                Debug.Log(handshake.error.ToString());
            }
            else 
            {

                Texture2D tex = DownloadHandlerTexture.GetContent(handshake);
                Sprite Pic = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
                playerAvatar.GetComponent<Image>().sprite = Pic;
                GameManager.Instance.avatarMy = Pic;
                PanelAvatar.sprite = Pic;
            }
        } }
    public void FillProPanel()
    {
        FindObjectOfType<PlayerInfoController>().FillProfileData(PanelAvatar.sprite, GameManager.Instance.nameMy, DataArray);

    }

    public Image PanelAvatar;
    private string YoutubeLink = "";
    private string WhatsappLink = "";
    private string FacebookLink = "";
    private string PrivacyPolicyLink = "";
    private string ContactUSLink = "";
    private string Referalcode;

    public void OpenWP()
    {
        Application.OpenURL(WhatsappLink);
    }
    public void PrivacyPolicyURL()
    {
        Application.OpenURL(PrivacyPolicyLink + "privacy-policy");
    }
    public void TermsConditionURL()
    {
        Application.OpenURL(PrivacyPolicyLink+"terms-condition");
    }
    public void ContactUSMail()
    {
        string email = ContactUSLink;
        string subject = MyEscapeURL("Contact Us");
        string body = MyEscapeURL("");
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    private string MyEscapeURL(string v)
    {
        throw new NotImplementedException();
    }

    public GameObject ProfilePanel;
    public void OpenWalletTT()
    {
        ProfilePanel.SetActive(true);

        for (int i = 0; i < ProfilePanel.transform.childCount; i++)
        {
            ProfilePanel.transform.GetChild(i).gameObject.SetActive(false);    
        }

        ProfilePanel.transform.GetChild(3).gameObject.SetActive(true);


    }

    public void OpenTelegram()
    {
        Application.OpenURL(FacebookLink);
    }
    public void OpenYoutube()
    {
        Application.OpenURL(YoutubeLink);
    }
    //public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    //{
    //    Debug.Log("Received Registration Token: " + token.Token);
    //    PlayerPrefs.SetString("fcm_token", token.Token);
    //    callUpdateToken(token.Token);
    //    //   UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
    //}

    //public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    //{
    //    Debug.Log("Received a new message from: " + e.Message.From);
    //    //   UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
    //}



    public void PlayBackgroundMusic()
    {
        if ((PlayerPrefs.GetInt(StaticStrings.MusicKey, 0) == 0))
        {
            BackgroundSound.Play();
        }
        else
        {
            BackgroundSound.Stop();
        }


    }
    void OnEnable()
    {
        PlayBackgroundMusic();
        SetNoOfPlayers(0);
    }

    public void Links(string w, string t)
    {
        WhatsappLink = w;
        FacebookLink = t;
    }
 

    public void openTermsDialog()
    {
#if UNITY_ANDROID
        //int marginPx = (int)GarlicUtils.DPToPx (40f);
#elif UNITY_IOS
			//int marginPx = (int)GarlicUtils.PtToPx(50);
#endif

        //	GarlicWebview.Instance.SetMargins(marginPx, marginPx, marginPx, marginPx);
        // GarlicWebview.Instance.SetFixedRatio(2, 5);
        //	GarlicWebview.Instance.Show(termsUrl);
    }

    public void openPrivacyDialog()
    {
#if UNITY_ANDROID
        //int marginPx = (int)GarlicUtils.DPToPx (40f);
#elif UNITY_IOS
			//int marginPx = (int)GarlicUtils.PtToPx(50);
#endif

        //	GarlicWebview.Instance.SetMargins(marginPx, marginPx, marginPx, marginPx);
        // GarlicWebview.Instance.SetFixedRatio(2, 5);
        //	GarlicWebview.Instance.Show(privacyUrl);
    }

    public void QuitApp()
    {
        PlayerPrefs.SetInt("GameRated", 1);
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=" + StaticStrings.AndroidPackageName);
#elif UNITY_IPHONE
        Application.OpenURL("itms-apps://itunes.apple.com/app/id" + StaticStrings.ITunesAppID);
#endif
        //Application.Quit();
    }


    public void LinkToFacebook()
    {
        GameManager.Instance.facebookManager.FBLinkAccount();
    }

    public GameObject List;

    public void ShowGameConfiguration(int index)
    {
        switch (index)
        {
            case 0:
                GameManager.Instance.type = MyGameType.TwoPlayer;
                break;
            case 1:
                GameManager.Instance.type = MyGameType.FourPlayer;
                break;
            case 2:
                GameManager.Instance.type = MyGameType.Private;
                break;
        }

        List.SetActive(true);
      
    }

    public Toggle T2, T4;

    public void SetNoOfPlayers(int index)
    {
        if (index == 0 && T4.isOn)
            return;
        if (index == 1 && T2.isOn)
            return;
        switch (index)
        {
            case 0:
                if(T2.isOn)
                GameManager.Instance.type = MyGameType.TwoPlayer;
                break;
            case 1:
                if(T4.isOn)
                GameManager.Instance.type = MyGameType.FourPlayer;
                break;

        }
     
    }

    public void TakeScreenshot()
    {
        ScreenCapture.CaptureScreenshot("TestScreenshot.png");
    }


    // Update is called once per frame
    void Update()
    {

    }


    public void backToMenuFromTableSelect()
    {
        GameManager.Instance.offlineMode = false;
        tablesCanvas.SetActive(false);
        menuCanvas.SetActive(true);
        gameTitle.SetActive(true);
    }

    public void showSelectTableScene(bool challengeFriend)
    {
        if (!challengeFriend)
            GameManager.Instance.inviteFriendActivated = false;

        //AdsManager.Instance.adsScript.ShowAd(AdLocation.GameStart);
        if (GameManager.Instance.offlineMode)
        {
            // TheMillButton.SetActive(false);
        }
        else
        {
            TheMillButton.SetActive(true);
        }
        menuCanvas.SetActive(false);
        tablesCanvas.SetActive(true);
        gameTitle.SetActive(false);
    }

    // public void playOffline()
    // {
    //     //GameManager.Instance.tableNumber = 0;
    //     GameManager.Instance.offlineMode = true;
    //     GameManager.Instance.roomOwner = true;
    //     showSelectTableScene(false);
    //     //SceneManager.LoadScene(GameManager.Instance.GameScene);
    // }

    public void playOffline()
    {
        //GameManager.Instance.tableNumber = 0;
        GameManager.Instance.offlineMode = true;
        GameManager.Instance.isLocalMultiplayer = true;
        GameManager.Instance.isLocalPLay = false;
        GameManager.Instance.GameScene = "GameScene";
        GameManager.Instance.isPlayingWithComputer = true;
        GameManager.Instance.roomOwner = true;
        // showSelectTableScene(false);
        GameManager.Instance.playfabManager.PlayofflineMode();
        //SceneManager.LoadScene(GameManager.Instance.GameScene);
    }

    public void switchUser()
    {
        PlayerPrefs.DeleteAll();
        // GameManager.Instance.playfabManager.destroy();
        //  GameManager.Instance.facebookManager.destroy();
        // GameManager.Instance.connectionLost.destroy();

        // GameManager.Instance.avatarMy = null;
        // PhotonNetwork.Disconnect();

        // PlayerPrefs.DeleteAll();
        // GameManager.Instance.resetAllData();
        //LocalNotification.ClearNotifications();
        //GameManager.Instance.myPlayerData.GetCoins() = 0;
        SceneManager.LoadScene("LoginSplash");
    }

    public void showChangeDialog()
    {
        changeDialog.SetActive(true);
    }

    public void changeUserName()
    {
       

        string newName = inputNewName.GetComponent<Text>().text;
        if (newName.Equals(StaticStrings.addCoinsHackString))
        {
            GameManager.Instance.playfabManager.addCoinsRequest(1000000);
            changeDialog.SetActive(false);
        }
        else
        {
            if (newName.Length > 0)
            {
                /*
                UpdateUserTitleDisplayNameRequest displayNameRequest = new UpdateUserTitleDisplayNameRequest()
                {
                    //DisplayName = newName
                    DisplayName = GameManager.Instance.playfabManager.PlayFabId
                };

                PlayFabClientAPI.UpdateUserTitleDisplayName(displayNameRequest, (response) =>
                {
                    Dictionary<string, string> data = new Dictionary<string, string>();
                    data.Add("PlayerName", newName);
                    UpdateUserDataRequest userDataRequest = new UpdateUserDataRequest()
                    {
                        Data = data,
                        Permission = UserDataPermission.Public
                    };

                    PlayFabClientAPI.UpdateUserData(userDataRequest, (result1) =>
                    {
                        Debug.Log("Data updated successfull ");
                        Debug.Log("Title Display name updated successfully");
                        PlayerPrefs.SetString("GuestPlayerName", newName);
                        PlayerPrefs.Save();
                        GameManager.Instance.nameMy = newName;
                        playerName.GetComponent<Text>().text = newName;
                    }, (error1) =>
                    {
                        Debug.Log("Data updated error " + error1.ErrorMessage);
                    }, null);

                }, (error) =>
                {
                    Debug.Log("Title Display name updated error: " + error.Error);

                }, null);

                changeDialog.SetActive(false);
            }
            else
            {
                tooShortText.SetActive(true);
            }*/
            }
        }
    }

    public void startQuickGame()
    {
        GameManager.Instance.facebookManager.startRandomGame();
    }

    public void startQuickGameTableNumer(int tableNumer, int fee)
    {
        GameManager.Instance.payoutCoins = fee;
        GameManager.Instance.tableNumber = tableNumer;
        GameManager.Instance.facebookManager.startRandomGame();
    }
    internal void startTimer()
    {
        if (GameManager.Instance.type == MyGameType.Private)
        {
            botTimer.SetActive(false);
            InitMenuScript.inst.botTimer.SetActive(false);
        }
        else
        {
            botTimer.SetActive(true);
            InitMenuScript.inst.botTimer.SetActive(true);
        }
        value = 60;
        InitMenuScript.inst.botTimer.GetComponent<Text>().text = "Waiting for Opponent 60 sec.";
        CancelInvoke("startTimerForOTP");
        InvokeRepeating("startTimerForOTP", 1, 1);

    }

    internal  int value = 60;
    internal void startTimerForOTP()
    {
        value--;
        TimeSpan timeSpanLeft = TimeSpan.FromSeconds(value);
        InitMenuScript.inst.botTimer.GetComponent<Text>().text = "Waiting for Opponent " + timeSpanLeft.ToString(@"ss") + " sec.";
        if (value <= 0)
        {
            InitMenuScript.inst.botNotice.SetActive(true);
            CancelInvoke("startTimerForOTP");

        }

    }

    string MyEscapeURL1(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }
    public void OpenMail()
    {
        Debug.Log("Check Email");
        string email = ContactUSLink;
        string subject = MyEscapeURL1("Contect Us");
        string body = MyEscapeURL1("");
        Debug.Log("Check Email11111"    );
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    public void showFacebookFriends()
    {

        //AdsManager.Instance.adsScript.ShowAd(AdLocation.FacebookFriends);
        GameManager.Instance.playfabManager.GetPlayfabFriends();
    }

    public void setTableNumber()
    {
        GameManager.Instance.tableNumber = Int32.Parse(GameObject.Find("TextTableNumber").GetComponent<Text>().text);
    }


    //     public void ShowRewardedAd()
    //     {
    // #if UNITY_ANDROID || UNITY_IOS
    //         if (Advertisement.IsReady("rewardedVideo"))
    //         {
    //             var options = new ShowOptions { resultCallback = HandleShowResult };
    //             Advertisement.Show("rewardedVideo", options);
    //         }
    // #endif
    //     }


    // #if UNITY_ANDROID || UNITY_IOS
    //     private void HandleShowResult(ShowResult result)
    //     {
    //         switch (result)
    //         {
    //             case ShowResult.Finished:
    //                 Debug.Log("The ad was successfully shown.");
    //                 GameManager.Instance.playfabManager.addCoinsRequest(StaticStrings.rewardForVideoAd);
    //                 //
    //                 // YOUR CODE TO REWARD THE GAMER
    //                 // Give coins etc.
    //                 break;
    //             case ShowResult.Skipped:
    //                 Debug.Log("The ad was skipped before reaching the end.");
    //                 break;
    //             case ShowResult.Failed:
    //                 Debug.LogError("The ad failed to be shown.");
    //                 break;
    //         }
    //     }
    // #endif

    public void OnClickShowWebview()
    {

#if UNITY_ANDROID
        //int marginPx = (int)GarlicUtils.DPToPx (30f);
#elif UNITY_IOS
			//int marginPx = (int)GarlicUtils.PtToPx(50);
#endif

        //	GarlicWebview.Instance.SetMargins(marginPx, marginPx, marginPx, marginPx);
        //	GarlicWebview.Instance.SetFixedRatio(2, 5);
        //	GarlicWebview.Instance.Show(urlText);
    }

    //private class GarlicWebviewCallbackReceiver : IGarlicWebviewCallback
    //{
    //    public void onClose()
    //    {
    //        Debug.Log("GarlicWebview: onClose");
    //    }

    //    public void onPageFinished(string url)
    //    {
    //        Debug.Log("GarlicWebview: onPageFinished [" + url + "]");
    //    }

    //    public void onPageStarted(string url)
    //    {
    //        Debug.Log("GarlicWebview: onPageStarted [" + url + "]");
    //    }

    //    public void onReceivedError(string errorMessage)
    //    {
    //        Debug.Log("GarlicWebview: onReceivedError [" + errorMessage + "]");
    //    }

    //    public void onShow()
    //    {
    //        Debug.Log("GarlicWebview: onShow");
    //    }
    //}

}
