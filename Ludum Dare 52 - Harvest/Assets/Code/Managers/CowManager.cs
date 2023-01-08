using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowManager : MonoBehaviour
{
    public AssetLibrary assetLibrary;
    public ParticleSystem cowParticle;

    // Start is called before the first frame update
    void Start()
    {
        if(assetLibrary == null)
        {
            assetLibrary = GameObject.FindGameObjectWithTag("GameController").GetComponent<AssetLibrary>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            PlayerManager playerManager = col.gameObject.GetComponent<PlayerManager>();

            // Hide the cow
            this.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0f);

            StartCoroutine(playerManager.CowCollision(this.gameObject));

            if(playerManager.cornValue > 2f)
            {
                playerManager.cornValue -= 2f;
            }
            else
            {
                playerManager.cornValue = 0f;
            }

            // Trigger particle
            cowParticle.Play();

            // Start the Cow Zone coroutine to start spawning a new cow
            // SpawnCowCoroutine;
        }
    }
}
