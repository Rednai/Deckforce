using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RelatedPos { UP, DOWN, LEFT, RIGHT, UP_RIGHT, UP_LEFT, DOWN_RIGHT, DOWN_LEFT};
public enum OutlineType { MOVE, RANGE, EFFECT };

public class Tile : MonoBehaviour
{
    public Entity tileEntity;
    public MeshRenderer floorMeshRenderer;
    public Vector2 tilePosition;
    private Animator animator;
    float heightDifference;

    // Start is called before the first frame update
    void Start()
    {
        tilePosition = new Vector2(transform.position.x, transform.position.z);
        gameObject.name = $"Case x:{tilePosition.x} y:{tilePosition.y}";
        animator = GetComponent<Animator>();
    }

    void Update() {
        // if (tileEntity != null) {
        //     Vector3 currentPosition = tileEntity.transform.position;
        //     currentPosition.y = floorMeshRenderer.transform.position.y + heightDifference;
        //     tileEntity.transform.position = currentPosition;
        // }
    }

    public void SetEntity(Entity entity) {
        tileEntity = entity;
        heightDifference = entity.transform.position.y - transform.position.y;
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
            case OutlineType.EFFECT:
                floorMeshRenderer.material.SetColor("_OutlineColor", Color.red);
                break;
            case OutlineType.RANGE:
                floorMeshRenderer.material.SetColor("_OutlineColor", Color.green);
                break;
            case OutlineType.MOVE:
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
            case RelatedPos.UP_LEFT:
                foundedObject = GameObject.Find($"Case x:{tilePosition.x + 1} y:{tilePosition.y + 1}");
                break;
            case RelatedPos.UP_RIGHT:
                foundedObject = GameObject.Find($"Case x:{tilePosition.x - 1} y:{tilePosition.y + 1}");
                break;
            case RelatedPos.DOWN_LEFT:
                foundedObject = GameObject.Find($"Case x:{tilePosition.x + 1} y:{tilePosition.y - 1}");
                break;
            case RelatedPos.DOWN_RIGHT:
                foundedObject = GameObject.Find($"Case x:{tilePosition.x - 1} y:{tilePosition.y - 1}");
                break;
        }
        if (foundedObject != null)
            return foundedObject.GetComponent<Tile>();
        return null;
    }
}
