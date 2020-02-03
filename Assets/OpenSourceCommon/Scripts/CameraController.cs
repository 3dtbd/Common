using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WizardsCode.Controller
{
    public class CameraController : MonoBehaviour
    {
        /*
            FEATURES
            WASD/Arrows:    Movement
            Q:    Drop
            E:    Climb
            Shift:    Move faster
            Control:    Move slower
            Mouse Look:    Right Mouse Button
            Esc:    Toggle cursor locking to screen (you can also press Ctrl+P to toggle play mode on and off).
        */

        public float cameraSensitivity = 90;
        public float climbSpeed = 4;
        public float normalMoveSpeed = 10;
        public float slowMoveFactor = 0.25f;
        public float fastMoveFactor = 3;
        public int maxVerticalAngle = 90;

        private float rotationX = 0.0f;
        private float rotationY = 0.0f;

        internal virtual void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            rotationX = transform.localRotation.eulerAngles.x;
            rotationY = transform.localRotation.eulerAngles.y;
        }

        void Update()
        {
            MouseLook();
            Move();
            CursorLock();
        }

        private static void CursorLock()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                switch (Cursor.lockState)
                {
                    case CursorLockMode.Locked:
                        Cursor.lockState = CursorLockMode.None;
                        break;
                    case CursorLockMode.None:
                        Cursor.lockState = CursorLockMode.Locked;
                        break;
                }
            }
        }

        private void Move()
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                transform.position += transform.forward * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
                transform.position += transform.right * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                transform.position += transform.forward * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
                transform.position += transform.right * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
            }
            else
            {
                transform.position += transform.forward * normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
                transform.position += transform.right * normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
            }


            if (Input.GetKey(KeyCode.E)) { transform.position += transform.up * climbSpeed * Time.deltaTime; }
            if (Input.GetKey(KeyCode.Q)) { transform.position -= transform.up * climbSpeed * Time.deltaTime; }
        }

        private void MouseLook()
        {
            if (Input.GetMouseButton(1))
            {
                float y = -Input.GetAxis("Mouse Y");
                rotationX +=  y * cameraSensitivity * Time.deltaTime;
                rotationX = Mathf.Clamp(rotationX, -maxVerticalAngle, maxVerticalAngle);

                float x = Input.GetAxis("Mouse X");
                rotationY += x * cameraSensitivity * Time.deltaTime;

                Quaternion localRotation = Quaternion.Euler(rotationX, rotationY, 0.0f);
                transform.localRotation = localRotation;
            }
        }
    }
}
