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

}
