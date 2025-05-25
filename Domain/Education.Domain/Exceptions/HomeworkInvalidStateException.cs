using Education.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions
{

    public class HomeworkInvalidStateException(Homework homework, string reason)
        : InvalidOperationException($"Homework {homework.Id}: {reason}")
    {
        public Homework Homework => homework;
        public string Reason => reason;
    }

}
