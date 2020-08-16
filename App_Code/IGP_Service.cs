using System;
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
    Product getProduct(int Product_ID);
    [OperationContract]
    int UpdateStock(int P_ID, int ItemsPurchased);
    [OperationContract]
    int AddItemsToShoppingList(int ListID ,int ShoppingList_ID, int Product_ID, int Quantity);
    [OperationContract]
    User getUser(int User_ID);
    [OperationContract]
    int getAllUsers();
    [OperationContract]
    double getProfit();




}
