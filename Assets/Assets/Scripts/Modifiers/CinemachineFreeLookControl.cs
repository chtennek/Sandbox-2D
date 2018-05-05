using Cinemachine;
using UnityEngine;

public class CinemachineFreeLookControl : MonoBehaviour
{
    public CinemachineFreeLook cam;
    public InputReceiver input;
    public string activateButtonName = "Camera";
    public string centerCameraButtonName = "Center Camera";
    public string axisPairName = "Aim";
    public Vector2 sensitivity = new Vector2(.1f, .01f);

    private void Reset()
    {
        cam = GetComponent<CinemachineFreeLook>();
        input = GetComponent<InputReceiver>();
    }

    private void Awake()
    {
        if (cam == null || input == null)
            Warnings.ComponentMissing(this);
    }

    private void Update()
    {
        if (cam == null || input == null)
            return;

        Vector2 movement = Vector2.zero;
        if (activateButtonName.Length == 0 || input.GetButton(activateButtonName))
            movement = input.GetAxisPair(axisPairName) * sensitivity;
        cam.m_XAxis.Value += movement.x;
        cam.m_YAxis.Value += movement.y;

        if (input.GetButtonDown(centerCameraButtonName))
            cam.m_XAxis.Value = cam.LookAt.rotation.eulerAngles.y;
    }
}
