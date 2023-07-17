using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class IKControl : MonoBehaviour
{
    protected static Animator animator;

    public static IKControl Control;

    //List<GameObject> disableOnFinish = new List<GameObject>();

    RaycastHit hit;

    public static string currentTaskTag;
    string handle = "Handle";
    string[] fruits;

    public bool ikActive = false;
    public static bool isGrab = false;
    public bool isNear = false;

    public static int currentTaskNumber;
    //int currentNumber = 0;
    int min = 1;
    int max = 6;

    public Transform rightHandObj = null;
    public Transform leftHandObj = null;
    public Transform lookObj = null;
    public Transform dropPoint = null;

    Vector3 oldPosition;
    Vector3 newPosition;

    public float middleDistance = 0.5f;
    public float nearDistance = 0.1f;
    //float smooth = 0;
    [Range(0,1)]
    public float speed = 0;
    [Range(0, 1)]
    public float headSpeed = 0;
    [Range(0, 1)]
    public float rigthHandRotationWeight = 0;

    public float lerp_speed=0.5f;
    public float lerp=0;
    public float height=0.2f;
    bool isOld = true;

    public delegate void DropInBasket();
    public static event DropInBasket onDropInBasket;



    
    void Start()
    {
        animator = GetComponent<Animator>();
        if (Control==null)
        {
            Control = this;
        }
        /*fruits = System.Enum.GetNames(typeof(Enums.Fruits));
        currentTaskNumber = Random.Range(min, max);
        currentTaskTag=fruits[Random.Range(0, fruits.Length - 1)];
        UIManager.UpdateTask(currentTaskNumber,currentTaskTag);*/
       
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
                    newPosition = Vector3.Lerp(/*rightHandObj.parent.position*/oldPosition, dropPoint.position, lerp);//save old position and calc from its
                    if (lerp < 1)
                    {

                        newPosition.y+= Mathf.Sin(lerp * Mathf.PI)*height;
                    }
                    //Debug.Log(newPosition.y) ;
                    rightHandObj.parent.position = newPosition;
                    //rightHandObj.parent.Rotate
                }
            }
        }
        if (Input.GetMouseButtonDown(0)&&!isGrab)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit)){
                //if(hit.transform.CompareTag(currentTaskTag))
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
                    //Debug.Log("HeadSpeed"+headSpeed);
                    animator.SetLookAtPosition(lookObj.position);
                }
                else
                {
                    animator.SetBool("LookAt Bool", false);
                }

                // Set the right hand target position and rotation, if one has been assigned
                if (rightHandObj != null)
                {
                    /*animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                    */
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, speed);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0.5f);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position); //Vector3.Lerp(animator.GetIKPosition(AvatarIKGoal.RightHand), rightHandObj.position,speed*Time.deltaTime));
                    animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
                    //smooth += Mathf.Clamp(speed * Time.deltaTime,0,1);
                }
                /*if (leftHandObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);

                    animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position); 
                    animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);
                }*/

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
        //fruit.GetComponent<Collider>().enabled = false;
        //fruit = animator.GetBoneTransform(HumanBodyBones.RightHand);
        oldPosition = fruit.position;
        isGrab = true;
        //animator.SetTrigger("Drop");
    }
    
    public /*static*/ void Drop(Transform fruit)
    {

        fruit.GetComponent<Rigidbody>().isKinematic = false;

        //fruit.GetComponent<Collider>().enabled = true;
        //rightHandObj = null;
        //lookObj = null;
        isOld = true;
        isGrab = false;
        isNear = false;
        //Debug.Log("Idle");
        animator.SetBool("LookAt Bool", false);
        animator.SetTrigger("Idle");
        if (fruit.CompareTag(ScoreManager.GetCurrentTaskTag()))
        {
            onDropInBasket?.Invoke();
            /*ReachBasket.PopUp();
            AddScore();*/
        }
    }

    /*void AddScore()
    {
        currentNumber++;
        if (currentNumber >= currentTaskNumber)
        {
            FinishGame();
        }
        UIManager.UpdateTask(currentTaskNumber - currentNumber, currentTaskTag);
    }
    */
    void FinishGame()
    {
        rightHandObj = null;
        lookObj = null;
        Camera.main.GetComponent<Animator>().SetTrigger("Dance");
        animator.SetTrigger("Dance");
        
    }
    /*
    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }*/
}
