using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public GameObject canvasLogin;
    public TMP_InputField txtName;
    public void OnLogin()
    {
        ServerSystem.playerid = txtName.text; 
        SceneManager.LoadScene("GamePlay", LoadSceneMode.Single);
        canvasLogin.SetActive(false);
    }
}
