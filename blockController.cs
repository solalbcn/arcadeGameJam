using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockController : MonoBehaviour
{
    public int hp=1;
    public Color[] colors;
    public int points = 10;
    // Start is called before the first frame update

    public void loseHp()
    {
        hp--;
        GameManager.instance.addScore(points);
        if (hp <= 0)
            Destroy(gameObject);
        if(hp!=0)
            this.GetComponent<SpriteRenderer>().color=colors[hp - 1];
    }
}
