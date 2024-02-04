using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.PlayerLoop;

public class InternetChecker : MonoBehaviour
{
    [SerializeField] float checkInterval = 5f; // Adjust frequency as needed
    [SerializeField] string serverUrl = "http://google.com"; // Or your own server

    Coroutine connectionCheckCoroutine;
    bool noconnection;

    void Start()
    {
        CheckConnection();
    }

    void CheckConnection()
    {
        if (connectionCheckCoroutine != null)
        {
            StopCoroutine(connectionCheckCoroutine);
        }

        connectionCheckCoroutine = StartCoroutine(CheckConnectionCoroutine());
    }
    private void Update()
    {
        if(noconnection && SceneManager.GetActiveScene().buildIndex == 0)
        {
            MainMenu.instance.NoConnectionPanel.SetActive(true);
        }
    }
    IEnumerator CheckConnectionCoroutine()
    {
        // Real-time connection attempt
        using (var www = new WWW(serverUrl))
        {
            yield return www;

            if (www.error == null)
            {
                if (noconnection && SceneManager.GetActiveScene().buildIndex == 0)
                {
                    MainMenu.instance.NoConnectionPanel.SetActive(false);
                }
                Debug.Log("Internet connection detected");
                // Handle connected state (enable online features, etc.)
            }
            else
            {
                noconnection = true;
                Debug.LogError("Internet connection lost: " + www.error);
                // Handle disconnected state (display offline message, etc.)
                if(SceneManager.GetActiveScene().buildIndex != 0)
                {
                    SceneManager.LoadScene(0);
                }
                // Additional checks (optional)
                if (Application.internetReachability != NetworkReachability.NotReachable)
                {
                    Debug.LogWarning("Application.internetReachability indicates connection, but WWW failed. Consider alternative checks or error handling.");
                }
            }
        }

        yield return new WaitForSeconds(checkInterval); // Adjust interval as needed
        Debug.Log("running again");
        CheckConnection(); // Restart the coroutine
    }
}
