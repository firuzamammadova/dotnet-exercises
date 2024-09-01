using System;

public class CircularBuffer<T>
{
    private readonly T[] _buffer;
    private int _writeIndex;
    private int _readIndex;
    private bool _isFull;

    public CircularBuffer(int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentException("Capacity must be greater than zero.", nameof(capacity));
        }

        _buffer = new T[capacity];
        _writeIndex = 0;
        _readIndex = 0;
        _isFull = false;
    }

    public T Read()
    {
        if (IsEmpty())
        {
            throw new InvalidOperationException("Cannot read from an empty buffer.");
        }

        var value = _buffer[_readIndex];
        MoveReadIndex();
        _isFull = false;

        return value;
    }

    public void Write(T value)
    {
        if (IsFull())
        {
            throw new InvalidOperationException("Cannot write to a full buffer.");
        }

        _buffer[_writeIndex] = value;
        MoveWriteIndex();

        if (_writeIndex == _readIndex)
        {
            _isFull = true;
        }
    }

    public void Overwrite(T value)
    {
        if (IsFull())
        {
            _buffer[_writeIndex] = value;
            MoveWriteIndex();
            MoveReadIndex();
        }
        else
        {
            Write(value);
        }
    }

    public void Clear()
    {
        _writeIndex = 0;
        _readIndex = 0;
        _isFull = false;
    }

    public bool IsEmpty()
    {
        return !_isFull && _writeIndex == _readIndex;
    }

    public bool IsFull()
    {
        return _isFull;
    }

    private void MoveWriteIndex()
    {
        _writeIndex = (_writeIndex + 1) % _buffer.Length;
    }

    private void MoveReadIndex()
    {
        _readIndex = (_readIndex + 1) % _buffer.Length;
    }
}
