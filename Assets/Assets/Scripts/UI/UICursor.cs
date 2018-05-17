using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class UICursor : MonoBehaviour
{
    public float duration = .1f;

    [SerializeField]
    private Transform m_target;
    public Transform Target
    {
        get { return m_target; }
        set
        {
            if (value == null)
            {
                if (material != null)
                    material.DOFade(0, duration);
                if (image != null)
                    image.DOFade(0, duration);
            }
            else if (m_target != null && value != null)
            {

                transform.DOMove(value.position, duration);
            }
            else if (m_target == null && value != null)
            {

                transform.position = value.position;

                if (material != null)
                    material.DOFade(1, duration);
                if (image != null)
                    image.DOFade(1, duration);
            }
        }
    }

    public Material material;
    public Image image;

    private void Reset()
    {
        material = GetComponent<Material>();
        image = GetComponent<Image>();
    }
}
