using UnityEngine;
using Cinemachine;
using Cinemachine.Utility;

public class RotateToTarget : CinemachineExtension
{
    public bool YRotationOnly = true;
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Aim)
        {
            var follow = VirtualCamera.Follow;
            if (follow != null)
            {
                Vector3 fwd = state.RawOrientation * Vector3.forward;
                if (YRotationOnly)
                    fwd = fwd.ProjectOntoPlane(state.ReferenceUp);
                follow.rotation = Quaternion.LookRotation(fwd, state.ReferenceUp);
            }
        }
    }
}
