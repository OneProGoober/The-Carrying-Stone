using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomStandAloneInputModule : StandaloneInputModule
{
    public PointerEventData GetPointerDataLeft()
    {
        return m_PointerData[kMouseLeftId];
    }

    public PointerEventData GetPointerDataRight()
    {
        return m_PointerData[kMouseRightId];
    }
}
