using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private CharacterController cc;
    public float moveSpeed;
    
    private float horizontalMove, verticalMove;
    private Vector3 dir;
    
    private void Start() {
        cc = GetComponent<CharacterController> ();
    }

    private void Update() {
        horizontalMove = Input.GetAxis("Horizontal") * moveSpeed;
        verticalMove =  Input.GetAxis("Vertical") * moveSpeed;

        dir = transform.forward * verticalMove + transform.right * horizontalMove;
        cc.Move(dir * Time.deltaTime);
    }
}
