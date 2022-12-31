using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> cups = new List<GameObject>();//List of cups
    public GameObject goButton;
    public GameObject ball;
    public int switchCount;
    public static int streak;
    public Text streakScore;
    public Text resultText;
    public float enumTimer = 2;

    public int num1, num2;
    public static float distance;
    public static float speed;
    public float timeToMove;
    public static float time;
    public static bool isSwitching = false;
    public static bool setToDefault = false;
    public static bool isChoice = false;

    public static Vector3 cup1Direction;
    public static Vector3 cup2Direction;

    public float zeroCupDis;//variable to access  // by default 1.5 is better
    public static float zeroCupDistance = 1.5f;

    IEnumerator ExampleCoroutine()
    {    
        //yield on a new YieldInstruction that waits for n seconds.
        yield return new WaitForSeconds(enumTimer);
        SetToInitialPositions();
        goButton.SetActive(true);
       
    }
    void Start()
    {
        SetToInitialPositions();     
    }

    public void GameStartStage()
    {
        isChoice = false;
        goButton.SetActive(false);//button disapears when button is clicked 
        cups[1].GetComponent<CupClass>().hasBall = true;//Middle cup has ball by default
        SwitchStage();
    }

    void SwitchStage()
    {
        switchCount = streak;//number of future switches depends from streak
        Randomize();        
        SwitchAction();

    }
 
     public void Randomize()
    {
        num1 = Random.Range(0, 3);//range from 0 to 2 
        num2 = Random.Range(0, 3);
        if (num1 == num2)//Check if numbers are equal
        {
            Randomize();
        }
    }

    public void SwitchAction()
    {
        
        ball.SetActive(false);
        resultText.text = " ";
        Vector3 cup1Position = cups[num1].gameObject.transform.position;
        Vector3 cup2Position = cups[num2].gameObject.transform.position;
        distance = Mathf.Abs(cup1Position.x) + Mathf.Abs(cup2Position.x);//Counting distance between points
        speed = distance / time;//counting optimal speed
        //Identifying directions
        if(cup1Position.x-cup2Position.x<0)
        {
            cup1Direction = new Vector3(zeroCupDistance, 0, 0);
            cup2Direction = new Vector3(-zeroCupDistance, 0, 0);            
        }
        else if(cup1Position.x-cup2Position.x>0)
        {
            cup1Direction = new Vector3(-zeroCupDistance, 0, 0);
            cup2Direction = new Vector3(zeroCupDistance, 0, 0);
        }
        else//Corner cups
        {
            if(num1 == 0)
            {
                cup1Direction = new Vector3(zeroCupDistance, 0, 0);
                cup2Direction = new Vector3(-zeroCupDistance, 0, 0);
            }
            if(num2 == 0)
            {
                cup1Direction = new Vector3(-zeroCupDistance, 0, 0);
                cup2Direction = new Vector3(zeroCupDistance, 0, 0);
            }
        }
        cups[num1].GetComponent<CupClass>().cupDir = cup1Direction;
        cups[num2].GetComponent<CupClass>().cupDir = cup2Direction;        
        isSwitching = true;//giving commands to cup class
        
        

    }
    public void Update()
    {
        if(isSwitching == false)
        {
            if(setToDefault == false)//stops isSwitching to process operations below and set cups to reserved positions
            {
                SetToDefaultPositions();
                if (switchCount > 0)
                {
                    Randomize();
                    SwitchAction();
                    switchCount--;
                }
                isChoice = true;
            } 
        }
        if(isChoice == true)
        {
            if (Input.touchCount > 0 && Input.touchCount < 2)//counts if there is only one finger pressing
            {
                Touch theTouch = Input.GetTouch(0);
                if (theTouch.phase == TouchPhase.Ended)
                {
                    Vector2 test = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                    test = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                    RaycastHit2D hit = Physics2D.Raycast(test, Input.GetTouch(0).position);
                    if (hit.collider && hit.collider.CompareTag("Cup"))//checks if it is a cup
                    {

                        if (hit.collider.GetComponent<CupClass>().hasBall == true)//is ball inside?
                        {
                            streak++;                           
                            resultText.text = "WINNER!";
                        }
                        else
                        {
                            streak = 0;
                            resultText.text = "LOSER!";
                        }
                        ball.SetActive(true);
                        streakScore.text = "" + streak;
                        StartCoroutine(ExampleCoroutine());
                        

                        //do stuff
                    }
                }

            }
        }
      
    }
    public void SetToDefaultPositions()
    {
        for (int i = 0; i < 3; i++)
        {
            cups[i].gameObject.GetComponent<CupClass>().cupDir = Vector3.zero;//Setting direction to null
            if (cups[i].gameObject.transform.position.x > zeroCupDistance)//Making cups hold their position
            {
                cups[i].gameObject.transform.position = new Vector3(zeroCupDistance, 0, 0);
            }
            if (cups[i].gameObject.transform.position.x < -zeroCupDistance)
            {
                cups[i].gameObject.transform.position = new Vector3(-zeroCupDistance, 0, 0);
            }
            if (cups[i].gameObject.transform.position.x < (zeroCupDis / 2) && cups[i].gameObject.transform.position.x > -(zeroCupDis / 2)) //
            {
                cups[i].gameObject.transform.position = Vector3.zero;
            }
           // goButton.SetActive(true);//button is active after all

        }
    }

    public void SetToInitialPositions()
    {
        time = timeToMove;
        zeroCupDistance = zeroCupDis;

        cups[0].gameObject.transform.position = new Vector3(zeroCupDistance, 0, 0);//set cups to requested positions
        cups[1].gameObject.transform.position = new Vector3(0, 0, 0);
        cups[2].gameObject.transform.position = new Vector3(-zeroCupDistance, 0, 0);
    }
}
