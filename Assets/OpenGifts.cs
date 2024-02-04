using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGifts : MonoBehaviour
{
    [SerializeField] GameObject openedGiftBox;

    public void OpenGift()
    {
        openedGiftBox.SetActive(true);
        gameObject.SetActive(false);
    }
}
