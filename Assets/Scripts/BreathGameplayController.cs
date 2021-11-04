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

    public GameObject ScoreTextObject;

    public float ScalingRate;
    public int RewardOnScore;

    [SerializeField]
    private bool buttonOnHold = false;
    private RectTransform breathRingTrans;
    private Text scoreText;
    [SerializeField]
    private bool outerRingAsGoal = false;

    private float timeCounter = 0.0f;

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
    }

    // Update is called once per frame
    void Update()
    {
        if ((breathRingTrans.sizeDelta.x < OuterRingSize && buttonOnHold) || (breathRingTrans.sizeDelta.x > (InnerRingSize - InnerRingRange) && !buttonOnHold)) breathRingScaling(buttonOnHold, Time.deltaTime);
        
        timeCounter += Time.deltaTime;
        if (timeCounter >= 1.0f)
        {
            score-=2;
            timeCounter = 0.0f;
            scoreDisplayUpdate();
        }
    }

    public void SetButtonOnHold(bool status)
    {
        buttonOnHold = status;

        if (!buttonOnHold && outerRingAsGoal && inGoalRange())
        {
            switchGoal();
            scoreIncrease();
        }
        else if (buttonOnHold && !outerRingAsGoal && inGoalRange())
        {
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
        score += RewardOnScore;
        scoreDisplayUpdate();
    }

    private void scoreDisplayUpdate()
    {
        scoreText.text = score.ToString();
    }

}
