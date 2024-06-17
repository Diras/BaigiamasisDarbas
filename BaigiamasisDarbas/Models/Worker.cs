using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaigiamasisDarbas.Models
{
    public class Worker
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Department { get; set; }


        public Worker()
        {

        }

        public Worker(int id, string name, string surname, string department)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Department = department;
        }

        public Worker( string name, string surname, string department)
        {
            Name = name;
            Surname = surname;
            Department = department;
        }


        public override string ToString()
        {
            return $"ID:{Id} - {Name} {Surname} - Department:{Department}";
        }
    }
}
