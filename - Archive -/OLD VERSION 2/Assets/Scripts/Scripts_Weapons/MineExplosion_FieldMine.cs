using UnityEngine;
using System.Collections;

public class MineExplosion_FieldMine : MineExplosion
{
	public override void ReactToTrigger (GameObject other)
	{
		if (other.tag == Tags.shell)
		{
			ShellBehaviour script = other.GetComponent<ShellBehaviour>();

			if (script.shooter != this.shooter)
			{
				script.DestroyShell (true);
			}
		}
	}
}
