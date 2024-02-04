using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HandleParticipation : MonoBehaviour
{
    [SerializeField] GameObject panel;
    void Start()
    {
        int random = Random.Range(0, 3);
        if(random != 1)
        {
            panel.SetActive(true);
            panel.transform.localScale = Vector3.zero;
            panel.transform.DOScale(Vector3.one, .5f);
        }
    }
}
