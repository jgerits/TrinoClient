using System.Collections;
using System.Data.Common;

namespace TrinoClient.EntityFrameworkCore.Storage;

/// <summary>
/// ADO.NET DbParameterCollection implementation for Trino
/// </summary>
public class TrinoDbParameterCollection : DbParameterCollection
{
    private readonly List<TrinoDbParameter> _parameters = new();

    public override int Count => _parameters.Count;

    public override object SyncRoot => ((ICollection)_parameters).SyncRoot;

    public override int Add(object value)
    {
        if (value is not TrinoDbParameter parameter)
            throw new ArgumentException("Parameter must be of type TrinoDbParameter", nameof(value));

        _parameters.Add(parameter);
        return _parameters.Count - 1;
    }

    public override void AddRange(Array values)
    {
        foreach (var value in values)
        {
            Add(value);
        }
    }

    public override void Clear()
    {
        _parameters.Clear();
    }

    public override bool Contains(object value)
    {
        return _parameters.Contains(value as TrinoDbParameter);
    }

    public override bool Contains(string value)
    {
        return _parameters.Any(p => p.ParameterName == value);
    }

    public override void CopyTo(Array array, int index)
    {
        ((ICollection)_parameters).CopyTo(array, index);
    }

    public override IEnumerator GetEnumerator()
    {
        return _parameters.GetEnumerator();
    }

    public override int IndexOf(object value)
    {
        return _parameters.IndexOf(value as TrinoDbParameter);
    }

    public override int IndexOf(string parameterName)
    {
        return _parameters.FindIndex(p => p.ParameterName == parameterName);
    }

    public override void Insert(int index, object value)
    {
        if (value is not TrinoDbParameter parameter)
            throw new ArgumentException("Parameter must be of type TrinoDbParameter", nameof(value));

        _parameters.Insert(index, parameter);
    }

    public override void Remove(object value)
    {
        if (value is TrinoDbParameter parameter)
            _parameters.Remove(parameter);
    }

    public override void RemoveAt(int index)
    {
        _parameters.RemoveAt(index);
    }

    public override void RemoveAt(string parameterName)
    {
        var index = IndexOf(parameterName);
        if (index >= 0)
            _parameters.RemoveAt(index);
    }

    protected override DbParameter GetParameter(int index)
    {
        return _parameters[index];
    }

    protected override DbParameter GetParameter(string parameterName)
    {
        var param = _parameters.FirstOrDefault(p => p.ParameterName == parameterName);
        return param ?? throw new IndexOutOfRangeException($"Parameter '{parameterName}' not found");
    }

    protected override void SetParameter(int index, DbParameter value)
    {
        if (value is not TrinoDbParameter parameter)
            throw new ArgumentException("Parameter must be of type TrinoDbParameter", nameof(value));

        _parameters[index] = parameter;
    }

    protected override void SetParameter(string parameterName, DbParameter value)
    {
        if (value is not TrinoDbParameter parameter)
            throw new ArgumentException("Parameter must be of type TrinoDbParameter", nameof(value));

        var index = IndexOf(parameterName);
        if (index >= 0)
            _parameters[index] = parameter;
        else
            throw new IndexOutOfRangeException($"Parameter '{parameterName}' not found");
    }
}
