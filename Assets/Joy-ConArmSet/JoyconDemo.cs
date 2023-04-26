using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JoyconDemo : MonoBehaviour {

    private static readonly Joycon.Button[] m_buttons =
        Enum.GetValues (typeof (Joycon.Button)) as Joycon.Button[];

    private List<Joycon> joycons;

    // Values made available via Unity

    public float[] stick;

    public Vector3 gyro;

    public Vector3 accel;

    public Quaternion orientation;

    float speed = 10f;

    public Vector3 standard = new Vector3 (7.0f, 0.5f, 5.0f);

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

                //Debug.Log ("Shoulder button 2 pressed");

                // GetStick returns a 2-element vector with x/y joystick components

                //Debug.Log (string.Format ("Stick x: {0:N} Stick y: {1:N}", j.GetStick () [0], j.GetStick () [1]));

                // Joycon has no magnetometer, so it cannot accurately determine its yaw value. Joycon.Recenter allows the user to reset the yaw value.

                j.Recenter ();

            }

            // GetButtonDown checks if a button has been released

            if (j.GetButtonUp (Joycon.Button.SHOULDER_2))

            {

                //Debug.Log ("Shoulder button 2 released");

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

            stick = joyconR.GetStick ();

            // Gyro values: x, y, z axis values (in radians per second)

            gyro = joyconR.GetGyro ();

            // Accel values:  x, y, z axis values (in Gs)

            accel = joyconR.GetAccel ();

            orientation = joyconR.GetVector ();

            //gameObject.transform.rotation = orientation;

            float step = speed;

            float step_x = accel.x; // * Time.deltaTime;
            float step_y = accel.y;
            float step_z = accel.z;

            gyro.x = 0.0f;
            gyro.z = 0.0f;
            accel.x = 0.0f;
            //accel.z = 0.0f;

            float gyro_y = gyro.y;

            //transform.position += transform.forward * (standard_y + gyro_y) * Time.deltaTime;

            //transform.position = Vector3.MoveTowards (transform.position, standard + gyro, step);
            //transform.position (standard.x + gyro.x, standard.y + gyro.y, standard.z + gyro.z);
            if (j.GetButton (Joycon.Button.SHOULDER_2)) {
                //if (joyconR.GetButtonDown (m_buttons[1])) {
                gyro = new Vector3 (0, 0, 0);

                accel = new Vector3 (0, 0, 1);

                standard = new Vector3 (7.0f, 0.5f, -2.0f);

                orientation = new Quaternion (0, 0, 0, 0);
            }

            // if ((standard + gyro).y > 100 || (standard + gyro).y < -100) {
            //     gyro.y = 0.0f;
            // }

            // if ((standard + gyro).z > 100 || (standard + gyro).z < -100) {
            //     gyro.z = 0.0f;
            // }

            // if (gyro.y < 0.015 && gyro.y > -0.015) {
            //     gyro.y = 0;
            // }

            // // if (accel.z >= -1.0f) {
            // //     gyro.z = 0.0f;
            // // }

            // //if (gyro > 0.2f || gyro < 0.2f) {
            // standard += gyro;
            //}

            //transform.position = Vector3.MoveTowards (transform.position, standard, step);

            // if (accel.y > 0.2f) {
            //     transform.position = Vector3.MoveTowards (transform.position, standard + gyro, step);
            //     //Debug.Log ("True");
            // } else if (accel.y < -0.2f) {
            //     transform.position = Vector3.MoveTowards (transform.position, standard + gyro, step);
            //     //Debug.Log ("True");
            // }

            // if (transform.position.y < 10.0f || transform.position.y > -10.0f) {
            //     transform.position = Vector3.MoveTowards (transform.position, transform.position + gyro, step);
            // }
        }

    }

}