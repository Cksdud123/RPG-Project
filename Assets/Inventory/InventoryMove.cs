using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryMove : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField]
    private Transform _targetTr; // 이동될 UI

    private Vector2 _beginPoint;
    private Vector2 _moveBegin;

    private void Awake()
    {
        // 이동 대상 UI를 지정하지 않은 경우, 자동으로 부모로 초기화
        if (_targetTr == null)
            _targetTr = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _beginPoint = _targetTr.position;

        _moveBegin = eventData.position;

    }

    public void OnDrag(PointerEventData eventData)
    {
        _targetTr.position = _beginPoint + (eventData.position - _moveBegin);
    }
}
