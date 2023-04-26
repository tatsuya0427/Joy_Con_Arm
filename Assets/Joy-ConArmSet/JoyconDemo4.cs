using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
// using Object;
using UnityEngine;

public class JoyconDemo4 : MonoBehaviour {

    private static readonly Joycon.Button[] m_buttons =
        Enum.GetValues (typeof (Joycon.Button)) as Joycon.Button[];

    private List<Joycon> joycons;

    // Values made available via Unity

    public float[] stick;

    public Vector3 gyro;

    public Vector3 accel;

    public Quaternion orientation;

    public Quaternion targetRotation;

    float speed = 10f;

    public Vector3 standard = new Vector3 (7.0f, 0.5f, 5.0f);

    public float standard_x = 7.0f;
    public float standard_y = 0.5f;
    public float standard_z = -2.0f;

    private Joycon joyconR;
    private Joycon joyconL;

    private Joycon.Button? m_pressedButtonL;
    private Joycon.Button? m_pressedButtonR;

    public GameObject targetObject;
    hand grab;

    void Start ()

    {

        gyro = new Vector3 (0, 0, 0);

        accel = new Vector3 (0, 0, 1);

        // get the public Joycon array attached to the JoyconManager in scene

        joycons = JoyconManager.Instance.j;

        if (joycons == null || joycons.Count <= 0) return;

        joyconL = joycons.Find (c => c.isLeft);
        joyconR = joycons.Find (c => !c.isLeft);

        //grab = targetObject.GetComponent<moveBox> ();
        grab = targetObject.gameObject.GetComponent<hand> ();

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

            stick = joyconL.GetStick ();

            // Gyro values: x, y, z axis values (in radians per second)

            gyro = joyconL.GetGyro ();

            // Accel values:  x, y, z axis values (in Gs)

            accel = joyconL.GetAccel ();

            orientation = joyconL.GetVector ();
            orientation = new Quaternion (-orientation.x, -orientation.z, -orientation.y, -orientation.w);
            targetRotation = Quaternion.Inverse (orientation);

            gameObject.transform.rotation = targetRotation;

            //--------------------------------------------------------------------------

            float step = speed;

            float step_x = accel.x; // * Time.deltaTime;
            float step_y = accel.y;
            float step_z = accel.z;

            // gyro.x = 0.0f;
            // gyro.z = 0.0f;
            // accel.x = 0.0f;
            //accel.z = 0.0f;

            float gyro_y = gyro.y;

            if (j.GetButton (Joycon.Button.SHOULDER_2)) {
                gyro = new Vector3 (0, 0, 0);

                accel = new Vector3 (0, 0, 1);

                standard = new Vector3 (7.0f, 0.5f, -2.0f);

                orientation = new Quaternion (0, 0, 0, 0);
            }

            if (j.GetButton (Joycon.Button.SHOULDER_1)) {
                grab.grap ();
            }

            if (j.GetButtonUp (Joycon.Button.SHOULDER_1)) {
                //grab.release ();
            }
        }
    }

}