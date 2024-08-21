using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sketch_Based
{
    class RegisterPojo
    {
        string name;
        string date;
        string description;
        string crime;
        string feature;
        string middlename, surname, registrationdate, age, address, gender, mobno, dob, aadharno;

        public string Name { get => name; set => name = value; }
        public string Date { get => date; set => date = value; }
        public string Description { get => description; set => description = value; }
        public string Crime { get => crime; set => crime = value; }
        public string Feature { get => feature; set => feature = value; }
        public string Middlename { get => middlename; set => middlename = value; }
        public string Surname { get => surname; set => surname = value; }
        public string Registrationdate { get => registrationdate; set => registrationdate = value; }
        public string Age { get => age; set => age = value; }
        public string Address { get => address; set => address = value; }
        public string Gender { get => gender; set => gender = value; }
        public string Mobno { get => mobno; set => mobno = value; }
        public string Dob { get => dob; set => dob = value; }
        public string Aadharno { get => aadharno; set => aadharno = value; }
    }
}
