using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFractureJustDestroy : ObjectFractureSuitScale
{
    public override void OnTriggerBroken()
    {
        Destroy(originalObject, 0.1f);
    }
}
