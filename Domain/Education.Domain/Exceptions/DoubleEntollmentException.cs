using Education.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions;
internal class DoubleEnrollmentException(Student student, Group group): InvalidOperationException($"{student.Name} has been enrolled yo group {group.Name} yet")
{
    public Student Student => student;
    public Group Group => group;
}