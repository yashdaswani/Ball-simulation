using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeaderManager : MonoBehaviour
{
    public static HeaderManager instance;
    

    [SerializeField] Text _coinsText;
    [SerializeField] Text _gemsText;

    private void Start()
    {
        int currentAmount = PlayerPrefs.GetInt("GoldAmount", 0);
        _coinsText.text = currentAmount.ToString();
        currentAmount = PlayerPrefs.GetInt("GemAmount", 0);
        _gemsText.text = currentAmount.ToString();
    }
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }
    public void AddCoins(int amount)
    {
        int currentAmount = PlayerPrefs.GetInt("GoldAmount", 0);
        currentAmount += amount;
        PlayerPrefs.SetInt("GoldAmount", currentAmount);
        _coinsText.text = currentAmount.ToString();
    }
    public void AddGems(int amount)
    {
        int currentAmount = PlayerPrefs.GetInt("GemAmount", 0);
        currentAmount += amount;
        PlayerPrefs.SetInt("GemAmount", currentAmount);
        _gemsText.text = currentAmount.ToString();
    }
    public void SubstractCoins(int amount)
    {
        int currentAmount = PlayerPrefs.GetInt("GoldAmount", 0);
        currentAmount -= amount;
        PlayerPrefs.SetInt("GoldAmount", currentAmount);
        _coinsText.text = currentAmount.ToString();
    }
    public void SubstractGems(int amount)
    {
        int currentAmount = PlayerPrefs.GetInt("GemAmount", 0);
        currentAmount -= amount;
        PlayerPrefs.SetInt("GemAmount", currentAmount);
        _gemsText.text = currentAmount.ToString();
    }
}
