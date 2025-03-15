﻿namespace NetCore.Migration.Common.Interface;

public interface IMigrationTask
{
	IEnumerable<Type> Dependencies { get; }
	Task ExecuteAsync(string[] args);
}
