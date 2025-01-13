using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    public Camera mainCamera; // 메인 카메라
    public Transform playerBody; // 캐릭터의 Transform
    public float rotationSpeed = 5f; // 회전 속도
    public LayerMask groundLayer; // 레이캐스트가 충돌할 레이어
    public Vector3 modelForwardDirection = Vector3.forward; // 모델의 기본 Forward 방향

    private void Update()
    {
        AimTowardsMouse();
    }

    private void AimTowardsMouse()
    {
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera가 설정되지 않았습니다!");
            return;
        }

        // 마우스 위치 가져오기
        Vector3 mousePosition = Input.mousePosition;

        // 레이캐스트를 위한 마우스 위치 -> 레이 생성
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        // 레이캐스트 수행 (땅과의 충돌 확인)
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            // 히트된 위치
            Vector3 hitPoint = hit.point;

            // 방향 계산 (캐릭터와 히트 지점 사이의 벡터)
            Vector3 direction = (hitPoint - playerBody.position).normalized;

            // 높이 제거 (2D 스타일 평면 회전)
            direction.y = 0;

            // 대상 회전 계산
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // 모델 기본 방향을 고려한 보정
            targetRotation *= Quaternion.FromToRotation(Vector3.forward, modelForwardDirection);

            // 부드럽게 회전
            playerBody.rotation = Quaternion.Slerp(playerBody.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("레이캐스트가 아무것도 맞추지 않았습니다!");
        }
    }
}
