using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExtensionsPack
{
	public static class TaskExtensions
	{
		public static async Task WaitUntil(Func<bool> predicate, CancellationToken? cancellationToken = null)
		{
			bool IsCanceled() => cancellationToken is { IsCancellationRequested: true };

			while (!IsCanceled() && !predicate())
			{
				await Task.Yield();
			}
		}	

		public static async Task WaitUntil(Func<bool> predicate, int millisecondsDelay, CancellationToken? cancellationToken = null)
		{
			bool IsCanceled() => cancellationToken is { IsCancellationRequested: true };	

			while (!IsCanceled() && !predicate())
			{
				await Task.Delay(millisecondsDelay);
			}
		}
	}
}