using System.Collections;
using UnityEngine.SceneManagement;
using Firebase.RemoteConfig;
using System.Threading.Tasks;
using Firebase.Extensions; // for ContinueWithOnMainThread
using System.IO;
using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;//for exit
#endif

public class ConnectorManager : MonoBehaviour
{
    public string firebasePath = "url";
    public string url;
    public string sceneName = "SampleScene";
    public GameObject errorPanel;

    public struct Link
    {
        public string url;
    }

    //public void SaveURL() //------------ Transfering data into Json Format
    //{
    //    Link data = new Link();
    //    data.url = url;
    //    string json = JsonUtility.ToJson(data);
    //    File.WriteAllText(Application.persistentDataPath + "/saveurlfile.json", json); //--------------------------------------- To use files   using System.IO;
    //}

    //public void LoadURL() //------------- Transfering data from Json Format
    //{
    //    string path = Application.persistentDataPath + "/saveurlfile.json";
    //    if (File.Exists(path))
    //    {
    //        string json = File.ReadAllText(path);
    //        Link data = JsonUtility.FromJson<Link>(json);
    //        url = data.url;
    //    }
    //}

    private void Start()
    {


        // LoadURL();//Load link
       
        FetchDataAsync();        
       
        //if (url == "")//link is empty
        //{
        //    try
        //    {
        //        FetchDataAsync();
        //        OpenVebViewWithLink();
        //    }
        //    catch(FirebaseException e)
        //    {
        //        Debug.LogError(e.Message);
        //    }       
        //   // CheckLink();
        //}
        //else
        //{
        //    CheckInetConnection();
        //}



    }

    void takeData(FirebaseRemoteConfig remoteConfig)
    {
       
        Debug.LogWarning(url);
        url = remoteConfig.GetValue(firebasePath).StringValue;//assigning data to url
        Debug.Log(remoteConfig.GetValue(firebasePath).StringValue);
        
    }
    public Task FetchDataAsync()//preparing data
    {
        
        Debug.Log("Fetching data...");
        System.Threading.Tasks.Task fetchTask =
        Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    private void FetchComplete(Task fetchTask)
    {
        Debug.LogWarning(FirebaseRemoteConfig.DefaultInstance.ConfigSettings);
        if (!fetchTask.IsCompleted)
        {
            Debug.LogError("Retrieval hasn't finished.");
            return;
        }
        FirebaseRemoteConfig remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        var info = remoteConfig.Info;
        if (info.LastFetchStatus != LastFetchStatus.Success)
        {
            Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
            return;
        }
        // Fetch successful. Parameter values must be activated to use.
        remoteConfig.ActivateAsync().ContinueWithOnMainThread(task => {//Activating data
                Debug.Log($"Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");
            takeData(remoteConfig);
            OpenVebViewWithLink();
        });
    }
    void SetDefaultData()
    {
        System.Collections.Generic.Dictionary<string, object> defaults =
  new System.Collections.Generic.Dictionary<string, object>();

        // These are the values that are used if we haven't fetched data from the
        // server
        // yet, or if we ask for values that the server doesn't have:
        defaults.Add("url", "");
        Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults);
    }

    public void CheckInetConnection()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Error. Check internet connection!");
            errorPanel.SetActive(true);
        }
        else
        {
            OpenVebViewWithLink();
        }
    }
    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void CheckLink()
    {
        if(url == "" || IsEmulator() || GetSIMState())
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            
            OpenVebViewWithLink();//Open web view link 
        }
    }

    public void OpenVebViewWithLink()
    {
        Application.OpenURL(url);
    }

    public bool IsEmulator()
    {
        return false;//-------------
    }

    public bool GetSIMState()
    {
        //TelephonyManager telMgr = (TelephonyManager)getSystemService(Context.TELEPHONY_SERVICE);
        //int simState = telMgr.getSimState();
        //switch (simState)
        //{
        //    case TelephonyManager.SIM_STATE_ABSENT:
        //        // do something
        //        break;
        //    case TelephonyManager.SIM_STATE_NETWORK_LOCKED:
        //        // do something
        //        break;
        //    case TelephonyManager.SIM_STATE_PIN_REQUIRED:
        //        // do something
        //        break;
        //    case TelephonyManager.SIM_STATE_PUK_REQUIRED:
        //        // do something
        //        break;
        //    case TelephonyManager.SIM_STATE_READY:
        //        // do something
        //        break;
        //    case TelephonyManager.SIM_STATE_UNKNOWN:
        //        // do something
        //        break;
        //}
        return true;//----------
    }
}
   



