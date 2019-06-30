using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.IRepositories
{
    public interface IRepo<T> : IDisposable
    {
        T Add(T item);
        bool Update(T item);
        bool Delete(T Item);
        bool Delete(int id);
        T Get(int id);
        List<T> GrtList();
    }
}
