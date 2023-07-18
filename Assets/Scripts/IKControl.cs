using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class IKControl : MonoBehaviour
{
    protected static Animator animator;

    RaycastHit hit;

    string handle = "Handle";

    [SerializeField] bool ikActive = true;
    bool isGrab = false;
    bool isNear = false;

    [SerializeField] Transform dropPoint = null;
    public Transform rightHandObj = null;
    public Transform lookObj = null;

    Vector3 oldPosition;
    Vector3 newPosition;

    [SerializeField] float middleDistance = 0.69f;
    [SerializeField] float nearDistance = 0.05f;
    [Range(0,1)]
    public float speed = 0;
    [Range(0, 1)]
    public float headSpeed = 0;
    [Range(0, 1)]
    [SerializeField] float rigthHandRotationWeight = 0.5f;

    [SerializeField] float lerp_speed=0.5f;
    float lerp=0;
    [SerializeField] float height=0.2f;
    bool isOld = true;

    public delegate void DropInBasket();
    public static event DropInBasket onDropInBasket;
        
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        ScoreManager.onTaskComplete += FinishGame;
    }

    private void OnDisable()
    {
        ScoreManager.onTaskComplete -= FinishGame;
    }

    private void Update()
    {
        if (rightHandObj != null&&!isOld)
        {
            if (!isGrab)
            {
                if (IsObjectNear(nearDistance))
                {
                    Grab(rightHandObj.parent);
                }
                else if (IsObjectNear(middleDistance) && !isNear)
                {
                    Near();
                }
            }
            else
            {
                if ((rightHandObj.parent.position - dropPoint.position).sqrMagnitude < nearDistance * nearDistance/2)
                {
                    Drop(rightHandObj.parent);
                    lerp = 0;
                }
                else
                {
                    lerp += lerp_speed * Time.deltaTime;
                    newPosition = Vector3.Lerp(oldPosition, dropPoint.position, lerp);//save old position and calc from its
                    if (lerp < 1)
                    {

                        newPosition.y+= Mathf.Sin(lerp * Mathf.PI)*height;
                    }
                    rightHandObj.parent.position = newPosition;
                }
            }
        }
        if (Input.GetMouseButtonDown(0)&&!isGrab)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit)){
                rightHandObj = hit.transform.Find(handle);
                lookObj = hit.transform;
                isOld = false;
                animator.SetBool("LookAt Bool", true);
            }
        }
    }

   

    //a callback for calculating IK
    void OnAnimatorIK()
    {
        if (animator)
        {

            //if the IK is active, set the position and rotation directly to the goal.
            if (ikActive)
            {

                // Set the look target position, if one has been assigned
                if (lookObj != null)
                {
                    animator.SetLookAtWeight(headSpeed);
                    animator.SetLookAtPosition(lookObj.position);
                }
                else
                {
                    animator.SetBool("LookAt Bool", false);
                }

                // Set the right hand target position and rotation, if one has been assigned
                if (rightHandObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, speed);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rigthHandRotationWeight);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position); 
                    animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
                    
                }

            }
            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            else
            {                
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                animator.SetLookAtWeight(0);
            }
        }
    }
    bool IsObjectNear(float distance)
    {
        if ((animator.GetBoneTransform(HumanBodyBones.RightHand).position - rightHandObj.position).sqrMagnitude < distance*distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void Near()
    {
        isNear = true;
        animator.SetTrigger("Near");
    }

    void Grab(Transform fruit)
    {
        fruit.GetComponent<Rigidbody>().isKinematic = true;
        oldPosition = fruit.position;
        isGrab = true;
        animator.SetTrigger("Grab");
    }
    
    void Drop(Transform fruit)
    {

        fruit.GetComponent<Rigidbody>().isKinematic = false;
        isOld = true;
        isGrab = false;
        isNear = false;
        animator.SetBool("LookAt Bool", false);
        animator.SetTrigger("Idle");
        if (fruit.CompareTag(ScoreManager.GetCurrentTaskTag()))
        {
            onDropInBasket?.Invoke();
        }
    }
    void FinishGame()
    {
        rightHandObj = null;
        lookObj = null;
        Camera.main.GetComponent<Animator>().SetTrigger("Dance");
        animator.SetTrigger("Dance");
        
    }
}
