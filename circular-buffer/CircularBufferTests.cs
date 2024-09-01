using System;
using Xunit;

public class CircularBufferTests
{
    [Fact]
    public void Reading_empty_buffer_should_fail()
    {
        var buffer = new CircularBuffer<int>(capacity: 1);
        Assert.Throws<InvalidOperationException>(() => buffer.Read());
    }

    [Fact]
    public void Can_read_an_item_just_written()
    {
        var buffer = new CircularBuffer<int>(capacity: 1);
        buffer.Write(1);
        Assert.Equal(1, buffer.Read());
    }

    [Fact]
    public void Writing_to_full_buffer_should_fail()
    {
        var buffer = new CircularBuffer<int>(capacity: 2);
        buffer.Write(1);
        buffer.Write(2);
        Assert.Throws<InvalidOperationException>(() => buffer.Write(3));
    }

    [Fact]
    public void Overwriting_in_full_buffer()
    {
        var buffer = new CircularBuffer<int>(capacity: 2);
        buffer.Write(1);
        buffer.Write(2);
        buffer.Overwrite(3);
        Assert.Equal(2, buffer.Read()); // Oldest value overwritten
        Assert.Equal(3, buffer.Read());
    }

    [Fact]
    public void Clearing_buffer_should_make_it_empty()
    {
        var buffer = new CircularBuffer<int>(capacity: 3);
        buffer.Write(1);
        buffer.Write(2);
        buffer.Write(3);
        buffer.Clear();
        Assert.Throws<InvalidOperationException>(() => buffer.Read());
    }

    [Fact]
    public void Writing_after_clearing_buffer()
    {
        var buffer = new CircularBuffer<int>(capacity: 2);
        buffer.Write(1);
        buffer.Clear();
        buffer.Write(2);
        Assert.Equal(2, buffer.Read());
    }

    [Fact]
    public void Read_wrapped_buffer()
    {
        var buffer = new CircularBuffer<int>(capacity: 3);
        buffer.Write(1);
        buffer.Write(2);
        buffer.Write(3);
        buffer.Read(); // Read 1
        buffer.Write(4); // Overwrite 1
        Assert.Equal(2, buffer.Read());
        Assert.Equal(3, buffer.Read());
        Assert.Equal(4, buffer.Read());
    }

    [Fact]
    public void Write_overwrite_and_read_wrapped_buffer()
    {
        var buffer = new CircularBuffer<int>(capacity: 2);
        buffer.Write(1);
        buffer.Write(2);
        buffer.Overwrite(3); // Overwrites 1
        buffer.Overwrite(4); // Overwrites 2
        Assert.Equal(3, buffer.Read());
        Assert.Equal(4, buffer.Read());
    }

    [Fact]
    public void Buffer_with_single_element()
    {
        var buffer = new CircularBuffer<int>(capacity: 1);
        buffer.Write(1);
        Assert.Equal(1, buffer.Read());
    }

    [Fact]
    public void Buffer_with_multiple_elements()
    {
        var buffer = new CircularBuffer<int>(capacity: 4);
        buffer.Write(1);
        buffer.Write(2);
        buffer.Write(3);
        buffer.Write(4);
        Assert.Equal(1, buffer.Read());
        Assert.Equal(2, buffer.Read());
        Assert.Equal(3, buffer.Read());
        Assert.Equal(4, buffer.Read());
    }
}
