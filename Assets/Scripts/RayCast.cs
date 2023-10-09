using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour
{
 
    void Start()
    {
        Physics2D.queriesStartInColliders = false;
    }


    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position,transform.right);
        if(hit.collider != null)
        {
            //encostamos em algo o tiro pego
            Debug.DrawLine(transform.position,hit.point,Color.green);
            Debug.Log(hit.transform.name);

        }
        else
        {
            //Não acertei nada 
            Debug.DrawRay(transform.position,transform.right,Color.red);

        }
        //desenha uma linha imaginaria na cena 
        // Debug.DrawLine();

        //3d
        RaycastHit hit3d;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray,out hit3d))
        {
            Debug.Log(hit3d.transform.name);
        }

        //2d 
        hit = Physics2D.GetRayIntersection(ray);
        if(hit)
        {
            Debug.Log(hit.transform.name);
        }

    }
}
