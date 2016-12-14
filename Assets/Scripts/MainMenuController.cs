using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

    public Text hS;

	// Use this for initialization
	void Start ()
    {
        HSFunction();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
     
    public void  Play()
    {
        SceneManager.LoadScene(1); //game scene yuklenmesi için gereken komutu veriyoruz
    }
    
    void HSFunction()
    {
        hS.text = PlayerPrefs.GetInt("HighScore").ToString();
    }
}
