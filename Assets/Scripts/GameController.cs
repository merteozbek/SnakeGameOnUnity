using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int maxSize;
    public int currentSize;
    public int xBound;
    public int yBound;
    public int score;
    public GameObject foodPrefab;
    public GameObject currentFood;
    public GameObject snakePrefab;
    public Snake head;
    public Snake tail;
    public int NESW; //North - East - South - West
    public Vector2 nextPos; //Yılanın Bir sonraki pozisyonu için belirlediğimiz değer
    public Text scoreText;


	// Use this for initialization
    void OnEnable()
    {
        Snake.hit += hit;
    }
	void Start ()
    {
        InvokeRepeating("TimerInvoke", 0, 0.5f);
        FoodFunction();
	}
    void OnDisable()
    {
        Snake.hit -= hit;
    }
	
	// Update is called once per frame
	void Update ()
    {
        ComChangeD();
	}
    void TimerInvoke()
    {
        Movement();
        StartCoroutine(checkVisible());
        if (currentSize >= maxSize) // eğer csize buyuk ve eşitse msize a 
        {
            TailFunction(); //çağır
        }
        else //diğer durumda
        {
            currentSize++; //current size eklenmeye devam etsin
        }
    }
    void Movement()
    {
        GameObject temp;
        nextPos = head.transform.position;
        switch (NESW)
        {
            case 0: //North - Up
                nextPos = new Vector2(nextPos.x, nextPos.y +1);
                break;

            case 1: //East - Right
                nextPos = new Vector2(nextPos.x +1 , nextPos.y);
                break;

            case 2://South - Down
                nextPos = new Vector2(nextPos.x, nextPos.y - 1);
                break;

            case 3://Wst - Left
                nextPos = new Vector2(nextPos.x -1 , nextPos.y);
                break;
        }
        temp = (GameObject)Instantiate(snakePrefab, nextPos, transform.rotation);
        head.Setnext(temp.GetComponent<Snake>());
        head = temp.GetComponent<Snake>();

        return;
    }

    void ComChangeD() //Yılan yönünü hangi tuşlar ile değiştirecek onları ayarlıyoruz bu bölümde
    {
        if (NESW != 2 && Input.GetKeyDown(KeyCode.W))//Yukarı
        {
            NESW = 0;
        }

        if (NESW != 3 && Input.GetKeyDown(KeyCode.D))//Sağ
        {
            NESW = 1;
        }

        if (NESW != 0 && Input.GetKeyDown(KeyCode.S))//Aşağı
        {
            NESW = 2;
        }

        if (NESW != 1 && Input.GetKeyDown(KeyCode.A))//Sol
        {
            NESW = 3;
        }   
    }

    void TailFunction()
    {
        Snake tempSnake = tail;
        tail = tail.GetNext();
        tempSnake.RemoveTail();
    }

    void FoodFunction()
    {
        int xPos = Random.Range(-xBound, xBound);
        int yPos = Random.Range(-yBound, yBound);

        currentFood = (GameObject)Instantiate(foodPrefab, new Vector2(xPos, yPos), transform.rotation);
        StartCoroutine(CheckRender(currentFood));
    }

    IEnumerator CheckRender(GameObject IN)
    {
        yield return new WaitForEndOfFrame();
        if (IN.GetComponent<Renderer>().isVisible == false)
        {
            if (IN.tag == "Food")
            {
                Destroy(IN);
                FoodFunction();
            }   
        }
    }

    void hit(string WhatWasSent)
    {
        if (WhatWasSent == "Food")
        {
            FoodFunction();
            maxSize++;
            score++;
            scoreText.text = score.ToString();
            int temp = PlayerPrefs.GetInt("HighScore");
            if (score > temp)
            {
                PlayerPrefs.SetInt("HighScore", score);  
            }
        }
        if (WhatWasSent == "Snake")
        {
            CancelInvoke("TimerInvoke");
            Exit();
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }

    void wrap()
    {
        if (NESW == 0)
        {
            head.transform.position = new Vector2(head.transform.position.x, -(head.transform.position.y -1));
        }
        else if (NESW == 1)
        {
            head.transform.position = new Vector2(-(head.transform.position.x -1), head.transform.position.y);
        }
        else if (NESW == 2)
        {
            head.transform.position = new Vector2(head.transform.position.x, -(head.transform.position.y + 1));
        }
        else if (NESW == 3)
        {
            head.transform.position = new Vector2(-(head.transform.position.x + 1), head.transform.position.y);
        }
    }

    IEnumerator checkVisible()
    {
        yield return new WaitForEndOfFrame();
        if (!head.GetComponent<Renderer>().isVisible)
        {
            wrap();
        }
    }
}
