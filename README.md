# BetterTasks
The handling of **tasks** in C# could be **better**. With this package, this becomes a reality for a few aspects.

## Overview
This project provides extension methods for handling tasks and value tasks with different synchronization contexts and exception handling strategies.

![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)

This library only supports **.NET 8.0+**.

## Usage

### `Captured` and `NonCaptured` Methods
- `Captured`: Configures a task or value task to continue on the captured synchronization context.
- `NonCaptured`: Configures a task or value task to not continue on any captured synchronization context.

### `Catch` Methods
- `Catch<TException>(ValueTask, Action<TException>, bool, bool)`: Runs a value task in the background and catches a specified exception type, providing options to continue on the captured synchronization context and suppress throwing the caught exception.
- `Catch<TException>(Task, Action<TException>, ConfigureAwaitOptions, bool)`: Runs a task in the background and catches a specified exception type, providing options for configuring task continuation and suppressing throwing the caught exception.

## Example

### Shorter variants for contextual capturing

```csharp
using BetterTasks;

// Instead of:
await DoWorkAsync().ConfigureAwait(false);
// Or:
await DoWorkAsync().ConfigureAwait(ConfigureAwaitOptions.None);
// You can now write:
await DoWorkAsync().NonCaptured();

// Instead of:
await DoWorkAsync().ConfigureAwait(true);
// Or:
await DoWorkAsync().ConfigureAwait(ConfigureAwaitOptions.ContinueOnCapturedContext);
// You can now write:
await DoWorkAsync().Captured();
```

### Managed background tasks

```csharp
using BetterTasks;

DoWorkAsync().Catch<FileNotFoundException>(e =>
{
	Log($"Error: {e.Message}");
});
```