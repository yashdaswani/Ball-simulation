using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class gameManager : MonoBehaviour
{
    public AudioSource levelComplete;
    public AudioSource ballHit;
    public AudioSource holderhit;
    public AudioSource holderFull;    
    public GameObject spawnPoints;
    public GameObject holder;
    public GameObject chearsScreen;
    public GameObject levelparent;
    public GameObject levelparentPlus;
    private GameObject detachableChild;
    private GameObject detachableball;
    private GameObject initial;
    private GameObject initial2;
    private GameObject final;
    private GameObject final2;
    public TMP_Text levelCount;
    public TMP_Text highestlevelCount;
    public TMP_Text movesCount;
    public TMP_Text notice;
    public int levelsCreated;
    public int childPosForBalls;
    public int forceFactor = 100;
    public GameObject[] ball;
    float a1= 0;
    float b1= 84;
    float c1= 67;
    int numHolderInLevel;
    int MovesCount = 0;
    int GiveHintCount = 0;
    public bool countchild = false;
    bool loadNextLevel = false;
    bool firstTouch = false;
    bool makeProjectileNow = false;
    bool projectileStarted = false;
    bool projectiling = false;
    bool projectileEnded = false;
    bool transitioning = false;
    bool WorkingOnPrevTouch = false;
    bool switchingback = false;
    public bool continueUpdating = false;
    public static gameManager instance;
    string Rewarded_Ad = "RewardedAd";
    void CreateLevel2(int level)
    {
        int holders;
        int activeHolders;
        if (level < 55)
        {
            holders = (level / 5) + 6;
            activeHolders = holders - 2;
        }
        else
        {
            holders = 17;
            activeHolders = 15;
        }

        Transform transfrom = levelparent.transform;
        GameObject NewParent = Instantiate(levelparentPlus, transfrom);
        for (int i = 0; i < 18; i++)
        {
            if (i < holders)
            {
                NewParent.transform.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                NewParent.transform.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        List<int> randomVals = new List<int>();
        for (int i = 0; i < activeHolders; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Transform ballTrans = NewParent.transform.GetChild(i).GetChild(1).GetChild(j).transform;
                int randomBall = 0;
                do
                {
                    randomBall = Random.Range(0, activeHolders);
                }
                while (randomVals.Contains(randomBall));
                randomVals.Add(randomBall);
                Instantiate(ball[randomBall], ballTrans);
                if (randomVals.Count == activeHolders)
                {
                    randomVals.Clear();
                }
            }
        }

    }
    private void Awake()
    {
        MyLevels = FirebaseSetup.instance.CurrentLeaderboardStatName;
        if(instance == null)
        {
            instance = this;
        }
    }

    public string MyLevels = "myLevel";
    
    public void AddTube()
    {
        AdsManager.instance.ShowAutoRewardedAd();
        //PlayerPrefs.SetInt(MyLevels, 34);
        if (PlayerPrefs.GetInt(MyLevels) < 4)
        {
            notice.text = "Available from level 5.";
            notice.transform.parent.gameObject.SetActive(true);
            lateFnction(2, "CloseGameObject");
            return;
        }
        if(PlayerPrefs.GetInt(MyLevels) <= levelsCreated)
        {
            int lasttubeindex = levelparent.transform.GetChild(PlayerPrefs.GetInt(MyLevels) - 1).transform.childCount - 1;
            if (!levelparent.transform.GetChild(PlayerPrefs.GetInt(MyLevels) - 1).GetChild(lasttubeindex).gameObject.activeInHierarchy)
            {
                if (levelparent.transform.GetChild(PlayerPrefs.GetInt(MyLevels) - 1).gameObject.activeInHierarchy)
                {
                    levelparent.transform.GetChild(PlayerPrefs.GetInt(MyLevels) - 1).GetChild(lasttubeindex).gameObject.SetActive(true);
                }

            }
            else
            {
                notice.text = "Already used this feature in this level.";
                notice.transform.parent.gameObject.SetActive(true);
                lateFnction(2, "CloseGameObject");
            }
        }
        else
        {
            int lasttubeindex = levelparent.transform.GetChild(21).transform.childCount - 1;
            if (!levelparent.transform.GetChild(21).GetChild(lasttubeindex).gameObject.activeInHierarchy)
            {
                if (levelparent.transform.GetChild(21).gameObject.activeInHierarchy)
                {
                    levelparent.transform.GetChild(21).GetChild(lasttubeindex).gameObject.SetActive(true);
                }

            }
            else
            {
                notice.text = "Already used this feature in this level.";
                notice.transform.parent.gameObject.SetActive(true);
                lateFnction(2, "CloseGameObject");
            }
        }
        
        //if(PlayerPrefs.GetInt(MyLevels) > levelsCreated)
        //{

        //}
        //else
        //{
        //    notice.text = "Use this feature after completing 20 levels first.";
        //    notice.transform.parent.gameObject.SetActive(true);
        //    lateFnction(2, "CloseGameObject");
        //}

    }
    void lateFnction(int lateTime, string nextFncnCall)
    {
        Invoke(nextFncnCall, lateTime);
    }
    void CloseGameObject()
    {
        notice.transform.parent.gameObject.SetActive(false);
    }
    void Start()
    {
        //hintcount();
        //loginWithPlayFab.instance.SendLeaderboard(PlayerPrefs.GetInt("highestLevel", 0));
        MovesCount = 0;
        if (!PlayerPrefs.HasKey(MyLevels))
        {
            PlayerPrefs.SetInt(MyLevels, 1);
        }
        continueUpdating = true;
        levelCount.text = PlayerPrefs.GetInt(MyLevels).ToString();
        if (PlayerPrefs.GetInt(MyLevels) <= levelsCreated)
        {
            for (int i = 0; i < PlayerPrefs.GetInt(MyLevels); i++)
            {
                if (i + 1 == PlayerPrefs.GetInt(MyLevels))
                {
                    levelparent.transform.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    levelparent.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
            numHolderInLevel = levelparent.transform.GetChild(PlayerPrefs.GetInt(MyLevels) - 1).childCount;
        }
        else
        {
            CreateLevel2(PlayerPrefs.GetInt(MyLevels));
        }
        highestlevelCount.text = "highest level : " + PlayerPrefs.GetInt("highestLevel").ToString();
    }
    void Update()
    {
        if(!WorkingOnPrevTouch)
        {
            checkTouch();
        }
        
        if(countchild)
        {
            PlayerPrefs.DeleteAll();
            countchild = false;
        }
        if (continueUpdating)
        {
            checkLevelCompletion();
        }
        if (PlayerPrefs.GetInt(MyLevels) >= PlayerPrefs.GetInt("highestLevel"))
        {
            int highestLevel = PlayerPrefs.GetInt(MyLevels);
            PlayerPrefs.SetInt("highestLevel", highestLevel);
            highestlevelCount.text = "highest level : " + PlayerPrefs.GetInt("highestLevel").ToString();

        }
    }
    public void levelPlus()
    {
        AdsManager.instance.ShowInterstital();
        continueUpdating = false;
        int temp = PlayerPrefs.GetInt(MyLevels);
        int temp2 = PlayerPrefs.GetInt("highestLevel");
        if(temp < temp2)
        {
            temp++;
            PlayerPrefs.SetInt(MyLevels, temp);
            loadLevel(SceneManager.GetActiveScene().buildIndex);
        }
        levelCount.text = PlayerPrefs.GetInt(MyLevels).ToString();

    }
    public void levelMinus()
    {
        AdsManager.instance.ShowInterstital();
        continueUpdating = false;
        int temp = PlayerPrefs.GetInt(MyLevels);
        int temp2 = PlayerPrefs.GetInt("highestLevel");
        if (temp > 1)
        {
            temp--;
            PlayerPrefs.SetInt(MyLevels, temp);
            loadLevel(SceneManager.GetActiveScene().buildIndex);
        }
        levelCount.text = PlayerPrefs.GetInt(MyLevels).ToString();
    }
    void UpdateMovesCount()
    {
        movesCount.text = MovesCount.ToString();
    }
    void checkLevelCompletion()
    {
        checkAllHolders();
    }
    void checkAllHolders()
    {
        int allComplete = 1;
        for (int i = 0; i < activeHolderCount(); i++)
        {
            if(PlayerPrefs.GetInt(MyLevels) <= levelsCreated)
            {
                GameObject temp = levelparent.transform.GetChild(PlayerPrefs.GetInt(MyLevels) - 1).GetChild(i).gameObject;
                allComplete = allComplete * checkOneHolder(temp);
            }
            else if(PlayerPrefs.GetInt(MyLevels) > levelsCreated)
            {
                GameObject temp = levelparent.transform.GetChild(21).GetChild(i).gameObject;
                allComplete = allComplete * checkOneHolder(temp);
            }
            
        }
        if(allComplete == 1 && !loadNextLevel)
        {
            NextLevel();
        }
    }
    int countAllChilds(GameObject holder)
    {
        int ReturnVal = 0;
        for(int i = 0; i < 5;  i++)
        {
            if (holder.transform.GetChild(1).GetChild(i).childCount != 0)
            {
                ReturnVal++;
            }
        }
        return ReturnVal;
    }
    int checkOneHolder(GameObject holder)
    {
        int returnValue = 0;
        if(holder.transform.GetChild(1).GetChild(0).childCount == 0)
        {
            returnValue = 1;
        }
        else if(holder.transform.GetChild(1).GetChild(0).childCount != 0)
        {
            if(countAllChilds(holder) == 5)
            {
                if(holder.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).gameObject.tag != "ball")
                {
                    if (holder.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).gameObject.tag == holder.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(0).gameObject.tag)
                    {
                        if (holder.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(0).gameObject.tag == holder.transform.GetChild(1).GetChild(2).GetChild(0).GetChild(0).gameObject.tag)
                        {
                            if (holder.transform.GetChild(1).GetChild(2).GetChild(0).GetChild(0).gameObject.tag == holder.transform.GetChild(1).GetChild(3).GetChild(0).GetChild(0).gameObject.tag)
                            {
                                if (holder.transform.GetChild(1).GetChild(3).GetChild(0).GetChild(0).gameObject.tag == holder.transform.GetChild(1).GetChild(4).GetChild(0).GetChild(0).gameObject.tag)
                                {
                                    holder.GetComponent<PolygonCollider2D>().enabled = false;
                                    for (int i = 0; i < 5; i++)
                                    {
                                        holder.transform.GetChild(1).GetChild(i).GetChild(0).GetChild(0).gameObject.tag = "ball";
                                    }
                                    HolderFullSound();
                                    returnValue = 1;
                                }
                            }
                        }
                    }
                }
                else
                {
                    int checkAll = 1;
                    for (int i = 0; i < 5; i++)
                    {
                        if(holder.transform.GetChild(1).GetChild(i).GetChild(0).GetChild(0).gameObject.tag == "ball")
                        {
                            checkAll = checkAll * 1;
                        }
                        else
                        {
                            checkAll = checkAll * 0;
                        }
                        
                    }
                    returnValue = checkAll;
                }
                
            }
            else
            {
                returnValue = 0;
            }
        }
        return returnValue; 
    }
    int activeHolderCount()
    {
        int holderCount = 0;

        if (PlayerPrefs.GetInt(MyLevels) <= levelsCreated)
        {
            for (int i = 0; i < numHolderInLevel; i++)
            {
                if (levelparent.transform.GetChild(PlayerPrefs.GetInt(MyLevels) - 1).GetChild(i).gameObject.activeInHierarchy && levelparent.transform.GetChild(PlayerPrefs.GetInt(MyLevels) - 1).GetChild(i).gameObject.tag != "noteCanvas")
                {
                    holderCount++;
                }
            }
        }
        else
        {
            if (PlayerPrefs.GetInt(MyLevels) < 55)
            {
                holderCount = (PlayerPrefs.GetInt(MyLevels) / 5) + 6;
            }
            else
            {
                holderCount = 17;
            }
        }
        
        return holderCount;
    }
    void NextLevel()
    {
        if (PlayerPrefs.GetInt("NoAds") == 0)
        {
            AdsManager.instance.ShowAutoRewardedAd();
        }
        loadNextLevel = true;
        Invoke("NextLevelAfterAnimation", 0.1f);
        chearsScreen.SetActive(true);
        chearsScreen.GetComponent<LevelCompleteManager>().LevelCompleted(PlayerPrefs.GetInt(MyLevels));
        levelComplete.Play();
    }
    void NextLevelAfterAnimation()
    {
        UpdateMovesCount();
        int temp = PlayerPrefs.GetInt(MyLevels);
        loginWithPlayFab.instance.SendLeaderboard(temp);
        temp++;
        PlayerPrefs.SetInt(MyLevels, temp);
        levelCount.text = temp.ToString();
        UpdateMovesCount();
    }
    public void NextButtonOnLevelClear()
    {
        MovesCount = 0;
        loadLevel(2);
        loadNextLevel = false;
    }
    void checkTouch()
    {
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !transitioning)
        {
            GiveHintCount = 0;
            Touch touch = Input.GetTouch(0);
            var pos = touch.position;
            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);
            if (hitInfo)
            {
                if(hitInfo.transform.gameObject.CompareTag("holder") && firstTouch == false)
                {
                    if(countAllChilds(hitInfo.transform.gameObject) != 0)
                    {
                        if(checkOneHolder(hitInfo.transform.gameObject) == 0)
                        {
                            initial = hitInfo.transform.gameObject;
                            firstTouch = true;
                            changeColorOnfirstTouch();
                            SoundOninitialTouch();
                        }   
                    }
                }
                else if((hitInfo.transform.gameObject.CompareTag("ball1") || hitInfo.transform.gameObject.CompareTag("ball2") || hitInfo.transform.gameObject.CompareTag("ball3") || hitInfo.transform.gameObject.CompareTag("ball4") || hitInfo.transform.gameObject.CompareTag("ball5") || hitInfo.transform.gameObject.CompareTag("ball6") || hitInfo.transform.gameObject.CompareTag("ball7") || hitInfo.transform.gameObject.CompareTag("ball8") || hitInfo.transform.gameObject.CompareTag("ball9") || hitInfo.transform.gameObject.CompareTag("ball10")) && firstTouch == false)
                {
                    initial = hitInfo.transform.gameObject.transform.parent.parent.parent.parent.gameObject;
                    firstTouch = true;
                    SoundOninitialTouch();
                    changeColorOnfirstTouch();
                }
                else if (hitInfo.transform.gameObject.CompareTag("holder") && firstTouch == true)
                {
                    if(hitInfo.transform.gameObject != initial)
                    {
                        if (countAllChilds(hitInfo.transform.gameObject) != 5)
                        {
                            final = hitInfo.transform.gameObject;
                            if (matchColrs())
                            {
                                SwitchBalls();
                            }
                        }
                        changeColorBackToWhite();
                        firstTouch = false;
                    }
                    
                }
                else if ((hitInfo.transform.gameObject.CompareTag("ball1") || hitInfo.transform.gameObject.CompareTag("ball2") || hitInfo.transform.gameObject.CompareTag("ball3") || hitInfo.transform.gameObject.CompareTag("ball4") || hitInfo.transform.gameObject.CompareTag("ball5") || hitInfo.transform.gameObject.CompareTag("ball6") || hitInfo.transform.gameObject.CompareTag("ball7") || hitInfo.transform.gameObject.CompareTag("ball8") || hitInfo.transform.gameObject.CompareTag("ball9") || hitInfo.transform.gameObject.CompareTag("ball10")) && firstTouch == true)
                {
                    if (hitInfo.transform.gameObject.transform.parent.parent.parent.parent.gameObject != initial)
                    {
                        if (countAllChilds(hitInfo.transform.gameObject.transform.parent.parent.parent.parent.gameObject) != 5)
                        {
                            final = hitInfo.transform.gameObject.transform.parent.parent.parent.parent.gameObject;
                            if (matchColrs())
                            {
                                SwitchBalls();
                            }
                        }
                        changeColorBackToWhite();
                        firstTouch = false;
                    }
                }
                else if (hitInfo.transform.gameObject.CompareTag("ballParent") && firstTouch == true)
                {
                    if (countAllChilds(hitInfo.transform.gameObject.transform.parent.parent.parent.gameObject) != 5)
                    {
                        final = hitInfo.transform.gameObject.transform.parent.parent.parent.gameObject;
                        if (matchColrs())
                        {
                            SwitchBalls();
                        }
                    }
                    changeColorBackToWhite();
                    firstTouch = false;
                }

            }
        }
    }
    void changeColorOnfirstTouch()
    {
        initial.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1,0,0,0.2f);
    }
    void changeColorBackToWhite()
    {
        initial.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.2f);
    }
    bool matchColrs()
    {
        if(getFinalChildNums()!= 0 && getInitialChildNums() != 0)
        {
            if ((initial.transform.GetChild(1).GetChild(getInitialChildNums() - 1).GetChild(0).GetChild(0).gameObject.tag == final.transform.GetChild(1).GetChild(getFinalChildNums()-1).GetChild(0).GetChild(0).gameObject.tag))
            {
                return true;
            }
        }
        else if(getFinalChildNums() == 0)
        {
            return true;
        }
        return false;
        
    }
    void SwitchBalls()
    {
        switchingback = false;
        WorkingOnPrevTouch = true;
        if((initial != final) && !transitioning)
        {
            transitioning = true;
            if (getFinalChildNums() < 5)
            {
                if (getInitialChildNums() > 0 && matchColrs())
                {
                    initial2 = initial;
                    final2 = final;
                    MovesCount++;
                    UpdateMovesCount();
                    detachableChild = initial.transform.GetChild(1).GetChild(getInitialChildNums() - 1).GetChild(0).gameObject;
                    detachableball = initial.transform.GetChild(1).GetChild(getInitialChildNums() - 1).GetChild(0).GetChild(0).gameObject;
                    makeProjectileMotion(initial, final);
                }
            }
        }
    }
    public void switchBack()
    {
        if(switchingback)
        {
            AdsManager.instance.ShowAutoRewardedAd();
            switchingback = false;
            transitioning = true;
            MovesCount++;
            UpdateMovesCount();
            detachableChild = final2.transform.GetChild(1).GetChild(getFinalChildNums() - 1).GetChild(0).gameObject;
            detachableball = final2.transform.GetChild(1).GetChild(getFinalChildNums() - 1).GetChild(0).GetChild(0).gameObject;
            detachableChild.transform.GetChild(0).transform.position = initial2.transform.GetChild(2).transform.position;
            detachableChild.transform.parent = initial2.transform.GetChild(1).GetChild(getInitialChildNums());
            transitioning = false;
        }
        
    }
    void makeProjectileMotion(GameObject initial, GameObject final)
    {
        projectileStarted = false;
        projectiling = false;
        projectileEnded = false;
        Vector3 point1;
        Vector3 point2;
        Vector3 point3;
        point1 = initial.transform.GetChild(2).transform.position;
        point3 = final.transform.GetChild(2).transform.position;
        detachableball.GetComponent<Rigidbody2D>().gravityScale = 0;
        point2 = new Vector3((point1.x + point3.x)/2, point1.y + 0.5f);
        detachableball.GetComponent<Rigidbody2D>().AddForce((point1 - detachableball.transform.position)*forceFactor);
        detachableball.GetComponent<CircleCollider2D>().isTrigger = true;
        detachableball.GetComponent<CircleCollider2D>().radius = 2.25f;
        detachableball.GetComponent<SpriteRenderer>().sortingOrder = 1;
        easetype = GetRandomEaseType();
        durationPeriod = Random.Range(0.3f,0.5f);
        makeProjectileNow = true;

    }
    Ease GetRandomEaseType()
    {
        // Array of available EaseTypes
        Ease[] allEaseTypes = {Ease.OutQuad, Ease.OutCirc, Ease.InBack, Ease.OutBack,
                                };

        // Get a random index from the array
        int randomIndex = Random.Range(0, allEaseTypes.Length);

        // Return the randomly selected EaseType
        return allEaseTypes[randomIndex];
    }
    public Ease easetype;
    public float durationPeriod;
    void makeProjectileMotion2()
    {
        Vector3 point1;
        Vector3 point2;
        Vector3 point3;
        point1 = initial.transform.GetChild(2).transform.position;
        point3 = final.transform.GetChild(2).transform.position;
        point2 = new Vector3((point1.x + point3.x) / 2, point1.y + 0.5f);
        
        if((detachableball.transform.position == point1 || detachableball.transform.position.y > point1.y) && !projectileStarted)
        {
            detachableball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            projectileStarted = true;
            detachableball.transform.DOMove(point2, durationPeriod).SetEase(easetype);
            
            //detachableball.GetComponent<Rigidbody2D>().AddForce((point2 - detachableball.transform.position) * forceFactor);
        }
        if((detachableball.transform.position == point2 || detachableball.transform.position.y > point2.y) && !projectiling)
        {
            detachableball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            projectiling = true;
            detachableball.transform.DOMove(point3, durationPeriod).SetEase(easetype);
            //detachableball.GetComponent<Rigidbody2D>().AddForce((point3 - detachableball.transform.position) * forceFactor);
        }
        if((detachableball.transform.position == point3) && !projectileEnded )
        {
            detachableball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            
            detachableChild.transform.GetChild(0).transform.position = final.transform.GetChild(2).transform.position;
            detachableChild.transform.parent = final.transform.GetChild(1).GetChild(getFinalChildNums());
            detachableball.GetComponent<Rigidbody2D>().gravityScale = 1;
            detachableball.GetComponent<CircleCollider2D>().isTrigger = false;
            Invoke("makeColliderActiveAgain", 0.1f);

            detachableball.GetComponent<SpriteRenderer>().sortingOrder = 0;
            if (matchColrs() && getFinalChildNums() < 5 && getInitialChildNums() > 0)
            {
                transitioning = false;
                SwitchBalls();
            }
            else
            {
                makeProjectileNow = false;
                projectileEnded = true;
                transitioning = false;
                WorkingOnPrevTouch = false;
                switchingback = true;
            }
            HolderHitSound();
        }
    }
    void makeColliderActiveAgain()
    {
        detachableball.GetComponent<CircleCollider2D>().radius = 2.3f;
    }
    private void FixedUpdate()
    {
        if(makeProjectileNow)
        {
            makeProjectileMotion2();
        }
    }
    int getFinalChildNums()
    {
        int finalChildNum = 0;
        for(int i = 0; i < 5; i++)
        {
            if (final.transform.GetChild(1).GetChild(i).childCount != 0)
            {
                finalChildNum++;
            }
        }
        return finalChildNum;
    }
    int getInitialChildNums()
    {
        int initialChildNum = 0;
        for (int i = 0; i < 5; i++)
        {
            if (initial.transform.GetChild(1).GetChild(i).childCount != 0)
            {
                initialChildNum++;
            }
        }
        return initialChildNum;
    }
    public void loadLevel(int level)
    {
        //AdsManager.instance.ShowInterstital();
        continueUpdating = false;
        SceneManager.LoadScene(level);
    }
    public void quitGame()
    {
        Application.Quit();
    }
    public void playAgain()
    {
        PlayerPrefs.DeleteKey(MyLevels);
        
        loadLevel(1);
    }
    public void SoundOninitialTouch()
    {
        ballHit.volume = 0.7f;
        ballHit.Play();
    }
    public void HolderFullSound()
    {
        holderFull.volume = 0.7f;
        holderFull.Play();
    }
    public void HolderHitSound()
    {
        holderhit.volume = 0.7f;
        holderhit.Play();
    }
}