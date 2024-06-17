using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaigiamasisDarbas.Models
{
    public class Admin : Worker
    {
        public string Password { get; set; }


        public Admin() { }
        public Admin(string name, string surname, string department, string password) 
            :base(name, surname, department)
        {
            Password = password;
        }


        public Admin(int id, string name, string surname, string department, string password)
           : base(id, name, surname, department)
        {
            Password = password;
        }

        public override string ToString()
        {
            return $"{Name} {Surname} - Department:{Department}";
        }
    }
}
