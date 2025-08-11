using System.Collections;
using UnityEngine;

public class GoodsMove : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Vector3 m_DestPosition;
    private Transform m_Transform;
    private RectTransform m_RectTransform;

    public void SetMove(int idx, Vector3 destPosition)
    {
        m_Transform = transform;
        m_RectTransform = GetComponent<RectTransform>();
        m_DestPosition = new Vector3(destPosition.x, destPosition.y, 0);

        StartCoroutine(MoveCoroutine(idx));
    }

    private IEnumerator MoveCoroutine(int idx)
    {
        yield return new WaitForSeconds(0.1f + 0.08f * idx);

        while(m_Transform.position.y < m_DestPosition.y)
        {
            m_Transform.position = Vector2.MoveTowards(m_Transform.position, m_DestPosition, moveSpeed * Time.deltaTime);
            var rectLocalPosition = m_RectTransform.localPosition;
            m_RectTransform.localPosition = new Vector3(rectLocalPosition.x, rectLocalPosition.y, 0f);
            yield return null;
        }

        Destroy(gameObject);
    }
}
