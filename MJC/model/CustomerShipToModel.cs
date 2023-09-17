using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MJC.common;
using MJC.config;

namespace MJC.model
{
    public struct CustomerShipToData
    {
        public int id { get; set; }
        public string name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }

        public CustomerShipToData(int _id, string _name, string _address1, string _address2, string _city, string _state, string _zipcode)
        {
            id = _id;
            name = _name;
            address1 = _address1;
            address2 = _address2;
            city = _city;
            state = _state;
            zipcode = _zipcode;
        }
    }
    public class CustomerShipToModel : DbConnection
    {
        public List<CustomerShipToData> LoadCustomerShipTosNyCustomerId(string filter, int customerId = 0)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    SqlDataReader reader;
                    List<CustomerShipToData> returnList = new List<CustomerShipToData>();

                    command.CommandText = @"select *
                                            from dbo.CustomerShipTos
                                            where customerId=@customerId";
                    command.Parameters.AddWithValue("@customerId", customerId);

                    if (filter != "")
                    {
                        command.CommandText = @"select *
                                                from dbo.CustomerShipTos
                                                where CustomerShipTos.name like @filter and customerId=@customerId";
                        command.Parameters.Add("@filter", System.Data.SqlDbType.VarChar).Value = "%" + filter + "%";
                        command.Parameters.AddWithValue("@customerId", customerId);
                    }

                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        returnList.Add(
                            new CustomerShipToData((int)reader[0], reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), reader[7].ToString(), reader[8].ToString())
                        );
                    }
                    reader.Close();

                    return returnList;
                }
            }
        }

        public bool AddCustomerShipTo(int custmerId, string name, string address1, string address2, string city, string state, string zipcode)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    //Get Total Number of Customers
                    command.CommandText = "INSERT INTO dbo.CustomerShipTos (custmerId, name, address1, address2, city, state, zipcode,createdAt, createdBy, updatedAt, updatedBy) OUTPUT INSERTED.ID VALUES (@Value1, @Value2, @Value3, @Value4, @Value5, @Value6, @Value7, @Value8, @Value9, @Value10, @Value11)";
                    command.Parameters.AddWithValue("@Value1", custmerId);
                    command.Parameters.AddWithValue("@Value2", name);
                    command.Parameters.AddWithValue("@Value3", address1);
                    command.Parameters.AddWithValue("@Value4", address2);
                    command.Parameters.AddWithValue("@Value5", city);
                    command.Parameters.AddWithValue("@Value6", state);
                    command.Parameters.AddWithValue("@Value7", zipcode);
                    command.Parameters.AddWithValue("@Value8", DateTime.Now);
                    command.Parameters.AddWithValue("@Value9", 1);
                    command.Parameters.AddWithValue("@Value10", DateTime.Now);
                    command.Parameters.AddWithValue("@Value11", 1);

                    Messages.ShowInformation("The new CustomerShipTo was added successfully.");
                }

                return true;
            }
        }

        public bool UpdateCustomerShipTo(int custmerId, string name, string address1, string address2, string city, string state, string zipcode, int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"UPDATE dbo.CustomerShipTos SET custmerId = @custmerId, name = @name, address1 = @address1, address2 = @address2, city = @city, state = @state, zipcode = @zipcode WHERE id = @id";
                    command.Parameters.AddWithValue("@custmerId", custmerId);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@address1", address1);
                    command.Parameters.AddWithValue("@address2", custmerId);
                    command.Parameters.AddWithValue("@city", city);
                    command.Parameters.AddWithValue("@state", state);
                    command.Parameters.AddWithValue("@zipcode", zipcode);
                    command.Parameters.AddWithValue("@id", id);

                    command.ExecuteNonQuery();

                    Messages.ShowInformation("The CustomerShipTo was updated successfully.");
                }

                return true;
            }
        }

        public bool DeleteCustomerShipTo(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    try
                    {
                        command.Connection = connection;
                        //Get Total Number of Customers
                        command.CommandText = "DELETE FROM dbo.CustomerShipTos WHERE id = @Value1";
                        command.Parameters.AddWithValue("@Value1", id);

                        command.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {
                        Sentry.SentrySdk.CaptureException(ex);
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
