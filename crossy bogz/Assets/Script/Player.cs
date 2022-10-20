using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] TMP_Text stepText;
    [SerializeField] ParticleSystem dieParticles;
    [SerializeField, Range(0.01f,1f)] float moveDuration= 0.2f;
    [SerializeField, Range(0.01f, 1f)] float jumpHeight = 0.5f;
    private int minZPos;
    private int extent;
    
    private float backBoundary;
    private float leftBoundary;
    private float rightBoundary;

    [SerializeField] private int maxTravel;
    public int MaxTravel { get => maxTravel; }
    [SerializeField] private int currentTravel;
    private int CurrentTravel { get => maxTravel; }
    public bool IsDie { get => this.enabled == false;  }
    public void SetUp(int minZPos, int extent)
    {
        backBoundary = minZPos - 1;
        leftBoundary = -(extent + 1);
        rightBoundary = extent + 1;

    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.UpArrow))
        //    Debug.Log("forward");
        //if(Input.GetKeyDown(KeyCode.DownArrow))
        //    Debug.Log("back");

        var moveDir = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.UpArrow))
            moveDir += new Vector3(0,0, 1);
        if (Input.GetKeyDown(KeyCode.DownArrow))
            moveDir += new Vector3(0, 0, -1);
        if (Input.GetKeyDown(KeyCode.RightArrow))
            moveDir += new Vector3(1, 0, 0);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            moveDir += new Vector3(-1, 0, 0);

        if (moveDir == Vector3.zero)
            return;
        
        if(IsJumping()==false)
        Jump(moveDir);
    }

    private void Jump(Vector3 targetDirection)
    {

        // atur rotasi
        var targetPosition = transform.position + targetDirection;
        transform.LookAt(targetPosition);

        // loncat ke atas
        var moveSeq = DOTween.Sequence(transform);
        moveSeq.Append(transform.DOMoveY(jumpHeight, moveDuration/2));
        moveSeq.Append(transform.DOMoveY(0, moveDuration/2));

        if (Tree.AllPositions.Contains(targetPosition))
            return;

        if (targetPosition.z <= backBoundary ||
            targetPosition.x <= leftBoundary ||
            targetPosition.x >= rightBoundary)
            return; 

        //transform.DOMoveY(2f, 0.1f)
        //    .OnComplete(() => transform.DOMoveY(0, 0.1f)); 
        //transform.DOMoveY(0.5f, 0.1f);
        //transform.DOMoveY(0.5f, 0.1f);

        // gerak maju mundur samping 
        transform.DOMoveX(targetPosition.x, moveDuration);
        transform        
                .DOMoveZ(targetPosition.z, moveDuration)
                .OnComplete(UpdateTravel);
    }

    private void UpdateTravel()
    {
        currentTravel = (int) this.transform.position.z;

        if(currentTravel > maxTravel)
            maxTravel = currentTravel;

        stepText.text = "STEP : " + maxTravel.ToString();
        
    }
    private bool IsJumping()
    {
        return DOTween.IsTweening(transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.enabled == false)
            return;
        // execute sekali pada frame saat nyenggol pertama x

        var car = other.GetComponent<Car>();
        if(car != null)
        {
            AnimateCrash(car);
        }
        

        //if(other.tag == "Car")
        //{
        //    // AnimateDie();
        //}
    }

    private void AnimateCrash(Car car)
    {
        //var isRight = car.transform.rotation.y == 90;

        //transform.DOMoveX(isRight ? 8 : -8, 1f);
        //transform
        //    .DORotate(Vector3.forward*360, 0.2f)
        //    .SetLoops(100, LoopType.Restart);
        // gepeng
        transform.DOScaleY(0.1f, 0.2f);
        transform.DOScaleX(1, 0.2f);
        transform.DOScaleZ(1, 0.2f);

        this.enabled = false;
        dieParticles.Play();
    }
}
