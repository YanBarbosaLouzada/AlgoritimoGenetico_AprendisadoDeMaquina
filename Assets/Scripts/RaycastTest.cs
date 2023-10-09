using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTest : MonoBehaviour {

    
    void Start() {
        Physics2D.queriesStartInColliders = false;
    }

    void Update() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position,transform.right);
        if (hit.collider != null)
            Debug.DrawLine(transform.position, hit.point,Color.red);
        else
            Debug.DrawRay(transform.position, transform.right,Color.green);


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit mouseHit;
        if (Physics.Raycast(ray,out mouseHit)) {
            Debug.Log(mouseHit.collider.name);

        }

        hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
        if (hit)
            Debug.Log(hit.collider);
    }
}
