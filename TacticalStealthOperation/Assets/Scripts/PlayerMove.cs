using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    private static readonly int isWalking = Animator.StringToHash("isWalking");
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private float speed = 1;
    private float vertical;
    private float horizontal;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void FixedUpdate() {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        characterAnimator.SetBool(isWalking, vertical != 0 || horizontal != 0);
        Vector3 addPosition = new Vector3(horizontal, 0, vertical).normalized*Time.fixedDeltaTime*speed;
        transform.LookAt(transform.position + addPosition);
        transform.position += addPosition;
    }
}
