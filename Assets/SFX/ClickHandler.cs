using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickHandler : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] Camera cam;
    void Update()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    Ray ray = cam.ScreenPointToRay(touch.position);
                    RaycastHit hit;

                    // Check if the ray hits something
                    if (Physics.Raycast(ray, out hit))
                    {
                        Vector3 collisionPoint = hit.point;
                        Instantiate(prefab, collisionPoint, prefab.transform.rotation);
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
    //effect 1 start speed: 5-80
    //effect 2 start speed: 5-10
}
