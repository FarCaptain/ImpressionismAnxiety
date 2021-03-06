using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVScrolling : MonoBehaviour
{
    public float ScrollX = 0.01f;
    public float ScrollY = 0.0f;
    private float curAnxiety = 100.0f;

    void Update () {
        float OffsetX = Time.time * ScrollX;
        float OffsetY = Time.time * ScrollY;
        
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(OffsetX, OffsetY);
        gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("_Anxiety", curAnxiety);
        curAnxiety = Mathf.Clamp(BreathGameplayController.score, 0, 100);
        // if( curAnxiety > 0 ) curAnxiety -= (Time.time/100.0f) ;
    }
}
