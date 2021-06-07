using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace WebAPI.Controllers
{
    public class Device
    {
        public int id { get; set; }
        public string ison { get; set; }
        public Device()
        {
            this.id = 0;
            this.ison = null;
        }
        public Device(int id, string ison)
        {
            this.id = id;
            this.ison = ison;
        }

        public Device(Device device)
        {
            this.id = device.id;
            this.ison = device.ison;
        }
    }

    

    public class tbLedController : ApiController
    {
        Device devRes = new Device();
        List<Device> devList = new List<Device>();
        MySqlConnection conn = WebApiConfig.conn();
        const string table_name = "tbled";

        public bool ConnectDb()
        {
            int times = 0;
            bool flag = false;
            while (!flag)
            {
                try
                {
                    conn.Open();
                    flag = true;
                    break;
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    conn.Close();
                    times++;
                }
                if (times == 5) break;
            }
            return flag;
        }

        public void ResetId(MySqlCommand cmd)
        {
            cmd.CommandText = $"SELECT COUNT(*) FROM {table_name}";
            var count = Convert.ToInt16(cmd.ExecuteScalar());
            cmd.CommandText = $"ALTER TABLE {table_name} AUTO_INCREMENT = {count + 1}";
            cmd.ExecuteScalar();
            conn.Close();
            conn.Open();
        }

        [HttpGet]
        // GET api/led/get?id=...       
        public object Get(int id)
        {
            if (!ConnectDb()) return "Fail to connect Db";
            MySqlCommand query = conn.CreateCommand();
            query.CommandText = $"SELECT * FROM {table_name} where id = {id}";
            MySqlDataReader fetch_query = query.ExecuteReader();
            while (fetch_query.Read())
            {
                Device device = new Device(Convert.ToInt16(fetch_query["id"]), fetch_query["ison"].ToString());
                devRes = device;
            }
            conn.Close();
            return devRes;
        }

        [HttpGet]
        //GET api/led/getall
        public object GetAll()
        {
            if (!ConnectDb()) return "Fail to connect Db";
            MySqlCommand query = conn.CreateCommand();
            query.CommandText = $"SELECT * FROM {table_name}";
            MySqlDataReader fetch_query = query.ExecuteReader();
            while (fetch_query.Read())
            {
                Device device = new Device(Convert.ToInt16(fetch_query["id"]), fetch_query["ison"].ToString());
                devList.Add(device);
            }
            conn.Close();
            return devList;
        }

        [HttpPost]
        // POST api/led/post
        public object Post(string new_ison)
        {
            if (!ConnectDb()) return "Fail to connect Db";
            MySqlCommand query = conn.CreateCommand();
            ResetId(query);
            query.CommandText = $"INSERT INTO {table_name} VALUES(NULL,{new_ison})";
            query.ExecuteScalar();
            conn.Close();
            return "Success!";
        }

        [HttpPost]
        //POST api/led/post?
        public string Post([FromBody]Device new_device)
        {
            if (!ConnectDb()) return "Fail to connect Db";
            MySqlCommand query = conn.CreateCommand();
            ResetId(query);
            query.CommandText = $"INSERT INTO {table_name} VALUES (NULL,'{new_device.ison}')";
            //return query.CommandText;
            query.ExecuteScalar();
            conn.Close();
            return "Success!";
        }

        [HttpPut]
        // PUT api/led/put?
        public string Put([FromBody] Device updateDevice)
        {
            if (!ConnectDb()) return "Fail to connect Db";
            MySqlCommand query = conn.CreateCommand();
            query.CommandText = $"UPDATE {table_name} SET ison = '{updateDevice.ison}' WHERE id = {updateDevice.id}";
            query.ExecuteScalar();
            conn.Close();
            return "Success!";
        }

        [HttpPut]
        //PUT api/led/putall?
        public string PutAll([FromBody] List<Device> updateList)
        {
            foreach(Device device in updateList)
            {
                Put(device);
            }
            return "Success!";
        }

        // DELETE api/led/delete?id=...
        public string Delete(int id)
        {
            if (!ConnectDb()) return "Fail to connect Db";
            MySqlCommand query = conn.CreateCommand();
            query.CommandText = $"DELETE FROM {table_name} WHERE id = {id}";
            query.ExecuteScalar();
            conn.Close();
            return "Success!";
        }
    }
}
