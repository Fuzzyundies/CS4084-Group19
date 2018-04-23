using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    public void ChangeToScene(int sceneToChangeTo)
    {
        SceneManager.LoadScene(sceneToChangeTo);
    }

    public void ExitGame()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        player.SaveUserData();
        StartCoroutine(player.UploadPlayerData());
        Application.Quit();
    }
}
