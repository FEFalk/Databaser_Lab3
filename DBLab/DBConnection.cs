﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;


namespace DBLabs
{
    public class DBConnection : DBLabsDLL.DBConnectionBase
    {
        public SqlConnection SQLConnection;
        public SqlCommand SQLCmd;
        public string Connectionstring;
        ///*
        // * The constructor
        // */
        public DBConnection()
        {
            SQLConnection = new SqlConnection(Connectionstring);
        }

        /*
         * The function to logon to the database
         * 
         * Parameters:
         *              username    The userid used to login to SQL Server
         *              password    The password for the userid
         *              
         * Return value:
         *              true    successful login
         *              false   Error
         */
        public override bool login(string username, string password)
        {
            Connectionstring = "Data Source=www3.idt.mdh.se; Initial Catalog=ffg12002_db;User ID=" + username + ";Password=" + password + ";";
            try
            {
                SQLConnection.ConnectionString = Connectionstring;
                SQLConnection.Open();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
                return false;
            }
           
            return true;
        }
        /*
         --------------------------------------------------------------------------------------------
         IMPLEMENTATION TO BE USED IN LAB 2. 
         --------------------------------------------------------------------------------------------
         */

        public void changeProcedure(string procName)
        {
            SQLCmd = new SqlCommand(procName, SQLConnection);
            SQLCmd.CommandType = CommandType.StoredProcedure;
        }
        public void executeCommand()
        {
            using (SQLConnection)
            {
                SQLConnection.ConnectionString = Connectionstring;
                SQLConnection.Open();
                SQLCmd.ExecuteNonQuery();
            }
        }
        
    
        public bool checkStudentsTablePK(string studentID)
        {
            using (SQLConnection = new SqlConnection(Connectionstring))
            {
                SQLConnection.Open();
                SqlCommand oldcmd = new SqlCommand("SELECT COUNT(*) from dbo.Students WHERE [StudentID] = @id", SQLConnection);
                oldcmd.Parameters.Add("@id", SqlDbType.Char);
                oldcmd.Parameters["@id"].Value = studentID;

                if ((int)oldcmd.ExecuteScalar() >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /*
         --------------------------------------------------------------------------------------------
         STUB IMPLEMENTATIONS TO BE USED IN LAB 3. 
         --------------------------------------------------------------------------------------------
        */


        /********************************************************************************************
         * DATABASE UPDATING METHODS
         *******************************************************************************************/

        /*
         * Add a prerequisite for a course
         * 
         * Parameters:
         *              cc          CourseCode of the course on which to add a prerequisite
         *              preReqcc    CourseCode of the course that is the prerequisite
         *              
         * Return value:
         *              1           Prerequisite added
         *              Any other   Error
         */
        public override int addPreReq(string cc, string preReqcc)
        {
            using (SQLConnection = new SqlConnection(Connectionstring))
            {
                try
                {
                    changeProcedure("addCoursePreReq");
                    SQLCmd.Parameters.Add("@courseID", SqlDbType.NVarChar).Value = cc;
                    SQLCmd.Parameters.Add("@preReq", SqlDbType.NVarChar).Value = preReqcc;
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message);
                }
                executeCommand();
            }

            return 1;
        }

        /*
         * Add a course instance for a course
         * 
         * Parameters:
         *              cc          CourseCode of the course on which to add a course instance
         *              year        The year for the course instance
         *              period      The period for the course instance
         *              
         * Return value:
         *              1           Course instance added
         *              Any other   Error
         */
        public override int addInstance(string cc, int year, int period)
        {
            using (SQLConnection = new SqlConnection(Connectionstring))
            {
                try
                {
                    changeProcedure("addCourseInstance");
                    SQLCmd.Parameters.Add("@courseID", SqlDbType.NVarChar).Value = cc;
                    SQLCmd.Parameters.Add("@year", SqlDbType.Int).Value = year;
                    SQLCmd.Parameters.Add("@period", SqlDbType.Int).Value = period;
                    executeCommand();
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message);
                }
                
            }

            return 1;
        }

        /*
         * Add a teacher staffing for a course
         * 
         * Parameters:
         *              pnr         "Personnummer" for the teacher to staff
         *              cc          CourseCode of the course on which to add a teacher
         *              year        The year for the course instance
         *              period      The period for the course instance
         *              hours       The number of hours to staff the teacher
         *              
         * Return value:
         *              1           Teacher staffing added
         *              Any other   Error
         */
        //public override int addTeacher(string pnr, string cc, int year, int period, int hours)
        //{
        //    return 1;
        //}
        public override int addStaff(string pnr, string cc, int year, int period, int hours)
        {

            using (SQLConnection = new SqlConnection(Connectionstring))
            {
                try
                {
                    changeProcedure("addTeacher");
                    SQLCmd.Parameters.Add("@SSN", SqlDbType.Char).Value = pnr;
                    SQLCmd.Parameters.Add("@courseID", SqlDbType.NVarChar).Value = cc;
                    SQLCmd.Parameters.Add("@year", SqlDbType.Int).Value = year;
                    SQLCmd.Parameters.Add("@period", SqlDbType.Int).Value = period;
                    SQLCmd.Parameters.Add("@hours", SqlDbType.Int).Value = hours;
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message);
                }
                executeCommand();
            }

            return 1;
        }

        /*
         * Add a labassistant staffing for a course
         * 
         * Parameters:
         *              studid      StudentID for the student to staff
         *              cc          CourseCode of the course on which to add a labassistant
         *              year        The year for the course instance
         *              period      The period for the course instance
         *              hours       The number of hours to staff the student
         *              
         * Return value:
         *              1           Labassistant staffing added
         *              Any other   Error
         */
        public override int addLabass(string studid, string cc, int year, int period, int hours, int salary)
        {

            using (SQLConnection = new SqlConnection(Connectionstring))
            {
                try
                {
                    changeProcedure("addLabass");
                    SQLCmd.Parameters.Add("@studentID", SqlDbType.Char).Value = studid;
                    SQLCmd.Parameters.Add("@courseID", SqlDbType.NVarChar).Value = cc;
                    SQLCmd.Parameters.Add("@year", SqlDbType.Int).Value = year;
                    SQLCmd.Parameters.Add("@period", SqlDbType.Int).Value = period;
                    SQLCmd.Parameters.Add("@hours", SqlDbType.Int).Value = hours;
                    SQLCmd.Parameters.Add("@hourlySalary", SqlDbType.Int).Value = salary;
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message);
                }
                executeCommand();
            }

            return 1;
        }


        /*
         * Add a new course
         * 
         * Parameters:
         *              cc          CourseCode of the course on which to add a labassistant
         *              name        The name of the course
         *              credits     The number of credits for the course
         *              responsible The "personnummer" of the course responsible staff
         *              
         * Return value:
         *              1           Course added
         *              Any other   Error
         */
        public override int addCourse(string cc, string name, double credits, string responsible)
        {

            using (SQLConnection = new SqlConnection(Connectionstring))
            {
                try
                {
                    SQLCmd = new SqlCommand("addCourse", SQLConnection);
                    changeProcedure("addCourse");
                    SQLCmd.Parameters.Add("@CourseID", SqlDbType.VarChar).Value = cc;
                    SQLCmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = name;
                    SQLCmd.Parameters.Add("@Points", SqlDbType.Float).Value = credits;
                    SQLCmd.Parameters.Add("@SSN", SqlDbType.Char).Value = responsible;
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message);
                }
                executeCommand();
            }
            return 1;
        }



        /********************************************************************************************
         * DATABASE QUERYING METHODS
         *******************************************************************************************/

        /*
         * Get student data for all students
         * 
         * Parameters
         *              None
         * 
         * Return value:
         *              DataTable with the following columns:
         *                  StudentID       VARCHAR     StudentID for Students
         *                  FirstName       VARCHAR     Students First Name
         *                  LastName        VARCHAR     Students Last Name
         *                  Gender          VARCHAR     Students Gender
         *                  StreetAdress    VARCHAR     Students StreetAdress
         *                  ZipCode         VARCHAR     Students "PostNummer"
         *                  BirthDate       DATETIME    Students BirthDate
         *                  StudentType     VARCHAR     Student type (Program Student, Exchange Student etc)
         *                  City            VARCHAR     Students City
         *                  Country         VARCHAR     Students Country
         *                  program         VARCHAR     Name of the program the student is enrolled to
         *                  PgmStartYear    INTEGER     Year the student enrolled to the program
         *                  credits         FLOAT       The number of credits that the student has completed
         */
        public override DataTable getStudentData()
        {
            //Dummy code - Remove!
            //Please note that you do not use DataTables like this at all when you are using a database!!

            //string query = "SELECT * FROM STUDENTDATA";

            //DataTable dt = new DataTable();
            //dt.Columns.Add("StudentID");
            //dt.Columns.Add("FirstName");
            //dt.Columns.Add("LastName");
            //dt.Columns.Add("Gender");
            //dt.Columns.Add("Streetadress");
            //dt.Columns.Add("ZipCode");
            //dt.Columns.Add("Birthdate");
            //dt.Columns.Add("StudentType");
            //dt.Columns.Add("City");
            //dt.Columns.Add("Country");
            //dt.Columns.Add("program");
            //dt.Columns.Add("PgmStartYear");
            //dt.Columns.Add("credits");
            //dt.Rows.Add("ssn11001", "Stud", "Studman", "Male", "StudentRoad 1", "773 33", "1985-11-20 00:00:00", "Program Student", "Västerås", "Sweden", "Datavetenskapliga programmet", 2011, 15);

            string query = "select * from STUDENTDATA";
            DataTable dt = new DataTable();

            using (SQLConnection = new SqlConnection(Connectionstring))
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, SQLConnection);
                    SQLConnection.Open();
                    da.Fill(dt);
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message);
                }
            }

            return dt;
        }

        /*
         * Get list of staff
         * 
         * Parameters
         *              None
         *
         * Return value
         *              DataTable with the following columns:
         *                  pnr             VARCHAR     "Personnummer" for the staff
         *                  fullname        VARCHAR     Full name (First name and Last Name) of the staff.
         */
        public override DataTable getStaff()
        {
            //Dummy code - Remove!
            //Please note that you do not use DataTables like this at all when you are using a database!!
            string query = "select * from STAFFDATA";
            DataTable dt = new DataTable();

            using (SQLConnection = new SqlConnection(Connectionstring))
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, SQLConnection);
                    SQLConnection.Open();
                    da.Fill(dt);
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message);
                }               
            }
            return dt;
        }

        /*
         * Get list of Potential Labasses (i.e. students)
         * 
         * Parameters
         *              None
         *
         * Return value
         *              DataTable with the following columns:
         *                  StudentID       VARCHAR     StudentID for all students
         *                  fullname        VARCHAR     Full name (First name and Last Name) of the students.
         */
        public override DataTable getLabasses()
        {
            //Dummy code - Remove!
            //Please note that you do not use DataTables like this at all when you are using a database!!
            //DataTable dt = new DataTable();
            //dt.Columns.Add("StudentID");
            //dt.Columns.Add("fullname");
            //dt.Rows.Add("ssn11001", "Stud Studman");
            
            DataTable dt = new DataTable();

            using (SQLConnection = new SqlConnection(Connectionstring))
            {
                try
                {
                    string query = "select SSN as StudentID, Firstname+' '+Lastname as fullname from LABASSDATA";
                    SqlDataAdapter da = new SqlDataAdapter(query, SQLConnection);
                    SQLConnection.Open();
                    da.Fill(dt);
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message);
                }
            }

            return dt;
        }

        /*
         * Get course data
         * 
         * Parameters
         *              None
         * 
         * Return value
         *              DataTable with the following columns:
         *                  coursecode      VARCHAR     Course Code
         *                  name            VARCHAR     Name of the Course
         *                  credits         FLOAT       Credits of the course
         *                  courseresponsible VARCHAR   "Personnummer" for the course responsible teacher
         *                  examiner        VARCHAR     "Personnummer" for the examiner
         */
        public override DataTable getCourses()
        {
            //Dummy code - Remove!
            //Please note that you do not use DataTables like this at all when you are using a database!!
            string query = "select * from COURSEDATA";
            DataTable dt = new DataTable();

            using (SQLConnection = new SqlConnection(Connectionstring))
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, SQLConnection);
                    SQLConnection.Open();
                    da.Fill(dt);
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message);
                }
            }
            return dt;
        }
        /*
         * Returns the salary costs for a course instance based on the teacher and lab assistent staffing.
         * 
         * Parameters:
         *              cc          CourseCode to the course to calculate the cost
         *              year        The year for the course instance
         *              period      The period for the course instance
         *              
         * Return value:
         *              integer     The cost in currency (SEK)
         */
        public override int getCourseCost(string cc, int year, int period)
        {
            //Dummy code - Remove!
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();

            using (SQLConnection = new SqlConnection(Connectionstring))
            {
                try
                {
                    changeProcedure("getCourseCost");
                    SQLCmd.Parameters.Add("@courseID", SqlDbType.NVarChar).Value = cc;
                    SQLCmd.Parameters.Add("@year", SqlDbType.Int).Value = year;
                    SQLCmd.Parameters.Add("@period", SqlDbType.Int).Value = period;
                    da.SelectCommand = SQLCmd;

                    SQLConnection.Open();
                    da.Fill(dt);
                    int courseCost;
                    //Om datat vi får tillbaka av proceduren är tomt/null så ska vi inte konvertera det till en int.
                    if (dt.Rows[0].Field<int?>("CourseCost") != null)
                    {
                        courseCost = dt.Rows[0].Field<int>("CourseCost");
                        return courseCost;
                    }
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message);
                }
            }
            return 0;
        }

        /*
         * Returns the staffed persons (both teachers and lab assistants) for a course instance
         * 
         * Parameters:
         *              cc          CourseCode to the course to show staffing for
         *              year        The year for the course instance
         *              period      The period for the course instance
         *              
         * Return value:
         *              DataTable with the relevant information
         *                  The table should show name, number of hours, the Task in the course and the hourly salary
         */
        public override DataTable getCourseStaffing(string cc, string year, string period)
        {
            //Dummy code - Remove!
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();

            using (SQLConnection = new SqlConnection(Connectionstring))
            {
                try
                {
                    SQLCmd = new SqlCommand("getCourseStaffing", SQLConnection);
                    SQLCmd.Parameters.Add("@courseID", SqlDbType.NVarChar).Value = cc;
                    SQLCmd.Parameters.Add("@year", SqlDbType.Int).Value = year;
                    SQLCmd.Parameters.Add("@period", SqlDbType.Int).Value = period;
                    SQLCmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = SQLCmd;

                    SQLConnection.Open();
                    da.Fill(dt);
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message);
                }
            }

            return dt;

        }

        /*
         * Returns the student course transcript ("Ladokudrag")
         * 
         * Parameters:
         *              studId      StudentID for student to show transcript for
         *              
         * Return value:
         *              DataTable with the relevant information
         *                  See lab-instructions for more information about this DataTable
         */
        public override DataTable getStudentRecord(string studId)
        {
            //Dummy code - Remove!
            //DataTable dt = new DataTable();


            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();

            using (SQLConnection = new SqlConnection(Connectionstring))
            {
                try
                {
                    SQLCmd = new SqlCommand("getStudentRecord", SQLConnection);
                    SQLCmd.Parameters.Add(new SqlParameter("@studentID", studId));
                    SQLCmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = SQLCmd;

                    SQLConnection.Open();
                    da.Fill(dt);
                    dt.Columns.Remove("StudentID");
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message);
                }
            }


            return dt;
        }

        /*
         * Returns the a list of all courses that are prerequisites to a course.
         * 
         * Parameters:
         *              cc      Course Code for the course to list prerequisites
         *              
         * Return value:
         *              DataTable with the relevant information
         *                  The Table should show at least coursecode and course name for all prerequisite courses
         */
        public override DataTable getPreReqs(string cc)
        {
            //Dummy code - Remove!
            //Please note that you do not use DataTables like this at all when you are using a database!!
            //DataTable dt = new DataTable();
            //dt.Columns.Add("Course Code");
            //dt.Columns.Add("Course Name");
            //dt.Rows.Add("DVA111", "C# course");

            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();

            using (SQLConnection = new SqlConnection(Connectionstring))
            {
                try
                {
                    SQLCmd = new SqlCommand("getCoursePreReqs", SQLConnection);
                    SQLCmd.CommandType = CommandType.StoredProcedure;
                    SQLCmd.Parameters.Add("@courseID", SqlDbType.NVarChar).Value = cc;
                    da.SelectCommand = SQLCmd;

                    SQLConnection.Open();
                    da.Fill(dt);

                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message);
                }
            }


            return dt;
        }


        /*
         * Get course instances for a course
         * 
         * Parameters
         *              cc      Course Code for the course to list course instances
         * 
         * Return value
         *              DataTable with the following columns:
         *                  year            INTEGER     The year of the course instance
         *                  period          INTEGER     The period of the course instance
         *                  instance        VARCHAR     The "Display text" for the instance, e.g. year(period) or similar
         */
        public override DataTable getInstances(string cc)
        {

            //Dummy code - Remove!
            //Please note that you do not use DataTables like this at all when you are using a database!!
            //DataTable dt = new DataTable();
            //dt.Columns.Add("year");
            //dt.Columns.Add("period");
            //dt.Columns.Add("instance");
            //dt.Rows.Add(2012, 4, "2012 p4");

            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();

            using (SQLConnection = new SqlConnection(Connectionstring))
            {
                try
                {
                    SQLCmd = new SqlCommand("getCourseInstance", SQLConnection);
                    SQLCmd.CommandType = CommandType.StoredProcedure;
                    SQLCmd.Parameters.Add("@courseID", SqlDbType.NVarChar).Value = cc;
                    da.SelectCommand = SQLCmd;

                    SQLConnection.Open();
                    da.Fill(dt);

                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message);
                }
            }

            return dt;
        }

        /*
        * Get list of telephone numbers for a student
        * 
        * Parameters
        *              studId      StudentID for the student
        * 
        * Return value
        *              DataTable with the following columns:
        *                  Type            VARCHAR     The type of telephone number (e.g., Home, Work, Cell etc.)
        *                  Number          VARCHAR     The telephone number
        */
        public override DataTable getStudentPhoneNumbers(string studId)
        {
            //Dummy code - Remove!
            //Please note that you do not use DataTables like this at all when you are using a database!!
            //DataTable dt = new DataTable();
            //dt.Columns.Add("Type");
            //dt.Columns.Add("Number");
            //dt.Rows.Add("Home", "021-121212");


            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();

            using (SQLConnection = new SqlConnection(Connectionstring))
            {
                try
                {
                    SQLCmd = new SqlCommand("getStudentPhoneNo", SQLConnection);
                    SQLCmd.Parameters.Add(new SqlParameter("@studentID", studId));
                    SQLCmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = SQLCmd;

                    SQLConnection.Open();
                    da.Fill(dt);
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message);
                }
            }


            return dt;
        }

        /*
        --------------------------------------------------------------------------------------------
         STUB IMPLEMENTATIONS TO BE USED IN LAB 4. 
        --------------------------------------------------------------------------------------------
        */


        /*
        * Get list years which have course instances
        * 
        * Parameters
        *              None      
        * 
        * Return value
        *              DataTable with the following column:
        *                  Year            INTEGER     A unique (no duplicates) list of all years which has course instances
        */
        public override DataTable getStaffingYears()
        {
            //Dummy code - Remove!
            //Please note that you do not use DataTables like this at all when you are using a database!!
            
            
            DataTable dt = new DataTable();

            using (SQLConnection = new SqlConnection(Connectionstring))
            {
                try
                {
                    string query = "select CAST(Year as varchar(6)) as Year from STAFFINGYEARS";
                    SqlDataAdapter da = new SqlDataAdapter(query, SQLConnection);
                    SQLConnection.Open();
                    da.Fill(dt);
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message);
                }
            }
            return dt;
        }

        /*
        * Get a matrix of all staffing for a year
        * 
        * Parameters
        *              year     The year to show staffings for      
        * 
        * Return value
        *              DataTable with suitable format
        *                  For more information about the format, see Lab instructions for lab 4
        */
        public override DataTable getStaffingGrid(string year)
        {
            //Dummy code - Remove!
            //Please note that you do not use DataTables like this at all when you are using a database!!
            
            DataTable dt = new DataTable();
            dt.Columns.Add("");
            dt.Columns.Add("");
            dt.Columns.Add("");
            dt.Columns.Add("");
            dt.Columns.Add("");
            dt.Columns.Add("");


            using (SQLConnection = new SqlConnection(Connectionstring))
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    SQLCmd = new SqlCommand("getStaffingGrid", SQLConnection);
                    SQLCmd.Parameters.Add("@Year", SqlDbType.Int).Value = year;
                    SQLCmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = SQLCmd;

                    SQLConnection.Open();
                    da.Fill(dt);
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message);
                }
            }

            return dt;
        }
    }
}



//                    if (dt.Rows.Count == 0)
//                    {
//                        SQLConnection.Close();
//                        dt = new DataTable();
//                        string query = "select FirstName, LastName from TEACHERDATA";
//                        da = new SqlDataAdapter(query, SQLConnection);
//                        SQLConnection.Open();
//                        da.Fill(dt);
//                        dt.Columns.Add("");
//                        dt.Columns.Add("");
//                        dt.Columns.Add("");
//                        dt.Columns.Add("");
//                    }