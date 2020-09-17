using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Web.UI;

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

    public int Register(string name, string surname, string email, string password, string status, DateTime date, string userType)
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

        if (tempUser != null)
        {
            //the email they're trying to change to is already in use
            return -2;
        }
        else
        {
            var user = (from u in db.Users
                    where u.ID.Equals(id)
                    select u).FirstOrDefault();

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
                    return -1;
                }
            }
            else
            {
                //user doesn't exist
                return 0;
            }
           
        }

    }

    public int addNewProduct(string name, int SubID, double price, double cost, int stockQty, string imgLocation)
    {
        var newProduct = new Product
        {
            Name = name,
            SubCategoryID = SubID,
            Price = (decimal)price,
            Cost = (decimal) cost,
            StockOnHand = stockQty,
            Image_Location = imgLocation
        };

        db.Products.InsertOnSubmit(newProduct);

        try
        {
            //product successfully added
            db.SubmitChanges();
            return 1;
        }
        catch (Exception ex)
        {
            //error occurred when attempting to add product
            ex.GetBaseException();
            return -1;
        }
    }

    //Function to update product specifics
    public int updateProduct(int id, string name, int SubId, double price, double cost, string imgLocation)
    {
        var product = getProduct(id);

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
                productsList.Add(pr);
            }
        }

        return productsList;
    }

    //Function to delete a product from the product table
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
            db.Products.DeleteOnSubmit(product);

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
                Name = pc.Name
            };

            categories.Add(tempCat);
        }

        return categories;
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
                CategoryID = sc.CategoryID
            };

            subcategories.Add(tempSubCat);
        }

        return subcategories;
    }

    public Invoice getOrder(int orderId)
    {
        var order = (from o in db.Invoices
                     where o.ID.Equals(orderId)
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
                Notes = order.Notes

            };

            return temp;
        }

    }

    public int addOrder(int customerId, string status, DateTime datePlaced, DateTime deliverDate, string message)
    {
        //check that a valid customer id is given
        var checkCustomerId = (from c in db.Users
                               where c.ID.Equals(customerId)
                               select c).FirstOrDefault();

        if (checkCustomerId != null)
        {
            var newOrder = new Invoice
            {
                CustomerID = customerId,
                Status = status,
                Date = datePlaced,
                DeliveryDatetime = deliverDate,
                Notes = message
            };
            db.Invoices.InsertOnSubmit(newOrder);

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

    //Function that can update order status, delivery date and gift message
   public int UpdateOrder(int customerId, string status, DateTime datePlaced, DateTime deliverDate, string message)
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


    //Function that returns all orders in db
    public List<Invoice> getAllOrders()
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
                ordersList.Add(ord);
            }
        }

        return ordersList;
    }

    //Function that returns a list of orders linked to a specific customer
    public List<Invoice> getAllCustomerOrders(int customerId)
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
                        Notes = o.Notes
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

    public int getUsersPerDay(DateTime day)
    {
        int totalRegistered = 0;

        dynamic users = (from u in db.Users
                     where u.DateRegistered.Equals(day) && u.UserType.Equals("customer")
                     select u);
        
        foreach(User u in users)
        {
            totalRegistered++;
        }

        return totalRegistered;
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
            var rProduct = new Product
            {
                ID = product.ID,
                Name = product.Name,
                SubCategoryID = product.SubCategoryID,
                Price = product.Price,
                Cost = product.Cost,
                StockOnHand = product.StockOnHand,
                Image_Location = product.Image_Location
            };
            return rProduct;
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
        var OrdItems = (from i in db.InvoiceLines
                        select i);

        foreach(InvoiceLine o in OrdItems)
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
    //Function used to return the Address
    public Address getAddress(int Address_ID)
    {
        var Addressinfo = (from a in db.Addresses
                           where a.ID.Equals(Address_ID)
                           select a).FirstOrDefault();

        if(Addressinfo == null)
        {
            return null;
        }else
        {
            return Addressinfo;
        }
    }
    //method used to add a new address into the database
    public int AddAdress(string line1, string line2, string suburb, string city, char billing, string type , int C_ID, string Province)
    {
        var address = (from ad in db.Addresses
                          where ad.Line1.Equals(line1) && ad.Line2.Equals(line2) && ad.Suburb.Equals(suburb)&&ad.City.Equals(city)
                          select ad).FirstOrDefault();

        if(address != null)
        {
            //means that the address already exists
            return 0;
        }else
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
            }catch(Exception e)
            {
                e.GetBaseException();
                return -1;
            }

        }

    }
    //method used to update the address
    public int UpdateAddress(int A_ID, string line1, string line2, string suburb, string city, char billing, string type, int Cus_ID)
    {
        var tempAddress = (from ad in db.Addresses
                           where ad.Line1.Equals(line1) && ad.Line2.Equals(line2) && 
                           ad.Suburb.Equals(suburb) && ad.City.Equals(city)
                           && ad.Billing.Equals(billing) && ad.CustomerID.Equals(Cus_ID)
                           select ad).FirstOrDefault();
        if(tempAddress != null)
        {
            return -1;
        }
        else
        {
            var address = getAddress(A_ID);
            if(address != null)
            {
                address.Line1 = line1;
                address.Line2 = line2;
                address.Suburb = suburb;
                address.City = city;
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
                    return -2;
                }

            }else
            {
                return 0;
            }
        }
    }

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
    public int AddCard(int Cust_ID,string description, string name, string number, DateTime expiry)
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
    public int UpdateCards(int c_ID, int Cust_ID, string description, string name, string number, DateTime expiry)
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
    //Get method for devices
    public Device getDevice(int D_ID)
    {
        var DeviceInfo = (from d in db.Devices
                          where d.DeviceID.Equals(D_ID)
                          select d).FirstOrDefault();
        if(DeviceInfo == null)
        {
            return null;
        }
        else
        {
            return DeviceInfo;
        }
    }
    //Function to add the device to the database
    public int AddDevices(string os)
    {
        var newDevice = new Device
        {
            OS = os
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

    //getter for list items
    public ListItem getListItem(int id)
    {
        var listitemInfo = (from l in db.ListItems
                            where l.ID.Equals(id)
                            select l).FirstOrDefault();

        if(listitemInfo == null)
        {
            return null;
        }else
        {
            return listitemInfo;
        }
    }

    //method that allows to add new product to the listitems
    public int AddListItems(int P_ID, int quantity)
    {
        var listinfo = (from l in db.ListItems
                        where l.ProductID.Equals(P_ID)
                        select l).FirstOrDefault();
        //if the product id is the same then increase the quantity only
        if (listinfo != null)
        {
           // listinfo.Quantity_ += quantity;
            return 0;
        }
        else
        {
            var newItem = new ListItem
            {
                ProductID = P_ID
            };
            db.ListItems.InsertOnSubmit(newItem);
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
    //method that allows you to update list items
    public int UpdateListItem(int id, int list_ID, int P_ID, int quantity)
    {
        var tempitem = (from l in db.ListItems
                        where l.ProductID.Equals(P_ID) && l.Quantity_.Equals(quantity)
                        select l).FirstOrDefault();
        if(tempitem != null)
        {
            //meaning that the product is already on the list
            return 0;
        }else
        {
            var listitem = getListItem(id);
            if(listitem != null)
            {
                listitem.ProductID = P_ID;
                listitem.Quantity_ = quantity;
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
    }
    //Get method for orderedItems
    public List<InvoiceLine> getOrderedItems(int id)
    {
        dynamic ordereditems = (from o in db.InvoiceLines
                            where o.InvoiceID.Equals(id)
                            select o);

        List<InvoiceLine> rList = new List<InvoiceLine>();

        if(ordereditems == null)
        {
            return null;
        }else
        {
            foreach(InvoiceLine line in ordereditems)
            {
                var temp = new InvoiceLine
                {
                    ID = line.ID,
                    InvoiceID = line.InvoiceID,
                    ProductID = line.ProductID, 
                    Qty = line.Qty
                };

                rList.Add(temp);
            }
            return rList;
        }
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

            foreach(Product pr in products)
            {
                var tempProduct = new Product
                {
                    ID = pr.ID,
                    Name = pr.Name,
                    SubCategoryID = pr.SubCategoryID,
                    Price = pr.Price,
                    Cost = pr.Cost,
                    StockOnHand = pr.StockOnHand,
                    Image_Location = pr.Image_Location
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

        foreach(Product pr in product)
        {
            var tempPro = new Product
            {
                ID = pr.ID,
                Name = pr.Name,
                SubCategoryID = pr.SubCategoryID,
                Price = pr.Price,
                Cost = pr.Cost,
                StockOnHand = pr.StockOnHand,
                Image_Location = pr.Image_Location
            };

            ProductList.Add(tempPro);
        }

        return ProductList;
    }

    public double profitPerProduct(int P_ID)
    {
        double profit = 0.0;
        double totalPrice = 0.0;
        double totalCost = 0.0;
        dynamic product = (from p in db.InvoiceLines
                           where p.ID.Equals(P_ID)
                           select p);
        dynamic Quantity = (from q in db.InvoiceLines
                            where q.ID.Equals(P_ID)
                            select q.Qty);

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
        dynamic product = (from p in db.InvoiceLines
                           select p);
        foreach(Product p in product)
        {
            if(p.SubCategoryID.Equals(S_ID))
            {
                profit += profitPerProduct(p.ID);
            }
        }
        return profit;
    }

    public double profitPerCat(int C_ID)
    {
        double profit = 0.0;
        dynamic product = (from p in db.InvoiceLines
                           select p);
        foreach(Product p in product)
        {
            dynamic subcat = getSubCat(p.SubCategoryID);
            foreach(SubCategory s in subcat)
            {
                profit += profitPerSubCat(s.SubID);
            }
        }
        return profit;
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
                CategoryID = subcat.CategoryID
            };
            return tempsub;
        }
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
                ID = cat.ID
            };
            return tempcat;
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
                CategoryID = sc.CategoryID
            };
            SubList.Add(tempsub);
        }
        return SubList;
    }

    public decimal calcProductVAT(int P_ID)
    {
        decimal VAT = 0;

        var product = (from p in db.Products
                       where p.ID.Equals(P_ID)
                       select p).FirstOrDefault();

        VAT = product.Price * (decimal)(0.15/1.15);

        return VAT;
    }

    public Product getProductByID(int P_ID)
    {
        dynamic product = (from p in db.Products
                           where p.ID.Equals(P_ID)
                           select p).FirstOrDefault();

        var temppro = new Product
        {
            ID = product.ID,
            Name = product.Name, 
            SubCategoryID = product.SubCategoryID,
            Price = product.Price, 
            Cost = product.Cost, 
            StockOnHand = product.StockOnHand, 
            Image_Location = product.Image_Location
        };
        return temppro;
        
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

    public int addInvoices(int customer_ID, string status, DateTime date, DateTime deliverDate, string notes, decimal total, int points)
    {
        var invoice = new Invoice
        {
            CustomerID = customer_ID,
            Status = status,
            Date = date,
            DeliveryDatetime = deliverDate,
            Notes = notes,
            Total = total,
            Points = points
        };
        db.Invoices.InsertOnSubmit(invoice);
        try
        {
            db.SubmitChanges();
            return invoice.ID;
        }
        catch (Exception ex)
        {
            ex.GetBaseException();
            return -2;
        }

    }

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
}
