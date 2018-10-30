using MySql.Data.MySqlClient;
using SysInfoManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ULinker.Common.Utility;
using ULinker.Models.DO;

namespace ULinker.DAL
{
    public class ApiDao
    {
        private static ApiDao _Instance = null;

        public static ApiDao Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ApiDao();
                return _Instance;
            }
        }

        private ApiDao()
        {

        }

        public Api GetById(int id)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                Api item = null;
                try
                {
                    string strCmd = @"SELECT * FROM `api` WHERE `Id`=@Id";
                    MySqlParameter[] paramters = new MySqlParameter[]
                {
                       new MySqlParameter("@Id",MySqlDbType.Int32)
                };

                    paramters[0].Value = id;

                    DataRow dr = db.GetDataRow(strCmd, paramters);
                    if (dr != null)
                    {
                        item = RowToObject(dr);
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("ApiDao.GetById.{0}", ex.Message), new Exception("error"));
                }

                return item;
            }
        }

        public List<Api> GetBySomeWhere(string keyword, int limit, int pageSize, out int total)
        {
            List<Api> filtered = new List<Api>();
            try
            {
                total = 0;
                filtered = _Filter(keyword, limit, pageSize);
                total = Count(keyword);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return filtered;
        }

        public bool Create(Api model)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    string strCmd = @"INSERT INTO `api` (`PlatformId`,`Name`,`FunctionName`,`Url`,`Description`,`CreateTime`,`ModifyTime`)
                                      (@PlatformId,@Name,@FunctionName,@Url,@Description,NOW(),NOW())";

                    MySqlParameter[] paramters = new MySqlParameter[]
             {
                       new MySqlParameter("@PlatformId",MySqlDbType.Int32),
                       new MySqlParameter("@Name",MySqlDbType.String),
                       new MySqlParameter("@FunctionName",MySqlDbType.String),
                       new MySqlParameter("@Url",MySqlDbType.String),
                       new MySqlParameter("@Description",MySqlDbType.String)
             };

                    paramters[0].Value = model.Id;
                    paramters[1].Value = model.Name;
                    paramters[2].Value = model.FunctionName;
                    paramters[3].Value = model.Url;
                    paramters[4].Value = model.Description;

                    int count = db.ExecuteNonQuery(strCmd, paramters);
                    return count > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("AdminUserDao.Create.{0}", ex.Message), new Exception("error"));
                    return false;
                }
            }
        }

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginAccountId"></param>
        /// <returns></returns>
        public bool PhysicDelete(int id, string loginAccountId)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    string strCmd = @"DELETE FROM `api` WHERE `Id`=@Id";

                    MySqlParameter[] paramters = new MySqlParameter[]
             {
                       new MySqlParameter("@Id",MySqlDbType.Int32)
             };

                    paramters[0].Value = id;

                    int count = db.ExecuteNonQuery(strCmd, paramters);
                    return count > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("ApiDao.PhysicDelete.{0}", ex.Message), new Exception("error"));
                    return false;
                }
            }
        }

        #region Private

        private List<Api> _Filter(string keyword, int limit, int pageSize)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                List<Api> list = new List<Api>();
                try
                {
                    string strCmd = @"SELECT * FROM `api` WHERE 1=1";

                    if (!string.IsNullOrEmpty(keyword))
                    {
                        strCmd += @" AND `Name` like " + string.Format("%{0}%", keyword);
                    }


                    strCmd += " ORDER BY CreateTime DESC";
                    strCmd += " LIMIT " + limit + "," + pageSize;

                    DataTable dt = db.GetDataSet(strCmd);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            Api item = RowToObject(row);
                            list.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("ApiDao._Filter.{0}", ex.Message), new Exception("error"));
                }
                return list;
            }
        }

        private int Count(string name)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = SysInfoManage.DENString(settings.ConnectionString);
            using (ULinker.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                int total = 0;
                try
                {
                    string strCmd = @" SELECT COUNT(1) FROM `platform` WHERE 1=1 ";

                    if (!String.IsNullOrEmpty(name))
                    {
                        strCmd += @" AND `Name` LIKE '%" + name + "%'";
                    }

                    DataRow row = db.GetDataRow(strCmd);
                    if (row != null)
                    {
                        total = int.Parse(row["COUNT(1)"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("PlatformDao.Count.{0}", ex.Message), ex);
                }
                return total;
            }
        }

        private Api RowToObject(DataRow row)
        {
            return new Api
            {
                Id = int.Parse(row["Id"].ToString()),
                PlatformId= int.Parse(row["PlatformId"].ToString()),
                Name = row["Name"].ToString(),
                FunctionName= row["FunctionName"].ToString(),
                Url= row["Url"].ToString(),
                CreateTime = row.IsNull("CreateTime") ? new DateTime() : DateTime.Parse(row["CreateTime"].ToString()),
                ModifyTime = row.IsNull("ModifyTime") ? new DateTime() : DateTime.Parse(row["ModifyTime"].ToString()),
                Description = row["Description"].ToString()
            };
        }

        #endregion
    }
}
