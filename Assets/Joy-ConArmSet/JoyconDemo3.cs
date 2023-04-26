using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JoyconDemo3 : MonoBehaviour {

    private static readonly Joycon.Button[] m_buttons =
        Enum.GetValues (typeof (Joycon.Button)) as Joycon.Button[];

    private List<Joycon> joycons;

    // Values made available via Unity

    public float[] Lstick;
    public float[] Rstick;

    public Vector3 Lgyro;
    public Vector3 Rgyro;

    public Vector3 Laccel;
    public Vector3 Raccel;

    public Quaternion Lorientation;
    public Quaternion Rorientation;

    float speed = 15f;

    public Vector3 Lstandard = new Vector3 (7.0f, 0.5f, -20.0f);
    public Vector3 Rstandard = new Vector3 (7.0f, 0.5f, 5.0f);

    public Vector3 Lstack = new Vector3 (0f, 0f, 0f);
    public Vector3 Rstack = new Vector3 (0f, 0f, 0f);

    public Vector3 LstopGyro = new Vector3 (0f, 0f, 0f); //Joyconの動きを止めた座標を記録しておく
    public Vector3 LbeforeAccel = new Vector3 (0f, 0f, 0f);
    public Vector3 LbeforeGyro = new Vector3 (0f, 0f, 0f);
    public bool Lmove = false;

    public bool hurikiri = false;

    public bool moveUp = false; // 上方向にJoyconが移動している場合にtrue
    public bool movedown = false; // 下方向にJoyconが移動している場合にtrue

    // public float standard_x = 7.0f;
    // public float standard_y = 0.5f;
    // public float standard_z = -2.0f;

    private Joycon joyconR;
    private Joycon joyconL;

    private Joycon.Button? m_pressedButtonL;
    private Joycon.Button? m_pressedButtonR;

    void Start ()

    {
        Lgyro = new Vector3 (0, 0, 0);

        Laccel = new Vector3 (0, 0, 1);

        // get the public Joycon array attached to the JoyconManager in scene

        joycons = JoyconManager.Instance.j;

        if (joycons == null || joycons.Count <= 0) return;

        joyconL = joycons.Find (c => c.isLeft);
        joyconR = joycons.Find (c => !c.isLeft);

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

        if (joycons.Count > 0)

        {

            Joycon j = joycons[0];

            // GetButtonDown checks if a button has been pressed (not held)

            if (j.GetButtonDown (Joycon.Button.SHOULDER_2))

            {

                Debug.Log ("Shoulder button 2 pressed");

                // GetStick returns a 2-element vector with x/y joystick components

                Debug.Log (string.Format ("Stick x: {0:N} Stick y: {1:N}", j.GetStick () [0], j.GetStick () [1]));

                // Joycon has no magnetometer, so it cannot accurately determine its yaw value. Joycon.Recenter allows the user to reset the yaw value.

                j.Recenter ();

            }

            // GetButtonDown checks if a button has been released

            if (j.GetButtonUp (Joycon.Button.SHOULDER_2))

            {

                Debug.Log ("Shoulder button 2 released");

            }

            // GetButtonDown checks if a button is currently down (pressed or held)

            if (j.GetButton (Joycon.Button.SHOULDER_2))

            {

                Debug.Log ("Shoulder button 2 held");

            }

            if (j.GetButtonDown (Joycon.Button.DPAD_DOWN)) {

                Debug.Log ("Rumble");

                // Rumble for 200 milliseconds, with low frequency rumble at 160 Hz and high frequency rumble at 320 Hz. For more information check:

                // https://github.com/dekuNukem/Nintendo_Switch_Reverse_Engineering/blob/master/rumble_data_table.md

                j.SetRumble (160, 320, 0.6f, 200);

                // The last argument (time) in SetRumble is optional. Call it with three arguments to turn it on without telling it when to turn off.

                // (Useful for dynamically changing rumble values.)

                // Then call SetRumble(0,0,0) when you want to turn it off.

            }

            Lstick = joyconL.GetStick ();
            Lgyro = joyconL.GetGyro ();
            Laccel = joyconL.GetAccel ();
            Lorientation = joyconL.GetVector ();

            Rstick = joyconR.GetStick ();
            Rgyro = joyconR.GetGyro ();
            Raccel = joyconR.GetAccel ();
            Rorientation = joyconR.GetVector ();

            //public Vector3 standard = new Vector3 (7.0f, 0.5f, 5.0f);
            float step = speed; //(speed = 10f)
            //Lgyro.x = 0.0f;
            //Lgyro.z = 0.0f;

            //加速度センサーが三次元的に計測しているので、よくわからなくなるが、振る動作はX軸回転なのでそれをY軸の移動に無理やり変換している。
            Laccel.y = Laccel.x;
            Laccel.x = 0.0f;
            //Laccel.z = 0.0f;

            Rgyro.x = 0.0f;
            Rgyro.z = 0.0f;

            //指定のボタンを押した際に座標のリセットを行う
            if (j.GetButton (Joycon.Button.SHOULDER_2)) {
                Lgyro = new Vector3 (0, 0, 0);
                Laccel = new Vector3 (0, 0, 0);
                Lorientation = new Quaternion (0, 0, 0, 0);
                Lstandard = new Vector3 (7.0f, 7.0f, -20.0f);

                Rgyro = new Vector3 (0, 0, 0);
                Raccel = new Vector3 (0, 0, 0);
                Rorientation = new Quaternion (0, 0, 0, 0);
                Rstandard = new Vector3 (7.0f, 7.0f, 5.0f);

            }

            //範囲外に行くようなあたいになった場合に加算する値を0にする
            // if ((standard + gyro).y > 100 || (standard + gyro).y < -100) {
            //     gyro.y = 0.0f;
            // }

            // if ((standard + gyro).z > 100 || (standard + gyro).z < -100) {
            //     gyro.z = 0.0f;
            // }

            //現在の位置から、移動した座標分だけ加算し、移動先の座標を求める
            //この誤差を減らすためのアイデア
            //・傾きが一定数以上の時は加算しない　→　範囲の調節が必要
            //ジャイロの移動先最大値(最初位置)を加速度が同方向に働いている時に保存しておき、
            //加速度が変わらないようならば最大値まで加算

            //商品の誤差なのかどうかわからないが、静止状態でわずかな入力があり、それが誤差を生んでるので切り捨てる(Math.Truncate)

            // if (standard.y < 5.0f && standard.y > -5.0f) {
            //     standard.y = 0.0f;
            // }

            //gameObject.transform.rotation = orientation;

            //静止状態で、gyro.y = 0.013ぐらいの値を検出していたので、それ以下の値は切り捨てる
            //Debug.Log ("gyro.y = " + (Math.Truncate ((gyro.y * 10.0f)) / 10.0f));

            //傾きはQuaternion(四次元数)を用いているらしい。
            //傾けすぎによる誤差防止は、値のリセットをした際に、初期状態のJoyconの傾きを保存しておいて、そこから動作の範囲外になるような傾きを検知する数式を構築する。
            //Debug.Log ("傾き = " + orientation.y);

            if (Lgyro.y < 0.015 && Lgyro.y > -0.015) {
                Lgyro.y = 0;
            }

            if (Lgyro.z < 0.015 && Lgyro.z > -0.015) {
                Lgyro.z = 0;
            }

            // if(Laccel.y  -0.1)

            // if (Rgyro.y < 0.015 && Rgyro.y > -0.015) {
            //     Rgyro.y = 0;
            // }

            // Laccel.y = Laccel.y * 100;
            // Laccel.y = (float) Math.Truncate (Laccel.y) / 100;

            // if (Rgyro.z < 0.015 && Rgyro.z > -0.015) {
            //     Rgyro.z = 0;
            // }

            // if (Laccel != LbeforeAccel) {
            //     //Debug.Log ("true");
            //     LstopGyro = Lgyro;
            //     //Lmove = true;
            // } else {
            //     //Debug.Log ("false");
            //     //Lmove = false;
            // }

            // if ((Laccel.y - LbeforeAccel.y) > -0.01 && (Laccel.y - LbeforeAccel.y) < 0.01) { //Joyconの動きが止まっている範囲
            //     hurikiri = false;
            // } else if ((Laccel.y - LbeforeAccel.y) < -0.5 || (Laccel.y - LbeforeAccel.y) > 0.5) {
            //     hurikiri = true;
            // } else {
            //     if (!hurikiri) {
            //         Lstandard += Lgyro;
            //     }
            // }

            // if (((Lgyro.y - LbeforeGyro.y) < -10.0f) || ((Lgyro.y - LbeforeGyro.y) > 10.0f)) {
            //     Debug.Log ("true");
            // } else {
            //     Lstandard += Lgyro;
            // }

            // if ((Laccel.y - LbeforeAccel.y) < -0.1 || (Laccel.y - LbeforeAccel.y) > 0.1) {
            //     if ((Laccel.y - LbeforeAccel.y) > 0) { // 加速度の差が正の場合、上方向に移動している。
            //         if ((Laccel.y - LbeforeAccel.y) > ((LbeforeAccel.y * 0.23) * -1)) {
            //             //3.11 > -1.8837 * -1 = true
            //             //反転、又は停止したとみなして座標の更新は行わない。
            //         } else {
            //             Lstandard += Lgyro;
            //         }
            //     } else if ((Laccel.y - LbeforeAccel.y) < 0) { // 加速度の差が負の場合、下方向に移動している。
            //         // moveUp = false;
            //         // movedown = true;
            //         Lstandard += Lgyro;
            //     }
            // } else {
            //     // moveUp = false;
            //     // movedown = false;
            // }

            //Lstandard += Laccel;
            Lstandard += Lgyro;

            Rstandard += Rgyro;
            //Math.Truncate (standard.y);

            //standard.y += 0.01f;

            //指定の座標に移動する移動方式であるtransform.positionを採用
            //GameObject.Find ("CubeL").transform.position = Vector3.MoveTowards (GameObject.Find ("CubeL").transform.position, Lstandard, step);

            //GameObject.Find ("CubeL").transform.position = Lstandard;

            //Debug.Log ("Before:" + LbeforeAccel.y);

            //Debug.Log ("now" + Laccel.y + "sub:" + (Laccel.y - LbeforeAccel.y));
            //Debug.Log ("gyro" + Lstandard.y + " sub" + (Lstandard.y - LbeforeGyro.y));
            LbeforeAccel = Laccel;
            LbeforeGyro = Lstandard;

            //GameObject.Find ("CubeR").transform.position = Vector3.MoveTowards (GameObject.Find ("CubeR").transform.position, Rstandard, step);

            if (GameObject.Find ("CubeL").transform.position.y > 25.0f && GameObject.Find ("CubeR").transform.position.y > 25.0f) {
                if (Lgyro.y > 1 && Rgyro.y > 1) {
                    GameObject.Find ("Cube3").GetComponent<move> ().chechJump = true;
                }
            }

            if (GameObject.Find ("CubeL").transform.position.y > 25.0f || GameObject.Find ("CubeR").transform.position.y > 25.0f) {
                //transform.position = Vector3.MoveTowards (transform.position, standard, step);
                //GameObject.Find ("Cube3").GetComponent<move> ().chechJump = true;
            }
        }
    }
}