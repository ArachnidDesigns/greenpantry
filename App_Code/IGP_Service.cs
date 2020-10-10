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

    //USER MANAGEMENT -------------------------------
    [OperationContract]
    //void newsletter(string subject, string body, string password);
    void newsletter(string senderemail, string subscriberemail, string subject, string body, string password, string smtp);

    [OperationContract]
    int login(string email, string password);

    [OperationContract]
    int register(string name, string surname, string email, string password, string status, DateTime date, string userType);

    [OperationContract]
    int addUserNumber(int id, string number);

    [OperationContract]
    int removeUser(int id);

    [OperationContract]
    int updateUserDetails(int id, string name, string surname, string email, string number);

    [OperationContract]
    int updateUserAdmin(int userID, int points, string usertype, string status);

    [OperationContract]
    int updatePassword(int id, string oldPassword, string newPassword);

    [OperationContract]
    User getUser(int User_ID);

    [OperationContract]
    List<User> getAllUsers();

    [OperationContract]
    int getNumUsers();

    [OperationContract]
    int updatePoints(int Cust_ID, int points);

    [OperationContract]
    int getUserPoints(int Cus_ID);

    //PRODUCT MANAGEMENT --------------------------------------------------------

    [OperationContract]
    int addNewProduct(string name, int SubID, double price, double cost, int stockQty, string imgLocation, string status, string description);

    [OperationContract]
    int updateProduct(int id, string name, int SubId, double price, double cost, string imgLocation,string status, int stock, string description);

    [OperationContract]
    List<Product> getAllProducts();

    [OperationContract]
    int removeProduct(int productId);

    [OperationContract]
    Product getProduct(int Product_ID);

    [OperationContract]
    int updateStock(int P_ID, int ItemsPurchased);

    [OperationContract]
    List<Product> getProductByCat(int Cat_ID);

    [OperationContract]
    List<Product> getProductBySubCat(int Sub_ID);

    [OperationContract]
    List<Product> searchProducts(string input);

    //CATEGORY MANAGEMENT --------------------------------------------------------

    [OperationContract]
    List<ProductCategory> getAllCategories();

    [OperationContract]
    ProductCategory getCat(int C_ID);

    [OperationContract]
    ProductCategory getCategorybyProductID(int p_ID);

    [OperationContract]
    int addCategory(string name, string status);

    [OperationContract]
    int updateCategories(int id, string name, string status);

    //SUBCATEGORY MANAGEMENT --------------------------------------------------------

    [OperationContract]
    List<SubCategory> getAllSubCategories();

    [OperationContract]
    SubCategory getSubCat(int S_ID);

    [OperationContract]
    List<SubCategory> getSubCatPerCat(int c_ID);

    [OperationContract]
    int addSubCategory(int catid, string name, string status);

    [OperationContract]
    int updateSubCategories(int id, int cat_ID, string name, string status);

    //INVOICE MANAGEMENT --------------------------------------------------------

    [OperationContract]
    Invoice getInvoice(int InvoiceID);

    [OperationContract]
    int addInvoice(int customerId, string status, DateTime datePlaced, DateTime deliverDate, string message, decimal total, int points);

    [OperationContract]
    int updateInvoice(int customerId, string status, DateTime datePlaced, DateTime deliverDate, string message, decimal total, int points);

    [OperationContract]
    List<Invoice> getAllInvoices();

    [OperationContract]
    List<Invoice> getAllCustomerInvoices(int customerId);

    //INVOICE LINE MANAGEMENT ---------------------------------------------------------

    [OperationContract]
    List<InvoiceLine> getAllInvoiceLines(int InvoiceID);

    [OperationContract]
    int addInvoiceLine(int product_ID, int invoice_ID, int quantity, decimal price);

    //SHOPPING LIST MANAGEMENT ---------------------------------------------------------

    [OperationContract]
    int addToList(int userID, int productID, int quantity);

    [OperationContract]
    List<ShoppingList> getList(int userID);

    [OperationContract]
    int updateList(int userID, int P_ID, int quantity);

    [OperationContract]
    int removeList(int userID);

    //ADDRESS MANAGEMENT -------------------------------------------------------------

    [OperationContract]
    Address getAddress(int Address_ID);

    [OperationContract]
    int addAddress(string line1, string line2, string suburb, string city, int zip, string type, int C_ID, string Province);

    [OperationContract]
    int updateAddress(string line1, string line2, string suburb, string city, string province, int zip, string type, int Cus_ID);

    //CARD MANAGEMENT -------------------------------------------------------------

    [OperationContract]
    Card getCard(int id);

    [OperationContract]
    int addCard(int Cus_ID, string description, string name, string number, DateTime expiry);

    [OperationContract]
    int updateCards(int c_ID, int Cust_ID, string description, string name, string number, DateTime expiry);

    //DEVICE MANAGEMENT -------------------------------------------------------------

    [OperationContract]
    Device getDevice(int userID);

    [OperationContract]
    int addDevices(int cust_ID, string useragent);

    //REPORT MANAGEMENT ------------------------------------------------------------

    [OperationContract]
    int getUsersPerDay(DateTime day);

    [OperationContract]
    double calculateProfit();

    [OperationContract]
    double profitPerProduct(int P_ID);

    [OperationContract]
    double profitPerSubCat(int S_ID);

    [OperationContract]
    double profitPerCat(int C_ID);

    [OperationContract]
    decimal calcProfitPerday(DateTime date);

    [OperationContract]
    decimal calcProductVAT(int P_ID);

    [OperationContract]
    int getNumProductsInSub(int subID);

    [OperationContract]
    int usersperWeek(DateTime currentDate);

    [OperationContract]
    double percentageUserChange(DateTime currentDate);

    [OperationContract]
    List<DateTime> getWeekDates(DateTime date);

    [OperationContract]
    decimal salesPerWeek(DateTime date);

    [OperationContract]
    double percentageSaleChanger(DateTime currentDate);

    [OperationContract]
    int NumsalesPerWeek(DateTime date);

    [OperationContract]
    double NumSaleChange(DateTime currentDate);

    [OperationContract]
    decimal calcCategoryTotalSales(int cId);

    [OperationContract]
    decimal calcSalesPerDay(DateTime date);

    [OperationContract]
    List<DateTime> getMonthDates(DateTime date);

    [OperationContract]
    int numProductSales(DateTime currentDate, int Product_ID);

    [OperationContract]
    double percProductSales(DateTime currentDate, int Product_ID);

    [OperationContract]
    List<Product> getfilteredProducts(int minPrice, int maxPrice);

    //TRAFFIC MANAGEMENT ------------------------------------------------------------
    [OperationContract]
    int addTraffic(string pageName, DateTime currentdate, int unique);

    [OperationContract]
    int singlePageTraffic(string pageName);

    [OperationContract]
    int trafficPerWeek(DateTime currentDate);

    [OperationContract]
    double TrafficChange(DateTime currentDate);

    [OperationContract]
    int singlePageUniqueTraffic(string pageName);

    [OperationContract]
    List<String> topPages();

    //CATEGORY SALES MANAGEMENT ------------------------------------------------------------
    [OperationContract]
    double percentageCategorySales(DateTime currentDate, int Cat_ID);

    [OperationContract]
    int numProductSalesperCategory(DateTime currentDate, int Cat_ID);

    //SubCATEGORY SALES MANAGEMENT ------------------------------------------------------------
    [OperationContract]
    double percentageSubCategorySales(DateTime currentDate, int SubCat_ID);

    [OperationContract]
    int numProductSalesperSubCategory(DateTime currentDate, int SubCat_ID);

    [OperationContract]
    List<int> TopProducts();

    [OperationContract]
    int getProQtySold(int P_ID);

    //THE ALGORITHM -------------------------------------------------------------------------
    [OperationContract]
    List<Product> recommendedProducts(int userID);

    [OperationContract]
    List<recommended> recommendTest(int userID);
    [OperationContract]
    List<string> getAllDevices();

    [OperationContract]
    int getTotOSUsers(string os);

    [OperationContract]
    List<int> WorstProducts();

    




}