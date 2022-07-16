using UnityEngine;

public class CylinderCollider : MonoBehaviour
{
    public bool isTrigger = false;
    public PhysicMaterial material;
    public Vector3 center;
    public float radius = 1;
    public float height = 2;
    [Min(3)]
    public int boxCount = 3;
    public Orientation orientation = Orientation.Y_axis;
    public enum Orientation { X_axis, Y_axis, Z_axis }

    public void BuildCollider()
    {
        ClearCollider();

        float anglePerBox = 180f / boxCount;
        int repeats = 0;

        Quaternion orientationRot = Quaternion.Euler(0, 0, 0);
        if(orientation == Orientation.X_axis) 
        {
            orientationRot = Quaternion.AngleAxis(90f, Vector3.forward);
        }
        if(orientation == Orientation.Z_axis) 
        {
            orientationRot = Quaternion.AngleAxis(90f, Vector3.left);
        }

        for (float angle = 0f; angle < 180f; angle += anglePerBox, repeats++)
        {
            GameObject box = new GameObject($"box{angle}");
            box.transform.position = transform.position + center;
            box.transform.parent = transform;
            box.transform.rotation = transform.rotation * orientationRot * Quaternion.AngleAxis(angle, Vector3.up);
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
            float sinLeft = Mathf.Sin((a - (anglePerBox / 2f)) * Mathf.Deg2Rad) * radius;
            float sinRight = Mathf.Sin((a + (anglePerBox / 2f)) * Mathf.Deg2Rad) * radius;
            float cosLeft = Mathf.Cos((a - (anglePerBox / 2f)) * Mathf.Deg2Rad) * radius;
            float cosRight = Mathf.Cos((a + (anglePerBox / 2f)) * Mathf.Deg2Rad) * radius;

            Vector3 boxHeight = Vector3.up * height / 2f;
            Vector3 pointLeft = new Vector3(sinLeft, 0, cosLeft);
            Vector3 pointRight = new Vector3(sinRight, 0, cosRight);
            if (orientation == Orientation.X_axis)
            {
                pointLeft = new Vector3(0, sinLeft, cosLeft);
                pointRight = new Vector3(0, sinRight, cosRight);
                boxHeight = Vector3.left * height / 2f;
            }
            if (orientation == Orientation.Z_axis)
            {
                pointLeft = new Vector3(sinLeft, cosLeft, 0);
                pointRight = new Vector3(sinRight, cosRight, 0);
                boxHeight = Vector3.forward * height / 2f;
            }

            Gizmos.DrawLine(transform.position + boxHeight + pointLeft + center, transform.position + boxHeight + pointRight + center);
            Gizmos.DrawLine(transform.position - boxHeight + pointLeft + center, transform.position - boxHeight + pointRight + center);
            Gizmos.DrawLine(transform.position + boxHeight + pointLeft + center, transform.position + boxHeight - pointRight + center);
            Gizmos.DrawLine(transform.position - boxHeight + pointLeft + center, transform.position - boxHeight - pointRight + center);
            Gizmos.DrawLine(transform.position + boxHeight + pointLeft + center, transform.position - boxHeight + pointLeft + center);

            if (repeats > boxCount * 2f)
            {
                Debug.Log("to much repeats");
                break;
            }
        }
    }
}
