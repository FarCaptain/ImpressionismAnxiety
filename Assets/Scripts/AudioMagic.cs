using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMagic : MonoBehaviour
{

    public AudioSource [] heartbeats;

    public static AudioMagic instance;
    private int currentPhase;

    private void Awake()
    {
        if ( instance == null )
            instance = this;
    }

    private void Start() {
        currentPhase = 0;
        heartbeats[currentPhase].Play();
    }

    private void Update() {
        int score = BreathGameplayController.score;
        if(score < 30)
        {
            if( currentPhase != 2)
            {
                SwapTrack(currentPhase, 2);
                currentPhase = 2;
            }
        }
        else if(score < 60)
        {
            if( currentPhase != 1)
            {
                SwapTrack(currentPhase, 1);
                currentPhase = 1;
            }
        }
        else
        {
            if( currentPhase != 0)
            {
                SwapTrack(currentPhase, 0);
                currentPhase = 0;
            }
        }
    }

    public void SwapTrack( int phase1, int phase2 )
    {
        StopAllCoroutines();
        StartCoroutine(FadeTrack(phase1, phase2));
    }

    public IEnumerator FadeTrack( int phase1, int phase2 )
    {
        float timeToFade = 2.00f;
        float timeElapsed = 0;
    
        heartbeats[phase2].Play();
        while(timeElapsed < timeToFade)
        {
            heartbeats[phase2].volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
            heartbeats[phase1].volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        heartbeats[phase1].Stop();
    }
}
