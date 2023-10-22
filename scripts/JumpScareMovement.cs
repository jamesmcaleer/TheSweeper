using UnityEngine;
using System.Collections;

public class JumScareMovement : MonoBehaviour {
	
	
	private Animator anim;
	private CharacterController controller;
	private int battle_state = 0;
	public float speed = 6.0f;
	public float runSpeed = 3.0f;
	public float turnSpeed = 60.0f;
	public float gravity = 20.0f;
	private Vector3 moveDirection = Vector3.zero;
	private float w_sp = 0.0f;
	private float r_sp = 0.0f;
	//private GameObject doorObject;
	
	// Use this for initialization
	void Start () 
	{						
		anim = GetComponent<Animator>();
		controller = GetComponent<CharacterController> ();
		w_sp = speed; //read walk speed
		r_sp = runSpeed; //read run speed
		battle_state = 0;
		runSpeed = 1;
        
        anim.SetInteger ("battle", 1);
        battle_state = 1;
        runSpeed = r_sp;
        anim.SetInteger ("moving", 7);
	

		
	}
	
	// Update is called once per frame
	void Update () 
	{		
		
        
    }
}



