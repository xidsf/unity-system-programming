using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Camera))]
public class CameraStackHelper : MonoBehaviour
{
    private void Start()
    {
        var mainCamrea = Camera.main;
        var mainCameraData = mainCamrea.GetUniversalAdditionalCameraData();
        mainCameraData.renderType = CameraRenderType.Base;

        var camera = GetComponent<Camera>();
        var cameraData = camera.GetUniversalAdditionalCameraData();
        cameraData.renderType = CameraRenderType.Overlay;
        mainCameraData.cameraStack.Add(camera);

        var uiCamera = UIManager.Instance.UICamera;
        var uiCameraData = uiCamera.GetUniversalAdditionalCameraData();
        uiCameraData.renderType = CameraRenderType.Overlay;
        mainCameraData.cameraStack.Add(uiCamera);
    }
}
