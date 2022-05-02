using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    [Header("Set Dynamically")]
    public static int levelSceneNumber = 2;
    public void MainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

	public void NextLevel() {
		levelSceneNumber++;
		//if (SceneManager.GetSceneAt(SceneManager.GetActiveScene().buildIndex + 1) == null) 
        //{
			//SceneManager.LoadScene("StartMenu");
		//}
        SceneManager.LoadScene(levelSceneNumber);
	    //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
