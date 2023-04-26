using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour {
    private CharacterController controller;
    private Vector3 moveDirection;

    public bool chechJump = false;

    // Start is called before the first frame update
    void Start () {
        controller = GetComponent<CharacterController> ();
        moveDirection = Vector3.zero;
    }

    // Update is called once per frame
    void Update () {
        if (controller.isGrounded) {
            if (chechJump) {
                moveDirection.y = 20; //ジャンプするベクトルの代入
            }
        } else {
            chechJump = false;
        }
        moveDirection.y -= 10 * Time.deltaTime; //重力計算
        controller.Move (moveDirection * Time.deltaTime);
    }
}