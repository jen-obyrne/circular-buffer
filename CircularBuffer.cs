using System;

public class CircularBuffer<T>
{
    private int _readPointer = 0;
    private int _readPointerLap = 0;
    private int _writePointer = 0;
    private int _writePointerLap = 0;
    private T[] _inputValues;
    private int _bufferSize;
    public CircularBuffer(int size)
    {
        _bufferSize = size;
        _inputValues = new T[_bufferSize];
    }

    public T Read()
    {
        //throw exception if you are trying to read more items, then there are items writtens
        if (_readPointer >= _writePointer && _readPointerLap == _writePointerLap) {
            throw new InvalidOperationException();
        }

        var item = _inputValues[_readPointer];
        _readPointer++;

        // should update read pointer
        if (_readPointerLap < _writePointerLap && _readPointer == _bufferSize)
        {
            _readPointer = 0;
            _readPointerLap++;
        }
        // update read pointer
        return item;
    }

    public void Write(T value)
    {
        CheckIfBufferIsFull();
        Write(_writePointer, value);
    }

    public void ForceWrite(T value)
    {
        ResetPointerBackToZero();
        BumpReadPointer();
        Write(_writePointer, value);
    }

    public void Clear()
    {
        _inputValues = new T[_bufferSize];
        _readPointer = 0;
        _writePointer = 0;
    }

    public void CheckIfBufferIsFull()
    {
        var maxItemsInBuffer = _bufferSize - 1;
        if (maxItemsInBuffer != 0 && _writePointer >= _bufferSize /*&& !IsOverwritingOldValues*/)
        {
            throw new InvalidOperationException();
        }
    }

    public void ResetPointerBackToZero()
    {
        if (_writePointer == _bufferSize) {
            _writePointer = 0;
            _writePointerLap++;
        }
    }

    private void Write(int index, T value)
    {
        _inputValues[index] = value;
       _writePointer ++;
    }

    private void BumpReadPointer()
    {
        if (_readPointer == _bufferSize) 
        {
            _readPointer =0;
        } 
        else 
        {
            _readPointer++;
        }
    }
}