using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangeEnemy : ScalerEnemy
{
    public Gradient colors;
    SpriteRenderer sp;

    new void Start() {
        base.Start();
        sp = GetComponent<SpriteRenderer>();
    }

    void Update() {
        Move();
    }


    protected override void OnCollisionEnter2D(Collision2D collision) {
        base.OnCollisionEnter2D(collision);
        sp.color = colors.Evaluate(Random.Range(0f,1f));


    }


}
