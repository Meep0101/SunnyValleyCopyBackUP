using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGathererUnit : MonoBehaviour, IUnit {
    
    private enum State {
        Idle,
        Moving
        //Animating,
    }

    private const float speed = 1f;

    private Vector3 targetPosition;
    private float stopDistance;
    private Action onArrivedAtPosition;
    private State state;

    private void Update() {
        switch (state) {
        case State.Idle:
            //animatedWalker.SetMoveVector(Vector3.zero);
            break;
        case State.Moving:
            HandleMovement();
            break;
        }
    }

    private void HandleMovement() { //Logic of CarAI
    #region 2D movement
        // if (Vector3.Distance(transform.position, targetPosition) > stopDistance) {
        //     Vector3 moveDir = (targetPosition - transform.position).normalized;

        //     float distanceBefore = Vector3.Distance(transform.position, targetPosition);
        //     animatedWalker.SetMoveVector(moveDir);
        //     transform.position = transform.position + moveDir * speed * Time.deltaTime;
        // } else {
        //     // Arrived
        //     animatedWalker.SetMoveVector(Vector3.zero);
        //     if (onArrivedAtPosition != null) {
        //         Action tmpAction = onArrivedAtPosition;
        //         onArrivedAtPosition = null;
        //         state = State.Idle;
        //         tmpAction();
        //     }
        // }
    #endregion
        
        //Possible dito ang MARKERS
        if (Vector3.Distance(transform.position, targetPosition) > stopDistance)
        {
            Vector3 moveDir = (targetPosition - transform.position).normalized;
            transform.position += moveDir * speed * Time.deltaTime;
        }
        else
        {
            // Arrived
            if (onArrivedAtPosition != null)
            {
                Action tmpAction = onArrivedAtPosition;
                onArrivedAtPosition = null;
                state = State.Idle;
                tmpAction();
            }
        }
    }

    public void SetTargetPosition(Vector3 targetPosition) {
        targetPosition.y = 0f; // change from z since 3D
        this.targetPosition = targetPosition;
    }

    public bool IsIdle() {
        return state == State.Idle;
    }

    public void MoveTo(Vector3 position, float stopDistance, Action onArrivedAtPosition) {
        SetTargetPosition(position);
        this.stopDistance = stopDistance;
        this.onArrivedAtPosition = onArrivedAtPosition;
        state = State.Moving;
    }
}