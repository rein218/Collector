using UnityEngine;

public class TableBorders : MonoBehaviour
{
    [SerializeField] private Collider2D m_Collider;
    public static Vector3 position { get; private set; } = Vector3.zero;
    public static float leftBorder { get; private set; } = -4.0f;
    public static float rightBorder { get; private set; } = 4.0f;
    public static float topBorder { get; private set; } = 4.0f;
    public static float bottomBorder { get; private set; } = -4.0f;



    public void Awake()
    {
        position = transform.position;
        leftBorder = m_Collider.bounds.min.x;
        bottomBorder = m_Collider.bounds.min.y;

        rightBorder = m_Collider.bounds.max.x;
        topBorder = m_Collider.bounds.max.y;
    }

}
