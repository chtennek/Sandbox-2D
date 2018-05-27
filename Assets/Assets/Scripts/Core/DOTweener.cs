using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public enum SerializableTweenType
{
    Position,
    Rotation,
    Scale,
    RelativePosition,
    RelativeRotation,
    RelativeScale,
    PunchPosition,
    PunchRotation,
    PunchScale,
    ShakePosition,
    ShakeRotation,
    ShakeScale,
    FadeIn,
    FadeOut,
    Fade,
}

[System.Serializable]
public class SerializableTween
{
    [Header("Object")]
    public Transform obj;

    [Space]
    public CanvasGroup canvas;
    public Graphic graphic;
    public Material material;

    [Header("Tween")]
    public SerializableTweenType type;
    public bool useStartValue = false;
    public Transform startTarget;
    public Vector3 startValue;
    public bool from = false;
    public Transform endTarget;
    public Vector3 endValue;

    [Header("Parameters")]
    public float appendTime;
    public float delay;
    public float duration = .1f;

    public int vibrato = 10;
    public float elasticityOrRandomness = 1;

    public Tweener DOFade(float endValue, float duration)
    {
        if (canvas != null)
            return canvas.DOFade(endValue, duration);
        if (graphic != null)
            return graphic.DOFade(endValue, duration);
        if (material != null)
            return material.DOFade(endValue, duration);
        return null;
    }
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

    public void SetEase(Sequence sequence)
    {
        if (useCustomCurve)
            sequence.SetEase(curve);
        else
            sequence.SetEase(type, overshootOrAmplitude, period);
    }

    public void SetEase(Tweener tweener)
    {
        if (useCustomCurve)
            tweener.SetEase(curve);
        else
            tweener.SetEase(type, overshootOrAmplitude, period);
    }
}

public class DOTweener : MonoBehaviour
{
    public bool playOnAwake;

    public EaseSettings ease;
    public float prependTime;
    public SerializableTween[] tweens;

    private Sequence sequence;

    public void SetTarget(Transform target)
    {
        foreach (SerializableTween tween in tweens)
            tween.endTarget = target;
    }

    private void Awake()
    {
        sequence = BuildSequence();
    }

    private void Start()
    {
        if (playOnAwake)
            PlayForward();
    }

    public void GotoStart()
    {
        sequence.Goto(0);
    }

    public void GotoEnd()
    {
        sequence.Goto(sequence.Duration());
    }

    public void Goto(float to)
    {
        sequence.Goto(to);
    }

    public void Flip()
    {
        sequence.Flip();
    }

    public void PlayForward()
    {
        sequence.PlayForward();
    }

    public void PlayBackwards()
    {
        sequence.PlayBackwards();
    }

    public YieldInstruction WaitForStart()
    {
        return sequence.WaitForPosition(0);
    }

    public YieldInstruction WaitForEnd()
    {
        return sequence.WaitForCompletion();
    }

    private Sequence BuildSequence()
    {
        Sequence seq = DOTween.Sequence();
        float time = 0;
        foreach (SerializableTween tween in tweens)
        {
            bool useStartValue = tween.useStartValue;
            Tweener initTweener = null;
            Vector3 startValue = tween.startValue;
            Tweener tweener = null;
            Vector3 endValue = tween.endValue;

            // Set start and end values based on tween type
            switch (tween.type)
            {
                case SerializableTweenType.Position:
                case SerializableTweenType.RelativePosition:
                case SerializableTweenType.PunchPosition:
                case SerializableTweenType.ShakePosition:
                    if (tween.startTarget != null)
                        startValue = tween.startTarget.position;

                    if (tween.obj != null)
                        initTweener = tween.obj.DOMove(startValue, 0);

                    if (tween.endTarget != null)
                        endValue = tween.endTarget.position;
                    break;
                case SerializableTweenType.Rotation:
                case SerializableTweenType.RelativeRotation:
                case SerializableTweenType.PunchRotation:
                case SerializableTweenType.ShakeRotation:
                    if (tween.startTarget != null)
                        startValue = tween.startTarget.eulerAngles;

                    if (tween.obj != null)
                        initTweener = tween.obj.DORotate(startValue, 0);

                    if (tween.endTarget != null)
                        endValue = tween.endTarget.eulerAngles;
                    break;
                case SerializableTweenType.Scale:
                case SerializableTweenType.RelativeScale:
                case SerializableTweenType.PunchScale:
                case SerializableTweenType.ShakeScale:
                    if (tween.startTarget != null)
                        startValue = tween.startTarget.localScale;
                    if (tween.obj != null)

                        initTweener = tween.obj.DOScale(startValue, 0);

                    if (tween.endTarget != null)
                        endValue = tween.endTarget.localScale;
                    break;

                case SerializableTweenType.FadeIn:
                    useStartValue = true;
                    initTweener = tween.DOFade(0, 0);
                    tweener = tween.DOFade(1, tween.duration);
                    break;
                case SerializableTweenType.FadeOut:
                    useStartValue = true;
                    initTweener = tween.DOFade(1, 0);
                    tweener = tween.DOFade(0, tween.duration);
                    break;
                case SerializableTweenType.Fade:
                    initTweener = tween.DOFade(tween.startValue.x, 0);
                    tweener = tween.DOFade(tween.endValue.x, tween.duration);
                    break;
            }

            // Create the main tween
            if (tween.obj != null)
                switch (tween.type)
                {
                    case SerializableTweenType.Position:
                        tweener = tween.obj.DOMove(endValue, tween.duration);
                        break;
                    case SerializableTweenType.Rotation:
                        tweener = tween.obj.DORotate(endValue, tween.duration);
                        break;
                    case SerializableTweenType.Scale:
                        tweener = tween.obj.DOScale(endValue, tween.duration);
                        break;

                    case SerializableTweenType.RelativePosition:
                        tweener = tween.obj.DOMove(transform.position + endValue, tween.duration);
                        break;
                    case SerializableTweenType.RelativeRotation:
                        Quaternion endRotation = transform.rotation * Quaternion.Euler(endValue);
                        tweener = tween.obj.DORotate(endRotation.eulerAngles, tween.duration);
                        break;
                    case SerializableTweenType.RelativeScale:
                        tweener = tween.obj.DOScale(Vector3.Scale(transform.localScale, endValue), tween.duration);
                        break;

                    case SerializableTweenType.PunchPosition:
                        tweener = tween.obj.DOPunchPosition(endValue, tween.duration, tween.vibrato, tween.elasticityOrRandomness);
                        break;
                    case SerializableTweenType.PunchRotation:
                        tweener = tween.obj.DOPunchRotation(endValue, tween.duration, tween.vibrato, tween.elasticityOrRandomness);
                        break;
                    case SerializableTweenType.PunchScale:
                        tweener = tween.obj.DOPunchScale(endValue, tween.duration, tween.vibrato, tween.elasticityOrRandomness);
                        break;

                    case SerializableTweenType.ShakePosition:
                        tweener = tween.obj.DOShakePosition(tween.duration, endValue, tween.vibrato, tween.elasticityOrRandomness);
                        break;
                    case SerializableTweenType.ShakeRotation:
                        tweener = tween.obj.DOShakeRotation(tween.duration, endValue, tween.vibrato, tween.elasticityOrRandomness);
                        break;
                    case SerializableTweenType.ShakeScale:
                        tweener = tween.obj.DOShakeScale(tween.duration, endValue, tween.vibrato, tween.elasticityOrRandomness);
                        break;
                }

            time += tween.appendTime;
            if (tween.from)
                tweener = tweener.From();

            if (useStartValue && initTweener != null)
                seq.Insert(time, initTweener);
            if (tweener != null)
                seq.Insert(time + tween.delay, tweener);
        }

        seq.PrependInterval(prependTime);
        seq.SetAutoKill(false).SetUpdate(true);
        ease.SetEase(seq);
        return seq;
    }
}
