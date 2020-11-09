using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPin
{
    void Reset();
    bool IsOut();
    void CheckIfIsOut();
}
