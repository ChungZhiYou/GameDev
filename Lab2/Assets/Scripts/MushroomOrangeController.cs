using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomOrangeController : MonoBehaviour
{
    private Rigidbody2D mushroomRigidBody;
    public float speed = 200;
    // Start is called before the first frame update
    void Start()
    {
        mushroomRigidBody = GetComponent<Rigidbody2D>();
        mushroomRigidBody.AddForce(new Vector3(1,1) * 10 , ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Time.deltaTime);
        // Debug.Log(mushroomRigidBody.velocity.magnitude);
        if (mushroomRigidBody.velocity.magnitude < 1.5){
            // Debug.Log(transform.right * speed);
            mushroomRigidBody.AddForce(new Vector3(1, 0) * speed * Time.deltaTime, ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        
        Debug.Log("Collided with something!");
        if (col.gameObject.CompareTag("Wall")) {
            Debug.Log("Collided with wall!");
            speed *= -1;
        }

        if (col.gameObject.CompareTag("Player")) {
            Debug.Log("Collided with Player!");
            speed = 0;
        }
    }

    void  OnBecameInvisible(){
        Debug.Log("Disappeared!?!!?");
        Destroy(gameObject);	
    }
    
}
