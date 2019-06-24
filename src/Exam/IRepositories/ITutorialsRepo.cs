using ExamTutorialsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.IRepositories
{
     public interface ITutorialsRepo
    {
        Task<Tutorial> Add(Tutorial item);
        Task<bool> Update(Tutorial item);
        Task<bool> Delete(int id);
        Task<Tutorial> Get(int id);
        Task<List<Tutorial>> GetList();
        Task<List<Tutorial>> GetList(int page = 1, int? pageLocalSize = null);
        int TotalItems { get; set; }


    }
}
