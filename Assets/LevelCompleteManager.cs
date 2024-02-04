using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteManager : MonoBehaviour
{
    [SerializeField] string[] glitterWords;
    [SerializeField] GameObject[] _stars;
    [SerializeField] Text _levelText;
    [SerializeField] TMPro.TMP_Text _welcomeText;

    public void LevelCompleted(int level)
    {
        _levelText.text = level.ToString();
        _welcomeText.text = glitterWords[Random.Range(0, glitterWords.Length)]+ "! level cleared";
        int random = Random.Range(0, 2);
        if(random == 0)
        {
            _stars[0].SetActive(true);
            _stars[1].SetActive(true);
            _stars[2].SetActive(false);
        }
        else
        {
            _stars[0].SetActive(true);
            _stars[1].SetActive(true);
            _stars[2].SetActive(true);
        }
    }
}
