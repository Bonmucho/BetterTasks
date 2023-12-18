using System.Runtime.CompilerServices;

namespace BetterTasks;

public static class TaskExtensions
{
	/// <summary>
	/// Configures the task to continue on the captured synchronization context.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredTaskAwaitable Captured(this Task task) =>
		task.ConfigureAwait(ConfigureAwaitOptions.ContinueOnCapturedContext);
	
	/// <summary>
	/// Configures the task to not continue on any captured synchronization context.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredTaskAwaitable NonCaptured(this Task task) =>
		task.ConfigureAwait(ConfigureAwaitOptions.None);
	
	/// <summary>
	/// Configures the task of a specified type to continue on the captured synchronization context.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredTaskAwaitable<T> Captured<T>(this Task<T> task) =>
		task.ConfigureAwait(ConfigureAwaitOptions.ContinueOnCapturedContext);
	
	/// <summary>
	/// Configures the task of a specified type to not continue on any captured synchronization context.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredTaskAwaitable<T> NonCaptured<T>(this Task<T> task) =>
		task.ConfigureAwait(ConfigureAwaitOptions.None);
	
	/// <summary>
	/// Configures the value task to continue on the captured synchronization context.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable Captured(this ValueTask task) =>
		task.ConfigureAwait(true);
	
	/// <summary>
	/// Configures the value task to not continue on any captured synchronization context.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable NonCaptured(this ValueTask task) =>
		task.ConfigureAwait(false);
	
	/// <summary>
	/// Configures the value task of a specified type to continue on the captured synchronization context.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<T> Captured<T>(this ValueTask<T> task) =>
		task.ConfigureAwait(true);
	
	/// <summary>
	/// Configures the value task of a specified type to not continue on any captured synchronization context.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<T> NonCaptured<T>(this ValueTask<T> task) =>
		task.ConfigureAwait(false);
	
	/// <summary>
	/// Runs the task in the background and catches a specified exception type.
	/// </summary>
	/// <typeparam name="TException">The type of exception to catch.</typeparam>
	/// <param name="task">The value task.</param>
	/// <param name="onException">Action to perform when the specified exception occurs.</param>
	/// <param name="continueOnCapturedContext">Flag to continue on the captured synchronization context.</param>
	/// <param name="suppressThrowing">Flag to suppress throwing the caught exception.</param>
	/// <exception cref="ArgumentNullException">Thrown when task or onException is null.</exception>
	public static void Catch<TException>(this ValueTask task,
		in Action<TException> onException, bool continueOnCapturedContext = false,
		bool suppressThrowing = true) where TException : Exception
	{
		ArgumentNullException.ThrowIfNull(task);
		ArgumentNullException.ThrowIfNull(onException);
		RunBackground(task, onException, continueOnCapturedContext, suppressThrowing);
	}
	
	/// <summary>
	/// Runs the task in the background and catches a specified exception type.
	/// </summary>
	/// <typeparam name="TException">The type of exception to catch.</typeparam>
	/// <param name="task">The task.</param>
	/// <param name="onException">Action to perform when the specified exception occurs.</param>
	/// <param name="options">Options for configuring task continuation.</param>
	/// <param name="suppressThrowing">Flag to suppress throwing the caught exception.</param>
	/// <exception cref="ArgumentNullException">Thrown when task or onException is null.</exception>
	public static void Catch<TException>(this Task task,
		in Action<TException> onException, ConfigureAwaitOptions options = ConfigureAwaitOptions.None,
		bool suppressThrowing = true) where TException : Exception
	{
		ArgumentNullException.ThrowIfNull(task);
		ArgumentNullException.ThrowIfNull(onException);
		RunBackground(task, onException, options, suppressThrowing);
	}

	private static async void RunBackground<TException>(Task task,
		Action<TException> onException, ConfigureAwaitOptions options,
		bool suppressThrowing) where TException : Exception
	{
		try
		{
			await task.ConfigureAwait(options);
		}
		catch (TException e)
		{
			onException(e);
			if (!suppressThrowing) throw;
		}
	}
	
	private static async void RunBackground<TException>(ValueTask task,
		Action<TException> onException, bool continueOnCapturedContext,
		bool suppressThrowing) where TException : Exception
	{
		try
		{
			await task.ConfigureAwait(continueOnCapturedContext);
		}
		catch (TException e)
		{
			onException(e);
			if (!suppressThrowing) throw;
		}
	}
}