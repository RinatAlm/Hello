using System.Collections;
using Firebase;
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

        LoadURL();
        getData();


    }
    void getData()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        FirebaseDatabase.DefaultInstance.GetReference(firebasePath).GetValueAsync().ContinueWithOnMainThread(task => {
          if (task.IsFaulted)
          {
                Debug.LogWarning("No references");
          }
          else if (task.IsCompleted)
          {
              DataSnapshot snapshot = task.Result;
              url =  snapshot.Value.ToString();
          }
      });
    }
    

  
}
