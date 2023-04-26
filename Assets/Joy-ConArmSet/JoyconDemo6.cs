using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JoyconDemo6 : MonoBehaviour {

    private static readonly Joycon.Button[] m_buttons =
        Enum.GetValues (typeof (Joycon.Button)) as Joycon.Button[];

    private List<Joycon> joycons;

    // Values made available via Unity

    public float[] Rstick;
    public float[] Lstick;

    public Vector3 Rgyro, Lgyro;

    public Vector3 Raccel, Laccel;

    public Quaternion Rorientation, Lorientation;

    public Quaternion RtargetRotation, LtargetRotation;

    float speed = 10f;

    public Vector3 standard = new Vector3 (7.0f, 0.5f, 5.0f);

    public float standard_x = 7.0f;
    public float standard_y = 0.5f;
    public float standard_z = -2.0f;

    private Joycon joyconR;
    private Joycon joyconL;

    private Joycon.Button? m_pressedButtonL;
    private Joycon.Button? m_pressedButtonR;

    GameObject rightHand;
    GameObject leftHand;

    void Start ()

    {

        Rgyro = new Vector3 (0, 0, 0);
        Lgyro = new Vector3 (0, 0, 0);

        Raccel = new Vector3 (0, 0, 0);
        Laccel = new Vector3 (0, 0, 0);

        // get the public Joycon array attached to the JoyconManager in scene

        joycons = JoyconManager.Instance.j;

        if (joycons == null || joycons.Count <= 0) return;

        joyconL = joycons.Find (c => c.isLeft);
        joyconR = joycons.Find (c => !c.isLeft);

        rightHand = GameObject.Find ("elbowR");
        leftHand = GameObject.Find ("elbowL");

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
            // Joycon jr = joyconR;

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

            Rstick = joyconR.GetStick ();

            // Gyro values: x, y, z axis values (in radians per second)

            Rgyro = joyconR.GetGyro ();

            // Accel values:  x, y, z axis values (in Gs)

            Raccel = joyconR.GetAccel ();

            Lstick = joyconL.GetStick ();
            Lgyro = joyconL.GetGyro ();
            Laccel = joyconL.GetAccel ();

            //--------------------------------------------------------------------------

            Rorientation = joyconR.GetVector ();
            Rorientation = new Quaternion (-Rorientation.x, -Rorientation.z, -Rorientation.y, -Rorientation.w);
            RtargetRotation = Quaternion.Inverse (Rorientation);

            rightHand.transform.rotation = RtargetRotation;

            //--------------------------------------------------------------------------

            //--------------------------------------------------------------------------

            Lorientation = joyconL.GetVector ();
            Lorientation = new Quaternion (-Lorientation.x, -Lorientation.z, -Lorientation.y, -Lorientation.w);
            LtargetRotation = Quaternion.Inverse (Lorientation);

            leftHand.transform.rotation = LtargetRotation;

            //--------------------------------------------------------------------------

            if (j.GetButton (Joycon.Button.SHOULDER_2)) {
                //if (joyconR.GetButtonDown (m_buttons[1])) {
                Rgyro = new Vector3 (0, 0, 0);
                Lgyro = new Vector3 (0, 0, 0);

                Raccel = new Vector3 (0, 0, 0);
                Laccel = new Vector3 (0, 0, 0);

                standard = new Vector3 (7.0f, 0.5f, -2.0f);

                Rorientation = new Quaternion (0, 0, 0, 0);
                Lorientation = new Quaternion (0, 0, 0, 0);
            }
        }
    }

}