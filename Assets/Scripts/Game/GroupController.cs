using System.Collections.Generic;
using UnityEngine;

public class GroupController : MonoBehaviour {
    public List<GameObject> groupMembers = new();

    public void AddToGroup(GameObject member) {
        GameManager.Instance.UpdateCounts();
        if (!groupMembers.Contains(member)) groupMembers.Add(member);
    }

    public int GetGroupSize() => groupMembers.Count;
}
