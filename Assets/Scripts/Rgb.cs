using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rgb : ScalerEnemy
{

    public Gradient gradient;
    SpriteRenderer sp;

    public override void Start(){
        base.Start();
        sp = GetComponent<SpriteRenderer>();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        sp.color = gradient.Evaluate(Random.Range(0f,1f));
    
    }
}
