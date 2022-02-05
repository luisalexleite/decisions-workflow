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

        public class CompareAttributes
        {
            public string attr1 { get; set; }
            public string attr2 { get; set; }
            public int val { get; set; }
        }

        public List<projects> GetProjectList(int user, bool terminated, string queryText = "", int queryOrder = 0)
        {
            RefreshDB();
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

        public users GetUserData(int user)
        {
            RefreshDB();
            return database.users.Where(u => u.id == user).FirstOrDefault();
        }

        public projects GetProjectData(int project)
        {
            RefreshDB();
            return database.projects.Where(p => p.id == project).FirstOrDefault();
        }

        public IQueryable<students> GetStudents(int project)
        {
            RefreshDB();
            return database.students.Where(s => s.project_id == project);
        }

        public int CountStudents(int project)
        {
            RefreshDB();
            return database.students.Where(s => s.project_id == project).Count();
        }

        public IQueryable<IGrouping<string, students>> StudentsByNationality(int project)
        {
            RefreshDB();
            return database.students.Where(s => s.project_id == project).GroupBy(s => s.national_code);
        }

        public IQueryable<IGrouping<int?, students>> StudentsBySchool(int project)
        {
            RefreshDB();
            return database.students.Where(s => s.project_id == project).GroupBy(s => s.school_id);
        }

        public schools GetSchool(int? school_id)
        {
            RefreshDB();
            return database.schools.Where(s => s.id == school_id).FirstOrDefault();
        }

        public bool CheckExistentProject(string project_name)
        {
            RefreshDB();
            if (database.projects.Where(s => s.project_name.Equals(project_name)).Count() > 0)
            {
                return true;
            }
            return false;
        }

        public List<int?> GetStudentAtributes(int student)
        {
            RefreshDB();
            return database.students_attributes.Where(sa => sa.student_id == student).Select(sa => sa.attr_value).ToList();
        }

        public List<functions> GetFunctionList(int project, string queryText = "", int queryOrder = 0)
        {
            RefreshDB();
            var functions = database.functions.Where(p => p.project_id == project).Where(p => p.func_name.Contains(queryText));

            switch (queryOrder)
            {
                case 0:
                    return functions.OrderBy(p => p.func_name).ToList();
                case 1:
                    return functions.OrderByDescending(p => p.func_name).ToList();
                case 2:
                    return functions.OrderBy(p => p.created_at).ToList();
                case 3:
                    return functions.OrderByDescending(p => p.created_at).ToList();
                default:
                    return functions.OrderBy(p => p.func_name).ToList();
            }

        }

    public List<CompareAttributes> GetCompareAttributes(int id)
        {
            RefreshDB();
            var project_id = database.functions.Where(f => f.id == id).Select(f => f.project_id).FirstOrDefault();
            var instance = database.attributes.Where(a => a.project_id == project_id).Select(a => a.attr_name);
            var count = instance.Count();

            List<CompareAttributes> attributeList= new List<CompareAttributes>();

            for (int i = 1; i <= count - 1; i++)
            {
                for (int j = 0; j <= i-1; j++)
                {
                    attributeList.Add(new CompareAttributes { attr1 = instance.ToList()[i].ToString(), attr2 = instance.ToList()[j].ToString(), val = 5 });
                }
            }

            return attributeList;
        }
        public void SetFunctionWeights(int id, float[] arr)
        {
            RefreshDB();
            var function = database.functions.Single(f => f.id == id);
            function.weight_set = true;

            int i = 0;
            database.attributes.Where(a => a.project_id == function.project_id).ToList().ForEach(fa =>
            {
                database.functions_attributes.InsertOnSubmit(new functions_attributes() { attr_id = fa.id, func_id = id, attr_weight = (float)arr[i] });
                i++;
            }
            );

            database.SubmitChanges();
        }

        public void UpdateFunctionWeights(int id, float[] arr)
        {
            RefreshDB();
            var changeWeightsQuery =
            from u in database.functions_attributes
            where u.func_id == id
            select u;

            int i = 0;
            foreach (var weight in changeWeightsQuery)
            {
                weight.attr_weight = arr[i];
                i++;
            }

            database.SubmitChanges();
        }

        public functions GetProjectByFunction(int id)
        {
            RefreshDB();
            return database.functions.Single(f => f.id == id);
        }

        public List<schools> GetSchoolList()
        {
            RefreshDB();
            return database.schools.ToList();
        }

        public void deleteStudent(int id)
        {
            RefreshDB();
            database.students_attributes.DeleteAllOnSubmit(database.students_attributes.Where(s => s.student_id == id));
            database.SubmitChanges();
            RefreshDB();
            database.students.DeleteOnSubmit(database.students.Where(s => s.id == id).FirstOrDefault());
            database.SubmitChanges();
        }

        public void RefreshDB()
        {
            database = new DatabaseDataContext();
        }

        public void TerminateProject(int id)
        {
            RefreshDB();
            database.projects.Where(p => p.id == id).FirstOrDefault().terminated = !database.projects.Where(p => p.id == id).FirstOrDefault().terminated;
            database.SubmitChanges();
        }

        public string[] getPoints(int id)
        {
            var data = database.students_attributes.Where(s => s.students.project_id == id).GroupBy(s => new { id = s.attr_id }).Select(g => new
            {
                Average = g.Average(p => p.attr_value),
                ID = g.Key.id
            }).OrderBy(g => g.Average).ToList();

            string[] points = new string[2];
            points[0] = database.attributes.Where(a => a.id == data[data.Count() - 1].ID).FirstOrDefault().attr_name;
            points[1] = database.attributes.Where(a => a.id == data[0].ID).FirstOrDefault().attr_name;

            return points;
        }
    }


}
