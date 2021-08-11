using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiningManager : MonoBehaviour
{
    public Camera cam;
    public Canvas cv;
    public TextMeshProUGUI uiText;
    public Joystick joystick;

    [Header("Target Vars")]
    public GameObject targetPrefab, crosshairPrefab;
    public GameObject targetParent;
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

    [Header("Visual stuff")]
    public TMP_InputField power;
    public TMP_InputField crossStr, limit, timer;
    public TextMeshProUGUI curtimer;

    private Mineral targetOre;

    [Header("Visual Rock Stuff")]
    [SerializeField]
    public List<GameObject> rocks;
    private int i = 0;
    private Vector2 bb;
    


    // Start is called before the first frame update
    void Start()
    {
        i =0;
        bb = new Vector2((float)cv.GetComponent<RectTransform>().sizeDelta.x / 2f, (float)cv.GetComponent<RectTransform>().sizeDelta.y / 2f);
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
    }

    public void AdjustDifficulty()
    {
        /*
            <->adjust all vars by player level %
            ->adjust power by player power
            ->adjust clamp/crosshairStr by player focus
        */
        if(plevel > targetOre.level)
        {
            int diff = plevel - targetOre.level;
            if(diff >= 2)
            {
                //favors player
                targetOre.durability = targetOre.durability - (int)(targetOre.durability * .2f);
            }
            else if(diff == 1)
            {
                targetOre.durability = targetOre.durability - (int)(targetOre.durability * .1f);
            }
        }
        else if(plevel < targetOre.level)
        {
            int diff2 = targetOre.level - plevel;
            if(diff2 >= 2)
            {
                //favors player
                targetOre.durability = targetOre.durability + (int)(targetOre.durability * .2f);
            }
            else if(diff2 == 1)
            {
                targetOre.durability = targetOre.durability + (int)(targetOre.durability * .1f);
            }
        }
    }

    public void SpawnTarget()
    {
        //spawn and set the location of the target and crosshair
        curTarget = Instantiate(targetPrefab, Vector3.zero, Quaternion.identity);
        crosshair = Instantiate(crosshairPrefab, Vector3.zero, Quaternion.identity);

        curTarget.transform.SetParent(targetParent.transform);
        curTarget.transform.localPosition = new Vector3(0,100,0);

        crosshair.transform.SetParent(targetParent.transform);
        crosshair.transform.localPosition = new Vector3(0,100,0);
        rb = crosshair.GetComponent<Rigidbody2D>();

        RectTransform CanvasRect = cv.GetComponent<RectTransform>();
        
        //set current time and update pos for moving the crosshair, bool for triggering the movement
        curRefreshRate = Random.Range(refreshRateMin, refreshRateMax);
        curTime = maxTime;
        UpdateCrosshairPos();
        targetStart = true;
        
    }

    public IEnumerator SpawnTargetLoop(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
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
                float dist = Vector2.Distance(rb.gameObject.transform.localPosition, new Vector2(0,0));
                if(dist <= perfectRange)
                {
                    uiText.text = "Perfect";
                }
                else if(dist > perfectRange && dist < aweRange)
                {
                    uiText.text = "Awesome";
                }
                else
                {
                    //if bad then the player missed, which ends the mining operation
                    //bool to stop the loop
                    uiText.text = "Bad";
                }
                //\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
                //make sparks fly and add force to rocks
                //sparks spawn on cross hair
                rocks[i].GetComponent<RockPileController>().TurnOnRocks();
                i++;
                //obj move to hit the rocks
                
            
                //add force @ pos of rocks[i] .forward
                

                //RESET HERE TO CALL THE SPAWN TARGET COROUTINE
                Destroy(curTarget, 1f);
                Destroy(crosshair, 1f);
                if(curLoop < targetLoopNum)
                {
                    curLoop++;
                    StartCoroutine(SpawnTargetLoop(2f));
                }
                targetStart = false;
            }
                #region view target distance constantly
                /*//***Constantly View target distance***\\
                float dist2 = Vector2.Distance(rb.gameObject.transform.localPosition, new Vector2(0,0));
                if(dist2 <= perfectRange)
                {
                    uiText.text = "Perfect";
                }
                else if(dist2 > perfectRange && dist2 < aweRange)
                {
                    uiText.text = "Awesome";
                }
                else if(dist2 > aweRange && dist2 < goodRange)
                {
                    uiText.text = "Good";
                }
                else
                {
                    uiText.text = "Bad";
                }
                */
                #endregion
        } 
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
            
            //check if too far away from the target to stop the cross hair
            if(Vector2.Distance(rb.gameObject.transform.localPosition, new Vector2(0,0)) < 400)
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
}
