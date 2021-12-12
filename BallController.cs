using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    Vector2 direction;
    public float maxPower = 1000f;
    public float powerUpSpeed = 1000f;
    public int nbCoupsMax=5;
    public float volume = 100f;
    public GameObject arrow;
    public AudioClip bounce;
    float timerGameOver=1f;
    float currentShot;
    float linearDragTimer = 0.01f;
    public float linearDragUpSpeed = 10f;
    float maxMagnitude;
    float power;
    Rigidbody2D rb;
    Gradient g = new Gradient();
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.nbCoupsMax = nbCoupsMax;
        GameManager.instance.updateRemainingShots(nbCoupsMax);
        GradientColorKey[] gck = new GradientColorKey[3];
        GradientAlphaKey[] gak = new GradientAlphaKey[3];
        gck[0].color = Color.red;
        gck[0].time = 1f;
        gck[1].color = Color.yellow;
        gck[1].time = 0.5f;
        gck[2].color = Color.green;
        gck[2].time = 0f;
        gak[0].alpha = 1.0F;
        gak[0].time = 1.0F;
        gak[1].alpha = 1.0F;
        gak[1].time = 0.5F;
        gak[2].alpha = 1.0F;
        gak[2].time = 0F;
        g.SetKeys(gck, gak);
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(nbCoupsMax>0)
        {
            if(Input.GetMouseButton(0) && rb.velocity == Vector2.zero)
            {
                rb.drag = 0.001f;
                direction= (Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position).normalized;
                if(power<maxPower)
                    power += powerUpSpeed * Time.deltaTime;
                arrow.GetComponent<SpriteRenderer>().color = g.Evaluate(power / maxPower);
            }
            if(Input.GetMouseButtonUp(0) && rb.velocity == Vector2.zero)
            {
                rb.AddForce(direction * power);
                currentShot = Time.time;
                power = 0;
                arrow.GetComponent<SpriteRenderer>().color = g.Evaluate(power / maxPower);
                nbCoupsMax--;
                if (nbCoupsMax <= 0)
                    timerGameOver = Time.time;
                GameManager.instance.updateRemainingShots(nbCoupsMax);
            }
        }
        if(rb.velocity!=Vector2.zero )
        {
            if (maxMagnitude < rb.velocity.magnitude)
            {
                maxMagnitude = rb.velocity.magnitude;
            }
            if (rb.velocity.magnitude < 0.01 * maxMagnitude)
                rb.velocity = Vector2.zero;
            rb.drag += Mathf.Log((1+rb.drag)*linearDragUpSpeed,2.78f)*Time.deltaTime;
            arrow.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (rb.velocity == Vector2.zero)
        {
            arrow.GetComponent<SpriteRenderer>().enabled = true;
        }
        if(rb.velocity==Vector2.zero && nbCoupsMax<=0 && Time.time-timerGameOver>1f)
        {
            StartCoroutine(gameOver());

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AudioSource.PlayClipAtPoint(bounce, transform.position, volume);
        if (collision.gameObject.tag=="destructible")
        {
            collision.gameObject.GetComponent<blockController>().loseHp();
            Camera.main.GetComponent<screenShake>().shakeAmount = rb.velocity.magnitude*0.01f;
            Camera.main.GetComponent<screenShake>().shakeDuration = 1f;
        }
    }
    IEnumerator gameOver()
    {
        GameManager.instance.image.SetActive(true);
        GameManager.instance.image.GetComponent<Animator>().SetTrigger("Start");

        yield return new WaitForSeconds(1f);
        GameManager.instance.gameOver();
    }
}
