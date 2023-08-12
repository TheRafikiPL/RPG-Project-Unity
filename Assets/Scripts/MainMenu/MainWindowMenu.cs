using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainWindowMenu : MonoBehaviour
{
    public void StartButton()
    {
        AudioSystem.instance.PlaySound("Click");
        SceneManager.LoadScene("CharacterCreator");
    }
    public void ExitButton()
    {
        AudioSystem.instance.PlaySound("Click");
        Application.Quit();
    }
    public void ConfigButton()
    {
        AudioSystem.instance.PlaySound("Click");
    }
}
