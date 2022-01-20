using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionsWorkFlow.Tests
{
    public class Project
    {
        public static int id = 0;
        public string name;
        public string description;
        public DateTime creationDate;
        public int user;
        public bool state;

        public Project(string _name, string _description, DateTime _creationDate, int _user, bool _state)
        {
            id += 1;
            name = _name;
            description = _description;
            creationDate = _creationDate;
            user = _user;
            state = _state;
        }
    }
}
