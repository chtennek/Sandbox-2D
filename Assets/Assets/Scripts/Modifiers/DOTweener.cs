using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public enum SerializableTweenType
{
    ToTransform,
    FromTransform,
    Punch,
    Shake,
    FadeIn,
    FadeOut
}

[System.Serializable]
public class SerializableTween
{
    public SerializableTweenType type;
    public Transform target;
    public float duration;
    public float appendTime;

    public int vibrato = 10;
    public float elasticityOrRandomness = 1;
}

[System.Serializable]
public class EaseSettings
{
    public float duration = 0;

    [Space]
    public Ease type = Ease.Linear;
    public float overshootOrAmplitude = 1.70158f;
    [Range(-1, 1)]
    public float period = 0;
    public int fps = 60;

    [Space]
    public bool useCustomCurve = false;
    public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
}

public class DOTweener : MonoBehaviour
{
    public bool playOnAwake = true;

    public EaseSettings ease;
    public float prependTime;
    public SerializableTween[] tweens;

    private Sequence sequence;

    private Graphic graphic;
    private Material material;

    private void Reset()
    {
        graphic = GetComponent<Graphic>();
        material = GetComponent<Material>();
    }

    private void Awake()
    {
        sequence = DOTween.Sequence();

        float time = 0;
        foreach (SerializableTween tween in tweens)
        {
            switch (tween.type)
            {
                case SerializableTweenType.ToTransform:
                    sequence.Insert(time, transform.DOMove(tween.target.position, tween.duration));
                    sequence.Insert(time, transform.DORotate(tween.target.eulerAngles, tween.duration));
                    sequence.Insert(time, transform.DOScale(tween.target.localScale, tween.duration));
                    break;

                case SerializableTweenType.FromTransform:
                    sequence.Insert(time, transform.DOMove(tween.target.position, tween.duration).From());
                    sequence.Insert(time, transform.DORotate(tween.target.eulerAngles, tween.duration).From());
                    sequence.Insert(time, transform.DOScale(tween.target.localScale, tween.duration).From());
                    break;

                case SerializableTweenType.Punch:
                    sequence.Insert(time, transform.DOPunchPosition(tween.target.position, tween.duration, tween.vibrato, tween.elasticityOrRandomness));
                    sequence.Insert(time, transform.DOPunchRotation(tween.target.eulerAngles, tween.duration, tween.vibrato, tween.elasticityOrRandomness));
                    sequence.Insert(time, transform.DOPunchScale(tween.target.localScale, tween.duration, tween.vibrato, tween.elasticityOrRandomness));
                    break;

                case SerializableTweenType.Shake:
                    sequence.Insert(time, transform.DOShakePosition(tween.duration, tween.target.position, tween.vibrato, tween.elasticityOrRandomness));
                    sequence.Insert(time, transform.DOShakeRotation(tween.duration, tween.target.eulerAngles, tween.vibrato, tween.elasticityOrRandomness));
                    sequence.Insert(time, transform.DOShakeScale(tween.duration, tween.target.localScale, tween.vibrato, tween.elasticityOrRandomness));
                    break;

                case SerializableTweenType.FadeIn:
                    if (graphic != null)
                        sequence.Insert(time, graphic.DOFade(1, tween.duration));
                    if (material != null)
                        sequence.Insert(time, material.DOFade(1, tween.duration));
                    break;

                case SerializableTweenType.FadeOut:
                    if (graphic != null)
                        sequence.Insert(time, graphic.DOFade(0, tween.duration));
                    if (material != null)
                        sequence.Insert(time, material.DOFade(0, tween.duration));
                    break;
            }
            time += tween.appendTime;
        }
        sequence.PrependInterval(prependTime);


        if (ease.useCustomCurve)
            sequence.SetEase(ease.curve);
        else
            sequence.SetEase(ease.type, ease.overshootOrAmplitude, ease.period);
    }

    private void Start()
    {
        if (playOnAwake)
            Play();
    }

    public void Play()
    {
        sequence.Play();
    }

    public void PlayBackwards()
    {
        sequence.PlayBackwards();
    }
}
