using Lab06_DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab06_BUS
{
    public class MajorService
    {
        StudentModel studentModel = new StudentModel();
        public List<Major> GetAllByFaculty(int facultyID)
        {
            return studentModel.Majors.Where(p => p.FacultyID == facultyID).ToList();
        }
    }
}
