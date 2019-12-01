using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class KeyAlpha1Message : InfaceMessage
{
    public void Execute()
    {
        Test.s_lRenderSys.SetLight(!Test.s_lRenderSys.GetLight());
        Test.s_lRenderSys.SetUpdate();
    }
}

