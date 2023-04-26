using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JoyconDemo2 : MonoBehaviour {

    private static readonly Joycon.Button[] m_buttons =
        Enum.GetValues (typeof (Joycon.Button)) as Joycon.Button[];

    private List<Joycon> joycons;

    // Values made available via Unity

    public float[] stick;

    public Vector3 gyro;

    public Vector3 accel;

    public Quaternion orientation;

    float speed = 10f;

    public Vector3 standard = new Vector3 (7.0f, 0.5f, -15.0f);

    public float standard_x = 7.0f;
    public float standard_y = 0.5f;
    public float standard_z = -2.0f;

    private Joycon joyconR;
    private Joycon joyconL;

    private Joycon.Button? m_pressedButtonL;
    private Joycon.Button? m_pressedButtonR;

    void Start ()

    {

        gyro = new Vector3 (0, 0, 0);

        accel = new Vector3 (0, 0, 1);

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

            stick = joyconL.GetStick ();

            // Gyro values: x, y, z axis values (in radians per second)

            gyro = joyconL.GetGyro ();

            // Accel values:  x, y, z axis values (in Gs)

            accel = joyconL.GetAccel ();

            orientation = joyconL.GetVector ();

            //gameObject.transform.rotation = orientation;

            //public Vector3 standard = new Vector3 (7.0f, 0.5f, 5.0f);
            float step = speed; //(speed = 10f)
            gyro.x = 0.0f;
            //gyro.z = 0.0f;

            //指定のボタンを押した際に座標のリセットを行う
            if (j.GetButton (Joycon.Button.SHOULDER_2)) {
                gyro = new Vector3 (0, 0, 0);
                accel = new Vector3 (0, 0, 0);
                orientation = new Quaternion (0, 0, 0, 0);
                standard = new Vector3 (7.0f, 0.5f, -20.0f);

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

            // if (gyro.y < 0.015 && gyro.y > -0.015) {
            //     gyro.y = 0;
            // }

            // if (gyro.z < 0.015 && gyro.z > -0.015) {
            //     gyro.z = 0;
            // }

            // standard += gyro;
            //Math.Truncate (standard.y);

            //standard.y += 0.01f;

            //指定の座標に移動する移動方式であるtransform.positionを採用
            //transform.position = Vector3.MoveTowards (transform.position, standard, step);

            // if (transform.position.y > 25.0f && GameObject.Find ("CubeR").transform.position.y > 25.0f) {
            //     Debug.Log ("JUMP!!");
            // }
        }

    }

}