using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class KeyIMessage : InfaceMessage
{
    public void Execute()
    {
        Test.s_model.SetRotation(Test.s_speed*5, 0, 0);
        Test.s_lRenderSys.SetUpdate();
    }
}