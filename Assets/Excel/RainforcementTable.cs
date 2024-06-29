using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class RainforcementTable : ScriptableObject
{
	public List<RainforcementEntity> Rainforcement; // Replace 'EntityType' to an actual type that is serializable.
}
