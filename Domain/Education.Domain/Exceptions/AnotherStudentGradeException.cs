using Education.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions;

public class AnotherStudentGradeException(Student student, Grade grade) : InvalidOperationException($"Grade {grade.Id} is not for studen {student.Name}")
{
public Student Student => student;
public Grade Grade => grade;

}