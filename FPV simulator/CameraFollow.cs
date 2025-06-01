using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Цель, за которой будет следовать камера (дрон)
    public Vector3 offset;   // Смещение камеры относительно цели
    public float smoothSpeed = 0.125f; // Скорость сглаживания движения

    void LateUpdate()
    {
        if (target == null)
            return;

        // Вычисление желаемой позиции камеры с учетом смещения
        Vector3 desiredPosition = target.position + target.rotation * offset;
        // Сглаживание движения камеры
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Поворот камеры для слежения за целью
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, smoothSpeed);
    }
}
