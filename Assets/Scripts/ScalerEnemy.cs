using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalerEnemy : BaseEnemy {


    public float scaleFactor = 0.1f;

    void Update() {
        Move();
    }


    protected override void OnCollisionEnter2D(Collision2D collision) {
        base.OnCollisionEnter2D(collision);

        float rScale = Random.Range(-1, 2);

        transform.localScale += new Vector3(scaleFactor * rScale,scaleFactor * rScale, 0);

    }


}
