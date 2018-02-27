using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableVector3 {
    public float x;
    public float y;
    public float z;

    public SerializableVector3(Vector3 v) {
        this.x = v.x;
        this.y = v.y;
        this.z = v.z;
    }

    public Vector3 ToVector3() {
        return new Vector3(x, y, z);
    }
}