using UnityEngine;

[ExecuteInEditMode]
public class AttachmentPointGizmo : MonoBehaviour
{
    public Transform hatAttachmentPoint;
    public Transform weaponAttachmentPoint;
    public float gizmoSize = 0.2f;

    private void OnDrawGizmos()
    {
        if (hatAttachmentPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(hatAttachmentPoint.position, gizmoSize);
            Gizmos.DrawLine(transform.position, hatAttachmentPoint.position);
        }

        if (weaponAttachmentPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(weaponAttachmentPoint.position, gizmoSize);
            Gizmos.DrawLine(transform.position, weaponAttachmentPoint.position);
        }
    }
}