using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nextLevel : MonoBehaviour
{
    public AudioClip clip;
    public float volume;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.active = false;
        AudioSource.PlayClipAtPoint(clip, transform.position, volume);
        GameManager.instance.LoadNextScene();
    }
}
