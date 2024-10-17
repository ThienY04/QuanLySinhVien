using Lab06_DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab06_BUS
{
    public class FacultyService
    {

        StudentModel studentModel = new StudentModel();
        public List<Faculty> GetAll()
        {
            return studentModel.Faculties.ToList();
        }
    }
}
