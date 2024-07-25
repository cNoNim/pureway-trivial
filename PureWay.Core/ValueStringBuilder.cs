using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace PureWay.Core;

internal ref struct ValueStringBuilder
{
	private char[]?    _array;
	private int        _pos;
	private Span<char> _span;

	public ValueStringBuilder(Span<char> initialBuffer)
	{
		_array = null;
		_span  = initialBuffer;
		_pos   = 0;
	}

	public ValueStringBuilder(int initialCapacity)
	{
		_array = ArrayPool<char>.Shared.Rent(initialCapacity);
		_span  = _array;
		_pos   = 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Dispose()
	{
		var toReturn = _array;
		this = default; // for safety, to avoid using pooled array if this instance is erroneously appended to again
		if (toReturn != null)
			ArrayPool<char>.Shared.Return(toReturn);
	}

	public ref char this[int index]
	{
		get
		{
			Debug.Assert(index < _pos);
			return ref _span[index];
		}
	}

	public int Capacity
	{
		get => _span.Length;
	}

	public int Length
	{
		get => _pos;
		set
		{
			Debug.Assert(value >= 0);
			Debug.Assert(value <= _span.Length);
			_pos = value;
		}
	}

	public Span<char> RawChars
	{
		get => _span;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Append(char c)
	{
		var pos = _pos;
		if ((uint) pos < (uint) _span.Length)
		{
			_span[pos] = c;
			_pos       = pos + 1;
		}
		else
			GrowAndAppend(c);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Append(string? s)
	{
		if (s == null)
			return;

		var pos = _pos;
		if (s.Length   == 1
		 && (uint) pos < (uint) _span.Length)
		{
			_span[pos] = s[0];
			_pos       = pos + 1;
		}
		else
			AppendSlow(s);
	}

	public void Append(char c, int count)
	{
		if (_pos > _span.Length - count)
			Grow(count);

		var dst = _span.Slice(_pos, count);
		for (var i = 0; i < dst.Length; i++)
			dst[i] = c;
		_pos += count;
	}

	public void Append(ReadOnlySpan<char> value)
	{
		var pos = _pos;
		if (pos > _span.Length - value.Length)
			Grow(value.Length);

		value.CopyTo(_span[_pos..]);
		_pos += value.Length;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Span<char> AppendSpan(int length)
	{
		var origPos = _pos;
		if (origPos > _span.Length - length)
			Grow(length);

		_pos = origPos + length;
		return _span.Slice(origPos, length);
	}

	public ReadOnlySpan<char> AsSpan(bool terminate)
	{
		if (terminate)
		{
			EnsureCapacity(Length + 1);
			_span[Length] = '\0';
		}

		return _span[.._pos];
	}

	public ReadOnlySpan<char> AsSpan() =>
		_span[.._pos];

	public ReadOnlySpan<char> AsSpan(int start) =>
		_span.Slice(start, _pos - start);

	public ReadOnlySpan<char> AsSpan(int start, int length) =>
		_span.Slice(start, length);

	public void EnsureCapacity(int capacity)
	{
		Debug.Assert(capacity >= 0);
		if ((uint) capacity > (uint) _span.Length)
			Grow(capacity - _pos);
	}

	public void Insert(int index, char value, int count)
	{
		if (_pos > _span.Length - count)
			Grow(count);

		var remaining = _pos - index;
		_span.Slice(index, remaining).CopyTo(_span[(index + count)..]);
		_span.Slice(index, count).Fill(value);
		_pos += count;
	}

	public void Insert(int index, string? s)
	{
		if (s == null)
			return;

		var count = s.Length;

		if (_pos > _span.Length - count)
			Grow(count);

		var remaining = _pos - index;
		_span.Slice(index, remaining).CopyTo(_span[(index + count)..]);
		s.CopyTo(_span[index..]);
		_pos += count;
	}

	public override string ToString()
	{
		var s = _span[.._pos].ToString();
		Dispose();
		return s;
	}

	public bool TryCopyTo(Span<char> destination, out int charsWritten)
	{
		if (_span[.._pos].TryCopyTo(destination))
		{
			charsWritten = _pos;
			Dispose();
			return true;
		}

		charsWritten = 0;
		Dispose();
		return false;
	}

	internal void AppendSpanFormattable<T>(T value, string? format = null, IFormatProvider? provider = null)
		where T : ISpanFormattable
	{
		if (value.TryFormat(
				_span[_pos..],
				out var charsWritten,
				format,
				provider))
			_pos += charsWritten;
		else
			Append(value.ToString(format, provider));
	}

	private void AppendSlow(string s)
	{
		var pos = _pos;
		if (pos > _span.Length - s.Length)
			Grow(s.Length);

		s.CopyTo(_span[pos..]);
		_pos += s.Length;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Grow(int additionalCapacityBeyondPos)
	{
		Debug.Assert(additionalCapacityBeyondPos > 0);
		Debug.Assert(
			_pos > _span.Length - additionalCapacityBeyondPos,
			"Grow called incorrectly, no resize is needed.");

		var length    = (int) Math.Max((uint) (_pos + additionalCapacityBeyondPos), (uint) _span.Length * 2);
		var poolArray = ArrayPool<char>.Shared.Rent(length);
		_span[.._pos].CopyTo(poolArray);
		var toReturn   = _array;
		_span = _array = poolArray;
		if (toReturn != null)
			ArrayPool<char>.Shared.Return(toReturn);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GrowAndAppend(char c)
	{
		Grow(1);
		Append(c);
	}
}
