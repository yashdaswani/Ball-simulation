using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RearrangeSizes : MonoBehaviour
{
    [SerializeField] Sprite tube;
    private void OnEnable()
    {
        SetSizes(transform.gameObject);
        //PlayerPrefs.SetInt(MyLevels, 1);
    }
    void SetSizes(GameObject go)
    {
        if(go.name == "Square")
        {
            go.transform.localScale = new Vector3(0.08f, 0.1f, 0.08f);
            go.GetComponent<PolygonCollider2D>().offset = new Vector2(0, 1.2f);
            go.GetComponent<SpriteRenderer>().sprite = tube;
            go.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
            go.GetComponent<SpriteRenderer>().sortingOrder = 1; ;
        }
        if(go.transform.childCount > 0)
        {
            for(int i = 0; i < go.transform.childCount; i++)
            {
                SetSizes(go.transform.GetChild(i).gameObject);
            }
        }
    }
}
