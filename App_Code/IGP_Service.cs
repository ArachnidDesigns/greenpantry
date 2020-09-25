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
    int register(string name, string surname, string email, string password, string status, DateTime date, string userType);

    [OperationContract]
    int addUserNumber(int id, string number);

    [OperationContract]
    int removeUser(int id);

    [OperationContract]
    int updateUserDetails(int id, string name, string surname, string email, string number, string oldPass, string newPass);

    [OperationContract]
    int addNewProduct(string name, int SubID, double price, double cost, int stockQty, string imgLocation);

    [OperationContract]
    int updateProduct(int id, string name, int SubId, double price, double cost, string imgLocation);

    [OperationContract]
    List<Product> getAllProducts();

    [OperationContract]
    int removeProduct(int productId);

    [OperationContract]
    List<ProductCategory> getAllCategories();

    [OperationContract]
    List<SubCategory> getAllSubCategories();

    [OperationContract]
    Invoice getOrder(int orderId);

    [OperationContract]
    int addInvoice(int customerId, string status, DateTime datePlaced, DateTime deliverDate, string message, decimal total, int points);

    [OperationContract]
    int updateInvoice(int customerId, string status, DateTime datePlaced, DateTime deliverDate, string message, decimal total, int points);

    [OperationContract]
    List<Invoice> getAllInvoices();

    [OperationContract]
    List<Invoice> getAllCustomerInvoices(int customerId);

    [OperationContract]
    int getUsersPerDay(DateTime day);

    [OperationContract]
    Product getProduct(int Product_ID);

    [OperationContract]
    int updateStock(int P_ID, int ItemsPurchased);

    [OperationContract]
    int addItemsToShoppingList(int ListID ,int ShoppingList_ID, int Product_ID, int Quantity);
    [OperationContract]
    User getUser(int User_ID);

    [OperationContract]
    int getNumUsers();

    [OperationContract]
    double calculateProfit();

    [OperationContract]
    Address getAddress(int Address_ID);

    [OperationContract]
    int addAddress(string line1, string line2, string suburb, string city, char billing, string type, int C_ID , string Province);

    [OperationContract]
    int updateAddress(int A_ID, string line1, string line2, string suburb, string city, char billing, string type, int Cus_ID);

    [OperationContract]
    Card getCard(int id);

    [OperationContract]
    int addCard(int Cus_ID,string description, string name, string number, DateTime expiry);

    [OperationContract]
    int updateCards(int c_ID, int Cust_ID, string description, string name, string number, DateTime expiry);

    [OperationContract]
    Device getDevice(int D_ID);

    [OperationContract]
    int addDevices(string os);

    [OperationContract]
    ListProduct getListProduct(int id);

    [OperationContract]
    int addListProduct(int P_ID, int quantity);

    [OperationContract]
    int updateListProduct(int id, int list_ID, int P_ID, int quantity);

    [OperationContract]
    List<InvoiceLine> getAllInvoiceLines(int InvoiceID);

    [OperationContract]
    List<Product> getProductByCat(int Cat_ID);

    [OperationContract]
    List<Product> getProductBySubCat(int Sub_ID);

    [OperationContract]
    double profitPerProduct(int P_ID);

    [OperationContract]
    double profitPerSubCat(int S_ID);

    [OperationContract]
    double profitPerCat(int C_ID);

    [OperationContract]
    SubCategory getSubCat(int S_ID);

    [OperationContract]
    ProductCategory getCat(int C_ID);

    [OperationContract]
    List<SubCategory> getSubCatPerCat(int c_ID);

    [OperationContract]
    decimal calcProductVAT(int P_ID);

    [OperationContract]
    int getNumProductsInSub(int subID);

    [OperationContract]
    int addInvoiceLine(int product_ID, int invoice_ID, int quantity, decimal price);

    [OperationContract]
    List<Product> searchProducts(string input);

    [OperationContract]
    int updatePoints(int Cust_ID, int points);

    [OperationContract]
    int getUserPoints(int Cus_ID);

    [OperationContract]
    ProductCategory getCategorybyProductID(int p_ID);

    [OperationContract]
    int usersperWeek(DateTime currentDate);

    [OperationContract]
    double percentageUserChange(DateTime currentDate);

    [OperationContract]
    decimal salesPerWeek(DateTime date);

    [OperationContract]
    int addCategory(int id, string name);

    [OperationContract]
    int addSubCategory(int id, string name);
    
    [OperationContract]
    int updateSubCategories(int id, string name);
    
    [OperationContract]
    int updateCategories(int id, string name);
    
    [OperationContract]
    double percentageSaleChanger(DateTime currentDate);

    [OperationContract]
    int NumsalesPerWeek(DateTime date);

    [OperationContract]
    double NumSaleChange(DateTime currentDate);
    
    [OperationContract]
    int numProductSales(DateTime currentDate, int Product_ID);

    [OperationContract]
    double percProductSales(DateTime currentDate, int Product_ID);
}
