using System;
using System.Collections.Generic;
using System.Linq;

//email
using System.Web;
using System.Net.Mail;
using System.Activities.Expressions;

public class recommended
{
    public Product product;
    public double rating;
}

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "GP_Service" in code, svc and config file together.
public class GP_Service : IGP_Service
{
    //connect to dbml
    DataClassesDataContext db = new DataClassesDataContext();

    //USER MANAGEMENT
    //PRODUCT MANAGEMENT
    //CATEGORY MANAGEMENT
    //SUBCATEGORY MANAGEMENT
    //SHOPPING LIST MANAGEMENT
    //LIST MANAGEMENT
    //ADDRESS MANAGEMENT
    //INVOICE MANAGEMENT
    //IINVOICE LINE MANAGEMENT
    //CARD MANAGEMENT
    //DEVICE MANAGEMENT
    //REPORT MANAGEMENT

    //USER MANAGEMENT -------------------------------------------------------------------------------------------------

    //Login 

    public void newsletter(string senderemail, string subscriberemail, string subject, string body,string password,string smtp )
    {

        MailMessage mail = new MailMessage(senderemail, subscriberemail, subject, body);
        SmtpClient client = new SmtpClient(smtp);
        client.UseDefaultCredentials = false;
        client.Port = 25; //this port is used by gmail and also 465 587
        client.Credentials = new System.Net.NetworkCredential(senderemail, password);
        client.EnableSsl = true; //ssl is required by gmail
        
        client.Send(mail);
        

    }
    public int login(string email, string password)
    {
        //check if user information is in the database
        var user = (from u in db.Users
                    where u.Email.Equals(email) && u.Password.Equals(Secrecy.HashPassword(password))
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

    //Register 
    public int register(string name, string surname, string email, string password, string status, DateTime date, string userType)
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
                Password = Secrecy.HashPassword(password),
                Status = status,
                DateRegistered = date,
                UserType = userType
            };
            db.Users.InsertOnSubmit(newUser);

            try
            {
                //all is well
                db.SubmitChanges();
                return newUser.ID;
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

    //Add phone number 
    public int addUserNumber(int id, string number)
    {
        var user = (from u in db.Users
                    where u.ID.Equals(id)
                    select u).FirstOrDefault();
       
        if(user != null)
        {
            user.PhoneNumber = number;

            try
            {
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
            return 0;
        }
    }

    //Make user profile inactive 
    public int removeUser(int id)
    {
        var user = (from u in db.Users
                    where u.ID.Equals(id)
                    select u).FirstOrDefault();

        if(user != null)
        {
            user.Status = "inactive";

            try
            {
                db.SubmitChanges();
                return 1;
            }
            catch(Exception ex)
            {
                ex.GetBaseException();
                return -1;
            }
        }
        else
        {
            //can't find users
            return 0;
        }
    }

    //Update user password 
    public int updatePassword(int id, string oldPassword, string newPassword)
    {
        //getUser
        var user = (from u in db.Users
                    where u.ID.Equals(id)
                    select u).FirstOrDefault();

        if(user == null)
        {
            //user does not exist
            return 0;
        }
        else
        {
            if(user.Password != Secrecy.HashPassword(oldPassword))
            {
                //given password doesn't match existing password
                return -2;
            }
            else
            {
                user.Password = Secrecy.HashPassword(newPassword);

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

    //Update user's details 
    public int updateUserDetails(int id, string name, string surname, string email, string number)
    {
        //check if the given email is already in use
        var tempUser = (from u in db.Users
                        where u.Email.Equals(email) && u.ID != id
                        select u).FirstOrDefault();

        if (tempUser != null)
        {
            //the email they're trying to change to is already in use
            return -1;
        }
        else
        {
            var user = (from u in db.Users
                        where u.ID.Equals(id)
                        select u).FirstOrDefault();

            if (user != null)
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
                catch (Exception ex)
                {
                    ex.GetBaseException();
                    return 0;
                }
            }
            else
            {
                //user doesn't exist
                return 0;
            }  
        }
    }

    public int updateUserAdmin(int userID, int points, string usertype, string status)
    {
        //check if user exists
        var user = (from u in db.Users
                        where u.ID.Equals(userID)
                        select u).FirstOrDefault();

        if (user == null)
        {
            //the user does not exist
            return -1;
        }
        else
        {
            user.Points = points;
            user.UserType = usertype;
            user.Status = status;

            try
            {
                db.SubmitChanges();
                return 1;
            }
            catch (Exception ex)
            {
                ex.GetBaseException();
                return 0;
            }
        }
    }

    //Function used to get a particular user based on its ID
    public User getUser(int User_ID)
    {
        var UserInfo = (from u in db.Users
                        where u.ID.Equals(User_ID)
                        select u).FirstOrDefault();

        if (UserInfo == null)
        {
            return null;
        }
        else
        {
            var newuser = new User
            {
                ID = UserInfo.ID,
                Name = UserInfo.Name,
                Surname = UserInfo.Surname,
                Email = UserInfo.Email,
                Password = UserInfo.Password,
                PhoneNumber = UserInfo.PhoneNumber,
                Status = UserInfo.Status,
                DateRegistered = UserInfo.DateRegistered,
                UserType = UserInfo.UserType
            };
            return newuser;
        }
    }

    public List<User> getAllUsers()
    {
        dynamic users = (from u in db.Users
                         select u);
        List<User> userList = new List<User>();

        foreach(User u in users)
        {
            var newuser = new User
            {
                ID = u.ID,
                Name = u.Name,
                Surname = u.Surname,
                Email = u.Email,
                Password = u.Password,
                PhoneNumber = u.PhoneNumber,
                Status = u.Status,
                DateRegistered = u.DateRegistered,
                UserType = u.UserType
            };
            userList.Add(newuser);
        }
        return userList;
    }

    //Function used to find the total number of users for the website
    public int getNumUsers()
    {
        var TotalUsers = 0;
        var user = (from u in db.Users
                    select u);

        foreach (User usr in user)
        {
            TotalUsers += 1;
        }
        return TotalUsers;
    }

    //ADDRESS MANAGEMENT -------------------------------------

    //Function used to return the Address
    public Address getAddress(int userID)
    {
        var Addressinfo = (from a in db.Addresses
                           where a.CustomerID.Equals(userID)
                           select a).FirstOrDefault();

        if (Addressinfo == null)
        {
            return null;
        }
        else
        {
            var tempAddress = new Address
            {
                ID = Addressinfo.ID,
                CustomerID = Addressinfo.CustomerID,
                Type = Addressinfo.Type,
                Billing = Addressinfo.Billing,
                Line1 = Addressinfo.Line1,
                Line2 = Addressinfo.Line2,
                Suburb = Addressinfo.Suburb,
                City = Addressinfo.City,
                Province = Addressinfo.Province
            };
            return tempAddress;
        }
    }
    //method used to add a new address into the database
    public int addAddress(string line1, string line2, string suburb, string city, char billing, string type, int C_ID, string Province)
    {
        var newAddress = new Address
        {
            Line1 = line1,
            Line2 = line2,
            Suburb = suburb,
            City = city,
            Billing = billing,
            Type = type,
            CustomerID = C_ID,
            Province = Province
        };

        db.Addresses.InsertOnSubmit(newAddress);
        try
        {
            db.SubmitChanges();
            return 1;
        }
        catch (Exception e)
        {
            e.GetBaseException();
            return -1;
        }
    }

    //method used to update the address
    public int updateAddress(string line1, string line2, string suburb, string city, string province, char billing, string type, int Cus_ID)
    {
        var address = (from ad in db.Addresses
                           where ad.CustomerID.Equals(Cus_ID)
                           select ad).FirstOrDefault();
        
        if (address == null)
        {
            return -1;
        }
        else
        {
            address.Line1 = line1;
            address.Line2 = line2;
            address.Suburb = suburb;
            address.City = city;
            address.Province = province;
            address.Billing = billing;
            address.Type = type;

            try
            {
                db.SubmitChanges();
                return 1;
            }
            catch (Exception ex)
            {
                ex.GetBaseException();
                return -1;
            }
        }
    }

    public int updatePoints(int Cust_ID, int points)
    {
        var user = (from u in db.Users
                    where u.ID.Equals(Cust_ID)
                    select u).FirstOrDefault();

        if (user != null)
        {
            user.Points = points;

            try
            {
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
            //can''t find user
            return 0;
        }
    }

    public int getUserPoints(int Cus_ID)
    {
        User user = (from p in db.Users
                     where p.ID.Equals(Cus_ID)
                     select p).FirstOrDefault();
        if (user == null)
        {
            //can't find user
            return 0;
        }
        else
        {
            int points = Convert.ToInt32(Math.Round(user.Points * 0.1));
            return points;
        }
    }

    //PRODUCT MANAGEMENT ---------------------------------------------------------------

    //add new product  
    public int addNewProduct(string name, int SubID, double price, double cost, int stockQty, string imgLocation,string status,string description)
    {
        var newProduct = new Product
        {
            Name = name,
            SubCategoryID = SubID,
            Price = (decimal)price,
            Cost = (decimal) cost,
            StockOnHand = stockQty,
            Image_Location = imgLocation,
            Status = status,
            Description = description
        };

        db.Products.InsertOnSubmit(newProduct);

        try
        {
            //product successfully added
            db.SubmitChanges();
            return newProduct.ID;
        }
        catch (Exception ex)
        {
            //error occurred when attempting to add product
            ex.GetBaseException();
            return -1;
        }
    }

    //Function to update product specifics  
    public int updateProduct(int id, string name, int SubId, double price, double cost, string imgLocation, string status, int stock, string description)
    {
        var product = (from p in db.Products
                       where p.ID.Equals(id)
                       select p).FirstOrDefault();

        if(product == null)
        {
            //invalid product id
            return 0;
        }
        else
        {
            product.Name = name;
            product.SubCategoryID = SubId;
            product.Price = (decimal)price;
            product.Cost = (decimal)cost;
            product.Image_Location = imgLocation;
            product.Status = status;
            product.StockOnHand = stock;
            product.Description = description;

            try
            {
                //changes successfully submitted
                db.SubmitChanges();
                return 1;
            }
            catch(Exception ex)
            {
                //Somethinf went wrong
                ex.GetBaseException();
                return -1;
            }
        }
    }

    //return all products in a list  
    public List<Product> getAllProducts()
    {
        dynamic productsList = new List<Product>();

        dynamic products = (from p in db.Products
                            select p);

        if(products == null)
        {
            return null;
        }
        else
        {
            foreach(Product pr in products)
            {
                var tempProduct = new Product
                {
                    ID = pr.ID,
                    Name = pr.Name,
                    Cost = pr.Cost,
                    Price = pr.Price,
                    Image_Location = pr.Image_Location,
                    StockOnHand = pr.StockOnHand,
                    SubCategoryID = pr.SubCategoryID,
                    Status = pr.Status,
                    Description = pr.Description
                };
                productsList.Add(tempProduct);
            }
        }
        return productsList;
    }

    //wrong need to set to inactive  
    public int removeProduct(int productId)
    {
        var product = (from p in db.Products
                       where p.ID.Equals(productId)
                       select p).FirstOrDefault();

        if(product == null)
        {
            //product does not exist
            return 0;
        }
        else
        {
            product.Status = "inactive";

            try
            {
                //successfully deleted
                db.SubmitChanges();
                return 1;
            }
            catch(Exception ex)
            {
                //something went wrong when trying to delete product
                ex.GetBaseException();
                return -1;
            }
        }         
    }

    //Get product by ProductID
    public Product getProduct(int Product_ID)
    {
        var product = (from p in db.Products
                       where p.ID.Equals(Product_ID)
                       select p).FirstOrDefault();

        if (product == null)
        {
            return null;
        }
        else
        {
            var rProduct = new Product
            {
                ID = product.ID,
                Name = product.Name,
                SubCategoryID = product.SubCategoryID,
                Price = product.Price,
                Cost = product.Cost,
                StockOnHand = product.StockOnHand,
                Image_Location = product.Image_Location,
                Description = product.Description,
                Status = product.Status
            };
            return rProduct;
        }
    }

    //Update stock per product
    public int updateStock(int P_ID, int ItemsPurchased)
    {
        var product = getProduct(P_ID);
        if (product == null)
        {
            return 0;
        }
        else
        {
            //Subtract the items purchased from the stock of the particular product
            product.StockOnHand -= ItemsPurchased;
            try
            {
                db.SubmitChanges();
                return 1;
            }
            catch (Exception e)
            {
                e.GetBaseException();
                return -1;
            }
        }
    }

    public int getNumProductsInSub(int subID)
    {
        dynamic product = (from p in db.Products
                           where p.SubCategoryID.Equals(subID)
                           select p);
        var ProductList = new List<Product>();
        int count = 0;

        foreach (Product pr in product)
        {
            count++;
        }
        return count;
    }

    //Method used to get all the products present within a Category
    public List<Product> getProductByCat(int Cat_ID)
    {
        dynamic subcategories = (from s in db.SubCategories
                                 where s.CategoryID.Equals(Cat_ID)
                                 select s);
        var ProductList = new List<Product>();

        foreach (SubCategory sb in subcategories)
        {
            dynamic products = (from p in db.Products
                                where p.SubCategoryID.Equals(sb.SubID)
                                select p);

            foreach (Product pr in products)
            {
                var tempProduct = new Product
                {
                    ID = pr.ID,
                    Name = pr.Name,
                    SubCategoryID = pr.SubCategoryID,
                    Price = pr.Price,
                    Cost = pr.Cost,
                    StockOnHand = pr.StockOnHand,
                    Image_Location = pr.Image_Location,
                    Status = pr.Status,
                    Description = pr.Description
                };

                ProductList.Add(tempProduct);
            }
        }
        return ProductList;
    }

    //Method used to return all the products present in a sub category
    public List<Product> getProductBySubCat(int Sub_ID)
    {
        dynamic product = (from p in db.Products
                           where p.SubCategoryID.Equals(Sub_ID)
                           select p);
        var ProductList = new List<Product>();

        foreach (Product pr in product)
        {
            var tempPro = new Product
            {
                ID = pr.ID,
                Name = pr.Name,
                SubCategoryID = pr.SubCategoryID,
                Price = pr.Price,
                Cost = pr.Cost,
                StockOnHand = pr.StockOnHand,
                Image_Location = pr.Image_Location,
                Status = pr.Status,
                Description = pr.Description
            };

            ProductList.Add(tempPro);
        }
        return ProductList;

    }   
    
    //basic search
    public List<Product> searchProducts(string input)
    {
        List<Product> productList = new List<Product>();
        dynamic product = (from p in db.Products
                           select p);
        dynamic category = (from c in db.ProductCategories
                            select c);
        dynamic subcategories = (from s in db.SubCategories
                                 select s);

        //Regex r = new Regex(@input.ToUpper() + "?");

        foreach (Product p in product)
        {
            //MatchCollection matchedNames = r.Matches(p.Name.ToUpper());
            //if(r.IsMatch(p.Name.ToUpper()))
            if (p.Name.ToUpper().Contains(input.ToUpper()))
            {
                dynamic pro = helpAllocate(p);
                productList.Add(pro);
            }
        }
        foreach (SubCategory s in subcategories)
        {
            //if (r.IsMatch(s.Name.ToUpper()))
            if (s.Name.ToUpper().Contains(input.ToUpper()))
            {
                dynamic productbysubCat = getProductBySubCat(s.SubID);

                foreach (Product p in productbysubCat)
                {
                    dynamic pro = helpAllocate(p);

                    Boolean contains = false;
                    foreach (Product pr in productList)
                    {
                        if (pr.ID.Equals(pro.ID))
                        {
                            contains = true;
                        }
                    }
                    if (contains.Equals(false))
                    {
                        productList.Add(pro);
                    }
                }
            }
        }
        foreach (ProductCategory c in category)
        {
            //if (r.IsMatch(c.Name.ToUpper()))
            if (c.Name.ToUpper().Contains(input.ToUpper()))
            {
                dynamic productbycat = getProductByCat(c.ID);

                foreach (Product pr in productbycat)
                {
                    dynamic pro = helpAllocate(pr);

                    Boolean contains = false;
                    foreach (Product p in productList)
                    {
                        if (p.ID.Equals(pro.ID))
                        {
                            contains = true;
                        }
                    }
                    if (contains.Equals(false))
                    {
                        productList.Add(pro);
                    }

                    //if (productList.Contains(pr) == false)
                    //  productList.Add(pro);
                }
            }
        }
        return productList;
    }

    //helper method for search (could be used in other functions)
    Product helpAllocate(Product p)
    {
        var tempProduct = new Product
        {
            ID = p.ID,
            Name = p.Name,
            Cost = p.Cost,
            Price = p.Price,
            Image_Location = p.Image_Location,
            SubCategoryID = p.SubCategoryID,
            StockOnHand = p.StockOnHand,
            Description = p.Description,
            Status = p.Status
        };
        return tempProduct;
    }

    //CATEGORY MANAGEMENT -------------------------------------------------------------------------

    //Function that returns all the Product Categories
    public List<ProductCategory> getAllCategories()
    {
        dynamic categories = new List<ProductCategory>();

        dynamic cats = (from c in db.ProductCategories
                        select c);

        foreach(ProductCategory pc in cats)
        {

            var tempCat = new ProductCategory
            {
                ID = pc.ID,
                Name = pc.Name,
                Status = pc.Status
            };

            categories.Add(tempCat);
        }

        return categories;
    }

    public ProductCategory getCat(int C_ID)
    {
        dynamic cat = (from c in db.ProductCategories
                       where c.ID.Equals(C_ID)
                       select c).FirstOrDefault();

        if (cat == null)
        {
            return null;
        }
        else
        {
            var tempcat = new ProductCategory
            {
                Name = cat.Name,
                ID = cat.ID,
                Status = cat.Status
            };
            return tempcat;
        }
    }

    public ProductCategory getCategorybyProductID(int p_ID)
    {
        dynamic product = getProduct(p_ID);
        dynamic subcategory = getSubCat(product.SubCategoryID);

        dynamic category = getCat(subcategory.CategoryID);

        return category;
    }

    public int addCategory(string name,string status)
    {
        var newCat = new ProductCategory
        {
            Name = name,
            Status = status
        };

        db.ProductCategories.InsertOnSubmit(newCat);

        try
        {
            db.SubmitChanges();
            return newCat.ID;
        }
        catch (Exception ex)
        {
            ex.GetBaseException();
            return -1;
        }
    }

    public int updateCategories(int id, string name,string status)
    {
        var category = (from c in db.ProductCategories
                        where c.ID.Equals(id)
                        select c).FirstOrDefault();

        if (category != null)
        {
            category.Name = name;
            category.Status = status;

            try
            {
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

            return 0;
        }
    }

    public int removeCategory(int catID)
    {
        var cat = (from c in db.ProductCategories
                   where c.ID.Equals(catID)
                   select c).FirstOrDefault();

        if (cat != null)
        {
            cat.Status = "inactive";

            try
            {
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
            //can't find users
            return 0;
        }
    }

    //SUBCATEGORY MANAGEMENT ----------------------------------------------------------

    public int addSubCategory(int catid, string name, string status)
    {
        var newSub = new SubCategory
        {
            Name = name,
            CategoryID = catid,
            Status = status
        };

        db.SubCategories.InsertOnSubmit(newSub);
        try
        {
            //sub successfully added
            db.SubmitChanges();
            return newSub.SubID;
        }
        catch (Exception ex)
        {
            //error occurred when attempting to add sub
            ex.GetBaseException();
            return -1;
        }
    }

    //Function that returns all the Product SubCategories
    public List<SubCategory> getAllSubCategories()
    {
        dynamic subcategories = new List<SubCategory>();

        dynamic subs = (from s in db.SubCategories
                        select s);

        foreach (SubCategory sc in subs)
        {
            var tempSubCat = new SubCategory
            {
                SubID = sc.SubID,
                Name = sc.Name,
                CategoryID = sc.CategoryID,
                Status = sc.Status
            };

            subcategories.Add(tempSubCat);
        }

        return subcategories;
    }

    public SubCategory getSubCat(int S_ID)
    {
        dynamic subcat = (from s in db.SubCategories
                          where s.SubID.Equals(S_ID)
                          select s).FirstOrDefault();

        if (subcat == null)
        {
            return null;
        }
        else
        {
            var tempsub = new SubCategory
            {
                SubID = subcat.SubID,
                Name = subcat.Name,
                CategoryID = subcat.CategoryID,
                Status = subcat.Status
            };
            return tempsub;
        }
    }

    public List<SubCategory> getSubCatPerCat(int c_ID)
    {
        dynamic subcat = (from s in db.SubCategories
                          where s.CategoryID.Equals(c_ID)
                          select s);

        var SubList = new List<SubCategory>();

        foreach (SubCategory sc in subcat)
        {
            var tempsub = new SubCategory
            {
                SubID = sc.SubID,
                Name = sc.Name,
                CategoryID = sc.CategoryID,
                Status = sc.Status
            };
            SubList.Add(tempsub);
        }
        return SubList;
    }

    public int removeSubCat(int subID)
    {
        var sub = (from s in db.SubCategories
                   where s.SubID.Equals(subID)
                   select s).FirstOrDefault();

        if (sub != null)
        {
            sub.Status = "inactive";

            try
            {
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
            //can't find users
            return 0;
        }
    }

    public int updateSubCategories(int id,int cat_ID, string name,string status)
    {
        var subcategory = (from sc in db.SubCategories
                           where sc.SubID.Equals(id)
                           select sc).FirstOrDefault();

        if (subcategory != null)
        {
            subcategory.Name = name;
            subcategory.CategoryID = cat_ID;
            subcategory.Status = status;

            try
            {
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
            return 0;
        }
    }

    //INVOICE MANAGEMENT -----------------------------------------------------------

    //Get Invoice by InvoiceID
    public Invoice getInvoice(int InvoiceID)
    {
        var order = (from o in db.Invoices
                     where o.ID.Equals(InvoiceID)
                     select o).FirstOrDefault();

        if(order == null)
        {
            return null;
        }
        else
        {
            var temp = new Invoice
            {
                ID = order.ID,
                CustomerID = order.CustomerID,
                Status = order.Status,
                Date = order.Date,
                DeliveryDatetime = order.DeliveryDatetime,
                Notes = order.Notes,
                Total = order.Total,
                Points = order.Points
            };
            return temp;
        }
    }

    //add invoice
    public int addInvoice(int customerId, string status, DateTime datePlaced, DateTime deliverDate, string message, decimal total, int points)
    {
        //check that a valid customer id is given
        var checkCustomerId = (from c in db.Users
                               where c.ID.Equals(customerId)
                               select c).FirstOrDefault();

        if (checkCustomerId != null)
        {
            var newInvoice = new Invoice
            {
                CustomerID = customerId,
                Status = status,
                Date = datePlaced,
                DeliveryDatetime = deliverDate,
                Notes = message,
                Total = total,
                Points = points
            };
            db.Invoices.InsertOnSubmit(newInvoice);

            try
            {
                //successfully submitted
                db.SubmitChanges();
                return 1;
            }
            catch (Exception ex)
            {
                //something went wrong
                ex.GetBaseException();
                return -1;
            }
        }
        else
        {
            //customer id provided is not valid
            return 0;
        }
    }

    //wrong
   public int updateInvoice(int customerId, string status, DateTime datePlaced, DateTime deliverDate, string message, decimal total, int points)
    {
        var order = (from o in db.Invoices
                     where o.CustomerID.Equals(customerId) && o.Date.Equals(datePlaced)
                     select o).FirstOrDefault();

        if(order == null)
        {
            //this order doesn't exist
            return 0;
        }
        else
        {
            order.Status = status;
            order.DeliveryDatetime = deliverDate;
            order.Notes = message;
            order.Total = total;
            order.Points = points;

            try
            {
                //changes successfully submitted
                db.SubmitChanges();
                return 1;
            }
            catch(Exception ex)
            {
                ex.GetBaseException();
                return -1;
            }
        }
    }

    //Get all invoices
    public List<Invoice> getAllInvoices()
    {
        dynamic ordersList = new List<Invoice>();

        dynamic allOrders = (from o in db.Invoices
                             select o);

        if(allOrders == null)
        {
            return null;
        }
        else
        {
            foreach(Invoice ord in allOrders)
            { 
                var newOrder = new Invoice
                {
                    ID = ord.ID,
                    CustomerID = ord.CustomerID,
                    Status = ord.Status,
                    Date = ord.Date,
                    DeliveryDatetime = ord.DeliveryDatetime,
                    Notes = ord.Notes,
                    Points = ord.Points,
                    Total = ord.Total
                };
            ordersList.Add(newOrder);
            }
            return ordersList;
        }
       
    }

    //Get invoice by UserID
    public List<Invoice> getAllCustomerInvoices(int customerId)
    {
        //check that the provided customer id is valid
        var checkCus = (from c in db.Users
                        where c.ID.Equals(customerId)
                        select c).FirstOrDefault();
       
        if(checkCus == null)
        {
            return null;
        }
        else
        {
            dynamic customersOrders = new List<Invoice>();

            //get all the orders linked to the customer
            dynamic ordersList = (from o in db.Invoices
                                  where o.CustomerID.Equals(customerId)
                                  select o);

            if(ordersList != null)
            {
                foreach(Invoice o in ordersList)
                {
                    var order = new Invoice
                    {
                        ID = o.ID,
                        CustomerID = o.CustomerID,
                        Status = o.Status,
                        Date = o.Date,
                        DeliveryDatetime = o.DeliveryDatetime,
                        Notes = o.Notes,
                        Total = o.Total,
                        Points = o.Points
                    };
                    customersOrders.Add(order);
                }

                return customersOrders;
            }
            else
            {
                return null;
            }
        }
    }

    //INVOICE LINE MANAGEMENT

    public int addInvoiceLine(int product_ID, int invoice_ID, int quantity, decimal price)
    {
        var invLine = new InvoiceLine
        {
            ProductID = product_ID,
            InvoiceID = invoice_ID,
            Qty = quantity,
            Price = price
        };
        db.InvoiceLines.InsertOnSubmit(invLine);
        try
        {
            db.SubmitChanges();
            return 1;
        }
        catch (Exception ex)
        {
            ex.GetBaseException();
            return -2;
        }
    }

    //Get all products in invoice line by passing invoice ID
    public List<InvoiceLine> getAllInvoiceLines(int InvoiceID)
    {
        dynamic ordereditems = (from o in db.InvoiceLines
                                where o.InvoiceID.Equals(InvoiceID)
                                select o);

        List<InvoiceLine> rList = new List<InvoiceLine>();

        if (ordereditems == null)
        {
            return null;
        }
        else
        {
            foreach (InvoiceLine line in ordereditems)
            {
                var temp = new InvoiceLine
                {
                    ID = line.ID,
                    InvoiceID = line.InvoiceID,
                    ProductID = line.ProductID,
                    Qty = line.Qty,
                    Price = line.Price,
                };

                rList.Add(temp);
            }
            return rList;
        }
    }

    //SHOPPING LIST MANAGEMENT ----------------------------------------

    //shopping list table needs to be updated then this function
    public int addToList(int userID, int productID, int quantity)
    {
        var product = (from s in db.ShoppingLists
                       where s.UserID.Equals(userID) && s.ProductID.Equals(productID)
                       select s).FirstOrDefault();

        if (product != null)
        {
            //already in list
            return 0;
        }
        else
        {
            var newList = new ShoppingList
            {
                UserID = userID,
                ProductID = productID,
                Quantity = quantity

            };
            db.ShoppingLists.InsertOnSubmit(newList);
            try
            {
                db.SubmitChanges();
                return 1;
            }
            catch (Exception e)
            {
                e.GetBaseException();
                return -1;
            }
        }
    }

    //getter for list items
    public List<ShoppingList> getList(int userID)
    {
        var list = (from s in db.ShoppingLists
                    where s.UserID.Equals(userID)
                    select s);

        List<ShoppingList> listS = new List<ShoppingList>();
        if (!list.Any())
        {
            return null;
        }
        else
        {
            foreach (ShoppingList s in list)
            {
                //if (s.UserID == null)
                  //  return null;

                var tempList = new ShoppingList
                {
                    UserID = s.UserID,
                    ProductID = s.ProductID,
                    Quantity = s.Quantity
                };
                listS.Add(tempList);
            }
            return listS;
        }
    }

    //method that allows you to update list items
    public int updateList(int userID, int P_ID, int quantity)
    {
        var list = (from s in db.ShoppingLists
                        where s.UserID.Equals(userID) && s.ProductID.Equals(P_ID)
                        select s).FirstOrDefault();

        if (list != null)
        {
            list.Quantity = quantity;
            
            try
            {
                db.SubmitChanges();
                return 1;
            }
            catch (Exception ex)
            {
                ex.GetBaseException();
                return -2;
            }
        }
        else
        {
            return -1;
        }
    }

    public int removeList(int userID)
    {
       dynamic list = (from s in db.ShoppingLists
                    where s.UserID.Equals(userID)
                    select s);

        if (list != null)
        {
            foreach(ShoppingList s in list)
            {
                db.ShoppingLists.DeleteOnSubmit(s);
            }
            
            try
            {
                //all is well
                db.SubmitChanges();
                return 1;
            }
            catch (Exception ex)
            {
                //something else went wrong
                ex.GetBaseException();
                return -1;
            }
        }
        else
        {
            //doesn't exist
            return 0;
        }
    }

    //CARD MANAGEMENT ----------------------------------------------------

    //Getter method for Cards
    public Card getCard(int id)
    {
        var Cardinfo = (from c in db.Cards
                        where c.ID.Equals(id)
                        select c).FirstOrDefault();

        if(Cardinfo == null)
        {
            return null;
        }else
        {
            return Cardinfo;
        }
    }

    //Method that allows the customer to add new card
    public int addCard(int Cust_ID,string description, string name, string number, DateTime expiry)
    {
        var tempCard = (from c in db.Cards
                        where c.Number.Equals(number)
                        select c).FirstOrDefault();
        if(tempCard != null)
        {
            //Card already exists
            return 0;
        }else
        {
            var newCard = new Card
            {
                CustomerID = Cust_ID,
                Description = description,
                Name = name,
                Number = number,
                Expiry = expiry

            };
            db.Cards.InsertOnSubmit(newCard);
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

    //Function used to update card details
    public int updateCards(int c_ID, int Cust_ID, string description, string name, string number, DateTime expiry)
    {
        var tempcard = (from c in db.Cards
                        where c.Number.Equals(number) && c.Description.Equals(description) && c.Name.Equals(name)&&
                        c.Expiry.Equals(expiry)
                        select c).FirstOrDefault();

        if(tempcard != null)
        {
            //card number is the same
            return -1;
        }else
        {
            var card = getCard(c_ID);
            if(card != null)
            {
                card.Description = description;
                card.Name = name;
                card.Number = number;
                card.Expiry = expiry;

                try
                {
                    db.SubmitChanges();
                    return 1;
                }
                catch (Exception ex)
                {
                    ex.GetBaseException();
                    return -2;
                }

            }else
            {
                return 0;
            }
        }
    }

    //DEVICE MANAGEMENT --------------------------------------------

    //Get method for devices
    public Device getDevice(int userID)
    {
        var DeviceInfo = (from d in db.Devices
                          where d.CustomerID.Equals(userID)
                          select d).FirstOrDefault();
        
        if(DeviceInfo == null)
        {
            return null;
        }
        else
        {
            var tempDevice = new Device
            {
                DeviceID = DeviceInfo.DeviceID,
                CustomerID = DeviceInfo.CustomerID,
                OS = DeviceInfo.OS
            };
            return tempDevice;
        }
    }
    //Function to add the device to the database
    public int addDevices(int cust_ID,string useragent)
    {
        var newDevice = new Device
        {
            CustomerID = cust_ID,
            OS = useragent
         
        };
        db.Devices.InsertOnSubmit(newDevice);
        try
        {
            db.SubmitChanges();
            return 1;
        }
        catch (Exception e)
        {
            e.GetBaseException();
            return -1;
        }
    }

    //REPORT MANAGEMENT ------------------------------------------

    //Function used to calculate the total profit generated 
    public double calculateProfit()
    {
        var totalprofit = 0.0;
        var Difference = 0.0;
        var CurrentProfit = 0.0;

        //Storing all the ordered items in a variable
        var OrdItems = (from i in db.InvoiceLines
                        select i);

        foreach (InvoiceLine o in OrdItems)
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

    public double profitPerProduct(int P_ID)
    {
        double profit = 0.0;
        double totalPrice = 0.0;
        double totalCost = 0.0;
        dynamic productID = (from p in db.InvoiceLines
                           where p.ID.Equals(P_ID)
                           select p);
        dynamic Quantity = (from q in db.InvoiceLines
                            where q.ID.Equals(P_ID)
                            select q.Qty);
        dynamic product = getProduct(productID);

        foreach(Product p in product)
        {
            totalCost = (double)(p.Cost * Quantity);
            totalPrice = (double)(p.Price * Quantity);
            profit += totalPrice - totalCost;
        }
        return profit;
    }

    public double profitPerSubCat(int S_ID)
    {
        double profit = 0.0;
        int productID;

        dynamic allInvoices = getAllInvoices();
        foreach(Invoice i in allInvoices)
        {
            dynamic allLines = getAllInvoiceLines(i.ID);
            foreach(InvoiceLine iL in allLines)
            {
                productID = iL.ProductID;
                dynamic product = getProduct(productID);
                if(product.SubCategoryID.Equals(S_ID))
                { 
                    profit += profitPerProduct(productID);
                }
            }
        }
        return profit;
    }

    //profit per category
    public double profitPerCat(int C_ID)
    {
        double profit = 0.0;
        //dynamic product;

        dynamic subCatList = getSubCatPerCat(C_ID);

        dynamic allInvoices = getAllInvoices();
        foreach(Invoice i in allInvoices)
        {
            dynamic allLines = getAllInvoiceLines(i.ID);
            foreach(InvoiceLine iL in allLines)
            {
                int productID = iL.ProductID;
                dynamic product = getProduct(productID);

                foreach(SubCategory sc in subCatList)
                {
                    if(product.SubCategoryID.Equals(sc.SubID))
                    {
                        profit += profitPerProduct(product.ID);
                    }
                }
            }
        }
        return profit;
    }

    //calculate the profit on sales for the given date
    public decimal calcProfitPerday(DateTime date)
    {
        decimal profit = 0;

        dynamic invoices = (from i in db.Invoices
                            where i.Date.Equals(date.ToShortDateString())
                            select i);

        if(invoices != null)
        {
            foreach(Invoice i in invoices)
            {
                dynamic invLines = getAllInvoiceLines(i.ID);

                foreach(InvoiceLine iLine in invLines)
                {
                    var invProduct = getProduct(iLine.ProductID);

                    profit += (decimal)((iLine.Price - invProduct.Cost) * iLine.Qty);
                }
            }
        }
        return profit;
    }

    //reverse calculate vat on products
    public decimal calcProductVAT(int P_ID)
    {
        decimal VAT = 0;

        var product = (from p in db.Products
                       where p.ID.Equals(P_ID)
                       select p).FirstOrDefault();

        VAT = product.Price * (decimal)(0.15/1.15);

        return VAT;
    }

    //Number of users registered per day
    public int getUsersPerDay(DateTime day)
    {
        int totalRegistered = 0;

        dynamic users = (from u in db.Users
                         where u.DateRegistered.Equals(day) && u.UserType.Equals("customer")
                         select u);

        foreach (User u in users)
        {
            totalRegistered++;
        }

        return totalRegistered;
    }

    //Number of users registered per week
    public int usersperWeek(DateTime currentDate)
    {
        dynamic DayList = getWeekDates(currentDate);
        int totalUsers = 0;
        foreach(DateTime d in DayList)
        {
            totalUsers += getUsersPerDay(d);
        }
        return totalUsers;
    }

    //Number of users
    private int numUsers()
    {
        dynamic user = (from u in db.Users
                        select u);
        int count = 0;
        foreach(User usr in user)
        {
            count += 1;
        }
        return count;
    }

    public double percentageUserChange(DateTime currentDate)
    {
        //getting the total number of users perweek
        int totalBefore = numUsers();//20
        int totalUsers = usersperWeek(currentDate);//1

        int Change = totalBefore + totalUsers; //21 
        int Newuser = Change - totalBefore;
        //new value - old value/oldvalue *100
        double percentageChange = ((Newuser*100)/ totalBefore);//19/20*100

        return percentageChange;
    }

    public List<DateTime> getWeekDates(DateTime date)
    {
        List<DateTime> weekDates = new List<DateTime>();

        var todayDate = date.Date;

        var day = (int)todayDate.DayOfWeek;
        const int totalNumDays = 7;

        for(int i = -day; i < (-day + totalNumDays); i++)
        {
            weekDates.Add(todayDate.AddDays(i).Date);
        }
        return weekDates;
    }

    public decimal salesPerWeek(DateTime date)
    {
        dynamic weekDates = getWeekDates(date.Date);
        dynamic invoices = (from p in db.Invoices
                            select p);

        decimal totalSales = 0;

        foreach(Invoice i in invoices)
        {
            if (weekDates.Contains(i.Date))
            {
                totalSales += (decimal)i.Total;
            }
        }
        return totalSales;
    }

    private double getAllSales()
    {
        dynamic sale = (from s in db.Invoices
                        select s);
        double totalSales = 0;
        foreach(Invoice i in sale)
        {
            totalSales += Convert.ToDouble(i.Total);
        }
        return totalSales;
    }

    public double percentageSaleChanger(DateTime currentDate)
    {
        double Salesbefore = getAllSales();
        double SalesinWeek = Convert.ToDouble(salesPerWeek(currentDate));

        double Change = SalesinWeek + Salesbefore;
        double difference = Change - Salesbefore;

        double percentage = ((difference * 100) / Salesbefore);
        return percentage;
    }

    public int NumsalesPerWeek(DateTime date)
    {
        dynamic weekDates = getWeekDates(date.Date);
        dynamic sale = (from s in db.Invoices
                        select s);
        int Counter = 0;
        foreach(Invoice i in sale)
        {
            if (weekDates.Contains(i.Date))
            {
                Counter += 1; 
            }
        }
        return Counter;
    }

    private int numInvoices()
    {
        dynamic sale = (from s in db.Invoices
                        select s);
        int Counter = 0;
        foreach(Invoice i in sale)
        {
            Counter += 1;
        }
        return Counter;
    }
    
    public double NumSaleChange(DateTime currentDate)
    {
        int AllInvoices = numInvoices();//7
        int  numbNewSales = NumsalesPerWeek(currentDate);//4
        int OldInvoices = AllInvoices - numbNewSales; //7-4 = 3
        double Difference = (AllInvoices - OldInvoices)*100;
        double percentage = (Difference / OldInvoices);
       
        return percentage;
    }

    public decimal calcCategoryTotalSales(int cId)
    {
        decimal totalSales = 0;
        dynamic invoiceSales = (from i in db.InvoiceLines
                                select i);

        foreach(InvoiceLine i in invoiceSales)
        {
            var productCategory = getCategorybyProductID(i.ProductID);
            if (productCategory.ID.Equals(cId))
            {
                totalSales += (decimal)(i.Qty * i.Price);
            }
        }
        return totalSales;
    }

    public decimal calcSalesPerDay(DateTime date)
    {
        decimal sales = 0;

        dynamic invoices = (from i in db.Invoices
                            where i.Date.Equals(date.Date)
                            select i);
        
        if(invoices != null)
        {
            foreach(Invoice inv in invoices)
            {
                sales += inv.Total;
            }
        }
        return sales;
    }

    public List<DateTime> getMonthDates(DateTime date)
    {
        List<DateTime> monthDates = new List<DateTime>();

        //List<DateTime> daysList = Enumerable.Range(1, DateTime.DaysInMonth(date.Year, date.Month))
        //                    .Select(day => new DateTime(date.Year, date.Month, day)).ToList();


        //dynamic dates = daysList.Select(day => new DateTime(date.year, date.month, day)); 

        dynamic daysList = Enumerable.Range(1, DateTime.DaysInMonth(date.Year, date.Month));

        foreach(var d in daysList)
        {
            DateTime newDate = new DateTime(date.Year, date.Month, d);
            monthDates.Add(newDate);
        }
        return monthDates;
    }

    //getting the weekly invoice line for a particular product
    public int numProductSales(DateTime currentDate, int Product_ID)
    {
        dynamic weekDates = getWeekDates(currentDate.Date);
        
        dynamic invoice = getAllInvoices();

        List<InvoiceLine> LineList = new List<InvoiceLine>();
        int Count = 0;
        //for each day get the product sales and add to the counter 
        foreach(Invoice i in invoice)
        {  
            if (weekDates.Contains(i.Date))
            {
                dynamic invoiceLine = getAllInvoiceLines(i.ID);
                foreach (InvoiceLine inv in invoiceLine)
                {
                    if (inv.ProductID.Equals(Product_ID))
                    {
                        Count += (1*inv.Qty);
                    }
                }
            }
        }
        return Count;
    }

    private int countProductsSold(int P_ID)
    {
        dynamic invLine = (from i in db.InvoiceLines
                           select i);
        int Count = 0;
        foreach(InvoiceLine inv in invLine)
        {
            if(inv.ProductID.Equals(P_ID))
            {
                Count += (1*inv.Qty);
            }           
        }
        return Count;
    }

    public double percProductSales(DateTime currentDate, int Product_ID)
    {
        int newproductSales = numProductSales(currentDate, Product_ID); //3
        int TotalNow = countProductsSold(Product_ID);//6
        //int newTotal = newproductSales + TotalBefore;
        int StartingValue = TotalNow - newproductSales; //6-3 = 3
        int Difference = (TotalNow-StartingValue) * 100; //6-3 * 100 = 300
        double percentageChange = (Difference /StartingValue);//300/3 = 100
        return percentageChange;
    }

    //Function used to return products in the given price range
    public List<Product> getfilteredProducts(int minPrice, int maxPrice)
    {
        List<Product> productlist = new List<Product>();
        //Retreiving all the products between a price range
        dynamic product = (from p in db.Products
                           where p.Price <= maxPrice && p.Price >= minPrice
                           select p);

        foreach(Product p in product)
        {
            var tempProduct = new Product
            {
                ID = p.ID,
                Name = p.Name,
                SubCategoryID = p.SubCategoryID,
                Price = p.Price,
                Cost = p.Cost,
                StockOnHand = p.StockOnHand,
                Image_Location = p.Image_Location,
                Status = p.Status,
                Description = p.Description
            };
            //Adding the required products to the product list
            productlist.Add(tempProduct);
        }
        return productlist;
    }

    //TRAFFIC MANAGEMENT -----------------------------------------------------------------
    public int addTraffic(string pageName, DateTime currentdate,int unique)
    {
        var newTraffic = new Traffic
        {
            PageName = pageName,
            Day = currentdate,
            Unique = unique
        };

        db.Traffics.InsertOnSubmit(newTraffic);
        try
        {
            db.SubmitChanges();
            return newTraffic.SessionId;
        }
        catch (Exception e)
        {
            e.GetBaseException();
            return -1;
        }
    }

    private List<Traffic> getAllTrafic()
    {
        dynamic traffic = (from t in db.Traffics
                           select t);
        dynamic trafList = new List<Traffic>();
        if(traffic ==null)
        {
            return null;
        }
        else
        {
            foreach(Traffic t in traffic)
            {
                var tempTraffic = new Traffic
                {
                    SessionId = t.SessionId,
                    PageName = t.PageName,
                    Day = t.Day,
                    Unique = t.Unique
                };
                trafList.Add(tempTraffic);
            }
            return trafList;
        }
    }

    public int singlePageTraffic(string pageName)
    {
        dynamic traffic = (from t in db.Traffics
                           where t.PageName.Equals(pageName)
                           select t);
        int Count = 0;
        foreach(Traffic t in traffic)
        {
            Count += 1;
        }
        return Count;
    }

    public int trafficPerWeek(DateTime currentDate)
    {
        dynamic weekDates = getWeekDates(currentDate.Date);
        dynamic traffic = getAllTrafic();
        int Count = 0;
        foreach(Traffic t in traffic)
        {
            foreach(DateTime d in weekDates)
            {
                if (d.ToString("d").Equals(t.Day.ToString("d")))
                {
                    Count += 1;
                }
            }
        }
        return Count;
    }

    private int getTrafficNum()
    {
        dynamic traffic = (from t in db.Traffics
                           select t);
        int Count = 0;
        foreach(Traffic t in traffic)
        {
            Count += 1;
        }
        return Count;
    }

    public double TrafficChange(DateTime currentDate)
    {   
        //total current traffic(new Traffic)
        int newTraffic = getTrafficNum();//2
        //Current weeks traffic 
        int weeksTraffic = trafficPerWeek(currentDate);//2

        //Starting value
        int StartingValue = newTraffic - weeksTraffic;//0
        int difference = (newTraffic - StartingValue)*100;
        if(StartingValue ==0)
        {
            return difference;
        }
        double percentage = difference / StartingValue;
        
        return percentage;
    }

    public int singlePageUniqueTraffic(string pageName)
    {
        dynamic traffic = (from t in db.Traffics
                           where t.PageName.Equals(pageName) && t.Unique.Equals(1)
                           select t);
        int Count = 0;
        foreach (Traffic t in traffic)
        {
            Count += 1;
        }
        return Count;
    }

    public List<string> topPages()
    {
        dynamic page = (from t in db.Traffics
                        where t!=null
                        group t by t.PageName into grp
                        orderby grp.Count() descending
                        select grp.Key);

        dynamic pageList = new List<String>();
        
        foreach(String p in page)
        {
            pageList.Add(p);
        }
        dynamic topPages = pageList.GetRange(0,5);

        return topPages;
    }

    //ProductCategory Management
    private int getallsalesbyCategory(int CatID)
    {
        dynamic sale = (from s in db.InvoiceLines
                        select s);
        int Count = 0;
        foreach (InvoiceLine i in sale)
        {
            dynamic category = getCategorybyProductID(i.ProductID);
            if(category.ID.Equals(CatID))
            {
                Count += 1;
            }

        }
        return Count;
    }
    public int numProductSalesperCategory(DateTime currentDate, int Cat_ID)
    {
        dynamic weekDates = getWeekDates(currentDate.Date);

        dynamic invoice = getAllInvoices();

        List<InvoiceLine> LineList = new List<InvoiceLine>();
        int Count = 0;
        //for each day get the product sales and add to the counter 
        foreach (Invoice i in invoice)
        {
            if (weekDates.Contains(i.Date))
            {
                dynamic invoiceLine = getAllInvoiceLines(i.ID);
                foreach (InvoiceLine inv in invoiceLine)
                {
                    dynamic category = getCategorybyProductID(inv.ProductID);
                    if (category.ID.Equals(Cat_ID))
                    {
                        Count += 1;
                    }
                }
            }
        }
        return Count;

    }
    public double percentageCategorySales(DateTime currentDate, int Cat_ID)
    {
        int TotalNewCount = getallsalesbyCategory(Cat_ID);
        int weeklySale = numProductSalesperCategory( currentDate, Cat_ID);
        int StartingValue = TotalNewCount - weeklySale; //6-3 = 3
        int Difference = (TotalNewCount - StartingValue) * 100;
        if (StartingValue == 0)
        {
            return Difference;
        }
        double percentageChange = (Difference / StartingValue);//300/3 = 100
        return percentageChange;
    }

    //Subcategory management-------------------------------------------------------
    private int getallsalesbySubCategory(int SubCatID)
    {
        dynamic sale = (from s in db.InvoiceLines
                        select s);
        int Count = 0;
        foreach (InvoiceLine i in sale)
        {
            dynamic product = getProduct(i.ProductID);
                if (product.SubCategoryID.Equals(SubCatID))
                {
                    Count += 1;
                }   
        }
        return Count;
    }
    public double percentageSubCategorySales(DateTime currentDate, int SubCat_ID)
    {
        int TotalNewCount = getallsalesbySubCategory(SubCat_ID);
        int weeklySale = numProductSalesperSubCategory(currentDate,SubCat_ID);
        int StartingValue = TotalNewCount - weeklySale; //6-3 = 3
        int Difference = (TotalNewCount - StartingValue) * 100;
        if (StartingValue == 0)
        {
            return Difference;
        }
        double percentageChange = (Difference / StartingValue);//300/3 = 100
        return percentageChange;

    }
    public int numProductSalesperSubCategory(DateTime currentDate, int SubCat_ID)
    {
        dynamic weekDates = getWeekDates(currentDate.Date);

        dynamic invoice = getAllInvoices();

        List<InvoiceLine> LineList = new List<InvoiceLine>();
        int Count = 0;
        //for each day get the product sales and add to the counter 
        foreach (Invoice i in invoice)
        {
            if (weekDates.Contains(i.Date))
            {
                dynamic invoiceLine = getAllInvoiceLines(i.ID);
                foreach (InvoiceLine inv in invoiceLine)
                {
                    dynamic product = getProduct(inv.ProductID);
                    if (product.ID.Equals(SubCat_ID))
                    {
                        Count += 1;
                    }
                }
            }
        }
        return Count;

    }

    public List<int> TopProducts()
    {
        dynamic product = (from t in db.InvoiceLines
                        where t != null
                        group t by t.ProductID into grp
                        orderby grp.Count() descending
                        select grp.Key);

        dynamic productList = new List<int>();

        foreach (int inv in product)
        {
            productList.Add(inv);
        }
        dynamic topproducts = productList.GetRange(0, 5);

        return topproducts;
    }
    public int getProQtySold (int P_ID)
    {
        dynamic product = (from p in db.InvoiceLines
                           where p.ProductID.Equals(P_ID)
                           select p);
        int Count = 0;
        foreach(InvoiceLine inv in product)
        {
            Count += inv.Qty;
        }
        return Count;

    }

    public List<Product> recommendedProducts(int userID)
    {
        List<Product> list = new List<Product>();

        //user similarity-----------------
        //our location
        dynamic ourAddress = getAddress(userID);
        //device used
        dynamic ourDevice = getDevice(userID);
        
        //get all products
        dynamic allProducts = getAllProducts();

        List<Product> ourProductList = new List<Product>();
        foreach (Product p in allProducts)
        {
            double userSimilar = 0;
            double productSimilar = 0;
            double categorySimilar = 0;
            double subSimilar = 0;

            //get all users
            dynamic allUsers = getAllUsers();
            foreach(User u in allUsers)
            {
                //invoice per user
                dynamic invoices = getAllCustomerInvoices(u.ID);
                foreach(Invoice i in invoices)
                {
                    //invoice line per invoice
                    dynamic allLines = getAllInvoiceLines(i.ID);
                    foreach(InvoiceLine il in allLines)
                    {
                        //has this user bought product p before?
                        if(p.ID.Equals(il.ProductID))
                        {
                            double location = 0;
                            //YES so is this user similar to us?
                            dynamic userAddress = getAddress(u.ID);
                            if(userAddress != null)
                            {
                                int province = 0;
                                int city = 0;
                                int suburb = 0;
                                if (userAddress.Province.Equals(ourAddress.Province))
                                {
                                    province = 1;
                                }
                                if (userAddress.City.Equals(ourAddress.City))
                                {
                                    city = 1;
                                }
                                if (userAddress.Suburb.Equals(ourAddress.Suburb))
                                {
                                    suburb = 1;
                                }
                                location = ((province + city + suburb) * 100) / 3;
                            }

                            //does user use same device?
                            int device = 0;
                            dynamic userDevice = getDevice(u.ID);
                            if (userDevice.OS.Equals(ourDevice.OS))
                            {
                                device = 100;
                            }
                            else if (userDevice.OS.Contains(ourDevice.OS))
                            {
                                device = 50;
                            }
                            //location is worth 60%, device worth 40% ---- 20% of this
                            userSimilar = ((location * 0.6) + (device * 0.4) * 0.2);
                        }
                    }
                }
            }

            dynamic ourInvoices = getAllCustomerInvoices(userID);
            foreach (Invoice i in ourInvoices)
            {
                dynamic ourLines = getAllInvoiceLines(i.ID);
                foreach (InvoiceLine il in ourLines)
                {
                    if (p.ID.Equals(il.ProductID))
                    {
                        //YES 
                        productSimilar = (1 * 0.5);
                    }
                    
                    //have I bought from this category before?
                    dynamic ourCat = getCategorybyProductID(il.ProductID);
                    dynamic cat = getCategorybyProductID(p.ID);
                    if (ourCat.ID.Equals(cat.ID))
                    {
                        categorySimilar = (1 * 0.1);
                    }

                    //have I bought from this subcategory before?
                    dynamic ourProduct = getProduct(il.ProductID);
                    dynamic ourSub = getSubCat(ourProduct.SubCategoryID);
                    dynamic sub = getSubCat(p.SubCategoryID);
                    if(ourSub.SubID.Equals(sub.SubID))
                    {
                        subSimilar = (1 * 0.2);
                    }
                }
            }
            //(productsimilarity * 0.5) + (category * 0.2) + (sub * 0.1) + (usersimilarity * 0.2)
            double totalSimilar = productSimilar + categorySimilar + subSimilar + userSimilar;

            //dynamic sortedList = sortList(p, totalSimilar);
            
            list.Add(p);
        }
        return list;
    }

    public List<recommended> recommendTest(int userID)
    {
        List<recommended> list = new List<recommended>();

        //user similarity-----------------
        //our location
        dynamic ourAddress = getAddress(userID);
        //device used
        dynamic ourDevice = getDevice(userID);

        //get all products
        dynamic allProducts = getAllProducts();

        List<Product> ourProductList = new List<Product>();
        foreach (Product p in allProducts)
        {
            double userSimilar = 0;
            double productSimilar = 0;
            double categorySimilar = 0;
            double subSimilar = 0;

            //get all users
            dynamic allUsers = getAllUsers();
            foreach (User u in allUsers)
            {
                //invoice per user
                dynamic invoices = getAllCustomerInvoices(u.ID);
                foreach (Invoice i in invoices)
                {
                    //invoice line per invoice
                    dynamic allLines = getAllInvoiceLines(i.ID);
                    foreach (InvoiceLine il in allLines)
                    {
                        //has this user bought product p before?
                        if (p.ID.Equals(il.ProductID))
                        {
                            double location = 0;
                            //YES so is this user similar to us?
                            dynamic userAddress = getAddress(u.ID);
                            if (userAddress != null)
                            {
                                int province = 0;
                                int city = 0;
                                int suburb = 0;
                                if (userAddress.Province.Equals(ourAddress.Province))
                                {
                                    province = 1;
                                }
                                if (userAddress.City.Equals(ourAddress.City))
                                {
                                    city = 1;
                                }
                                if (userAddress.Suburb.Equals(ourAddress.Suburb))
                                {
                                    suburb = 1;
                                }
                                location = ((province + city + suburb) * 100) / 3;
                            }

                            //does user use same device?
                            int device = 0;
                            dynamic userDevice = getDevice(u.ID);
                            if (userDevice.OS.Equals(ourDevice.OS))
                            {
                                device = 100;
                            }
                            else if (userDevice.OS.Contains(ourDevice.OS))
                            {
                                device = 50;
                            }
                            //location is worth 60%, device worth 40% ---- 20% of this
                            userSimilar = ((location * 0.6) + (device * 0.4) * 0.2);
                        }
                    }
                }
            }

            dynamic ourInvoices = getAllCustomerInvoices(userID);
            foreach (Invoice i in ourInvoices)
            {
                dynamic ourLines = getAllInvoiceLines(i.ID);
                foreach (InvoiceLine il in ourLines)
                {
                    if (p.ID.Equals(il.ProductID))
                    {
                        //YES 
                        productSimilar = (1 * 0.5);
                    }

                    //have I bought from this category before?
                    dynamic ourCat = getCategorybyProductID(il.ProductID);
                    dynamic cat = getCategorybyProductID(p.ID);
                    if (ourCat.ID.Equals(cat.ID))
                    {
                        categorySimilar = (1 * 0.1);
                    }

                    //have I bought from this subcategory before?
                    dynamic ourProduct = getProduct(il.ProductID);
                    dynamic ourSub = getSubCat(ourProduct.SubCategoryID);
                    dynamic sub = getSubCat(p.SubCategoryID);
                    if (ourSub.SubID.Equals(sub.SubID))
                    {
                        subSimilar = (1 * 0.2);
                    }
                }
            }
            //(productsimilarity * 0.5) + (category * 0.2) + (sub * 0.1) + (usersimilarity * 0.2)
            double totalSimilar = productSimilar + categorySimilar + subSimilar + userSimilar;

            list = sortList(list, p, totalSimilar);

            //list.Add(p);
        }
        return list;
    }


    private List<recommended> sortList(List<recommended> listRec, Product p, double total)
    {
        listRec.Add(new recommended() { rating = total, product = p });

        List<recommended> SortedList = listRec.OrderByDescending(o => o.rating).ToList();
        return SortedList;
    }
}