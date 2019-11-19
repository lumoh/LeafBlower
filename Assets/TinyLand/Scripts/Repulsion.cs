using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repulsion : MonoBehaviour {

    public float force = 300f;
  
    void Start () {
		
	}	
        void OnCollisionEnter(Collision c)
        {
            
            float force = 300;
          
            if (c.gameObject.tag == "Player")
            {
               
                Vector3 dir = c.contacts[0].point - transform.position;
             
                dir = -dir.normalized;
             
                GetComponent<Rigidbody>().AddForce(dir * force);
            }
        }
    }
