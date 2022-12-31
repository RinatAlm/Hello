using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupClass : MonoBehaviour
{
    public bool hasBall = false;
    public Vector3 cupDir;//Where cup will move

    public void Update()
    {
        if(GameManager.isSwitching == true)
        {
            if(GameManager.time>=0)
            {
                transform.Translate(cupDir * 2 * GameManager.speed * Time.deltaTime);
                GameManager.time-=Time.deltaTime;//If time is over => stop moving cups
            }
            else
            {
                GameManager.setToDefault = false;//we changed smth
                GameManager.isSwitching = false;
                GameManager.time = FindObjectOfType<GameManager>().timeToMove;//Setting time to default time              
                
            }
          
        }
    }
}
