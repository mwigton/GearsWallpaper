using System;
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Gear : MonoBehaviour 
{
    public enum RotateDirection{ forward, reverse }
    public enum RotateAxis { x, y, z }

    [Serializable]
    public class GearSettings
    {
        public Gear parentGear;
        public bool isDriveGear;
        public int gearRadianPosition;

        public GameObject gearMesh;
        public GameObject pinMesh;

        [Serializable]
        public class Dimensions
        {
            public float gearRadius = 10;
            public float teethLength = 3;
            public int spokeCount = 3;
        }

        [Serializable]
        public class Rotation
        {
            public RotateDirection rotateDirection;
            public RotateAxis rotateAxis;
            public float speed = 5;
            public bool localSpaceRotation = true;
        }

        public Dimensions dimensions;
        public Rotation rotation;
    }

    public GearSettings settings;

    private bool _hasParentSettings;
    private Transform _trans;

    void Awake()
    {
        if (_trans == null) _trans = transform;

        if (Application.isPlaying)
        {
            GearManager.instance.gears.Add(this);
            if (settings.isDriveGear) { GearManager.instance.driveGears.Add(this); }
        }
    }

    void Start()
    {
        if (!settings.parentGear && !settings.isDriveGear)
        {
            Debug.LogWarning("No parent gear assigned", this);
        }
    }

    void Update()
    {
        if (Application.isPlaying)
        {
            if (GearManager.gearsReady)
            {
                if (settings.isDriveGear) { RotateDriveGear(); return; }
                if (settings.parentGear) { RotateSlaveGear(); return; }
            }
        }
        else
        {
            if (settings.parentGear)
            {
                PositionGear(settings.gearRadianPosition, settings.parentGear._trans.position);
            }
        }

    }

    //Uncomment for debug gizmos
    //void OnDrawGizmos()
    //{
        //Gizmos.DrawWireSphere(_trans.position, settings.dimensions.gearRadius);
        //Gizmos.DrawWireSphere(_trans.position, settings.dimensions.gearRadius + settings.dimensions.teethLength);
    //}

    private void RotateDriveGear()
    {
        Vector3 rotateAmount = new Vector3(0, 0, 0);

        float rotateBy = (settings.rotation.speed * GearManager.globalSpeedMultiplier) * Time.deltaTime;

        switch (settings.rotation.rotateAxis)
        {
            case RotateAxis.x:
                rotateAmount.x = rotateBy;
                break;

            case RotateAxis.y:
                rotateAmount.y = rotateBy;
                break;

            case RotateAxis.z:
                rotateAmount.z = rotateBy;
                break;
        }

        if (settings.rotation.rotateDirection == RotateDirection.reverse)
        {
            rotateAmount = -rotateAmount;
        }

        if (settings.rotation.localSpaceRotation)
        {
            transform.Rotate(rotateAmount, Space.Self);
        }
        else
        {
            transform.Rotate(rotateAmount, Space.World);
        }
    }

    private void RotateSlaveGear()
    {
        Vector3 rotateAmount = new Vector3(0, 0, 0);
        Gear _parentGear = settings.parentGear;

        GetParentSettings();

        float rotateBy = (settings.rotation.speed * GearManager.globalSpeedMultiplier) * Time.deltaTime;

        switch (_parentGear.settings.rotation.rotateAxis)
        {
            case RotateAxis.x:
                rotateAmount.x = rotateBy;
                break;

            case RotateAxis.y:
                rotateAmount.y = rotateBy;
                break;

            case RotateAxis.z:
                rotateAmount.z = rotateBy;
                break;
        }

        if (settings.rotation.rotateDirection == RotateDirection.reverse)
        {
            rotateAmount = -rotateAmount;
        }

        if (_parentGear.settings.rotation.localSpaceRotation)
        {
            transform.Rotate(rotateAmount, Space.Self);
        }
        else
        {
            transform.Rotate(rotateAmount, Space.World);
        }
    }

    float offset;
    public void PositionGear(float degreePosition, Vector3 originalGearCenter)
    {
        GetParentSettings();
        offset = (settings.dimensions.gearRadius * 2) + settings.dimensions.teethLength;

        Vector3 gearPosition;
        const float degrees2Radian = 57.2957795f;
        float parentRotationAmount = 0;

       
        switch (settings.rotation.rotateAxis)
        {
            case RotateAxis.x:
                _trans.localEulerAngles = new Vector3(-degreePosition * 2,_trans.localEulerAngles.y, _trans.localEulerAngles.z);
                parentRotationAmount = settings.parentGear._trans.localEulerAngles.x;
                break;

            case RotateAxis.y:
                _trans.localEulerAngles = new Vector3(_trans.localEulerAngles.x, -degreePosition * 2, _trans.localEulerAngles.z);
                parentRotationAmount = settings.parentGear._trans.localEulerAngles.y;
                break;

            case RotateAxis.z:
                _trans.localEulerAngles = new Vector3(_trans.localEulerAngles.x, _trans.localEulerAngles.y, degreePosition * 2);
                parentRotationAmount = settings.parentGear._trans.localEulerAngles.z;
                break;
        }

        degreePosition /= degrees2Radian;
        float circleRotationAmount = ((parentRotationAmount) / degrees2Radian) / 2;

        switch (settings.rotation.rotateAxis)
        {
            case RotateAxis.x:
                degreePosition -= circleRotationAmount;
                break;

            case RotateAxis.y:
                degreePosition -= circleRotationAmount;
                break;

            case RotateAxis.z:
                degreePosition += circleRotationAmount;
                break;
        }

        gearPosition.x = offset * Mathf.Cos(degreePosition) + originalGearCenter.x;
        gearPosition.y = offset * Mathf.Sin(degreePosition) + originalGearCenter.y;

        _trans.position = new Vector3(gearPosition.x, gearPosition.y, _trans.position.z);
    }

    public void PositionGear()
    {
        _trans = transform;
        PositionGear(settings.gearRadianPosition, settings.parentGear._trans.position);
    }

    private void GetParentSettings()
    {
        settings.rotation.rotateAxis = settings.parentGear.settings.rotation.rotateAxis;
        settings.rotation.localSpaceRotation = settings.parentGear.settings.rotation.localSpaceRotation;
        settings.rotation.speed = settings.parentGear.settings.rotation.speed;

        if (settings.parentGear.settings.rotation.rotateDirection == RotateDirection.forward){
            settings.rotation.rotateDirection = RotateDirection.reverse;
        }

        if (settings.parentGear.settings.rotation.rotateDirection == RotateDirection.reverse){
            settings.rotation.rotateDirection = RotateDirection.forward;
        }
    }

    public void SetParentSettings()
    {
        if (settings.isDriveGear)
        {
            _hasParentSettings = true;
            return;
        }

        GetParentSettings();
        _hasParentSettings = true;
    }

    public bool isParentSettingsSet()
    {
        return _hasParentSettings;
    }
}
