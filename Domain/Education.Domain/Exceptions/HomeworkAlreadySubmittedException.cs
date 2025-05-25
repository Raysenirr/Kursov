using Education.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions
{
    public class HomeworkAlreadySubmittedException(Homework homework)
        : InvalidOperationException($"Homework {homework.Id} has already been submitted.")
    {
        public Homework Homework => homework;
    }
}
