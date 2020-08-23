﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IGP_Service" in both code and config file together.
[ServiceContract]
public interface IGP_Service
{
    [OperationContract]
    int login(string email, string password);

    [OperationContract]
    int Register(string name, string surname, string email, string password, string number);

    [OperationContract]
    int UpdateUserDetails(int id, string name, string surname, string email, string number);

    [OperationContract]
    int UpdatePassword(int id, string oldPassword, string newPassword);

    [OperationContract]
    int addNewProduct(string name, int SubID, double price, double cost, int stockQty, string description);

    [OperationContract]
    int removeProduct(int productId);

    [OperationContract]
    List<ProductCategory> getAllCategories();

    [OperationContract]
    List<ProductCategory> getAllSubCategories();

    [OperationContract]
    Order getOrder(int customerId, DateTime datePlaced);

    [OperationContract]
    int addOrder(int customerId, string status, DateTime datePlaced, DateTime deliverDate, string message);

    [OperationContract]
    int UpdateOrder(int customerId, string status, DateTime datePlaced, DateTime deliverDate, string message);

    [OperationContract]
    List<Order> getAllOrders();

    [OperationContract]
    Product getProduct(int Product_ID);
    [OperationContract]
    int UpdateStock(int P_ID, int ItemsPurchased);
    [OperationContract]
    int AddItemsToShoppingList(int ListID ,int ShoppingList_ID, int Product_ID, int Quantity);
    [OperationContract]
    User getUser(int User_ID);
    [OperationContract]
    int getNumUsers();
    [OperationContract]
    double CalculateProfit();
    [OperationContract]
    Address getAddress(int Address_ID);
    [OperationContract]
    int AddAdress(string line1, string line2, string suburb, string city, char billing, string type);
    [OperationContract]
    int UpdateAddress(int A_ID, string line1, string line2, string suburb, string city, char billing, string type, int Cus_ID);
    [OperationContract]
    Card getCard(int id);
    [OperationContract]
    int AddCard(int Cus_ID,string description, string name, string number, DateTime expiry);
    [OperationContract]
    int UpdateCards(int c_ID, int Cust_ID, string description, string name, string number, DateTime expiry);
    [OperationContract]
    Device getDevice(int D_ID);
    [OperationContract]
    int AddDevices(string os);
    [OperationContract]
    ListItem getListItem(int id);
    [OperationContract]
    int AddListItems(int P_ID, int quantity);
    [OperationContract]
    int UpdateListItem(int id, int list_ID, int P_ID, int quantity);
    [OperationContract]
    OrderItem getOrderedItems(int id);
    [OperationContract]
    List<Product> getProductByCat(int Cat_ID);
    [OperationContract]
    List<Product> getProductBySubCat(int Sub_ID);









}