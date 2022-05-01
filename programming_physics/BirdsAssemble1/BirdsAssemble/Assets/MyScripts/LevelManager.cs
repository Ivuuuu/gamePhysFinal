using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Button[] LevelButtons;//make an array of buttons.
    public string[] LevelScenes;//make an array of scenes
    public string scene;//for name of the scene.

    
    private void Awake()
    {
        
        int ReachedLevel = PlayerPrefs.GetInt("ReachedLevel", 1);//When player plays game the first level is open.

        if(PlayerPrefs.GetInt(scene) >= 2)//when player is done with first level...
        {
            ReachedLevel = PlayerPrefs.GetInt(scene);//unlock next level.
        }

        LevelButtons = new Button[transform.childCount];//gather all the buttons.

        for(int i = 0; i < LevelButtons.Length; i++)//however many buttons...
        {
            LevelButtons[i] = transform.GetChild(i).GetComponent<Button>();//find the buttons.

            LevelButtons[i].GetComponentInChildren<Text>().text = (i + 1).ToString();//find text and assign a number to each button (level).

            if (i + 1 > ReachedLevel)//If level hasn't been reached then...
            {
                LevelButtons[i].interactable = false;//remain locked until level has been reached.
            }
        }
    }

    public void LevelScene(int Level)
    {
        //load levels.
        PlayerPrefs.SetInt(scene, Level);
		SceneManager.LoadScene(LevelScenes[Level]);
	}
}
