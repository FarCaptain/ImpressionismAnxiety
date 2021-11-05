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

    // Start is called before the first frame update
    void Start()
    {
        slideRenderer = slide.GetComponent<Renderer>();
        goodPerioud = Random.Range(30, 200);

        // gameObject.GetComponent<>
    }

    // Update is called once per frame
    void Update()
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
        }
    }
}
