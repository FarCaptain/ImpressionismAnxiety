using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreathGameplayController : MonoBehaviour
{
    public GameObject OuterRingObject;
    public float OuterRingSize;
    public float OuterRingRange;
    public GameObject InnerRingObject;
    public float InnerRingSize;
    public float InnerRingRange;
    public GameObject BreathRingObject;
    public static int score =100;
    public int MaxScore = 100;
    public int MinScore = 0;

    public GameObject ScoreTextObject;

    // 100: 100~60 60~30 30~0
    // 250~=4s/ 400 / 600
    public float ScalingRate;
    public int RewardOnScore;

    // audios
    private AudioSource InhaleAudio;
    private AudioSource ExhaleAudio;
    
    public AudioSource InhaleAudioShort;
    public AudioSource ExhaleAudioShort;

    public AudioSource InhaleAudioMid;
    public AudioSource ExhaleAudioMid;

    public AudioSource InhaleAudioLong;
    public AudioSource ExhaleAudioLong;
    


    [SerializeField]
    private bool buttonOnHold = false;
    private RectTransform breathRingTrans;
    private Text scoreText;
    [SerializeField]
    private bool outerRingAsGoal = false;

    private float timeCounter = 0.0f;

    public Camera Cam;
    public string PopupTag;

    public List<GameObject> PopupObjects;

    [SerializeField]
    private int livePopupsCount;

    public float popupsSpawnMaxTime;
    public float popupsSpawnMinTime;

    private float popupsSpawnTimeLeft;

    public int popupsSpawnMaxAmount;
    public int popupsSpanwMinAmount;

    private float popupsSpawnNextAmount;




    // Start is called before the first frame update
    void Start()
    {
        breathRingTrans = BreathRingObject.GetComponent<RectTransform>();
        scoreText = ScoreTextObject.GetComponent<Text>();
        OuterRingObject.GetComponent<RectTransform>().sizeDelta = new Vector2(OuterRingSize, OuterRingSize);
        OuterRingObject.GetComponent<CircleGraphic>().edgeThickness = OuterRingRange / 2;
        InnerRingObject.GetComponent<RectTransform>().sizeDelta = new Vector2(InnerRingSize, InnerRingSize);
        InnerRingObject.GetComponent<CircleGraphic>().edgeThickness = InnerRingRange / 2;

        InnerRingObject.GetComponent<CircleGraphic>().color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
        OuterRingObject.GetComponent<CircleGraphic>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        scoreDisplayUpdate();

        foreach (GameObject popup in PopupObjects)
        {
            popup.SetActive(false);
            livePopupsCount = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        touchPopup();
        
        if(score < 30)
        {
            ScalingRate = 600;
            RewardOnScore = 1;
            InhaleAudio = InhaleAudioShort;
            ExhaleAudio = ExhaleAudioShort;
        }
        else if(score < 60)
        {
            ScalingRate = 300;
            RewardOnScore = 3;
            InhaleAudio = InhaleAudioMid;
            ExhaleAudio = ExhaleAudioMid;
        }
        else
        {
            ScalingRate = 150;
            RewardOnScore = 4;
            InhaleAudio = InhaleAudioLong;
            ExhaleAudio = ExhaleAudioLong;
        }

        if ((breathRingTrans.sizeDelta.x < OuterRingSize && buttonOnHold) || (breathRingTrans.sizeDelta.x > (InnerRingSize - InnerRingRange) && !buttonOnHold)) breathRingScaling(buttonOnHold, Time.deltaTime);
        
        timeCounter += Time.deltaTime;
        if (timeCounter >= 1.0f)
        {
            score = Mathf.Clamp(score - 2, MinScore, MaxScore);
            timeCounter = 0.0f;
            scoreDisplayUpdate();
        }
    }

    public void SetButtonOnHold(bool status)
    {
        buttonOnHold = status;

        if (!buttonOnHold && outerRingAsGoal && inGoalRange())
        {
            // exhale
            ExhaleAudio.Play();
            switchGoal();
            scoreIncrease();
        }
        else if (buttonOnHold && !outerRingAsGoal && inGoalRange())
        {
            // inhale
            InhaleAudio.Play();
            switchGoal();
            scoreIncrease();
        }
    }

    private void breathRingScaling(bool ifExpand, float deltaTime)
    {
        if (ifExpand) breathRingTrans.sizeDelta = new Vector2(breathRingTrans.sizeDelta.x + deltaTime * ScalingRate, breathRingTrans.sizeDelta.y + deltaTime * ScalingRate);
        else breathRingTrans.sizeDelta = new Vector2(breathRingTrans.sizeDelta.x - deltaTime * ScalingRate, breathRingTrans.sizeDelta.y - deltaTime * ScalingRate);
    }

    private bool inGoalRange()
    {
        if (outerRingAsGoal)
        {
            if (breathRingTrans.sizeDelta.x <= (OuterRingSize + Time.deltaTime * ScalingRate) && breathRingTrans.sizeDelta.x >= (OuterRingSize - OuterRingRange)) return true;
            else return false;
        }
        else
        {
            if (breathRingTrans.sizeDelta.x <= InnerRingSize && breathRingTrans.sizeDelta.x >= (InnerRingSize - Time.deltaTime * ScalingRate - InnerRingRange)) return true;
            else return false;
        }
    }

    private void switchGoal()
    {
        outerRingAsGoal = !outerRingAsGoal;
        if (outerRingAsGoal)
        {
            OuterRingObject.GetComponent<CircleGraphic>().color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
            InnerRingObject.GetComponent<CircleGraphic>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        }
        else
        {
            InnerRingObject.GetComponent<CircleGraphic>().color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
            OuterRingObject.GetComponent<CircleGraphic>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        }
    }

    private void scoreIncrease()
    {
        score = Mathf.Clamp(score + RewardOnScore, MinScore, MaxScore);
        scoreDisplayUpdate();
    }

    private void scoreDisplayUpdate()
    {
        scoreText.text = score.ToString();
    }

    private void touchPopup()
    {
        //Debug.Log("touchCount:" + Input.touchCount);
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Ray ray = Cam.ScreenPointToRay(Input.GetTouch(i).position);
                //Debug.Log("Ray:" + ray.direction);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    //Debug.Log("rayCast:" + hit.collider.gameObject.name);

                    if (hit.collider != null && hit.collider.gameObject.tag == PopupTag)
                    {
                        hit.collider.gameObject.SetActive(false);
                        livePopupsCount--;
                    }
                }
            }
        }
    } 

    private void popupsRespawn()
    {

    }

}
