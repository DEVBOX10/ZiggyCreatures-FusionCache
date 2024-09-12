﻿using System.Threading;
using System.Threading.Tasks;
using MessagePack;
using MessagePack.Resolvers;

namespace ZiggyCreatures.Caching.Fusion.Serialization.NeueccMessagePack;

/// <summary>
/// An implementation of <see cref="IFusionCacheSerializer"/> which uses Neuecc's famous MessagePack serializer.
/// </summary>
public class FusionCacheNeueccMessagePackSerializer
	: IFusionCacheSerializer
{
	/// <summary>
	/// The options class for the <see cref="FusionCacheNeueccMessagePackSerializer"/> class.
	/// </summary>
	public class Options
	{
		/// <summary>
		/// The optional <see cref="MessagePackSerializerOptions"/> object to use.
		/// </summary>
		public MessagePackSerializerOptions? SerializerOptions { get; set; }
	}

	/// <summary>
	/// Create a new instance of a <see cref="FusionCacheNeueccMessagePackSerializer"/> object.
	/// </summary>
	/// <param name="options">The <see cref="MessagePackSerializerOptions"/> to use: if not specified, the contract-less (<see cref="ContractlessStandardResolver"/>) options will be used.</param>
	public FusionCacheNeueccMessagePackSerializer(MessagePackSerializerOptions? options = null)
	{
		// PER @neuecc 'S SUGGESTION: DEFAULT TO THE CONTRACTLESS RESOLVER
		_serializerOptions = options ?? ContractlessStandardResolver.Options;
	}

	/// <summary>
	/// Create a new instance of a <see cref="FusionCacheNeueccMessagePackSerializer"/> object.
	/// </summary>
	/// <param name="options">The <see cref="Options"/> to use.</param>
	public FusionCacheNeueccMessagePackSerializer(Options? options)
		: this(options?.SerializerOptions)
	{
		// EMPTY
	}

	private readonly MessagePackSerializerOptions? _serializerOptions;

	/// <inheritdoc />
	public byte[] Serialize<T>(T? obj)
	{
		return MessagePackSerializer.Serialize<T?>(obj, _serializerOptions);
	}

	/// <inheritdoc />
	public T? Deserialize<T>(byte[] data)
	{
		return MessagePackSerializer.Deserialize<T?>(data, _serializerOptions);
	}

	/// <inheritdoc />
	public ValueTask<byte[]> SerializeAsync<T>(T? obj, CancellationToken token = default)
	{
		// PER @neuecc 'S SUGGESTION: AVOID AWAITING ON A MEMORY STREAM
		return new ValueTask<byte[]>(Serialize(obj));
	}

	/// <inheritdoc />
	public ValueTask<T?> DeserializeAsync<T>(byte[] data, CancellationToken token = default)
	{
		// PER @neuecc 'S SUGGESTION: AVOID AWAITING ON A MEMORY STREAM
		return new ValueTask<T?>(Deserialize<T>(data));
	}
}
