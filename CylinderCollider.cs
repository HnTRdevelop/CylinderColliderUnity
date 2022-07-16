using UnityEngine;

public class CylinderCollider : MonoBehaviour
{
    public float radius = 1;
    public float height = 2;
    public bool isTrigger = false;
    [Min(3)]
    public int boxCount = 3;
    public PhysicMaterial material;

    public void BuildCollider()
    {
        ClearCollider();

        float anglePerBox = 180f / boxCount;
        int repeats = 0;
        for (float angle = 0f; angle < 180f; angle += anglePerBox, repeats++)
        {
            GameObject box = new GameObject($"box{angle}");
            box.transform.position = transform.position;
            box.transform.parent = transform;
            box.transform.rotation = transform.rotation * Quaternion.AngleAxis(angle, Vector3.up);
            box.layer = transform.gameObject.layer;
            BoxCollider col = box.AddComponent<BoxCollider>();
            col.material = material;
            col.isTrigger = isTrigger;
            float sinLeft = Mathf.Sin((angle - (anglePerBox / 2f)) * Mathf.Deg2Rad) * radius;
            float sinRight = Mathf.Sin((angle + (anglePerBox / 2f)) * Mathf.Deg2Rad) * radius;
            float cosLeft = Mathf.Cos((angle - (anglePerBox / 2f)) * Mathf.Deg2Rad) * radius;
            float cosRight = Mathf.Cos((angle + (anglePerBox / 2f)) * Mathf.Deg2Rad) * radius;
            Vector3 pointLeft = new Vector3(sinLeft, 0, cosLeft);
            Vector3 pointRight = new Vector3(sinRight, 0, cosRight);
            float boxThikness = Vector3.Distance(pointLeft, pointRight);
            float boxRadius = Vector3.Distance(pointLeft, -pointRight);
            Vector3 size = new Vector3(boxThikness, height, boxRadius);
            col.size = size;

            if (repeats > boxCount)
            {
                Debug.Log("to much repeats");
                break;
            }
        }
    }

    public void ClearCollider()
    {
        if (transform.childCount > 0)
        {
            while (transform.childCount != 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    DestroyImmediate(transform.GetChild(0).gameObject);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        float anglePerBox = 180f / boxCount;
        int repeats = 0;
        for (float a = 0f; a < 360f; a += anglePerBox, repeats++)
        {
            Vector3 boxHeight = Vector3.up * height / 2f;
            float sinLeft = Mathf.Sin((a - (anglePerBox / 2f)) * Mathf.Deg2Rad) * radius;
            float sinRight = Mathf.Sin((a + (anglePerBox / 2f)) * Mathf.Deg2Rad) * radius;
            float cosLeft = Mathf.Cos((a - (anglePerBox / 2f)) * Mathf.Deg2Rad) * radius;
            float cosRight = Mathf.Cos((a + (anglePerBox / 2f)) * Mathf.Deg2Rad) * radius;
            Vector3 pointLeft = new Vector3(sinLeft, 0, cosLeft);
            Vector3 pointRight = new Vector3(sinRight, 0, cosRight);

            Gizmos.DrawLine(transform.position + boxHeight + pointLeft, transform.position + boxHeight + pointRight);
            Gizmos.DrawLine(transform.position - boxHeight + pointLeft, transform.position - boxHeight + pointRight);
            Gizmos.DrawLine(transform.position + boxHeight + pointLeft, transform.position + boxHeight - pointRight);
            Gizmos.DrawLine(transform.position - boxHeight + pointLeft, transform.position - boxHeight - pointRight);
            Gizmos.DrawLine(transform.position + boxHeight + pointLeft, transform.position - boxHeight + pointLeft);

            if (repeats > boxCount * 2f)
            {
                Debug.Log("to much repeats");
                break;
            }
        }
    }
}
