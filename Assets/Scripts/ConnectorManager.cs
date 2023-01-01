using System.Collections;
using Firebase;
using Firebase.RemoteConfig;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Extensions; // for ContinueWithOnMainThread
using System.IO;
using UnityEngine;

public class ConnectorManager : MonoBehaviour
{
    public string firebasePath = "url";
    public string url;

    public struct Link
    {
        public string url;
    }

    public void SaveURL() //------------ Transfering data into Json Format
    {
        Link data = new Link();
        data.url = url;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/saveurlfile.json", json); //--------------------------------------- To use files   using System.IO;
    }

    public void LoadURL() //------------- Transfering data from Json Format
    {
        string path = Application.persistentDataPath + "/saveurlfile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Link data = JsonUtility.FromJson<Link>(json);
            url = data.url;
        }
    }


    void Awake()
    {

    }

    private void Start()
    {
       

        LoadURL();//Load link
        FetchDataAsync();
       
        //if (url == null)//link is empty
        //{
        //    getLink();
        //    OpenVebViewWithLink();

        //}
        //else
        //{
        //    CheckInetConnection();
        //}



    }
    public Task FetchDataAsync()//preparing data
    {
        Debug.Log("Fetching data...");
        System.Threading.Tasks.Task fetchTask =
        Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync();
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    private void FetchComplete(Task fetchTask)
    {
        if (!fetchTask.IsCompleted)
        {
            Debug.LogError("Retrieval hasn't finished.");
            return;
        }

        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        var info = remoteConfig.Info;
        if (info.LastFetchStatus != LastFetchStatus.Success)
        {
            Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
            return;
        }

        // Fetch successful. Parameter values must be activated to use.
        remoteConfig.ActivateAsync().ContinueWithOnMainThread(task => {//Activating data
                Debug.Log($"Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");
                url = remoteConfig.GetValue("url").StringValue;//assignening data url
            });
    }


    public void CheckInetConnection()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Error. Check internet connection!");
        }
        else
        {
            OpenVebViewWithLink();
        }
    }

    public void CheckLink()
    {
        if(url == null || IsEmulator() || GetSIMState())
        {

        }
        else
        {
            OpenVebViewWithLink();
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
   



