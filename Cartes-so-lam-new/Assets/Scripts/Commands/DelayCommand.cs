using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DelayCommand : Command
{
	private float delay;

	public DelayCommand(float timeToWait)
	{
		delay = timeToWait;
	}

	public override void StartCommandExecution()
	{
		var s = DOTween.Sequence();
		s.PrependInterval(delay);
		s.OnComplete(CommandExecutionComplete);
	}
}
