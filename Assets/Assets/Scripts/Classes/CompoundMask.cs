using UnityEngine;

[System.Serializable]
public class CompoundMask
{
    public LayerMask layerMask = ~0;
    public string[] tagMask = new string[0];
    public bool ignoreSiblings = false;

    public bool Check(Transform self, Transform other)
    {
        if (ignoreSiblings && other.parent != null && self.parent == other.parent)
            return false;
        if (layerMask.Contains(other.gameObject.layer) == false)
            return false;

        if (tagMask.Length == 0)
            return true;
        for (int i = 0; i < tagMask.Length; i++)
        {
            if (other.tag == tagMask[i])
                return true;
        }

        return false;
    }
}