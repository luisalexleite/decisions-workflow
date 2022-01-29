using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionsWorkFlow.Database
{
    public class DatabaseContent
    {
        private DatabaseDataContext database = new DatabaseDataContext();

        public List<projects> GetProjectList (int user, bool terminated, string queryText = "", int queryOrder = 0)
        {
           int[] usersList = database.users_projects.Where(up => up.user_id == user).Select(up => up.project_id).ToArray();

           var projects = database.projects.Where(p => p.project_admin == user || usersList.Contains(p.id)).Where(p => p.project_name.Contains(queryText)).Where(p => p.terminated == terminated);
           
            switch (queryOrder)
            {
                case 0:
                    return projects.OrderBy(p => p.project_name).ToList();
                case 1:
                    return projects.OrderByDescending(p => p.project_name).ToList();
                case 2:
                    return projects.OrderBy(p => p.created_at).ToList();
                case 3:
                    return projects.OrderByDescending(p => p.created_at).ToList();
                default:
                    return projects.OrderBy(p => p.project_name).ToList();
            }

        }

        public users GetUserData (int user)
        {
            return database.users.Where(u => u.id == user).FirstOrDefault();
        }

        public projects GetProjectData (int project)
        {
            return database.projects.Where(p => p.id == project).FirstOrDefault();
        }

        public int CountStudents(int project)
        {
            return database.students.Where(s => s.project_id == project).Count();
        }

        public IQueryable<IGrouping<string, students>> StudentsByNationality(int project)
        {
            return database.students.Where(s => s.project_id == project).GroupBy(s => s.national_code);
        }

        public IQueryable<IGrouping<int?, students>> StudentsBySchool(int project)
        {
           return database.students.Where(s => s.project_id == project).GroupBy(s => s.school_id);
        }

        public schools GetSchool(int? school_id)
        {
            return database.schools.Where(s => s.id == school_id).FirstOrDefault();
        }
    }
}
