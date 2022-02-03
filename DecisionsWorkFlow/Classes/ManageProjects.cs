using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using DecisionsWorkFlow.Database;
using System.Globalization;

namespace ProjectoSAD.ManageProjects
{
    public class Aluno {
        public string Name { get; set; }

        public string School { get; set; }

        public string SchoolNumber { get; set; }

        public string Nationality { get; set; }

        public double SaatyIndex { get; set; }

        public double DwfPoints { get; set; }

        public Aluno(string name, string school, string schoolNumber, string nationality, double saatyIndex, double dwfPoints)
        {
            Name = name;
            School = school;
            SchoolNumber = schoolNumber;
            Nationality = nationality;
            SaatyIndex = saatyIndex;
            DwfPoints = dwfPoints;
        }
    }

    public class ManageProjects
    {

        public ManageProjects(int projectId, int functionId)
        {
            DatabaseDataContext database = new DatabaseDataContext();
            ProjectId = projectId;
            //array preenchida com a importancia de 3 - de acordo com o numero de atributos
            int lenght = database.attributes.Where(a => a.project_id == ProjectId).Count();
            AtrWeight = database.functions_attributes.Where(fa => fa.func_id == functionId).Select(fa => fa.attr_weight).ToArray();
        }

        public long ProjectId { get; set; }
        public float?[] AtrWeight { get; set; }

        //lista de alunos com todos os dados necessarios
        //nome, escola, n_escola, ... (classe Aluno)
        public List<Aluno> getStudentData() {

            DatabaseDataContext database = new DatabaseDataContext();

            List<students> students = database.students.Where(s => s.project_id == ProjectId).OrderBy(s => s.id).ToList();

            int studentsCount = database.students.Where(s => s.project_id == ProjectId).Count();



            List<Aluno> alunos = new List<Aluno>();

            double[] saatyIndex = new double[studentsCount];

            //obter valores necessarios para o calculo do indice saaty
            database.attributes.Where(s => s.project_id == ProjectId).OrderBy(a => a.id).ToList().ForEach(atribute => {

                int?[] val = database.students_attributes.Where(sa => sa.attr_id == atribute.id).OrderBy(sa => sa.student_id).Select(sa => sa.attr_value).ToArray();

                double[] weightsMatrix = this.weightsMatrix(val, studentsCount);

                int indexI = 0;

                students.ForEach(student =>
                {

                    for (int i = 0; i < AtrWeight.Length; i++)
                    {
                        saatyIndex[indexI] += (double)AtrWeight[i] * (double)weightsMatrix[indexI];
                    }

                    indexI++;

                });
            });

            //atribuir indice saaty a cada aluno
            int indexJ = 0;
            students.ForEach(student => {
                double saatyIndexVal = Math.Round(saatyIndex[indexJ] / saatyIndex.Sum(), 3);
                int?[] valStudent = database.students_attributes.Where(sa => sa.student_id == student.id).OrderBy(sa => sa.attr_id).Select(sa => sa.attr_value).ToArray();
                alunos.Add(new Aluno(student.student_name,
                student.schools.school_name,
                student.student_id,
                new RegionInfo(student.national_code).DisplayName, saatyIndexVal, dwfPoints(valStudent, AtrWeight)));
                indexJ++;
            });
            List<Aluno> alunosOrdenados = alunos.OrderByDescending(aluno => aluno.DwfPoints).OrderByDescending(aluno => aluno.SaatyIndex).ToList();
            return alunosOrdenados;
        }

        //execucao de todas as funcoes necessarias para retornar
        //os valores necesssarios para o calculo final
        public float[] weightsMatrix(float[] values)
        {
            int lenght = (int)(values.Length * (double)((values.Length + 1) / 2)) - values.Length;
            float[,] val = new float[lenght,lenght];
            int index = 0;

            for (int i = 0; i < lenght; i++)
            {
                for (int j = 0; j < lenght; j++)
                {
                    if(i==j) { 
                        val[i,j] = 1;
                        val[j,i] = 1;
                    } else if (i<j)
                    {
                        val[i,j] = 1/values[index];
                        val[j,i] = values[index];
                        index++;
                    }
                }
            }

            return convertMatrix(val);

            /*int[,] weightsMatrix = new int[AtrWeight.Length, AtrWeight.Length];

            for (int i = 0; i < AtrWeight.Length; i++)
            {
                for (int j = 0; j < AtrWeight.Length; j++)
                {
                    weightsMatrix[i, j] = AtrWeight[i] - AtrWeight[j];
                }
            }

            return convertMatrix(weightsMatrix);*/

        }

        //normalizar matriz qualitativa - diferença entre os atributos
        public double[] weightsMatrix(int?[] AtrWeight, int studentCount) {

            int[,] weightsMatrix = new int[AtrWeight.Length, studentCount];

            for (int i = 0; i < AtrWeight.Length; i++)
            {
                for (int j = 0; j < studentCount; j++)
                {
                     weightsMatrix[i, j] = (int)(AtrWeight[i] - AtrWeight[j]);
                }
            }

            return convertMatrix(weightsMatrix);

        }

        //converter matriz qualitativa (1,2,3,4,5) para escala de saaty
        public double[] convertMatrix(int[,] matrix) {

            double[,] convertedMatrix = new double[matrix.GetLength(0),matrix.GetLength(1)];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++) {
                    convertedMatrix[i,j] = normalizeValue(matrix[i, j]);
                }

            }

            double[] valuesInLine = new double[convertedMatrix.GetLength(1)];

            double sum = 0;
            for (int i = 0; i < valuesInLine.Length; i++)
            {
                double notNormalizedValue = getRowValue(GetRow(convertedMatrix, i));
                valuesInLine[i] = notNormalizedValue;
                sum += notNormalizedValue;
            }

            for (int i = 0; i < valuesInLine.Length; i++)
            {
                double temp = valuesInLine[i];
                valuesInLine[i] = temp / sum;
            }

            return valuesInLine;

        }

        public float[] convertMatrix(float[,] matrix)
        {

            float[] valuesInLine = new float[matrix.GetLength(1)];

            float sum = 0;
            for (int i = 0; i < valuesInLine.Length; i++)
            {
                float notNormalizedValue = getRowValue(GetRow(matrix, i));
                valuesInLine[i] = notNormalizedValue;
                sum += notNormalizedValue;
            }

            for (int i = 0; i < valuesInLine.Length; i++)
            {
                float temp = valuesInLine[i];
                valuesInLine[i] = temp / sum;
            }

            return valuesInLine;

        }

        //normalizar valor da linha
        public double getRowValue(double[] row) { 

        double rowValue = 1;

        for (int i = 0; i < row.Length; i++)  {
                rowValue *= row[i];
        }

        double exp = 1 / (double)row.Length;

        return Math.Pow(rowValue, exp); ;

        }

        public float getRowValue(float[] row)
        {

            float rowValue = 1;

            for (int i = 0; i < row.Length; i++)
            {
                rowValue *= row[i];
            }

            float exp = 1 / (float)row.Length;

            return (float)Math.Pow(rowValue, exp); ;

        }

        //converter para escala de saaty
        public double normalizeValue(int weightNotNormalized) { 
            switch (weightNotNormalized)
            {
                case 4:
                    return 9;
                case 3:
                    return 7;
                case 2:
                    return 5;
                case 1:
                    return 3;
                case 0:
                    return 1;
                case -1:
                    return (double)1 / (double)3;
                case -2:
                    return (double)1 / (double)5;
                case -3:
                    return (double)1 / (double)7;
                case -4:
                    return (double)1 / (double)9;
                default:
                    return 1;
            }
        }

        public float normalizeValueWeights(int weightNotNormalized)
        {
            switch (weightNotNormalized)
            {
                case 1:
                    return 9;
                case 2:
                    return 7;
                case 3:
                    return 5;
                case 4:
                    return 3;
                case 5:
                    return 1;
                case 6:
                    return (float)1 / (float)3;
                case 7:
                    return (float)1 / (float)5;
                case 8:
                    return (float)1 / (float)7;
                case 9:
                    return (float)1 / (float)9;
                default:
                    return 1;
            }
        }

        //obter array 1D de uma array 2D
        public double[] GetRow(double[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                    .Select(x => matrix[rowNumber, x])
                    .ToArray();
        }

        public float[] GetRow(float[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                    .Select(x => matrix[rowNumber, x])
                    .ToArray();
        }

        //criterio de desempate - pontos dwf
        public int dwfPoints(int?[] val, float?[] matrix) {
            float max = 0;
            float atual = 0;

            for (int i = 0; i < val.Length; i++) {
                max += (float)(5 * matrix[i]);
                atual += (float)(val[i] * matrix[i]);
            }

            int returnVal = (int)Math.Ceiling(((double)atual / (double)max) * (double)100);

            return returnVal;
        }
    }


}
