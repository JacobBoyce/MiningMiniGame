using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Cinemachine;

public class MiningManager : MonoBehaviour
{
    public CinemachineController camController;
    public Joystick joystick;

    [Header("Target Vars")]
    public GameObject targetPrefab;
    public GameObject targetParent, crosshairPrefab;
    private GameObject curTarget, crosshair;
    private Rigidbody2D rb;
    private float refreshRateMin = .5f, refreshRateMax = 1.2f, curRefreshRate;
    public float maxTime = 5f, curTime;
    private bool targetStart = false;
    private int randDirectionVal, randSignVal;

    [Header("Init stuff")]
    public int targetLoopNum;
    public int curLoop, initialWaitTime;

    [Header("Difficulty Vars")]
    public float freq;
    public float str, clamp, crosshairStr;
    private int plevel, ppower, pfocus;
    [Space(10)]
    public float perfectRange;
    public float aweRange;    

    [Header("Mining UI")]
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI rewardText, xpText, oresRecievedText;
    private int largeOreTotal, smallOreTotal;
    public GameObject miningResults;

    [Header("Visual stuff")]
    public TMP_InputField power;
    public TMP_InputField crossStr, limit, timer;
    public TextMeshProUGUI curtimer;

    private Mineral targetOre;

    [Header("Visual Rock Stuff")]
    [SerializeField]
    public List<GameObject> rocks;
    private int rockPileIterator = 0;

    [Header("Probability and Score")]
    public int completedScore;
    public int curScore;
    private int chancePerf = 75, chanceAwe = 80;
    


    // Start is called before the first frame update
    void Start()
    {
        rockPileIterator = 0;
        completedScore = 0;
        //ui updating
        power.text = str.ToString();
        crossStr.text = crosshairStr.ToString();
        limit.text = clamp.ToString();
        timer.text = maxTime.ToString();

        //get ore stats
        targetOre = GetComponent<MiningLibrary>().GetOre(PlayerPrefs.GetString("Ore"));

        //get player stats
        plevel = PlayerPrefs.GetInt("Plevel");
        ppower = PlayerPrefs.GetInt("Ppower");
        pfocus = PlayerPrefs.GetInt("Pfocus");

        //adjust values depending on player stats
        AdjustDifficulty();
        clamp = targetOre.maxVel;
        crosshairStr = targetOre.durability;

        //start the first target, to then loop
        StartCoroutine(SpawnTargetLoop(initialWaitTime));

        //reset player prefs to prevent cheating
        PlayerPrefs.SetInt("XPtoget", 0);
        PlayerPrefs.SetInt("LargeOreTotal", 0);
        PlayerPrefs.SetInt("SmallOreTotal", 0);
        PlayerPrefs.SetString("DoneMining", "false");
    }

    public void AdjustDifficulty()
    {
        /*
            <->adjust all vars by player level %
            ->adjust clamp/crosshairStr by player focus
            ->percentage of winning rocks are affected by level and players focus (level first then focus)
        */
        //if the player is higher
        if(plevel > targetOre.level)
        {
            int diff = plevel - targetOre.level;
            if(diff >= 2)
            {
                //favors player
                targetOre.durability = targetOre.durability - (int)(targetOre.durability * .4f);
                targetOre.maxVel = targetOre.maxVel - (int)(targetOre.maxVel *.2f);
                chancePerf += 25;
                chanceAwe += 20;
            }
            else if(diff == 1)
            {
                targetOre.durability = targetOre.durability - (int)(targetOre.durability * .2f);
                targetOre.maxVel = targetOre.maxVel - (int)(targetOre.maxVel *.1f);
                chancePerf += 10;
                chanceAwe += 10;
            }
        }
        //if the player is lower
        else if(plevel < targetOre.level)
        {
            int diff2 = targetOre.level - plevel;
            if(diff2 >= 2)
            {
                //favors rock
                targetOre.durability = targetOre.durability + (int)(targetOre.durability * .2f);
                targetOre.maxVel = targetOre.maxVel + (int)(targetOre.maxVel *.2f);
            }
            else if(diff2 == 1)
            {
                targetOre.durability = targetOre.durability + (int)(targetOre.durability * .1f);
                targetOre.maxVel = targetOre.maxVel + (int)(targetOre.maxVel *.1f);
            }
        }
    }

    public void SpawnTarget()
    {
        //turn off ui that showed the result
        resultText.gameObject.SetActive(false);
        rewardText.gameObject.SetActive(false);

        //spawn and set the location of the target and crosshair
        curTarget = Instantiate(targetPrefab, Vector3.zero, Quaternion.identity);
        crosshair = Instantiate(crosshairPrefab, Vector3.zero, Quaternion.identity);

        curTarget.transform.SetParent(targetParent.transform);
        crosshair.transform.SetParent(targetParent.transform);
        rb = crosshair.GetComponent<Rigidbody2D>();

        curTarget.transform.localPosition = Vector3.zero;
        crosshair.transform.localPosition = Vector3.zero;
        
        //set current time and update pos for moving the crosshair, bool for triggering the movement
        curRefreshRate = Random.Range(refreshRateMin, refreshRateMax);
        curTime = maxTime;
        UpdateCrosshairPos();
        targetStart = true;
        
    }

    public IEnumerator SpawnTargetLoop(float waitTime)
    {
        //***** Set camera script call here!!!!!!!!!!!!!
        camController.SwitchPriority("rest", rockPileIterator);
        yield return new WaitForSeconds(waitTime);
        //call cam script to change targets
        camController.SwitchPriority("focus", rockPileIterator);
        yield return new WaitForSeconds(2f);
        SpawnTarget();
    }
    
    public void Update()
    {
        if(targetStart == true)
        {
            //ui timer
            curtimer.text = curTime.ToString("n2");

            curTime -= Time.deltaTime;
            if(curTime <= 0)
            {
                //stop target from moving
                targetStart = false;
                rb.simulated = false;

                //calc distance to get score
                float dist = Vector2.Distance(rb.gameObject.transform.localPosition, curTarget.transform.localPosition);
                //turn on text ui to see result
                resultText.gameObject.SetActive(true);
                rewardText.gameObject.SetActive(true);

                //roll to see what rewards you get for specific performance
                int roll = Random.Range(0, 100);
                if(dist <= perfectRange)
                {
                    resultText.text = "Perfect!";
                    curScore = 2;

                    if(roll <= chancePerf)
                    {
                        rewardText.text = "Got\n" + targetOre.name + " (Lg)!";
                        largeOreTotal++;
                    }
                    else
                    {
                        rewardText.text = "Got\n" + targetOre.name + " (Sm)!";
                        smallOreTotal++;
                    }
                    
                }
                else if(dist > perfectRange && dist < aweRange)
                {
                    resultText.text = "Great!";
                    curScore = 1;

                    if(roll <= chanceAwe)
                    {
                        rewardText.text = "Got\n" + targetOre.name + " (Sm)!";
                        smallOreTotal++;
                    }
                    else
                    {
                        rewardText.text = "No Luck...";
                    }
                }
                else
                {
                    //if bad then the player missed, which ends the mining operation
                    //bool to stop the loop
                    resultText.text = "whoops...";
                    rewardText.text = "";
                    curScore = 0;
                }
                completedScore += curScore;

                //\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
                //make sparks fly and add force to rocks -- pass in score to show certain rock animations
                rocks[rockPileIterator].GetComponent<RockPileController>().TurnOnRocks(curScore);
                rockPileIterator++;
                //obj move to hit the rocks                

                //RESET HERE TO CALL THE SPAWN TARGET COROUTINE
                Destroy(curTarget);
                Destroy(crosshair);
                if(curLoop < targetLoopNum)
                {
                    curLoop++;
                    StartCoroutine(SpawnTargetLoop(2f));
                }
                else
                {
                    //mining completed
                    StartCoroutine("MiningCompleted");
                }
                targetStart = false;
            }
            /*
                #region view target distance constantly
                //***Constantly View target distance***\\
                float dist2 = Vector2.Distance(rb.gameObject.transform.localPosition, curTarget.transform.localPosition);
                if(dist2 <= perfectRange)
                {
                    uiText.text = "Perfect";
                }
                else if(dist2 > perfectRange && dist2 < aweRange)
                {
                    uiText.text = "Awesome";
                }
                else
                {
                    uiText.text = "Bad";
                }
                
                #endregion
                */
        } 
    }

    public IEnumerator MiningCompleted()
    {
        yield return new WaitForSeconds(2f);
        //turn of result and reward ui and others
        resultText.gameObject.SetActive(false);
        rewardText.gameObject.SetActive(false);
        joystick.gameObject.SetActive(false);

        //turn on end mining results
        miningResults.SetActive(true);
        completedScore *= 10;
        xpText.text = (completedScore).ToString();
        string temp1 = largeOreTotal == 0 ? "" : "+" + largeOreTotal + " " + targetOre.name + " (Lg)\n";
        string temp2 = smallOreTotal == 0 ? "" : "+" + smallOreTotal + " " + targetOre.name + " (Sm)";
        oresRecievedText.text = temp1 + temp2;

        PlayerPrefs.SetInt("XPtoget", completedScore);
        PlayerPrefs.SetInt("LargeOreTotal", largeOreTotal);
        PlayerPrefs.SetInt("SmallOreTotal", smallOreTotal);
        PlayerPrefs.SetString("DoneMining", "true");
    }

    //Moving the crosshair
    #region fixedUpdate

    public void FixedUpdate()
    {
        if(targetStart == true)
        {
            curRefreshRate -= Time.deltaTime;
            if(curRefreshRate <= 0)
            {
                curRefreshRate = Random.Range(refreshRateMin, refreshRateMax);
                UpdateCrosshairPos();
            }
            
            rb.velocity += new Vector2(joystick.Horizontal*str, joystick.Vertical*str);
            //newCrosshair.GetComponent<RectTransform>().Translate(new Vector2(joystick.Horizontal*5, joystick.Vertical*5));
            
            //check if too far away from the target to stop the cross hair
            if(Vector2.Distance(rb.gameObject.transform.localPosition, curTarget.transform.localPosition) < 800)
            {
                //check which direction to move
                if(randDirectionVal == 1)
                {
                    //y sin
                    rb.AddRelativeForce(new Vector2(crosshairStr*randSignVal, crosshairStr * Mathf.Sin(Time.time * freq)));
                }
                else if(randDirectionVal == 2)
                {
                    //x sin
                    rb.AddRelativeForce(new Vector2(crosshairStr * Mathf.Sin(Time.time * freq), crosshairStr*randSignVal));
                }
            }   
            else
            {
                rb.velocity = Vector2.zero;
            }
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, clamp);
        }
    }

    public void UpdateCrosshairPos()
    {       
        randDirectionVal = Random.Range(1,3);
        randSignVal = Random.Range(0,2)*2-1;
    }
    #endregion

    //for visual ui testing
    public void UpdateValues()
    {
        str = float.Parse(power.text);
        crosshairStr = float.Parse(crossStr.text);
        clamp = float.Parse(limit.text);
        maxTime = float.Parse(timer.text);
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("MiningSearch");
    }

}
