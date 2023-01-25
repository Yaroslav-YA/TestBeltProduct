using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IKControl : MonoBehaviour
{
    protected static Animator animator;

    public static IKControl Control;

    RaycastHit hit;

    public static string currentTaskTag;
    string handle = "Handle";
    string[] fruits;

    public bool ikActive = false;
    public static bool isGrab = false;
    public bool isNear = false;

    public static int currentTaskNumber;
    int currentNumber = 0;
    int min = 1;
    int max = 6;

    public Transform rightHandObj = null;
    public Transform leftHandObj = null;
    public Transform lookObj = null;
    public Transform dropPoint = null;

    public float middleDistance = 0.5f;
    public float nearDistance = 1;
    //float smooth = 0;
    [Range(0,1)]
    public float speed = 0.1f;
    

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (Control==null)
        {
            Control = this;
        }
        fruits = System.Enum.GetNames(typeof(UIManager.Fruits));
        currentTaskNumber = Random.Range(min, max);
        currentTaskTag=fruits[Random.Range(0, fruits.Length - 1)];
        UIManager.UpdateTask(currentTaskNumber,currentTaskTag);
    }

    private void Update()
    {
        if (rightHandObj != null)
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
                if ((rightHandObj.parent.position - dropPoint.position).sqrMagnitude < nearDistance * nearDistance)
                {
                    Drop(rightHandObj.parent);
                }
                else
                {
                    rightHandObj.parent.position = Vector3.Lerp(rightHandObj.parent.position, dropPoint.position, speed * Time.deltaTime);
                }
            }
        }
        if (Input.GetMouseButtonDown(0)&&!isGrab)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit)){
                //if(hit.transform.CompareTag(currentTaskTag))
                rightHandObj = hit.transform.Find(handle);
                lookObj = hit.transform;
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
                    animator.SetLookAtWeight(1);
                    animator.SetLookAtPosition(lookObj.position);
                }

                // Set the right hand target position and rotation, if one has been assigned
                if (rightHandObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);

                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, speed);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, speed);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position); //Vector3.Lerp(animator.GetIKPosition(AvatarIKGoal.RightHand), rightHandObj.position,speed*Time.deltaTime));
                    animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
                    //smooth += Mathf.Clamp(speed * Time.deltaTime,0,1);
                }
                if (leftHandObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);

                    animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position); 
                    animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);
                }

            }

            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            else
            {
                //smooth = 0;
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                animator.SetLookAtWeight(0);
            }
        }
    }
    public bool IsObjectNear(float distance)
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
    public void Near()
    {
        isNear = true;
        animator.SetTrigger("Near");
    }

    public void Grab(Transform fruit)
    {
        //rightHandObj.parent.GetComponent<Rigidbody>().isKinematic = true;
        //rightHandObj.parent = animator.GetBoneTransform(HumanBodyBones.RightHand);
        // if () ;
        fruit.GetComponent<Rigidbody>().isKinematic = true;
        //fruit = animator.GetBoneTransform(HumanBodyBones.RightHand);
        isGrab = true;
        animator.SetTrigger("Drop");
    }
    
    public /*static*/ void Drop(Transform fruit)
    {
        fruit.GetComponent<Rigidbody>().isKinematic = false;
        rightHandObj = null;
        isGrab = false;
        isNear = false;
        animator.SetTrigger("Idle");
        if (fruit.CompareTag(currentTaskTag))
        {
            ReachBasket.PopUp();
            AddScore();
        }
    }

    void AddScore()
    {
        currentNumber++;
        if (currentNumber >= currentTaskNumber)
        {
            //Finish
        }
        UIManager.UpdateTask(currentTaskNumber - currentNumber, currentTaskTag);
    }
}
