using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class EchoGridManager : GridManager
{
    public void MoveAll(float x, float y, float z)
    {
        foreach (GridEntity entity in entities)
        {
            EchoGridEntity echo = entity as EchoGridEntity;
            if (echo != null)
                entity.SubmitMove(echo.inputScaleFactor.x * x, echo.inputScaleFactor.y * y, echo.inputScaleFactor.z * z);
            else
                entity.SubmitMove(x, y, z);
        }
    }
}
