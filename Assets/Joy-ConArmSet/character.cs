using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class character : MonoBehaviour {

    private static readonly Joycon.Button[] m_buttons =
        Enum.GetValues (typeof (Joycon.Button)) as Joycon.Button[];

    private List<Joycon> joycons; //JoyConクラスの格納

    // Values made available via Unity

    public float[] Rstick; //Rスティック
    public float[] Lstick; //Lスティック

    　　 //-----------------------------
    //RJoyConのセンサー数値
    public Vector3 Rgyro;
    public Vector3 Raccel;
    public Quaternion Rorientation;
    public Quaternion RtargetRotation;
    //-----------------------------

    //-----------------------------
    //LJoyConのセンサー数値
    public Vector3 Lgyro;
    public Vector3 Laccel;
    public Quaternion Lorientation;
    public Quaternion LtargetRotation;
    //-----------------------------

    //腕の振り回しアニメーション中はtrueに変更
    private bool now_swingAnim = false;
    private bool ready_punch = false;

    float speed = 10f;

    public Vector3 standard = new Vector3 (7.0f, 0.5f, 5.0f);

    public float standard_x = 7.0f;
    public float standard_y = 0.5f;
    public float standard_z = -2.0f;

    private Joycon joyconR;
    private Joycon joyconL;

    private Joycon.Button? m_pressedButtonL;
    private Joycon.Button? m_pressedButtonR;

    public GameObject shoulderR;
    public GameObject rightHand;
    public GameObject targetObject;
    public GameObject elbow;

    public GameObject checker;
    // public GameObject moveBox1;
    // public GameObject moveBox2;
    // public GameObject moveBox3;
    // public GameObject moveBox4;
    // public GameObject moveBox5;

    private Vector3 elbowPosition;
    hand grab;

    void Start ()

    {
        //-----------------------------
        //Joy-Conの初期セットアップ構成。
        //接続されているJoyConを探してjoyconRとjoyconLに格納する。
        Rgyro = new Vector3 (0, 0, 0);
        Raccel = new Vector3 (0, 0, 0);

        Lgyro = new Vector3 (0, 0, 0);
        Laccel = new Vector3 (0, 0, 0);

        // get the public Joycon array attached to the JoyconManager in scene

        joycons = JoyconManager.Instance.j;

        if (joycons == null || joycons.Count <= 0) return;

        joyconL = joycons.Find (c => c.isLeft);
        joyconR = joycons.Find (c => !c.isLeft);

        grab = targetObject.gameObject.GetComponent<hand> ();
        //-----------------------------

    }

    // Update is called once per frame

    void Update () {

        m_pressedButtonL = null;
        m_pressedButtonR = null;

        foreach (var button in m_buttons) {
            if (joyconL.GetButton (button)) {
                m_pressedButtonL = button;
            }
            if (joyconR.GetButton (button)) {
                m_pressedButtonR = button;
            }
        }

        // make sure the Joycon only gets checked if attached

        if (joycons.Count > 0) {

            //Joycon j = joycons[0];
            Joycon Lj = joyconL;
            Joycon Rj = joyconR;

            //--------------------------------------------------------------------------

            Rstick = joyconR.GetStick ();
            Rgyro = joyconR.GetGyro ();
            Raccel = joyconR.GetAccel ();

            Lstick = joyconL.GetStick ();
            Lgyro = joyconL.GetGyro ();
            Laccel = joyconL.GetAccel ();


            if (Rj.GetButtonDown (Joycon.Button.DPAD_UP)) {
                shoulderR.transform.DORotate(new Vector3(80, -63, -Rorientation.y), 0.3f).OnComplete(Swing_Anim_Finish);
                rightHand.transform.DORotate(new Vector3(80, -80, -Lorientation.y), 0.3f).OnComplete(Swing_Anim_Finish);
                now_swingAnim = true;
            }


            //--------------------------------------------------------------------------
            //各JoyConから角度を取得
            Rorientation = joyconR.GetVector ();
            Rorientation = new Quaternion (-Rorientation.x, -Rorientation.z, -Rorientation.y, -Rorientation.w);
            

            Lorientation = joyconL.GetVector ();
            Lorientation = new Quaternion (-Lorientation.x, -Lorientation.z, -Lorientation.y, -Lorientation.w);
            
            // if((Lorientation.x > 240 && Lorientation.x < 270) && (Lorientation.y > 170 && Lorientation.y < 250)){
            //     checker.SetActive(true);
            // }else{
            //     checker.SetActive(false);
            //     RtargetRotation = Quaternion.Inverse (Rorientation)
            //     rightHand.transform.rotation = RtargetRotation;

            //     Debug.Log("x " + Lorientation.x);
            //     Debug.Log("y " + Lorientation.y);
            // }
            //--------------------------------------------------------------------------
            //右肩オブジェクトに反映
            RtargetRotation = Quaternion.Inverse (Rorientation);
            Debug.Log("Rorientation.x = " + RtargetRotation.x.ToString());
            float converte = RtargetRotation.x * 360;
            Debug.Log("result.x = " + converte.ToString());
            int sep_num = 12;
            int result = 0;
            for(int i = 0; i < sep_num; i++){
                result += (360/sep_num);
                if(converte < result){
                    RtargetRotation.x = result;
                    break;
                }
            }

            rightHand.transform.rotation = RtargetRotation;
            //--------------------------------------------------------------------------
            //右手オブジェクトに反映
            // LtargetRotation = Quaternion.Inverse (Lorientation);

            // shoulderR.transform.rotation = LtargetRotation;
            //--------------------------------------------------------------------------
            
            float step = speed;

            elbowPosition = elbow.transform.position;
            rightHand.transform.position = elbowPosition;

            if (Rj.GetButtonDown (Joycon.Button.SHOULDER_2)) {
                Rj.Recenter ();
                Lj.Recenter ();
            }

            if (Rj.GetButton (Joycon.Button.SR)) {
                // moveBox1.transform.position = new Vector3 (35.5f, -15.5f, -20f);
                // moveBox2.transform.position = new Vector3 (15f, -15.5f, -20f);
                // moveBox3.transform.position = new Vector3 (-5f, -15.5f, -20f);
                // moveBox4.transform.position = new Vector3 (-25f, -15.5f, -20f);
                // moveBox5.transform.position = new Vector3 (-45f, -15.5f, -20f);
            }

            if (Rj.GetButton (Joycon.Button.SHOULDER_1)) {
                grab.grap ();
            }

            if (Rj.GetButtonUp (Joycon.Button.SHOULDER_1)) {
                grab.release (Raccel);
            }
        }
    }

    public void Swing_Anim_Finish(){
        now_swingAnim = false;
        Debug.Log("anim finished");
    }
}