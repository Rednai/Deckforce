using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RelatedPos { UP, DOWN, LEFT, RIGHT };
public enum OutlineType { MOVE, RANGE, EFFECT };

public class Tile : MonoBehaviour
{
    public Entity tileEntity;
    public MeshRenderer floorMeshRenderer;
    public Vector2 tilePosition;
    private Animator animator;
    public Transform tileFloor;

    // Start is called before the first frame update
    void Awake()
    {
        tilePosition = new Vector2(transform.position.x, transform.position.z);
        gameObject.name = $"Case x:{tilePosition.x} y:{tilePosition.y}";
        animator = GetComponent<Animator>();
    }

    void Update() {
        // C'est moche, il faudrait implémenter un pivot
        if (tileEntity != null) {
            Vector3 entityPosition = tileEntity.transform.position;
            entityPosition.y = tileFloor.position.y + 0.5f;
            tileEntity.transform.position = entityPosition;
        }
    }

    public void StartAnimation() {
        animator.SetBool("Selected", false);
        animator.SetBool("Selected", true);
    }

    public void StopAnimation() {
        animator.SetBool("Selected", false);
    }
    
    public void SetOutline(OutlineType outlineType) {
        floorMeshRenderer.material.SetFloat("_Outline", 0.1f);
        switch (outlineType) {
            case OutlineType.MOVE:
                floorMeshRenderer.material.SetColor("_OutlineColor", Color.red);
                break;
            case OutlineType.RANGE:
                floorMeshRenderer.material.SetColor("_OutlineColor", Color.green);
                break;
            case OutlineType.EFFECT:
                floorMeshRenderer.material.SetColor("_OutlineColor", Color.blue);
                break;
        }
    }

    public void StopOutline() {
        floorMeshRenderer.material.SetFloat("_Outline", 0f);
    }

    public Tile GetRelatedPos(RelatedPos relatedPos) {
        GameObject foundedObject = null;

        switch (relatedPos) {
            case RelatedPos.UP:
                foundedObject = GameObject.Find($"Case x:{tilePosition.x} y:{tilePosition.y + 1}");
                break;
            case RelatedPos.DOWN:
                foundedObject = GameObject.Find($"Case x:{tilePosition.x} y:{tilePosition.y - 1}");
                break;
            case RelatedPos.LEFT:
                foundedObject = GameObject.Find($"Case x:{tilePosition.x + 1} y:{tilePosition.y}");
                break;
            case RelatedPos.RIGHT:
                foundedObject = GameObject.Find($"Case x:{tilePosition.x - 1} y:{tilePosition.y}");
                break;
        }
        if (foundedObject != null)
            return foundedObject.GetComponent<Tile>();
        return null;
    }
}
