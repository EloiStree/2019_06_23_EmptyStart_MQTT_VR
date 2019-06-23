using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWithTracker : MQTTPlayerInfoRegularSender
{
    public OVRManagerInfo m_ovrManager;
    public OVRCameraRig m_cameraRig;

    public override string ToSendEveryNowAndThen()
    {
        m_ovrManager = GetOVRManagerInfo();
        return JsonUtility.ToJson(m_ovrManager);
    }

    public static OVRManagerInfo GetOVRManagerInfo()
    {



        OVRManagerInfo ovrManager;
        ovrManager.inSceneInfo = GetInSceneInfo();
        ovrManager.tracker = GetTrackerInfo();
        ovrManager.userProfile = GetUserProfile();
        ovrManager.boundary = GetBoundaryInfo();
        ovrManager.batteryLevel = OVRManager.batteryLevel;
        ovrManager.batteryStatus = OVRManager.batteryStatus;
        ovrManager.batteryTemperature = OVRManager.batteryTemperature;
        ovrManager.cpuLevel = OVRManager.cpuLevel;
        ovrManager.gpuLevel = OVRManager.gpuLevel;
        ovrManager.gpuUtilLevel = OVRManager.gpuUtilLevel;
        ovrManager.gpuUtilSupported = OVRManager.gpuUtilSupported;
        ovrManager.hasInputFocus = OVRManager.hasInputFocus;
        ovrManager.hasVrFocus = OVRManager.hasVrFocus;
        ovrManager.isHmdPresent = OVRManager.isHmdPresent;
        ovrManager.isPowerSavingActive = OVRManager.isPowerSavingActive;
        ovrManager.loadedXRDevice = OVRManager.loadedXRDevice;
        ovrManager.OCULUS_UNITY_NAME_STR = OVRManager.OCULUS_UNITY_NAME_STR;
        ovrManager.OPENVR_UNITY_NAME_STR = OVRManager.OPENVR_UNITY_NAME_STR;
        ovrManager.pluginVersion = GetVersionInfo(OVRManager.pluginVersion);
        ovrManager.sdkVersion = GetVersionInfo(OVRManager.sdkVersion);
        ovrManager.utilitiesVersion = GetVersionInfo(OVRManager.utilitiesVersion);
        ovrManager.tiledMultiResLevel = OVRManager.tiledMultiResLevel;
        ovrManager.tiledMultiResSupported = OVRManager.tiledMultiResSupported;
        ovrManager.volumeLevel = OVRManager.volumeLevel;
        ovrManager.headAndHands = GetHeadHandsAnchors();
        return ovrManager;
    }
    private static OVRCameraRig cameraRig;
    public OVRCameraRig CameraRig { get {
            if (cameraRig == null)
                cameraRig = GameObject.FindObjectOfType<OVRCameraRig>();
            return cameraRig;
        }
    }
    public static OVRAnchorsInfo GetHeadHandsAnchors()
    {
        OVRCameraRig cameraRig = GameObject.FindObjectOfType<OVRCameraRig>();
        OVRAnchorsInfo result = new OVRAnchorsInfo() ;
        if (cameraRig) {
            result.m_rootUnityPosition = new SpacePoint(cameraRig.trackerAnchor);
            result.m_head = new SpaceTrackedPoint();
            result.m_head.SetPoint(cameraRig.centerEyeAnchor, cameraRig.trackingSpace, cameraRig.centerEyeAnchor);

            result.m_leftHand = new SpaceTrackedPoint();
            result.m_leftHand.SetPoint(cameraRig.leftHandAnchor, cameraRig.trackingSpace, cameraRig.centerEyeAnchor);

            result.m_rightHand = new SpaceTrackedPoint();
            result.m_rightHand.SetPoint(cameraRig.rightHandAnchor, cameraRig.trackingSpace, cameraRig.centerEyeAnchor);
        }
      
        return result;
    }

    private static OVRManagerInSceneInfo GetInSceneInfo()
    {
        OVRManagerInSceneInfo result = new OVRManagerInSceneInfo() ;

        result.allowRecenter = OVRManager.instance.AllowRecenter;
        result.chromatic = OVRManager.instance.chromatic;
        //result.chromaKeyColor = OVRManager.instance.chromaKeyColor;
        result.headPoseRelativeOffsetRotation = OVRManager.instance.headPoseRelativeOffsetRotation;
        result.headPoseRelativeOffsetTranslation = OVRManager.instance.headPoseRelativeOffsetTranslation;
        result.isSupportedPlatform = OVRManager.instance.isSupportedPlatform;
        result.isUserPresent = OVRManager.instance.isUserPresent;
        result.maxRenderScale = OVRManager.instance.maxRenderScale;
        result.minRenderScale = OVRManager.instance.minRenderScale;
        result.monoscopic = OVRManager.instance.monoscopic;
        result.profilerTcpPort = OVRManager.instance.profilerTcpPort;
        //result.runInEditMode = OVRManager.instance.runInEditMode;
        result.usePositionTracking = OVRManager.instance.usePositionTracking;
        result.useRecommendedMSAALevel = OVRManager.instance.useRecommendedMSAALevel;
        result.useRotationTracking = OVRManager.instance.useRotationTracking;


        return result;
    }

    private static OVRVersion GetVersionInfo(Version version)
    {
        OVRVersion result = new OVRVersion();
        result.build = version.Build;
        result.major = version.Major;
        result.majorRevision = version.MajorRevision;
        result.minor = version.Minor;
        result.minorRevision = version.MinorRevision;
        result.revision = version.Revision;
        return result;
    }

    public static OVRTrackersInfo GetTrackerInfo()
    {
        OVRTrackersInfo result = new OVRTrackersInfo();
        result.count = OVRManager.tracker.count;
        result.isEnabled = OVRManager.tracker.isEnabled;
        result.isPositionTracked = OVRManager.tracker.isPositionTracked;
        result.isPresent = OVRManager.tracker.isPresent;
        result.trackers = new OVRTrackerInfo[result.count];
        for (int i = 0; i < result.count; i++)
        {
            //            result.trackers[i].b = OVRManager.tracker.GetPose(i).flipZ
            result.trackers[i].farZ = OVRManager.tracker.GetFrustum(i).farZ;
            result.trackers[i].fov = OVRManager.tracker.GetFrustum(i).fov;
            result.trackers[i].nearZ = OVRManager.tracker.GetFrustum(i).nearZ;
            result.trackers[i].orientation = OVRManager.tracker.GetPose(i).orientation;
            result.trackers[i].position = OVRManager.tracker.GetPose(i).position;
            result.trackers[i].isPosevalide = OVRManager.tracker.GetPoseValid(i);
            result.trackers[i].isPresent = OVRManager.tracker.GetPresent(i);


        }
       return result;
    }

    public static OVRUserProfileInfo GetUserProfile()
    {
        OVRUserProfileInfo result = new OVRUserProfileInfo();
        if (OVRManager.profile) {
            result.eyeDepth = OVRManager.profile.eyeDepth;
            result.eyeHeight = OVRManager.profile.eyeHeight;
            result.ipd = OVRManager.profile.ipd;
            result.name = OVRManager.profile.name;
            result.neckHeight = OVRManager.profile.neckHeight;
        }

        return result;    }

    public static OVRBoundaryInfo GetBoundaryInfo()
    {
        OVRBoundaryInfo boundary = new OVRBoundaryInfo();
        boundary.isBoundaryVisible = OVRManager.boundary.GetVisible();
        boundary.isConfigured = OVRManager.boundary.GetConfigured();
        boundary.boundaryDimensionArea = OVRManager.boundary.GetDimensions(OVRBoundary.BoundaryType.PlayArea);
        boundary.boundaryDimensionOuter = OVRManager.boundary.GetDimensions(OVRBoundary.BoundaryType.OuterBoundary);
        boundary.boundaryGeometryArea = OVRManager.boundary.GetGeometry(OVRBoundary.BoundaryType.PlayArea);
        boundary.boundaryGeometryOuter = OVRManager.boundary.GetGeometry(OVRBoundary.BoundaryType.OuterBoundary);
        return boundary;
    }
}
[System.Serializable]
public struct OVRManagerInSceneInfo
{
    internal bool chromatic;
    internal bool allowRecenter;
    internal Color chromaKeyColor;
    internal Vector3 headPoseRelativeOffsetRotation;
    internal Vector3 headPoseRelativeOffsetTranslation;
    internal bool isSupportedPlatform;
    internal bool isUserPresent;
    internal float maxRenderScale;
    internal float minRenderScale;
    internal bool monoscopic;
    internal int profilerTcpPort;
    internal bool runInEditMode;
    internal bool usePositionTracking;
    internal bool useRecommendedMSAALevel;
    internal bool useRotationTracking;
}
[System.Serializable]
public struct OVRManagerInfo
{
    public float batteryLevel;
    public int batteryStatus;
    public float batteryTemperature;
    public int cpuLevel;
    public int gpuLevel;
    public float gpuUtilLevel;
    public bool gpuUtilSupported;
    public bool hasInputFocus;
    public bool hasVrFocus;
    public bool isHmdPresent;
    public bool isPowerSavingActive;
    public OVRManager.XRDevice loadedXRDevice;
    public OVRVersion pluginVersion;
    public OVRVersion sdkVersion;
    public OVRVersion utilitiesVersion;
    public OVRManager.TiledMultiResLevel tiledMultiResLevel;
    public bool tiledMultiResSupported;
    public float volumeLevel;

    public string OCULUS_UNITY_NAME_STR;
    public string OPENVR_UNITY_NAME_STR;
    public OVRBoundaryInfo boundary;
    public OVRUserProfileInfo userProfile;
    public OVRTrackersInfo tracker;
    internal OVRManagerInSceneInfo inSceneInfo;
    internal OVRAnchorsInfo headAndHands;
}

[System.Serializable]
public struct OVRVersion
{
    internal int build;
    internal int major;
    internal short majorRevision;
    internal int minor;
    internal short minorRevision;
    internal int revision;
}
[System.Serializable]
public struct OVRBoundaryInfo
{
    public bool isBoundaryVisible;
    public bool isConfigured;
    public Vector3 boundaryDimensionArea;
    public Vector3 boundaryDimensionOuter;
    public Vector3[] boundaryGeometryArea;
    public Vector3[] boundaryGeometryOuter;
}

[System.Serializable]
public struct OVRUserProfileInfo
{
    public float eyeDepth;
    public float eyeHeight;
    public float ipd;
    public string name;
    public float neckHeight;
}
[System.Serializable]
public struct OVRTrackersInfo
{
    public int count;
    public bool isEnabled;
    public bool isPositionTracked;
    public bool isPresent;
    public OVRTrackerInfo[] trackers;
}

[System.Serializable]
public struct OVRTrackerInfo
{
    public float farZ;
    public Vector2 fov;
    public float nearZ;
    public Quaternion orientation;
    public Vector3 position;
    public bool isPosevalide;
    public bool isPresent;
}
[System.Serializable]
public struct OVRAnchorsInfo
{
    public SpaceTrackedPoint m_head;
    public SpaceTrackedPoint m_leftHand;
    public SpaceTrackedPoint m_rightHand;
    internal SpacePoint m_rootUnityPosition;
}