using Spectre.Console.Cli;

namespace LoadTestToolbox;

public sealed class TypeResolver(IServiceProvider provider) : ITypeResolver, IDisposable
{
	private readonly IServiceProvider _provider = provider ?? throw new ArgumentNullException(nameof(provider));

	public object? Resolve(Type? type)
		=> type != null
			? _provider.GetService(type)
			: null;

	public void Dispose()
	{
		if (_provider is IDisposable disposable)
		{
			disposable.Dispose();
		}
	}
}