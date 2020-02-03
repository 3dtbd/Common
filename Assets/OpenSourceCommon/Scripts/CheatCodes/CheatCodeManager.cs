using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace WizardsCode.Cheats
{
    /// <summary>
    /// CheatCodeManager provides a set of key combinations that will enable and disable cheats.
    /// These are designed for testing purposes.
    /// Cheats always start with a `F1` and ends with `Enter`
    /// </summary>
    public class CheatCodeManager : MonoBehaviour
    {
        [Tooltip("Set to true if you want to enable god mode.")]
        public bool enableGodMode = true;
        [Tooltip("How long (in seconds) to stay in command capture mode before timing out.")]
        public float timeout = 10;

        private bool capturing = false;

        private AbstractCheatCode[] codes;
        private string input;
        private float startTime;

        void Awake()
        {
            codes = GetComponents<AbstractCheatCode>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!enableGodMode) return;

            if (capturing)
            {
                if (Time.time > startTime + timeout)
                {
                    StopCapturing();
                }

                foreach (char c in Input.inputString)
                {
                    if (c == '\b') // has backspace/delete been pressed?
                    {
                        if (input.Length != 0)
                        {
                            input = input.Substring(0, input.Length - 1);
                        }
                    }
                    else if ((c == '\n') || (c == '\r')) // enter/return
                    {
                        StopCapturing();
                        ExecuteCommand();
                    }
                    else
                    {
                        input += c;
                        Debug.Log("Cheat code entered so far (hit return when complete): " + input);
                    }
                }
            }

            if (Input.GetKeyUp(KeyCode.F1))
            {
                if (capturing)
                {
                    StopCapturing();
                } 
                else
                {
                    StartCapturing();
                }
            }
        }

        private void ExecuteCommand()
        {
            bool executed = false;
            for (int i = 0; i < codes.Length; i++)
            {
                MatchCollection matches = codes[i].CommandRegex().Matches(input);
                if (matches.Count > 0)
                {
                    StopCapturing();
                    Debug.Log("Executing command " + codes[i] + " with input " + input);
                    codes[i].Execute(matches);
                    executed = true;
                }
            }

            if (!executed)
            {
                Debug.LogWarning("Unable to match " + input + " to a cheat code.");
            }
        }

        private void StartCapturing()
        {
            capturing = true;
            input = "";
            startTime = Time.time;
        }

        void StopCapturing()
        {
            capturing = false;
            Debug.Log("Stopped capturing cheat code.");
        }

        void OnGUI()
        {
            if (!capturing) return;

            GUI.Label(new Rect((Screen.width - 100) / 2, (Screen.height - 20) / 2, 100, 20), input);
        }
    }
}