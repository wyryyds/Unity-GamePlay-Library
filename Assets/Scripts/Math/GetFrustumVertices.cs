using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetFrustumVertices : MonoBehaviour
{
    Vector3 topLeftNear;
    Vector3 topRightNear;
    Vector3 bottomRightNear;
    Vector3 bottomLeftNear;
    Vector3 topRightFar;
    Vector3 topLeftFar;
    Vector3 bottomRightFar;
    Vector3 bottomLeftFar;

    private void Awake()
    {
        Camera mainCamera = Camera.main; // 获取主相机或目标相机

        Vector3[] frustumCornersNear = new Vector3[4];
        Vector3[] frustumCornersFar = new Vector3[4];
        mainCamera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), mainCamera.nearClipPlane, Camera.MonoOrStereoscopicEye.Mono, frustumCornersNear);
        mainCamera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), mainCamera.farClipPlane, Camera.MonoOrStereoscopicEye.Mono, frustumCornersFar);
        // 视锥体的八个角顶点
        topLeftNear = frustumCornersNear[0];
        topRightNear = frustumCornersNear[1];
        bottomRightNear = frustumCornersNear[2];
        bottomLeftNear = frustumCornersNear[3];
        topLeftFar = frustumCornersFar[0];
        topRightFar = frustumCornersFar[1];
        bottomRightFar = frustumCornersFar[2];
        bottomLeftFar = frustumCornersFar[3];

        for(int i = 0; i < 4; i ++)
        {
            Debug.Log(frustumCornersNear[i]);
        }

        for(int i = 0; i < 4; i ++)
        {
            Debug.Log(frustumCornersFar[i]);
        }


    }
    private void Start()
    {
        
    }

    void Update()
    {
        // this example shows the different camera frustums when using asymmetric projection matrices (like those used by OpenVR).

        var camera = GetComponent<Camera>();
        Vector3[] frustumCorners = new Vector3[4];
        camera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), camera.farClipPlane, Camera.MonoOrStereoscopicEye.Mono, frustumCorners);

        for (int i = 0; i < 4; i++)
        {
            var worldSpaceCorner = camera.transform.TransformVector(frustumCorners[i]);
            Debug.DrawRay(camera.transform.position, worldSpaceCorner, Color.blue);
        }

        camera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), camera.nearClipPlane, Camera.MonoOrStereoscopicEye.Mono, frustumCorners);

        for (int i = 0; i < 4; i++)
        {
            var worldSpaceCorner = camera.transform.TransformVector(frustumCorners[i]);
            Debug.DrawRay(camera.transform.position, worldSpaceCorner, Color.green);
        }

    }
}
