using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanMerger : MonoBehaviour
{
    private static StickmanMerger instance;

    public static StickmanMerger Instance => instance ?? (instance = FindObjectOfType<StickmanMerger>());

    [SerializeField] private Transform pistol, smg;

    [SerializeField] private bool isPistolPositionSet, isSMGPositionSet;
    [SerializeField] private float positionSettingSpeed;

    [SerializeField] private bool isPistolRotationSet, isSMGRotationSet;
    [SerializeField] private float rotationSettingSpeed;

    [SerializeField] private bool isGunChange;

    [SerializeField] private Transform finishLine;

    [SerializeField] private GameObject playerStickmanPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        SetPosition(pistol, isPistolPositionSet, transform.childCount - 1);
        SetRotation(pistol, isPistolRotationSet, transform.childCount - 1);

        SetAllPositionsAndRotations(smg, isSMGPositionSet, isSMGRotationSet);

        SetPosition(smg, isSMGPositionSet, transform.childCount - 1);
        SetRotation(smg, isSMGRotationSet, transform.childCount - 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CollectableStickman"))
        {
            if (transform.childCount >= 8)
            {
                Destroy(other.gameObject);
            }

            if (transform.childCount <= 1)
            {
                SetAnimation(pistol, transform.childCount - 1, true);
                SetMaterial(pistol, transform.childCount - 1);
            }

            other.transform.SetParent(transform);
            Destroy(other.GetComponent<Rigidbody>());
            Destroy(other);

            if (transform.childCount >= 2 && transform.childCount <= 4)
            {
                this.tag = "Pistol";

                SetAnimation(pistol, transform.childCount - 1, true);
                SetMaterial(pistol, transform.childCount - 1);

                isPistolPositionSet = true;
                isPistolRotationSet = true;
            }

            if (transform.childCount == 5)
            {
                isPistolPositionSet = false;
                isPistolRotationSet = false;

                this.tag = "SMG";

                isSMGPositionSet = true;
                isSMGRotationSet = true;

                ChangeGun(pistol, smg);
            }

            if (transform.childCount >= 6 && transform.childCount <= 8)
            {
                isGunChange = false;

                this.tag = "SMG";

                SetAnimation(smg, transform.childCount - 1, true);
                SetMaterial(smg, transform.childCount - 1);
            }
        }

        if (other.CompareTag("Portoon"))
        {
            if (transform.childCount <= 1)
            {
                this.tag = "Untagged";

                isPistolPositionSet = false;
                isPistolRotationSet = false;

                isSMGPositionSet = false;
                isSMGRotationSet = false;

                GetFirstStickman();

                GameManager.Instance.OnGameFailed();
            }
            else
            {
                ReduceStickman();
            }          
        }

        if (other.CompareTag("Stone"))
        {
            if (transform.position.z < finishLine.position.z)
            {
                if (transform.childCount <= 1)
                {
                    this.tag = "Untagged";

                    isPistolPositionSet = false;
                    isPistolRotationSet = false;

                    isSMGPositionSet = false;
                    isSMGRotationSet = false;

                    GetFirstStickman();

                    GameManager.Instance.OnGameFailed();
                }
                else
                {
                    ReduceStickman();
                }
            }
            else
            {
                this.tag = "Untagged";

                isPistolPositionSet = false;
                isPistolRotationSet = false;

                isSMGPositionSet = false;
                isSMGRotationSet = false;

                GetFirstStickman();

                GameManager.Instance.OnGameCompleted();
            }
        }
    }

    private void SetAnimation(Transform gun, int stickmanIndex, bool isAnimationActive)
    {
        Animator stickmanAnimator = transform.GetChild(stickmanIndex).GetComponent<Animator>();
        string gunStickmanTag = gun.GetChild(stickmanIndex).tag;

        stickmanAnimator.SetBool(gunStickmanTag, isAnimationActive);
    }

    private void SetMaterial(Transform gun, int stickmanIndex)
    {
        SkinnedMeshRenderer stickmanMeshRenderer = transform.GetChild(stickmanIndex).GetComponentInChildren<SkinnedMeshRenderer>();
        Material gunStickmanMaterial = gun.GetChild(stickmanIndex).GetComponentInChildren<SkinnedMeshRenderer>().material;

        stickmanMeshRenderer.material = gunStickmanMaterial;
    }

    private void SetPosition(Transform gun, bool isGunPositionSet, int stickmanIndex)
    {
        if (isGunPositionSet == true)
        {
            Transform stickmanTransform = transform.GetChild(stickmanIndex);
            Vector3 gunStickmanPosition = gun.GetChild(stickmanIndex).position;

            stickmanTransform.position = Vector3.Lerp(stickmanTransform.position, gunStickmanPosition, Time.deltaTime * positionSettingSpeed);
        }       
    }

    private void SetRotation(Transform gun, bool isGunRotationSet, int stickmanIndex)
    {
        if (isGunRotationSet == true)
        {
            Transform stickmanTransform = transform.GetChild(stickmanIndex);
            Quaternion gunStickmanRotation = gun.GetChild(stickmanIndex).rotation;

            stickmanTransform.rotation = Quaternion.Lerp(stickmanTransform.rotation, gunStickmanRotation, Time.deltaTime * rotationSettingSpeed);
        }      
    }

    private void ChangeGun(Transform oldGun, Transform newGun)
    {
        for(int i = 0; i < transform.childCount - 1; i++)
        {
            SetAnimation(oldGun, i, false);
        }

        for(int j = 0; j < transform.childCount; j++)
        {            
            SetAnimation(newGun, j, true);
            SetMaterial(newGun, j);
        }

        isGunChange = true;
    }

    private void SetAllPositionsAndRotations(Transform gun, bool isGunPositionSet, bool isGunRotationSet)
    {
        if (isGunChange == true)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                SetPosition(gun, isGunPositionSet, i);
                SetRotation(gun, isGunRotationSet, i);
            }
        }
    }

    private void ReduceStickman()
    {
        var lastStickman = transform.GetChild(transform.childCount - 1).gameObject;
        Destroy(lastStickman);
    }

    private void GetFirstStickman()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        Instantiate(playerStickmanPrefab, transform.position, Quaternion.identity);
    }
}
