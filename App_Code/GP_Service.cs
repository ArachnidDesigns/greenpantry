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

    public int Register(string name, string surname, string email, string password, string number)
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

            var c = new Customer
            {
                CustomerID = newUser.ID,
                Points = 0
            };
            db.Customers.InsertOnSubmit(c);

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

    public int UpdatePassword(int id, string oldPassword, string newPassword)
    {
        var user = getUser(id);

        if(user == null)
        {
            //user does not exist
            return 0;
        }
        else
        {
            if(user.Password != oldPassword)
            {
                //given password doesn't match existing password
                return -2;
            }
            else
            {
                user.Password = newPassword;

                try
                {
                    //all is well
                    db.SubmitChanges();
                    return 1;
                }
                catch(Exception ex)
                {
                    //something else went wrong
                    ex.GetBaseException();
                    return -1;
                }
            }
        }

    }

    public int UpdateUserDetails(int id, string name, string surname, string email, string number)
    {
        //check if the given email is already in use
        var tempUser = (from u in db.Users
                        where u.Email.Equals(email) && u.ID != id
                        select u).FirstOrDefault();

        if(tempUser != null)
        {
            //the email they're trying to change to is already in use
            return -2;
        }
        else
        {
            var user = getUser(id);

           if(user != null)
            {
                user.Name = name;
                user.Surname = surname;
                user.Email = email;
                user.PhoneNumber = number;

                try
                {
                    db.SubmitChanges();
                    return 1;
                }
                catch(Exception ex)
                {
                    ex.GetBaseException();
                    return 1;
                }
            }
            else
            {
                //user doesn't exist
                return 0;
            }
           
        }

    }


    //Function used to return a product record
    public Product getProduct(int Product_ID)
    {
        var product = (from p in db.Products
                       where p.ID.Equals(Product_ID)
                       select p).FirstOrDefault();

        if(product == null)
        {
            return null;
        }else
        {
            return product;
        }
    }

    //Function that allows to subtract the items purchased from the stock
    public int UpdateStock(int P_ID, int ItemsPurchased)
    {
        var product = getProduct(P_ID);
        if(product == null)
        {
            return 0;   
        }else
        {
            //Subtract the items purchased from the stock of the particular product
            product.StockOnHand -= ItemsPurchased;
            try
            {
                db.SubmitChanges();
                return 1;
            }catch(Exception e)
            {
                e.GetBaseException();
                return -1;
            }
        }
        
    }

    //Functions used for adding items to a shopping list
    public int AddItemsToShoppingList(int ListID , int ShoppingList_ID, int Product_ID, int quantity)
    {
        var item = (from i in db.ListItems
                    where i.ID.Equals(ListID)
                    select i).FirstOrDefault();
        //if the item does not exist on the shopping list
        if(item == null)
        {
            var newItem = new ListItem
            {
                ListID = ShoppingList_ID,
                ProductID = Product_ID,
                Quantity_ = quantity

            };
            db.ListItems.InsertOnSubmit(newItem);
            try
            {
                db.SubmitChanges();
                return 1;
            }catch(Exception e)
            {
                e.GetBaseException();
                return -1;
            }
        }else
        {
            return 0;
        }
    }

    //Function used to get a particular user based on its ID
    public User getUser(int User_ID)
    {
        var UserInfo = (from u in db.Users
                        where u.ID.Equals(User_ID)
                        select u).FirstOrDefault();

        if(UserInfo == null)
        {
            return null;
        }else
        {
            return UserInfo;
        }
    }

    //Function used to find the total number of users for the website
    public int getNumUsers()
    {
        var TotalUsers = 0;
        var user = (from u in db.Users
                    select u);

        foreach(User usr in user)
        {
            TotalUsers += 1;
        }

        return TotalUsers;
    }

    //Function used to calculate the total profit generated 
    public double CalculateProfit()
    {
        var totalprofit = 0.0;
        var Difference = 0.0;
        var CurrentProfit = 0.0;

        //Storing all the ordered items in a variable
        var OrdItems = (from i in db.OrderItems
                        select i);

        foreach(OrderItem o in OrdItems)
        {
             CurrentProfit = 0.0;
            //getting the current product
            var CurrentProduct = getProduct(o.ProductID);

            //calculating the difference (Selling price - Cost price)
            Difference = (double)(CurrentProduct.Price - CurrentProduct.Cost);

            //multiplying the difference with the number of products of a particular type sold
            CurrentProfit = Difference * o.Qty;

            //Adding the current profit to the total profit
            totalprofit += CurrentProfit;

        }
        return totalprofit;


    }
}
