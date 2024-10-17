using Lab06_DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab06_BUS
{
    public class StudentService
    {
        StudentModel studentModel = new StudentModel();
        public List<Student> GetAll()
        {
            return studentModel.Students.ToList();
        }
        public List<Student> GetAllHasNoMajor()
        {
            return studentModel.Students.Where(p => p.MajorID == null).ToList();
        }
        public List<Student> GetAllHasNoMajor(int facultyID)
        {
            return studentModel.Students.Where(p => p.MajorID == null && p.FacultyID == facultyID).ToList();
        }
        public Student FindById(string studentID)
        {
            return studentModel.Students.FirstOrDefault(p => p.StudentID == studentID);
        }
        public void InsertUpdate(Student s)
        {
            studentModel.Students.AddOrUpdate(s);
            studentModel.SaveChanges();
        }
        public void AddOrUpdate(Student student)
        {
            if (studentModel.Students.Any(p => p.StudentID == student.StudentID))
            {
                studentModel.Entry(student).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                studentModel.Students.Add(student);
            }
            studentModel.SaveChanges();
        }
        public void Delete(string studentID)
        {
            // Tìm sinh viên theo StudentID
            var student = studentModel.Students.FirstOrDefault(p => p.StudentID == studentID);
            if (student != null)
            {
                // Xóa sinh viên nếu tìm thấy
                studentModel.Students.Remove(student);
                studentModel.SaveChanges();
            }
        }
    }
}
