using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;
    public GameObject NoConnectionPanel;
    private void Awake()
    {
        instance = this;
    }
    public void TryOtherGames()
    {
        Application.OpenURL("https://vickychaudhary.netlify.app");
    }
}
