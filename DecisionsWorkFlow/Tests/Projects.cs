using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionsWorkFlow.Tests
{
    internal class Projects
    {
        public string name;
        public string description;
        public DateTime creationDate;
        public int user;
        public bool state;

        public Projects(string _name, string _description, DateTime _creationDate, int _user, bool _state)
        {
            name = _name;
            description = _description;
            creationDate = _creationDate;
            user = _user;
            state = _state;
        }
    }
}
