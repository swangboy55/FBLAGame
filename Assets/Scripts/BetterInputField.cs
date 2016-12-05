using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UI;

/// <summary>
/// unity UI is bad
/// this class exists solely to counteract the inevitable highlighting of text (thanks unity)
/// </summary>
public class BetterInputField : InputField
{

    private int CPSave = 0;

    //override the mouse input issue
    //unity UI is bad
    protected override void LateUpdate()
    {
        //utilize reflection to set the input field values manually
        //and override the select all on focus
        var cspi = typeof(InputField).GetProperty("caretSelectPositionInternal", BindingFlags.Instance | BindingFlags.NonPublic);
        var cpi = typeof(InputField).GetProperty("caretPositionInternal", BindingFlags.Instance | BindingFlags.NonPublic);
        var shouldActivateNextUpdate = typeof(InputField).GetField("m_ShouldActivateNextUpdate", BindingFlags.Instance | BindingFlags.NonPublic);
        
        if(isFocused)
        {
            CPSave = (int)cpi.GetValue(this, null);
        }

        bool doSetCP = false;
        if((bool)shouldActivateNextUpdate.GetValue(this) && !isFocused)
        {
            doSetCP = true;
        }

        base.LateUpdate();
        if (cspi != null && cpi != null)
        {
            if(doSetCP)
            {
                cpi.SetValue(this, CPSave, null);
            }
            cspi.SetValue(this, cpi.GetValue(this, null), null);
        }
    }
}
