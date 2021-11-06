using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlitchingSlides : MonoBehaviour
{
    [SerializeField]
    private GameObject slide;
    
    [SerializeField]
    private Texture[] textures;
    
    private Renderer slideRenderer;
    private int randomTextureIndex;
    private int goodPerioud;
    private float timeCounter = 0.0f;

    // Start is called before the first frame update
    public void Start()
    {
        slideRenderer = slide.GetComponent<Renderer>();
        goodPerioud = Random.Range(100, 300);

        // gameObject.GetComponent<>
    }

    // Update is called once per frame
    public void Update()
    {
        // randomTextureIndex = Random.Range(0, textures.Length);
        // slideRenderer.material.mainTexture = textures[randomTextureIndex];

        if( goodPerioud > 0 ) goodPerioud --;
        else
        {
            // goodPerioud = Random.Range(3000, 2000000);
            // bad! glitch!
            randomTextureIndex = Random.Range(0, textures.Length);
            slideRenderer.material.mainTexture = textures[randomTextureIndex];
            slideRenderer.material.SetTexture ("_EmissionMap", textures[randomTextureIndex]);

            // if not resolved, extra decrease
            timeCounter += Time.deltaTime;
            if (timeCounter >= 1.0f)
            {
                BreathGameplayController.score = Mathf.Clamp(BreathGameplayController.score - 2, 0, 100);
                timeCounter = 0.0f;
            }
            


            // Place holder
            if(Input.GetKeyDown(KeyCode.Space))
            {
                goodPerioud = Random.Range(200, 1000);
                slideRenderer.material.mainTexture = textures[0];
                slideRenderer.material.SetTexture ("_EmissionMap", textures[0]);
            }
        }
    }
}
