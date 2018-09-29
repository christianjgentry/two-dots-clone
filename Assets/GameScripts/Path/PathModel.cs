﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathModel : MonoBehaviour
{
  [SerializeField]
  public List<TileSlot> path = new List<TileSlot>();
  private ColorPalette pathColor = ColorPalette.None;

  public void SetInitialSlot(TileSlot initialSlot)
  {
    ColorPalette itemType = initialSlot.GetItemType();
    path.Clear();
    if (itemType == ColorPalette.None) return;
    path.Add(initialSlot);
    pathColor = itemType;
  }
  public void AttemptAddPath(TileSlot newTile)
  {
    // Don't add to empty path
    if (path.Count == 0) return;
    if (path.Count > 1 && path[path.Count - 2] == newTile) return;
    
    ColorPalette itemType = newTile.GetItemType();
    TileSlot lastSlot = path[path.Count - 1];
    if (itemType != ColorPalette.All && itemType != pathColor) return;

    // If last item, remove last item
    if (lastSlot == newTile && path.Count > 1)
    {
      path.RemoveAt(path.Count - 1);
      return;
    }
    if (ContainsLoop()) return;

    if (!lastSlot.adjacentTiles.Contains(newTile)) return;
    path.Add(newTile);
  }
  public void PathReset()
  {
    pathColor = ColorPalette.None;
    path.Clear();
  }
  public void ClearPathItems()
  {
    List<TileItem> itemsToRemove = new List<TileItem>();
    foreach (TileSlot slot in path)
    {
      itemsToRemove.Add(slot.GetItem());
    }
    foreach (TileItem item in itemsToRemove)
    {
      item.ClearItem();
    }
  }

  public List<Vector3> GetPathLocations()
  {
    List<Vector3> pathLocationList = new List<Vector3>();
    foreach (TileSlot slot in path)
    {
      pathLocationList.Add(slot.transform.position);
    }
    return pathLocationList;
  }
  public ColorPalette GetPathColor()
  {
    return pathColor;
  }

  public bool ContainsLongPath()
  {
    if (path.Count > 1) return true;
    return false;
  }

  public bool ContainsLoop()
  {
    if(path.Count<2) return false;
    TileSlot lastSlot = path[path.Count - 1];
    List<TileSlot> butLast = new List<TileSlot>(path);
    butLast.RemoveAt(butLast.Count - 1);
    return butLast.Contains(lastSlot);
  }
}
