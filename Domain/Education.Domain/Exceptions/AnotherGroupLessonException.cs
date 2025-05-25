using Education.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions
{
    public class AnotherGroupLessonException(Student student, Group group) : InvalidOperationException($"Student {student.Name} can not visit lesson of {group.Name} lesson")
    {
        public Student Student => student;
        public Group Group => group;

    }
}
