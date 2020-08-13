using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "GP_Service" in code, svc and config file together.
public class GP_Service : IGP_Service
{
    //connect to dbml
    DataClassesDataContext db = new DataClassesDataContext();

    public int login(string email, string password)
    {
        //check if user information is in the database
        var user = (from u in db.Users
                    where u.Email.Equals(email) && u.Password.Equals(password)
                    select u).FirstOrDefault();


        if(user != null)
        {
            //if user with given email and password exists, return their ID
            return user.ID;
        }
        else
        {
            return 0;
        }
    }

    int Register(string name, string surname, string email, string password, string number)
    {
        //check if a user with the given email exists
        var user = (from u in db.Users
                    where u.Email.Equals(email)
                    select u).FirstOrDefault();

        //if the given email is unique
        if(user == null)
        {
            var newUser = new User
            {
                Name = name,
                Surname = surname,
                Email = email,
                Password = password,
                PhoneNumber = number
            };
            db.Users.InsertOnSubmit(newUser);

            try
            {
                //all is well
                db.SubmitChanges();
                return 1;
            }
            catch (Exception ex)
            {
                ex.GetBaseException();
                return -1;
            }

        }
        else
        {
            //a user with the given email already exists
            return 0;
        }
    }
}
