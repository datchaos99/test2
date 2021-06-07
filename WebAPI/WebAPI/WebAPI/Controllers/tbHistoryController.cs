using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class LogMessage
    {
        public DateTime time { get; set; }
        public string start { get; set; }
        public int end { get; set; }
        public string turn { get; set; }

        public LogMessage()
        {
            this.time = new DateTime(1999, 3, 28, 5, 30, 11);
            this.start = null;
            this.end = 0;
            this.turn = null;
        }

        public LogMessage(DateTime time, string start, int end, string turn)
        {
            this.time = time;
            this.start = start;
            this.end = end;
            this.turn = turn;
        }

        public LogMessage(LogMessage old_log)
        {
            this.time = old_log.time;
            this.start = old_log.start;
            this.end = old_log.end;
            this.turn = old_log.turn;
        }
    }

    public class tbHistoryController : ApiController
    {
        LogMessage mesRes = new LogMessage();
        List<LogMessage> mesList = new List<LogMessage>();
        MySqlConnection conn = WebApiConfig.conn();

        const string table_name = "tbhistory";

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

        //[HttpGet]
        //// GET api/led/get?id=...       
        //public object Get(int id)
        //{
        //    if (!ConnectDb()) return "Fail to connect Db";
        //    MySqlCommand query = conn.CreateCommand();
        //    query.CommandText = $"SELECT * FROM {table_name} where id = {id}";
        //    MySqlDataReader fetch_query = query.ExecuteReader();
        //    while (fetch_query.Read())
        //    {
        //        Device device = new Device(Convert.ToInt16(fetch_query["id"]), fetch_query["ison"].ToString());
        //        devRes = device;
        //    }
        //    conn.Close();
        //    return devRes;
        //}

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
                LogMessage message = new LogMessage(DateTime.Parse(fetch_query["time"].ToString()), fetch_query["start"].ToString(), Convert.ToInt16(fetch_query["end"]), fetch_query["turn"].ToString());
                mesList.Add(message);
            }
            conn.Close();
            return mesList;
        }


        [HttpPost]
        //POST api/led/post?
        public string Post([FromBody] LogMessage new_message)
        {
            if (!ConnectDb()) return "Fail to connect Db";
            MySqlCommand query = conn.CreateCommand();
            ResetId(query);
            query.CommandText = $"INSERT INTO {table_name} VALUES (NULL,'{new_message.time.ToString("yyyy/MM/dd HH:mm:ss")}','{new_message.start}',{new_message.end},'{new_message.turn}')";
            //return query.CommandText;
            query.ExecuteScalar();
            conn.Close();
            return "Success!";
        }

        //[HttpPut]
        //// PUT api/led/put?
        //public string Put([FromBody] LogMessage updatedMessage)
        //{
        //    if (!ConnectDb()) return "Fail to connect Db";
        //    MySqlCommand query = conn.CreateCommand();
        //    query.CommandText = $"UPDATE {table_name} SET start = '{updatedMessage.start}', end = new_message.end, turn = new_message.turn WHERE time = {updatedMessage.time}";
        //    query.ExecuteScalar();
        //    conn.Close();
        //    return "Success!";
        //}

        //[HttpPut]
        ////PUT api/led/putall?
        //public string PutAll([FromBody] List<LogMessage> updateList)
        //{
        //    foreach (LogMessage message in updateList)
        //    {
        //        Put(message);
        //    }
        //    return "Success!";
        //}

        // DELETE api/led/delete?id=...
        public string DeleteAll()
        {
            if (!ConnectDb()) return "Fail to connect Db";
            MySqlCommand query = conn.CreateCommand();
            query.CommandText = $"DELETE FROM {table_name}";
            query.ExecuteScalar();
            conn.Close();
            return "Success!";
        }
    }
}
