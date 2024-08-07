using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ADOLibrary
{
    public  class EmployeeDataStore
    {
        SqlConnection connection;
        SqlCommand command;
        SqlDataReader reader;

        public EmployeeDataStore(string connectionString)
        {
            connection = new SqlConnection(connectionString);
        }

        //return all emps records
        //public   return  Name ([...]) { .... }

        public List<Employee> GetEmployees()
        {
            try
            {
                string sql = "Select empno, ename, hiredate, sal from emp";
                command = new SqlCommand(sql, connection);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                reader = command.ExecuteReader();

                List<Employee> employees = new List<Employee>();

                while (reader.Read())
                {
                    Employee employee = new Employee();
                    employee.EmpNo = (int)reader["empno"];
                    employee.EmpName = reader["ename"].ToString();
                    //employee.HireDate = (DateTime)reader["hiredate"];
                    //employee.Salary = (decimal)reader["sal"];

                    employee.HireDate = reader["hiredate"] as DateTime?;
                    employee.Salary = reader["sal"] as decimal?;

                  

                    //add employee object into collection
                    employees.Add(employee);
                }
                return employees;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        //search employee by empno
        public Employee GetEmpByNo(int empno)
        {
            try
            {
                string sql = "Select empno, ename, hiredate, sal from emp where empno = @eno";
                command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("eno", empno);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                reader = command.ExecuteReader();

                Employee employee = null;

                if (reader.Read())
                {
                     employee = new Employee();
                    employee.EmpNo = (int)reader["empno"];
                    employee.EmpName = reader["ename"].ToString();
                    //employee.HireDate = (DateTime)reader["hiredate"];
                    //employee.Salary = (decimal)reader["sal"];

                    employee.HireDate = reader["hiredate"] as DateTime?;
                    employee.Salary = reader["sal"] as decimal?;


                }
                return employee;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }


        //add employee record
        public int AddEmployee(Employee employee)
        {
            try
            {
                string sql = "Insert into emp(empno, ename, hiredate, sal) values (@empno, @ename, @hiredate, @sal) ";
                command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("empno", employee.EmpNo);
                command.Parameters.AddWithValue("ename", employee.EmpName);
                command.Parameters.AddWithValue("hiredate", employee.HireDate);
                command.Parameters.AddWithValue("sal", employee.Salary);

                //Check if any parameter value is null (i.e. C# null) convert that as database null value
                foreach (SqlParameter param in command.Parameters)
                {
                    if (param.Value == null)
                    {
                        param.Value = DBNull.Value;
                    }
                }

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                int count = command.ExecuteNonQuery();
                return count;
               
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        //modify employee record
        public int UpdateEmployee(Employee employee) {
            try
            {
                string sql = "update emp set ename = @ename, hiredate = @hiredate, sal = @sal where empno = @empno";

                command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("empno", employee.EmpNo);
                command.Parameters.AddWithValue("ename", employee.EmpName);
                command.Parameters.AddWithValue("hiredate", employee.HireDate);
                command.Parameters.AddWithValue("sal", employee.Salary);

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                int count = command.ExecuteNonQuery();

                return count;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        //remove employee record
        public int RemoveEmployee(int empno)
        {
            try
            {
                string sql = "delete from emp where empno = @empno ";

                command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("empno", empno);
              

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                int count = command.ExecuteNonQuery();

                return count;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        //add employee record using SP
        public int AddEmployee_sp(Employee employee)
        {
            try
            {
                
                command = new SqlCommand("INSERT_EMP", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("empno", employee.EmpNo);
                command.Parameters.AddWithValue("ename", employee.EmpName);
                command.Parameters.AddWithValue("hiredate", employee.HireDate);
                command.Parameters.AddWithValue("sal", employee.Salary);

                //Check if any parameter value is null (i.e. C# null) convert that as database null value
                foreach (SqlParameter param in command.Parameters)
                {
                    if (param.Value == null)
                    {
                        param.Value = DBNull.Value;
                    }
                }

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                int count = command.ExecuteNonQuery();
                return count;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        //SP WITH DATA
        public List<Employee> GetEmployeesByDept(int deptno)
        {
            try
            {              
                command = new SqlCommand("GETEMPSBYDEPT", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DEPTNO", deptno);

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                reader = command.ExecuteReader();

                List<Employee> employees = new List<Employee>();

                while (reader.Read())
                {
                    Employee employee = new Employee();
                    employee.EmpNo = (int)reader["empno"];
                    employee.EmpName = reader["ename"].ToString();
                    //employee.HireDate = (DateTime)reader["hiredate"];
                    //employee.Salary = (decimal)reader["sal"];

                    employee.HireDate = reader["hiredate"] as DateTime?;
                    employee.Salary = reader["sal"] as decimal?;



                    //add employee object into collection
                    employees.Add(employee);
                }
                return employees;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

    }
}
