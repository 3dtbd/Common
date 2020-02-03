using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace WizardsCode.Cheats
{
    /// <summary>
    /// The abstract class representing a Cheat Code. 
    /// Implement this and add it to the Game Object with the CheatCodeManager attached.
    /// </summary>
    [Serializable]
    public abstract class AbstractCheatCode : MonoBehaviour
    {
        /// <summary>
        /// A regular expression that needs to be matched to execute this command.
        /// Example implementation:
        /// Regex rx = new Regex(@"\b(?<word>\w+)\s+(\k<word>)\b",
        ///    RegexOptions.Compiled | RegexOptions.IgnoreCase);
        ///    return rx;
        /// </summary>
        public abstract Regex CommandRegex();

        /// <summary>
        /// This method will be called whenever the cheat code is enabled.
        /// </summary>
        public abstract void Execute(MatchCollection matches);
    }
}