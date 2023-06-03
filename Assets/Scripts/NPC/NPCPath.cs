using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCPath
{
    [SerializeField] List<GameObject> nodes;
    [SerializeField] NPCPathType pathType;
    private int nodeIndex = 0;
    private GameObject currentNode;
    public List<GameObject> Nodes => nodes;
    public int NodeIndex => nodeIndex;
    public GameObject CurrentNode => currentNode;

    public void Initialize() {
        nodeIndex = 0;
        currentNode = nodes[nodeIndex];
    }

    public void InitializeStatic() {
        nodeIndex = 1;
        currentNode = nodes[0];
    }

    public void NextNodeIndex() {
        nodeIndex++;
        GetCorrectNextIndex();
    }

    public void GetCorrectNextIndex() {
        if (pathType == NPCPathType.CIRCULAR_LOOPING) {
            nodeIndex = nodeIndex % nodes.Count;
        }
        else if(pathType == NPCPathType.REVERSE_LOOPING) {
            if (nodeIndex == nodes.Count - 1) {
                nodeIndex = 0;
                nodes.Reverse();
            }
        }
        else if(pathType == NPCPathType.SPINNING) {
            nodeIndex = nodeIndex % nodes.Count;
            if (nodeIndex == 0) {
                nodeIndex = 1;
            }
        }
    }

    private float ApproximateValue(float value) {
        if (value >= 0.5f) {
            return 1.0f;
        } else if((value < 0.5f && value > 0.0f) || (value > -0.5f && value < 0.0f)) {
            return 0.0f;
        } else if(value < -0.5f) {
            return -1.0f;
        }
        return value;
    }

    private Vector2 ComputeCurrentDirection() {
        return (nodes[nodeIndex].transform.position - currentNode.transform.position).normalized;
    }

    public Vector2 GetCurrentDirection() {
        Vector2 currentDirection = ComputeCurrentDirection();
        currentDirection.x = ApproximateValue(currentDirection.x);
        currentDirection.y = ApproximateValue(currentDirection.y);
        if (pathType != NPCPathType.SPINNING) {
            currentNode = nodes[nodeIndex];
        }
        return currentDirection;
    }
}
