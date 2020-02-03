using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using WizardsCode.Tools.DocGen;

namespace WizardsCode.Cheats
{
    [DocGen("Set the game speed with 's' followed by a float value, followed by the enter key, e.g. 's2' for double speed or 's0.5' for half speed.")]
    public class GameSpeed : AbstractCheatCode
    {
        public override Regex CommandRegex()
        {
            Regex rx = new Regex(@"s(\d+\.?\d*)",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return rx;
        }

        public override void Execute(MatchCollection matches)
        {
            Time.timeScale = float.Parse(matches[0].Groups[1].ToString());
        }
    }
}