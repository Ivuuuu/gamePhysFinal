using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagement : MonoBehaviour
{
    //Not needed for this game!
    //public static GameObject Hello;

    [SerializeField] string button;//is used for name of scene.
    public GameObject Settings;//is used for the settings button (settingsCanvas).

    private bool On = true, Off = false;//your private bool variables.

    /// <summary>
    /// For operating the play button.
    /// </summary>
    public void Playbutton()
    {
        SceneManager.LoadScene(button);
    }

    /// <summary>
    /// For operating the settings button.
    /// </summary>
    public void SettiingsButton()
    {
        //if settings canvas is off then turn it on
        //else if on then turn off.
        if (Settings.activeInHierarchy == false)
        {
            Settings.SetActive(On);
            Debug.Log("Settings: ON");
        }
        else
        {
            Settings.SetActive(Off);
            Debug.Log("Settings: OFF");
        }

    }

    /// <summary>
    /// For operating the info button.
    /// </summary>
    public void InfoButton()
    {
        
    }

    /// <summary>
    /// For operating the sound button.
    /// </summary>
    public void SoundButton()
    {

    }

    /// <summary>
    /// For operating the quit button.
    /// </summary>
    public void QuitButton()
    {
        /*Quits the game.*/
        Application.Quit();
        Debug.Log("Quit");
    }
}
