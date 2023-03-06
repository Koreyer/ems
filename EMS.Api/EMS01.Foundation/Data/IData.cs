using System;

namespace EMS01.Foundation.Data
{
    public interface IData<T>
    {
        public T Id { get; set; }
        public string? Name { get; set; }
    }
}
