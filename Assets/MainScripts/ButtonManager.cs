using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonManager : MonoBehaviour
{
    public float speed = 2.0f;
    private float startTime;
    public Button btnLyra;
    private Transform startPos, endPos;
    public Transform[] currentPos;
    float distanceMake;
    int currentPosIndex =0;
    public Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        currentPosIndex = 0;
        //SetPoints();
        SubscribeToButtons();
        anim = GetComponent<Animator>();
        anim.SetBool("isWalking",false);
    }
    void SubscribeToButtons()
    {
        btnLyra.onClick.AddListener(()=>StartCoroutine(MoveTargetToPosition(currentPos,speed)));
    }
    // Update is called once per frame
    void SetPoints()
    {
        startPos = currentPos[currentPosIndex];
        endPos = currentPos[currentPosIndex + 1];
        startTime = Time.time;
        distanceMake = Vector3.Distance(startPos.position, endPos.position);
        
       
    }

    IEnumerator MoveTargetToPosition(Transform[] pos,float duration)
    {
        SetPoints();
        /*float distCovered = (Time.time - startTime) * speed;
        float journey = distCovered / distanceMake;*/

        float time = 0;
        while (time <duration)
        {
            transform.position = Vector2.Lerp(startPos.position, endPos.position, time/duration);
            time += Time.deltaTime;
            anim.SetBool("isWalking", true);
            Debug.Log("here "+currentPosIndex);
            yield return null;
        }
        

        if (currentPosIndex < pos.Length - 2)
        {
            currentPosIndex++;
            //SetPoints();
            transform.position = pos[currentPosIndex].position;
            anim.SetBool("isWalking", false);
            Debug.Log("there " + currentPosIndex);
        }
        
        if(currentPosIndex == pos.Length - 2)
        {
            anim.SetBool("isWalking", false);
            Debug.Log("here put something else, a trigger maybe");
            yield return null;
        }

        /*if (pos[currentPosIndex-1] == endPos*//*pos.Length==5*//*)
        {
            yield break;
        }*/


    }

    
}
